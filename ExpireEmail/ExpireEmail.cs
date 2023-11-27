using System;
using System.IO;
using System.Net.Mail;
using System.Net;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using _23._1News.Models.Db;

namespace ExpireEmail
{
    public class ExpireEmail
    {
        [FunctionName("ExpireEmail")]
        public void Run([QueueTrigger("newsletterqueue", Connection = "AzureWebJobsStorage")] User user, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {user.Email}");

            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("local.settings,json", true, true)
               .AddEnvironmentVariables()
               .Build();

            MailMessage mailMessage = new();
            SmtpClient smtpClient = new SmtpClient(configuration["Host"]);
            int port = int.Parse(configuration["port"]);

            try
            {
                mailMessage.From = new MailAddress(configuration["EmailAddress"],
                    configuration["23.1News"]);
                mailMessage.To.Add(user.Email);
                mailMessage.Subject = "Your subscription will expire soon";
                mailMessage.Body = "<p>On"
                    + DateTime.Now.AddDays(2).ToShortDateString()
                    + "Your subscription will expire in two days. Continue to enjoy reading our news by subscribing again </p> ";
                mailMessage.IsBodyHtml = true;

                smtpClient.Port = port;
                smtpClient.Credentials =
                    new NetworkCredential(configuration["Server"], configuration["Password"]);
                smtpClient.Send(mailMessage);

               

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
