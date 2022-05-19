using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Noted.Services
{
    public class EmailSender : IEmailSender
    {
        IConfiguration Configuration;
        ILogger<EmailSender> Logger;
        public EmailSender(IConfiguration configuration,ILogger<EmailSender> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string serverMail = Configuration["Mail:Address"];
            string password = Configuration["Mail:Password"];
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(serverMail);
                message.To.Add(new MailAddress(email));
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = "<html><body> " + htmlMessage + " </body></html>";
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(serverMail,password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message);
            }
            catch (Exception e) {
                Logger.LogInformation(e.Message);
            }
        }
    }
}
