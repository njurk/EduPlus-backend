using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login/admin")]
    public async Task<IActionResult> LoginAdmin([FromBody] LoginDto dto)
    {
        try
        {
            var result = await _authService.LoginAdminAsync(dto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("login/teacher")]
    public async Task<IActionResult> LoginTeacher([FromBody] LoginDto dto)
    {
        try
        {
            var result = await _authService.LoginTeacherAsync(dto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("login/mobile")]
    public async Task<IActionResult> LoginMobile([FromBody] LoginDto dto)
    {
        try
        {
            var result = await _authService.LoginMobileAsync(dto);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout([FromQuery] string viewName = "Unknown")
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var email = User.FindFirst(ClaimTypes.Email)?.Value ?? User.FindFirst(ClaimTypes.Name)?.Value ?? "";
        var roleLevel = int.Parse(User.FindFirst("MaxRoleLevel")?.Value ?? "0");

        _authService.Logout(userId, email, roleLevel, viewName);

        return Ok(new { message = "Wylogowano pomyślnie" });
    }
}

