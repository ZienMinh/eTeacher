using eTeacher.Core.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace eTeacher.Core.Services
{

    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;

        public EmailService(string smtpHost, int smtpPort, string fromEmail, string smtpUser, string smtpPass)
        {
            _smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };
            _fromEmail = fromEmail;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }

}
