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
    public IActionResult Logout()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var email = User.FindFirst(ClaimTypes.Email)?.Value ?? User.FindFirst(ClaimTypes.Name)?.Value ?? "";
        var roleLevelsStr = User.FindFirst("roleLevels")?.Value ?? "";
        var roleLevels = roleLevelsStr.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

        _authService.Logout(userId, email, roleLevels);

        return Ok(new { message = "Wylogowano pomyślnie" });
    }
}

