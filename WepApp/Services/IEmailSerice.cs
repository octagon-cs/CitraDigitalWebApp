using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace WebApp.Services
{
    public interface IEmailService
    {
        bool SendEmail(MailRequest mail)
        {

            return false;
        }
    }



    public class EmailService
    {

    }

    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }


    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

}