using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using BusinessLogic.Templates;

namespace BusinessLogic.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string toEmail, string code);
        Task SendTicketCreatedEmailAsync(string toEmail, int ticketNumber, string reason, string content);
        Task SendTicketClosedEmailAsync(string toEmail, int ticketNumber, string reason, string content, string adminResponse, string resolvedBy);
        Task SendNewGradeEmailAsync(string toEmail, string studentName, string subjectName, string gradeValue, string teacherName, DateTime issueDate);
        Task SendNegativeAttendanceEmailAsync(string toEmail, string studentName, string subjectName, string attendanceType, string teacherName, DateTime date);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendPasswordResetEmailAsync(string toEmail, string code)
            => SendAsync(toEmail, "EduPlus - resetowanie hasła", EmailTemplates.PasswordReset(code));

        public Task SendTicketCreatedEmailAsync(string toEmail, int ticketNumber, string reason, string content)
            => SendAsync(toEmail, $"EduPlus - zgłoszenie #{ticketNumber} zostało przyjęte", EmailTemplates.TicketCreated(ticketNumber, reason, content));

        public Task SendTicketClosedEmailAsync(string toEmail, int ticketNumber, string reason, string content, string adminResponse, string resolvedBy)
            => SendAsync(toEmail, $"EduPlus - odpowiedź na zgłoszenie #{ticketNumber}", EmailTemplates.TicketClosed(ticketNumber, reason, content, adminResponse, resolvedBy));

        public Task SendNewGradeEmailAsync(string toEmail, string studentName, string subjectName, string gradeValue, string teacherName, DateTime issueDate)
            => SendAsync(toEmail, $"EduPlus - nowa ocena z przedmiotu {subjectName}", EmailTemplates.NewGrade(studentName, subjectName, gradeValue, teacherName, issueDate.ToString("dd.MM.yyyy")));

        public Task SendNegativeAttendanceEmailAsync(string toEmail, string studentName, string subjectName, string attendanceType, string teacherName, DateTime date)
            => SendAsync(toEmail, $"EduPlus - nowy wpis frekwencji", EmailTemplates.NegativeAttendance(studentName, subjectName, attendanceType, teacherName, date.ToString("dd.MM.yyyy")));

        private async Task SendAsync(string toEmail, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("EduPlus", _configuration["Email:From"]));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };

            using var client = new SmtpClient();
            await client.ConnectAsync(_configuration["Email:SmtpServer"], int.Parse(_configuration["Email:SmtpPort"]!), SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_configuration["Email:Username"], _configuration["Email:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
