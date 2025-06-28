using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Web.Application.Common.Constants;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Web.Application.Common.Helpers
{
    public class MailHelper
    {
        public static string SendMessage(string mailTo, string subject, string bodyMail)
        {
            string mail_host = AppConstant.MAIL_SERVER_HOST;
            string mail_user = AppConstant.MAIL_USER;
            string mail_pass = AppConstant.MAIL_PASS;
            string mail_domain = AppConstant.MAIL_DOMAIN;
            string mail_urlbase = AppConstant.MAIL_URLBASE;

            SmtpClient mail = new SmtpClient(mail_host);

            MailAddress from = new MailAddress(mail_user + "@" + mail_domain);

            MailAddress to = new MailAddress(mailTo);

            MailMessage message = new MailMessage(from, to);
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.IsBodyHtml = true;

            message.Body = bodyMail;

            message.Subject = subject;
            mail.Credentials = new System.Net.NetworkCredential(mail_user, mail_pass, mail_domain);
            try
            {
                mail.Send(message);
                return "success";
            }
            catch 
            {
                throw;
            }
        }
        public static async Task<string> SendMessage(string mailFrom, string mailTo, string subject, string bodyMail)
        {
            if (AppConstant.USING_AWS)
            {
                return SendMessageUsingAWS(mailFrom, mailTo, subject, bodyMail);

            }
            else if (AppConstant.USING_AWSSDK)
            {
                return await SendMessageUsingAWSSDK(mailFrom, mailTo, subject, bodyMail);

            }
            string mail_host = AppConstant.MAIL_SERVER_HOST;
            string mail_user = AppConstant.MAIL_USER;
            string mail_pass = AppConstant.MAIL_PASS;
            string mail_domain = AppConstant.MAIL_DOMAIN;
            string mail_urlbase = AppConstant.MAIL_URLBASE;

            SmtpClient mail = new SmtpClient(mail_host);

            MailAddress from = new MailAddress(mailFrom);

            MailAddress to = new MailAddress(mailTo);

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to);
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.IsBodyHtml = true;

            message.Body = bodyMail;

            message.Subject = subject;
            mail.Credentials = new System.Net.NetworkCredential(mail_user, mail_pass, mail_domain);
            try
            {
                mail.Send(message);
                return "success";

            }
            catch 
            {
                throw;
            }
        }
        public static string SendMessageUsingAWS(string mailFrom, string mailTo, string subject, string bodyMail)
        {
            try
            {
                String username = AppConstant.AWS_MAIL_USER; 
                String password = AppConstant.AWS_MAIL_PASS;
                String host = AppConstant.AWS_MAIL_SERVER_HOST;
                int port = 587;

                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(mailFrom);
                message.To.Add(new MailAddress(mailTo));
                message.Subject = subject;
                message.Body = bodyMail;

                // Create and configure a new SmtpClient
                SmtpClient client =
                    new SmtpClient(host, port);
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(username, password);
                // Enable SSL encryption
                client.EnableSsl = true;

                // Send the email. 
                try
                {

                    client.Send(message);
                }
                catch 
                {
                    throw;
                }
                return "success";

            }
            catch 
            {
                throw;
            }

        }
        public static async Task<string> SendMessageUsingAWSSDK(string mailFrom, string mailTo, string subject, string bodyMail)
        {
            string awsAccessKey = AppConstant.AWSSDK_ID;
            string awsSecretKey = AppConstant.AWSSDK_PASS;

            // Acceptable values are EUWest1, USEast1, and USWest2.
            using (var client = new AmazonSimpleEmailServiceClient(awsAccessKey, awsSecretKey, RegionEndpoint.USEast1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = mailFrom,
                    Destination = new Amazon.SimpleEmail.Model.Destination
                    {
                        ToAddresses =
                        new List<string> { mailTo }
                    },
                    Message = new Amazon.SimpleEmail.Model.Message
                    {
                        Subject = new Content(subject),
                        Body = new Amazon.SimpleEmail.Model.Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = bodyMail
                            }
                        }
                    },
                    // If you are not using a configuration set, comment
                    // or remove the following line 
                    //ConfigurationSetName = "LuatVietNam"
                };
                try
                {
                    Console.WriteLine("Sending email using Amazon SES...");
                    var response = await client.SendEmailAsync(sendRequest);
                    Console.WriteLine("The email was sent successfully.");
                    return "success";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + ex.Message);
                    throw;
                }
            }
        }
    }
}
