using Data.Data;
using Microsoft.AspNetCore.Http;
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
        Task<LoginResponseDto> LoginAdminAsync(LoginDto dto);
        Task<LoginResponseDto> LoginTeacherAsync(LoginDto dto);
        Task<LoginResponseDto> LoginMobileAsync(LoginDto dto);
        void Logout(int userId, string email, IEnumerable<int> roleLevels);
    }

    public class AuthService : IAuthService
    {
        private readonly EduPlusDbContext _context;
        private readonly IPasswordHashService _passwordHashService;
        private readonly IConfiguration _configuration;
        private readonly IEventLogService _eventLogService;

        public AuthService(
            EduPlusDbContext context,
            IPasswordHashService passwordHashService,
            IConfiguration configuration,
            IEventLogService eventLogService)
        {
            _context = context;
            _passwordHashService = passwordHashService;
            _configuration = configuration;
            _eventLogService = eventLogService;
        }

        public async Task<LoginResponseDto> LoginAdminAsync(LoginDto dto)
        {
            var user = await GetAuthenticatedUser(dto);
            var roleLevels = user.UserRoles.Select(ur => ur.Role.Level).ToList();

            if (!user.UserRoles.Any(ur => ur.Role.Level == 1))
            {
                _eventLogService.Log("LOGIN_FAIL", user.Id, user.Email, roleLevels, "AdminLogin attempt");
                throw new UnauthorizedAccessException("Panel administratora jest przeznaczony tylko dla administratorów");
            }

            var response = BuildLoginResponse(user);
            _eventLogService.Log("LOGIN", user.Id, user.Email, roleLevels);
            return response;
        }

        public async Task<LoginResponseDto> LoginTeacherAsync(LoginDto dto)
        {
            var user = await GetAuthenticatedUser(dto);
            var roleLevels = user.UserRoles.Select(ur => ur.Role.Level).ToList();
            var maxLevel = roleLevels.Min();

            if (maxLevel == 1 || maxLevel >= 3)
            {
                _eventLogService.Log("LOGIN_FAIL", user.Id, user.Email, roleLevels, "TeacherLogin attempt");
                throw new UnauthorizedAccessException("Panel nauczyciela jest przeznaczony tylko dla nauczycieli");
            }

            var response = BuildLoginResponse(user);
            _eventLogService.Log("LOGIN", user.Id, user.Email, roleLevels);
            return response;
        }

        public async Task<LoginResponseDto> LoginMobileAsync(LoginDto dto)
        {
            var user = await GetAuthenticatedUser(dto);
            var roleLevels = user.UserRoles.Select(ur => ur.Role.Level).ToList();
            var maxLevel = roleLevels.Min();

            if (maxLevel <= 2)
            {
                _eventLogService.Log("LOGIN_FAIL", user.Id, user.Email, roleLevels, "MobileLogin attempt");
                throw new UnauthorizedAccessException("Aplikacja mobilna jest przeznaczona tylko dla uczniów i rodziców");
            }

            var response = BuildLoginResponse(user);

            if (maxLevel == 3)
            {
                var parentStudent = await _context.ParentStudents
                    .Include(ps => ps.Student)
                    .FirstOrDefaultAsync(ps => ps.ParentId == user.Id);
                
                if (parentStudent?.Student != null)
                {
                    response.StudentName = $"{parentStudent.Student.FirstName} {parentStudent.Student.LastName}";
                }
            }

            _eventLogService.Log("LOGIN", user.Id, user.Email, roleLevels);
            return response;
        }

        public void Logout(int userId, string email, IEnumerable<int> roleLevels)
        {
            _eventLogService.Log("LOGOUT", userId, email, roleLevels);
        }

        private async Task<Data.Data.Entities.User> GetAuthenticatedUser(LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !user.IsActive || !_passwordHashService.VerifyPassword(dto.Password, user.Password))
            {
                _eventLogService.Log("LOGIN_FAIL", null, dto.Email, reason: "invalid credentials");
                throw new UnauthorizedAccessException("Błędny email lub hasło");
            }

            return user;
        }

        private LoginResponseDto BuildLoginResponse(Data.Data.Entities.User user)
        {
            var tokenString = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                Token = tokenString,
                UserId = user.Id,
                UserEmail = user.Email,
                UserName = $"{user.FirstName} {user.LastName}",
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
                MaxRoleLevel = user.UserRoles.Max(ur => ur.Role.Level)
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
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim("roleLevels", string.Join(",", user.UserRoles.Select(ur => ur.Role.Level).OrderBy(l => l)))
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
