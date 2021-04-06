﻿using MailKit.Security;

namespace PucMinas.Services.Charity.Configuration
{
    public class EmailSenderOptions
    {
        public EmailSenderOptions()
        {
            Host_SecureSocketOptions = SecureSocketOptions.SslOnConnect;
        }
        
        public string Host_Address { get; set; }

        public int Host_Port { get; set; }

        public string Host_Username { get; set; }

        public string Host_Password { get; set; }

        public SecureSocketOptions Host_SecureSocketOptions { get; set; }

        public string Sender_EMail { get; set; }

        public string Sender_Name { get; set; }

        public string Api_Key { get; set; }
    }
}
