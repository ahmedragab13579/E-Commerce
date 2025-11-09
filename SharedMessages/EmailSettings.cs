using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedMessages
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int Port          { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail{ get; set; }
        public string Password { get; set; }



        private EmailSettings()
        {
            
        }


        public EmailSettings(string SmtpServer, int Port, string SenderName, string SenderEmail, string Password)
        {
            if (string.IsNullOrEmpty(SmtpServer))
                throw new ArgumentNullException(nameof(SmtpServer));
            if (string.IsNullOrEmpty(SenderName))
                throw new ArgumentNullException(nameof(SenderName));
            if (string.IsNullOrEmpty(SenderEmail))
                throw new ArgumentNullException(nameof(SenderEmail));
            if(string.IsNullOrEmpty(Password))
                throw new ArgumentNullException(nameof(Password));
            if (Port <= 0)
                throw new ArgumentOutOfRangeException(nameof(Port), "Port must be greater than zero.");
            this.SmtpServer  = SmtpServer;
            this.Port        = Port;
            this.SenderName  = SenderName;
            this.SenderEmail = SenderEmail;
            this.Password    = Password;

        }
    }
}
