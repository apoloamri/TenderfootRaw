using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Tenderfoot.Database;
using Tenderfoot.TfSystem;
using Tenderfoot.TfSystem.Diagnostics;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Net
{
    public static class TfEmail
    {
        public static void Send(string mailTo, string emailFile, Dictionary<string, object> items)
        {
            try
            {
                var emailContent = ReadEmailFromFile(emailFile, items);
                SendBase(mailTo, emailContent.Title, emailContent.Message);
            }
            catch (Exception ex) when (!TfSettings.System.Debug)
            {
                TfDebug.WriteLog(ex);
            }
        }

        private static void SendBase(string mailTo, string title, string message)
        {
            var mailFrom = TfSettings.Web.SmtpEmail;
            var mail = new MailMessage(mailFrom, mailTo) { IsBodyHtml = true };

            var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = TfSettings.Web.SmtpHost,
                Port = TfSettings.Web.SmtpPort,
                Credentials = new NetworkCredential(mailFrom, TfSettings.Web.SmtpPassword)
            };
            
            mail.Subject = title;
            mail.Body = message;

            client.Send(mail);

            var emails = Schemas.Emails;
            emails.Entity.mail_from = mailFrom;
            emails.Entity.mail_to = mailTo;
            emails.Entity.mail_title = title;
            emails.Entity.mail_body = message;
            emails.Insert();
        }

        private static EmailContent ReadEmailFromFile(string fileName, Dictionary<string, object> items)
        {
            var message = File.ReadAllText(Path.Combine(TfSettings.SystemResources.EmailFiles, fileName + ".html"));
            message = message.Replace("{url}", TfSettings.Web.SiteUrl);
            message = message.Replace("{api_url}", TfSettings.Web.ApiUrl);
            message = message.FormatFromDictionary(items);

            var match = Regex.Match(message, @"<title>\s*(.+?)\s*</title>");
            var title = string.Empty;

            if (match.Success)
            {
                title = match.Groups[1].Value;
            }

            return new EmailContent()
            {
                Title = title,
                Message = message
            };
        }

        private class EmailContent
        {
            public string Title { get; set; }
            public string Message { get; set; }
        }
    }
}
