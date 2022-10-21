using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Mailjet.Client;
using Mailjet.Client.Resources;

namespace WebApp.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(MailRequest mail);
    }



    public class EmailService : IEmailService
    {
         public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task<bool> SendEmail(MailRequest mailRequest)
        {
            try
            {
                IConfigurationSection emailConfiguration = Configuration.GetSection("MailSettings");
                MailjetClient client = new MailjetClient(emailConfiguration["Mail"], emailConfiguration["Password"]);
                MailjetRequest request = new MailjetRequest
                {
                    Resource = SendV31.Resource,
                }
                 .Property(Send.Messages,
                    new JArray {
                         new JObject {
                          {
                           "From",
                           new JObject {
                            {"Email", emailConfiguration["Email"]},
                            {"Name", emailConfiguration["DisplayName"]}
                           }
                          }, {
                           "To",
                           new JArray {
                            new JObject {
                             {
                              "Email",
                              mailRequest.ToEmail
                             }, {
                              "Name",
                              mailRequest.ToEmail
                             }
                            }
                           }
                          }, {
                           "Subject",
                           mailRequest.Subject
                          }, {
                           "TextPart",
                           "None"
                          }, {
                           "HTMLPart",
                           mailRequest.Body
                          }, {
                           "CustomID",
                           "AppGettingStartedTest"
                          }
                         }
                 });
                MailjetResponse response = await client.PostAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                    Debug.WriteLine(response.GetData());
                }
                else
                {
                    Debug.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                    Debug.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                    Debug.WriteLine(response.GetData());
                    Debug.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
                }

                return true;

            }
            catch (System.Exception ex)
            {
                Debug.WriteLine("Exception caught in CreateTestMessage2(): {0}", ex.ToString());
                return false;
            }
        }
    }

    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        //public List<IFormFile> Attachments { get; set; }
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