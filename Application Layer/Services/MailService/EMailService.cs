using Domain_Layer.DTOs;
using Domain_Layer.Interfaces.ServiceInterfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Services.MailService
{
    public class EMailService : IEMailService
    {
        private readonly MailSettings options;

        public EMailService(IOptions<MailSettings> options)
        {
            this.options = options.Value; 
        }
        public async Task SendEmail(EmailDTo Email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(options.EMail),
                Subject = Email.Subject,
            };
            mail.To.Add(MailboxAddress.Parse(Email.To));
            mail.From.Add(new MailboxAddress(options.DisplayName, options.EMail));

            var builder = new BodyBuilder();

            builder.TextBody = Email.Body;

            mail.Body = builder.ToMessageBody();

            using var stmp = new SmtpClient();

           await stmp.ConnectAsync(options.Host, options.Port, SecureSocketOptions.StartTls);
            await stmp.AuthenticateAsync(options.EMail, options.Password);

            await stmp.SendAsync(mail);

            await stmp.DisconnectAsync(true);




        }
    }
}
