using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace Multigroup.Common.Notifications
{
    public class EmailSender : IEmailSender
    {
        public void Send(string content, string mailTo, string subject, string ccAddress = null)
        {
                var from = ConfigurationManager.AppSettings["SMTPmailSender"];
                var name = ConfigurationManager.AppSettings["SMTPSenderName"];

                MailMessage msg = new MailMessage();
                msg.To.Add(mailTo);
                msg.From = new MailAddress(from, name, System.Text.Encoding.UTF8);
                msg.Subject = subject;
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.Body = content;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                //msg.IsBodyHtml = false;

                var smtpClient = GetSmtpClient();

                smtpClient.Send(msg);
        }

        public void SendMail(string content, string[] mailTo, string subject, Stream[] attachments, string[] attachmentFilenames)
        {
            SendMailTask(content, mailTo, subject, attachments, attachmentFilenames);
            //var writeLog = new Task(() => SendMailTask(content, mailTo, subject, attachments, attachmentFilenames));
            //writeLog.Start();
        }

        public void SendHtmlMail(string content, string[] mailTo, string[] mailCc, string subject, Stream[] attachments, string[] attachmentFilenames)
        {
            var smtpClient = GetSmtpClient();
            var mailMessage = SetMessageFields(mailTo, mailCc, subject, content, attachments, attachmentFilenames, true);

            smtpClient.Send(mailMessage);
        }

        private void SendMailTask(string content, string[] mailTo, string subject, Stream[] attachments, string[] attachmentFilenames)
        {
            var smtpClient = GetSmtpClient();
            var mailMessage = SetMessageFields(mailTo, null, subject, content, attachments, attachmentFilenames, false);

            smtpClient.Send(mailMessage);
        }

        private static SmtpClient GetSmtpClient()
        {
            var smtpServer = ConfigurationManager.AppSettings["SMTP"];
            var timeOut = ConfigurationManager.AppSettings["SMTPServerTimeOut"];
            var port =  ConfigurationManager.AppSettings["SMTPPort"];
            var enableSsl = ConfigurationManager.AppSettings["SMTPEnableSsl"];
            var user = ConfigurationManager.AppSettings["SMTPUserAccount"];
            var password = ConfigurationManager.AppSettings["SMTPPasswordAccount"];

            return new SmtpClient
                       {
                           Credentials = new System.Net.NetworkCredential(user, password),
                           Port = int.Parse(port),
                           Host = smtpServer,
                           Timeout = int.Parse(timeOut) * 60 * 1000,
                           EnableSsl = bool.Parse(enableSsl)
                       };
        }

        private static MailMessage SetMessageFields(string[] mailTo, string[] mailCc, string subject, string content, Stream[] attachments, string[] attachmentFilenames, bool htmlBody)
        {
            var mailMessage = new MailMessage();
            var fromAddress = new MailAddress(ConfigurationManager.AppSettings["SMTPSenderAddress"], ConfigurationManager.AppSettings["SMTPSenderName"]);

            mailMessage.From = fromAddress;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = htmlBody;
            mailMessage.Body = content;

            foreach (var to in mailTo)
            {
                mailMessage.To.Add(to);
            }

            if (mailCc != null)
            {
                foreach (var cc in mailCc)
                {
                    if (!string.IsNullOrEmpty(cc))
                    {
                        mailMessage.CC.Add(cc);
                    }
                }
            }

            if (attachments != null)
            {
                var i = 0;
                foreach (var attachmentStream in attachments)
                {
                    var attachment = new Attachment(attachmentStream, MediaTypeNames.Application.Octet);
                    var disposition = attachment.ContentDisposition;

                    disposition.CreationDate = new DateTime();
                    disposition.ModificationDate = new DateTime();
                    disposition.ReadDate = new DateTime();
                    disposition.Size = attachmentStream.Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    disposition.FileName = attachmentFilenames[i++];
                    mailMessage.Attachments.Add(attachment);
                }
            }

            return mailMessage;
        }
    }
}