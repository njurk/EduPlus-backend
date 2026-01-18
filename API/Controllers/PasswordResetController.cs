using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordResetController : ControllerBase
    {
        private readonly IPasswordResetService _service;
        private readonly IEmailService _emailService;
        private readonly IPasswordHashService _passwordHashService;

        public PasswordResetController(IPasswordResetService service, IEmailService emailService, IPasswordHashService passwordHashService)
        {
            _service = service;
            _emailService = emailService;
            _passwordHashService = passwordHashService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestReset([FromBody] RequestPasswordResetDto dto)
        {
            try
            {
                await _service.RequestResetAsync(dto.Email, _emailService);
                return Ok(new { message = "Link do resetu hasła został wysłany" });
            }
            catch (KeyNotFoundException)
            {
                return Ok(new { message = "Jeśli podany email jest w naszym systemie, link zostanie wysłany" });
            }
            catch (Exception)
            {
                return Ok(new { message = "Jeśli podany email jest w naszym systemie, link zostanie wysłany" });
            }
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var success = await _service.ResetPasswordAsync(dto, _passwordHashService);
            if (!success)
                return BadRequest(new { message = "Token jest nieprawidłowy lub wygasł" });

            return Ok(new { message = "Hasło zostało zresetowane pomyślnie" });
        }
    }
}
