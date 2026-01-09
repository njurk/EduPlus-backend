using API.DTOs;
using BusinessLogic.Services;
using Data.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly SchoolDbContext _context;

    private readonly IPasswordHashService _passwordHashService;

    private readonly IConfiguration _configuration;

    public AuthController(SchoolDbContext context, IPasswordHashService passwordHashService, IConfiguration configuration)
    {
        _context = context;
        _passwordHashService = passwordHashService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null || !user.IsActive)
            return Unauthorized("Błędny email lub hasło");

        if (!_passwordHashService.VerifyPassword(dto.Password, user.Password))
            return Unauthorized("Błędny email lub hasło");

        if (!user.UserRoles.Any(ur => ur.Role.Level == 1))
            return Unauthorized("Nie posiadasz odpowiednich uprawnień");

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
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new
        {
            Token = tokenString,
            UserId = user.Id,
            UserName = $"{user.FirstName} {user.LastName}",
            Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
        });
    }
}
