using Award.Core.Interfaces;
using Award.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Helper
{
    public class EmailHelper
    {
        private IEmailSender _emailSender;
        public EmailHelper( IEmailSender emailsender)
        {
            _emailSender = emailsender;
        }
        
        public void SendCustomEmail(string[] to, string subject,string body,string Purpous)
        {
            // we will check this method according to purpous
            var message = new Message(to, subject, body);
            _emailSender.SendEmail(message);
        }
    }
}
