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
                string to = mailRequest.ToEmail;
                string from = _mailSettings.Mail;
               // MailMessage message = new MailMessage(addressFrom, addressTo);
                MailMessage message = new MailMessage(from, to);
                message.Subject = mailRequest.Subject;

              //  var builder = new BodyBuilder();
                if (mailRequest.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in mailRequest.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            message.Attachments.Add(new Attachment(ms, file.FileName, file.ContentType));
                            }
                        }
                    }
                }
                message.Body = mailRequest.Body;
                message.IsBodyHtml = true;
                message.From = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
                SmtpClient smtp = new SmtpClient(_mailSettings.Host,_mailSettings.Port);
                smtp.Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
                smtp.EnableSsl = true; 
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                //try
                //{
                //    smtp.Send(message);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                //        ex.ToString());
                //}

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