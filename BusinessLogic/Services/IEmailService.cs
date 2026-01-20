using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace BusinessLogic.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
        Task SendTicketCreatedEmailAsync(string toEmail, int ticketNumber, string reason, string content);
        Task SendTicketClosedEmailAsync(string toEmail, int ticketNumber, string reason, string content, string adminResponse, string resolvedBy);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("EduPlus", _configuration["Email:From"]));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "EduPlus - resetowanie hasła";

            message.Body = new TextPart("html")
            {
                Text = $@"
                    <p>Otrzymaliśmy prośbę o reset hasła do twojego konta w serwisie EduPlus. Kliknij poniższy link, aby zresetować hasło:</p>
                    <p><a href=""{resetLink}"">{resetLink}</a></p>
                    <p>Link jest ważny przez 1 godzinę</p>
                    <p>Jeśli nie prosiłeś o reset hasła, zignoruj tę wiadomość.</p>
                    <br/>
                    <p>Pozdrawiamy,<br/>Zespół EduPlus</p>
                "
            };

            await SendEmailAsync(message);
        }

        public async Task SendTicketCreatedEmailAsync(string toEmail, int ticketNumber, string reason, string content)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("EduPlus", _configuration["Email:From"]));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = $"EduPlus - zgłoszenie #{ticketNumber} zostało przyjęte";

            message.Body = new TextPart("html")
            {
                Text = $@"
                    <h2>Twoje zgłoszenie zostało przyjęte</h2>
                    <p><strong>Numer zgłoszenia:</strong> #{ticketNumber}</p>
                    <p><strong>Powód:</strong> {reason}</p>
                    <p><strong>Treść zgłoszenia:</strong></p>
                    <div style=""background-color: #f5f5f5; padding: 12px; border-radius: 4px; margin: 8px 0;"">{content}</div>
                    <br/>
                    <p>Dziękujemy za przesłanie zgłoszenia. Nasz zespół zajmie się nim najszybciej jak to możliwe.</p>
                    <br/>
                    <p>Pozdrawiamy,<br/>Zespół EduPlus</p>
                "
            };

            await SendEmailAsync(message);
        }

        public async Task SendTicketClosedEmailAsync(string toEmail, int ticketNumber, string reason, string content, string adminResponse, string resolvedBy)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("EduPlus", _configuration["Email:From"]));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = $"EduPlus - odpowiedź na zgłoszenie #{ticketNumber}";

            message.Body = new TextPart("html")
            {
                Text = $@"
                    <h2>Twoje zgłoszenie zostało rozpatrzone</h2>
                    <p><strong>Numer zgłoszenia:</strong> #{ticketNumber}</p>
                    <p><strong>Powód:</strong> {reason}</p>
                    <p><strong>Treść zgłoszenia:</strong></p>
                    <div style=""background-color: #f5f5f5; padding: 12px; border-radius: 4px; margin: 8px 0;"">{content}</div>
                    <p><strong>Rozpatrzone przez:</strong> {resolvedBy}</p>
                    <p><strong>Odpowiedź:</strong></p>
                    <div style=""background-color: #e8f5e9; padding: 12px; border-radius: 4px; margin: 8px 0;"">{adminResponse}</div>
                    <br/>
                    <p>Pozdrawiamy,<br/>Zespół EduPlus</p>
                "
            };

            await SendEmailAsync(message);
        }

        private async Task SendEmailAsync(MimeMessage message)
        {
            using var client = new SmtpClient();
            
            await client.ConnectAsync(
                _configuration["Email:SmtpServer"], 
                int.Parse(_configuration["Email:SmtpPort"]!), 
                SecureSocketOptions.StartTls
            );

            await client.AuthenticateAsync(
                _configuration["Email:Username"], 
                _configuration["Email:Password"]
            );

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
