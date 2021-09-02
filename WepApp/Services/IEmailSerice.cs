using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
//using MailKit.Net.Smtp;
//using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
//using MimeKit;


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
            var email = new System.Net.Mail.MailMessage();
            //email.Sender = new System.Net.Mail.MailAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            email.To.Add(mailRequest.ToEmail);
            email.Subject = mailRequest.Subject;
                //var builder = new BodyBuilder();
                //if (mailRequest.Attachments != null)
                //{
                //    byte[] fileBytes;
                //    foreach (var file in mailRequest.Attachments)
                //    {
                //        if (file.Length > 0)
                //        {
                //            using (var ms = new MemoryStream())
                //            {
                //                file.CopyTo(ms);
                //                fileBytes = ms.ToArray();
                //            }
                //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                //        }
                //    }
                //}
                //builder.HtmlBody = mailRequest.Body;
                //email.Sender.Name=_mailSettings.DisplayName;
                //email.Body = builder.ToMessageBody();
                email.From = new MailAddress(_mailSettings.Mail, "Pertamina KIM");
                email.Body = mailRequest.Body;
                 using var smtp = new System.Net.Mail.SmtpClient( _mailSettings.Host, _mailSettings.Port);

                smtp.EnableSsl = true;

                //smtp.UseDefaultCredentials = false;

                smtp.Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
                // =========================================================

                smtp.Send(email);



                //smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                //smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                //await smtp.SendAsync(email);
                //smtp.Disconnect(true);
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