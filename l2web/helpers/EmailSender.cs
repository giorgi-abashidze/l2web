using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace l2web.helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration config;
        public EmailSender(IConfiguration _config) {
            config = _config;
        }
        
        public async Task SendEmailAsync(string _email, string _subject, string _message)
        {
            var apiKey = config.GetValue<string>("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(config.GetValue<string>("EMAIL"));
            var subject = _subject;
            var to = new EmailAddress(_email);
            string plainTextContent = null;
            var htmlContent = _message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        
          
        }
    }
}
