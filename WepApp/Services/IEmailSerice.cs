using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace WebApp.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(MailRequest mail);
    }



    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public Task<bool> SendEmail(MailRequest mailRequest)
        {
          try
          {

              string email = mailRequest.ToEmail;
              string subject =mailRequest.Subject;
              string htmlMessage =mailRequest.Body;
                IConfigurationSection emailConfiguration =
                 Configuration.GetSection("MailSettings");
                var from = emailConfiguration["Mail"].ToString();
                MailMessage message = new MailMessage(from, email);
                message.Subject = subject;
                message.Body = htmlMessage;
                message.IsBodyHtml = true;

                message.From = new MailAddress(from, emailConfiguration["DisplayName"]);
                SmtpClient smtp = new SmtpClient(emailConfiguration["HOST"], Convert.ToInt32(emailConfiguration["Port"]));
                smtp.Credentials = new NetworkCredential(from, emailConfiguration["Password"]);
                smtp.EnableSsl = true;
                smtp.Send(message);



                return Task.FromResult(true);
          }
          catch (System.Exception ex)
          {
              
              throw new SystemException(ex.Message);
          }
        }
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