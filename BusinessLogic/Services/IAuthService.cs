using Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
    }

    public class AuthService : IAuthService
    {
        private readonly SchoolDbContext _context;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IConfiguration _configuration;

        public AuthService(SchoolDbContext context, IPasswordHashService passwordHashService, IConfiguration configuration)
        {
            _context = context;
            _passwordHashService = passwordHashService;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !user.IsActive || !_passwordHashService.VerifyPassword(dto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Błędny email lub hasło");
            }

            if (!user.UserRoles.Any(ur => ur.Role.Level == 1))
            {
                throw new UnauthorizedAccessException("Nie posiadasz odpowiednich uprawnień");
            }

            var tokenString = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                Token = tokenString,
                UserId = user.Id,
                UserName = $"{user.FirstName} {user.LastName}",
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            };
        }

        private string GenerateJwtToken(Data.Data.Entities.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        };

            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(4),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
