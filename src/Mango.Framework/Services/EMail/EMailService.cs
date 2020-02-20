using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace Mango.Framework.Services.EMail
{
    public class EMailService:IEMailService
    {
        private EMailOptions _options;
        public EMailService(EMailOptions options)
        {
            _options = options;
        }
        public async Task<bool> SendEmail(string email, string subject, string message)
        {
            bool sendResult = false;
            try
            {
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_options.FromEMail, _options.FromName);
                mailMessage.To.Add(new MailAddress(email));
                mailMessage.Subject = subject;
                mailMessage.Body = message;

                using var client = new SmtpClient();
                client.Host = _options.SmtpServerUrl;
                client.Port = _options.SmtpServerPort;
                client.EnableSsl = false;
                
                client.Credentials = new NetworkCredential(_options.SmtpAuthenticateEmail, _options.SmtpAuthenticatePasswordText);
                await client.SendMailAsync(mailMessage);
                sendResult = true;
            }
            catch
            {
                sendResult = false;
            }
            return sendResult;
        }
    }
}
