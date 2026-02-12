using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace BusinessLogic.Services
{
    public interface IPasswordResetService
    {
        Task<string> RequestResetAsync(string email, IEmailService emailService);
        Task<bool> ValidateTokenAsync(string token);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto, IPasswordHashService passwordHashService);
    }

    public class PasswordResetService : IPasswordResetService
    {
        private readonly EduPlusDbContext _context;
        private readonly double _tokenExpirationHours;

        public PasswordResetService(EduPlusDbContext context, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _context = context;
            _tokenExpirationHours = double.Parse(configuration["PasswordReset:TokenExpirationHours"] ?? "1");
        }

        public async Task<string> RequestResetAsync(string email, IEmailService emailService)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new KeyNotFoundException("Użytkownik o podanym adresie email nie istnieje");

            var code = new Random().Next(100000, 999999).ToString();
            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Token = code,
                ExpirationDate = DateTime.Now.AddHours(_tokenExpirationHours)
            };

            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            await emailService.SendPasswordResetEmailAsync(email, code);

            return code;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var resetToken = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == token && t.ExpirationDate > DateTime.Now);

            return resetToken != null;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto, IPasswordHashService passwordHashService)
        {
            var resetToken = await _context.PasswordResetTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == dto.Token && t.ExpirationDate > DateTime.Now);

            if (resetToken == null) return false;

            resetToken.User.Password = passwordHashService.HashPassword(dto.NewPassword);
            resetToken.User.UpdatedAt = DateTime.Now;

            _context.PasswordResetTokens.Remove(resetToken);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
