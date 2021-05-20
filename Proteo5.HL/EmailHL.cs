using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Proteo5.HL
{
    //Credits https://gist.github.com/kdarty/772c77a9b527ef1c6dc5
    //        http://csharphelper.com/blog/2014/12/how-to-send-email-in-c/
    //        https://stackoverflow.com/questions/18358534/send-inline-image-in-email
    public class EmailHL
    {
        public static Result SendEmailAsync(EmailHLModel email)
        {
            try
            {
                // Make the mail message.
                MailAddress from_address =
                    new MailAddress(email.fromEmail, email.fromName);
                MailAddress to_address =
                    new MailAddress(email.toEmail, email.toName);
                MailMessage message =
                    new MailMessage(from_address, to_address);

                message.Subject = email.subject;

                // Check for and handle HTML based Email Body Text
                if (email.isHtml)
                {
                    message.IsBodyHtml = true;

                    // Strip out HTML Tags for Plain Text View
                    string plainTextBody = StripHTML(email.body);

                    // Create both Plain Text and HTML Alternate Views to be kind to all Email Clients
                    AlternateView plainView = AlternateView.CreateAlternateViewFromString(plainTextBody, System.Text.Encoding.UTF8, "text/plain");

                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(email.body, null, MediaTypeNames.Text.Html);

                    foreach (var item in email.LinkedResources)
                    {
                        htmlView.LinkedResources.Add(item);
                    }

                    // Add Alternate Views
                    message.AlternateViews.Add(plainView);
                    message.AlternateViews.Add(htmlView);
                }
                else
                {
                    message.Body = email.body;
                }

                // Add Attachment
                foreach (var item in email.Attachments)
                {
                    message.Attachments.Add(item);
                }

                // Get the SMTP client.
                SmtpClient smtp = new SmtpClient(email.host);
                NetworkCredential Credentials = new NetworkCredential(
                        from_address.Address, email.password);
                smtp.Credentials = Credentials;
                smtp.Port = email.port;
                smtp.EnableSsl = email.enable_ssl;
                smtp.UseDefaultCredentials = false;
                smtp.Send(message);

                return new Result(ResultsStates.success);
            }
            catch (Exception ex)
            {
                return new Result(ResultsStates.error) { Message = ex.Message, Exception = ex };
            }
        }

        private static string StripHTML(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                input = Regex.Replace(input, "<.*?>", String.Empty);
            }
            return input;
        }
    }

    public class EmailHLModel
    {
        public EmailHLModel()
        {
            this.LinkedResources = new();
            this.Attachments = new();
        }

        public string toName { get; set; }
        public string toEmail { get; set; }
        public string fromName { get; set; }
        public string fromEmail { get; set; }
        public string host { get; set; }
        public int port { get; set; }
        public bool enable_ssl { get; set; }
        public string password { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public bool isHtml { get; set; }
        public List<LinkedResource> LinkedResources { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
