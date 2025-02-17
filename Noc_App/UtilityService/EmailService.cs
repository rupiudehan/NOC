
using MimeKit;
using MailKit.Net.Smtp;
using Noc_App.Models;
using MailKit.Security;
using System.Security.Authentication;

namespace Noc_App.UtilityService
{
    //public class EmailService
    //{
    //    public async Task SendEmailAsync(string toEmail, string subject, string body)
    //    {
    //        var smtpClient = new SmtpClient
    //        {
    //            Host = "your-smtp-server.com", // Replace with your SMTP server
    //            Port = 587,                     // Replace with your SMTP port
    //            Credentials = new NetworkCredential("your-email@example.com", "your-password"), // Replace with your email credentials
    //            EnableSsl = true
    //        };

    //        var mailMessage = new MailMessage
    //        {
    //            From = new MailAddress("your-email@example.com"), // Replace with your email
    //            Subject = subject,
    //            Body = body,
    //            IsBodyHtml = true
    //        };

    //        mailMessage.To.Add(toEmail);

    //        await smtpClient.SendMailAsync(mailMessage);
    //    }
    //}
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public int SendEmail2(EmailModel emailModel, string subject = "Reset Password")
        {
            var emailMessage = new MimeMessage();
            var from = _config["EmailSettings:From"];
            emailMessage.From.Add(new MailboxAddress(System.Text.Encoding.UTF8, subject, from));
            emailMessage.To.Add(new MailboxAddress(emailModel.To, emailModel.To));
            emailMessage.Subject = emailModel.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(emailModel.Content)
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.CheckCertificateRevocation = false;
                    // Allow SSLv3.0 and all versions of TLS
                    client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                    client.Connect(_config["EmailSettings:SmtpServer"],
                       Convert.ToInt32(_config["EmailSettings:Port"]),
                       SecureSocketOptions.Auto);

                    //client.Connect(_config["EmailSettings:SmtpServer"], Convert.ToInt32(_config["EmailSettings:Port"]), true);
                    client.Authenticate(_config["EmailSettings:From"], _config["EmailSettings:Password"]);
                    client.Send(emailMessage);
                    return 1;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }
        public void SendEmail(EmailModel emailModel,string subject= "Reset Password")
        {
            var emailMessage = new MimeMessage();
            var from = _config["EmailSettings:From"];
            emailMessage.From.Add(new MailboxAddress( System.Text.Encoding.UTF8, subject, from));
            emailMessage.To.Add(new MailboxAddress(emailModel.To, emailModel.To));
            emailMessage.Subject = emailModel.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(emailModel.Content)
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.CheckCertificateRevocation = false;
                    // Allow SSLv3.0 and all versions of TLS
                    client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                    client.Connect(_config["EmailSettings:SmtpServer"],
                       Convert.ToInt32(_config["EmailSettings:Port"]),
                       SecureSocketOptions.Auto);

                    //client.Connect(_config["EmailSettings:SmtpServer"], Convert.ToInt32(_config["EmailSettings:Port"]), true);
                    client.Authenticate(_config["EmailSettings:From"], _config["EmailSettings:Password"]);
                    client.Send(emailMessage);

                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    //client.Dispose();

                }
            }
        }
    }
}
