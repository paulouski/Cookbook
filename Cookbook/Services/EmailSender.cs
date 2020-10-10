using System.Threading.Tasks;
using Cookbook.Models.ThirdPartyServiceSettings;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Cookbook.Services
{
    public class EmailService
    {
        public MailSettings Settings { get; }

        public EmailService(MailSettings settings)
        {
            Settings = settings;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", Settings.Username));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync(Settings.Username, Settings.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
