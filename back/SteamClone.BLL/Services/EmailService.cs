using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SteamClone.BLL.Settings;

namespace SteamClone.BLL.Services
{
    public class EmailService : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> options, ILogger<EmailService> logger)
        {
            _logger = logger;

            var emailSettings = options.Value;

            _fromEmail = emailSettings.FromEmail;

            _smtpClient = new SmtpClient(emailSettings.Host, emailSettings.Port)
            {
                Credentials = new NetworkCredential(emailSettings.FromEmail, emailSettings.Password),
                EnableSsl = emailSettings.EnableSsl
            };
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                    From = new MailAddress(_fromEmail)
                };

                mailMessage.To.Add(email);

                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception)
            {
                _logger.LogInformation("Enter password and email for smtp client");
            }
        }
    }
}
