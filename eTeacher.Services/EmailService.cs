using Repositories;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace eTeacher.Core.Services
{
    using Repositories;
    using System.Net.Mail;
    using System.Net;

    namespace eTeacher.Core.Services
    {
        public class EmailService : IEmailService
        {
            private readonly SmtpClient _smtpClient;
            private readonly string _fromEmail;

            public EmailService(IConfiguration configuration)
            {
            var smtpHost = configuration["Smtp:Host"];
            var smtpPort = int.Parse(configuration["Smtp:Port"]);
            var smtpUser = configuration["Smtp:User"];
            var smtpPass = configuration["Smtp:Pass"];
            _fromEmail = configuration["Smtp:FromEmail"];

                _smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };
                _fromEmail = _fromEmail;
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



}
