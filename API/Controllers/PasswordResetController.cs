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
            }
            catch { }
            return Ok();
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenDto dto)
        {
            var isValid = await _service.ValidateTokenAsync(dto.Token);
            if (!isValid)
                return BadRequest("Kod jest nieprawidłowy lub wygasł");
            return Ok();
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var success = await _service.ResetPasswordAsync(dto, _passwordHashService);
            if (!success)
                return BadRequest("Kod jest nieprawidłowy lub wygasł");
            return Ok();
        }
    }
}
