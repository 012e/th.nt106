using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace Whut
{

    public class EmailSender
    {
        /// <summary>
        /// Sends an email using Gmail's SMTP server with an App Password, including attachments.
        /// </summary>
        /// <param name="fromEmail">Your full Gmail address.</param>
        /// <param name="appPassword">The 16-digit App Password generated for your application.</param>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The email subject.</param>
        /// <param name="body">The email body.</param>
        /// <param name="isBodyHtml">Set to true if the body contains HTML.</param>
        /// <param name="attachmentPaths">A list of file paths for the attachments.</param>
        public static void SendGmailEmail(string fromEmail, string appPassword, string toEmail, string subject, string body, bool isBodyHtml = false, List<string> attachmentPaths = null)
        {
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(fromEmail, appPassword);
                smtpClient.Timeout = 20000; // Set a reasonable timeout in milliseconds

                using (MailMessage mailMessage = new MailMessage(fromEmail, toEmail, subject, body))
                {
                    mailMessage.IsBodyHtml = isBodyHtml;

                    // Add attachments
                    List<System.Net.Mail.Attachment> attachments = new List<System.Net.Mail.Attachment>();
                    if (attachmentPaths != null)
                    {
                        foreach (string attachmentPath in attachmentPaths)
                        {
                            if (File.Exists(attachmentPath))
                            {
                                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(attachmentPath);
                                mailMessage.Attachments.Add(attachment);
                                attachments.Add(attachment); // Keep track for disposing
                            }
                            else
                            {
                                throw new Exception($"File {attachmentPath} not found.");
                            }
                        }
                    }

                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Email sent successfully!");
                }
            }
        }

        /// <summary>
        /// Sends an email using Gmail's SMTP server with an App Password asynchronously, including attachments.
        /// </summary>
        /// <param name="fromEmail">Your full Gmail address.</param>
        /// <param name="appPassword">The 16-digit App Password generated for your application.</param>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The email subject.</param>
        /// <param name="body">The email body.</param>
        /// <param name="isBodyHtml">Set to true if the body contains HTML.</param>
        /// <param name="attachmentPaths">A list of file paths for the attachments.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static async Task SendGmailEmailAsync(string fromEmail, string appPassword, string toEmail, string subject, string body, bool isBodyHtml = false, List<string> attachmentPaths = null)
        {
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(fromEmail, appPassword);
                smtpClient.Timeout = 20000; // Set a reasonable timeout in milliseconds

                using (MailMessage mailMessage = new MailMessage(fromEmail, toEmail, subject, body))
                {
                    mailMessage.IsBodyHtml = isBodyHtml;

                    // Add attachments
                    List<System.Net.Mail.Attachment> attachments = new List<System.Net.Mail.Attachment>();
                    if (attachmentPaths != null)
                    {
                        foreach (string attachmentPath in attachmentPaths)
                        {
                            if (File.Exists(attachmentPath))
                            {
                                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(attachmentPath);
                                mailMessage.Attachments.Add(attachment);
                                attachments.Add(attachment); // Keep track for disposing
                            }
                            else
                            {
                                throw new Exception($"File {attachmentPath} not found.");
                            }
                        }
                    }

                    await smtpClient.SendMailAsync(mailMessage);
                    Console.WriteLine("Email sent successfully (async)!");
                }
            }
        }
    }
}
