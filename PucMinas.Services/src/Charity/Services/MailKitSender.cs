﻿using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using PucMinas.Services.Charity.Configuration;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Services
{
    public class MailKitEmailSender : IEmailSender
    {
        public MailKitEmailSender(IOptions<EmailSenderOptions> options)
        {
            this.Options = options.Value;
        }

        public EmailSenderOptions Options { get; set; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(email, subject, message);
        }

        public Task Execute(string to, string subject, string message)
        {
            // create message
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(Options.Sender_EMail);
            if (!string.IsNullOrEmpty(Options.Sender_Name))
                email.Sender.Name = Options.Sender_Name;
            email.From.Add(email.Sender);
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = string.Format(GetEmailTemplate(), message) };

            //send email
            using (var smtp = new SmtpClient())
            {
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                 smtp.Connect(Options.Host_Address, Options.Host_Port, Options.Host_SecureSocketOptions);
                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                //smtp.AuthenticationMechanisms.Remove("XOAUTH2");

                smtp.Authenticate(Options.Host_Username, Options.Host_Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }

            return Task.FromResult(true);
        }

        private string GetEmailTemplate()
        {
            string email = @"<html>
              <head>  
                  <meta charset=UTF-8/>  
                  <title>Doa Sonhos - Provendo a Caridade</title>        
                  <meta name=\""viewport\"" content=\""width=device-width, initial-scale=1.0\""/>
              </head>
              <body>         
               {0}
              </body>
              </html>";

            return email;

        }
    }
}