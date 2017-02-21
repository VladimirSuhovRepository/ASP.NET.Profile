using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Profile.DAL.Identity
{
    public class EmailService : IIdentityMessageService
    {
        private string _userName;
        private string _password;

        private string _smtpHost;
        private int _smtpPort;

        public EmailService(
            string userName, 
            string password, 
            string smtpHost, 
            int smtpPort)
        {
            _userName = userName;
            _password = password;
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
        }

        public Task SendAsync(IdentityMessage message)
        {
            var client = new SmtpClient
            {
                Host = _smtpHost,
                Port = _smtpPort,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_userName, _password),
                EnableSsl = true
            };

            var mail = new MailMessage(_userName, message.Destination);

            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;

            return client.SendMailAsync(mail);
        }
    }
}
