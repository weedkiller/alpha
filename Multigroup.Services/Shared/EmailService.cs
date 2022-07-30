using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Services.Shared
{
    public class EmailService
    {
        SmtpClient smtpClient;

        public EmailService()
        {
        }

        

        public void SendEmail(string To, string Subject, string Body, bool isHtml, Attachment attachment)
        {
            BuildDefaultClient();

            MailMessage mail = getDefaultMessage();
            mail.To.Add(To);
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = isHtml;
            if (attachment != null)
            {
                mail.Attachments.Add(attachment);
            }

            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("No se ha podido enviar el email");
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                smtpClient.Dispose();
            }

        }

        public void SendEmailSeller(string To, string Subject, string Body, bool isHtml, Attachment attachment, List<Attachment> attachments)
        {
            BuildDefaultClient();

            MailMessage mail = getDefaultMessage();
            mail.To.Add(To);
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = isHtml;
            if (attachment != null)
            {
                mail.Attachments.Add(attachment);
            }
            foreach(Attachment doc in attachments)            
            {
                mail.Attachments.Add(doc);
            }


            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("No se ha podido enviar el email");
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                smtpClient.Dispose();
            }

        }

        private MailMessage getDefaultMessage()
        {
            return  new System.Net.Mail.MailMessage
            {
                From = new MailAddress("noreply@adm.concesionariamg.com")
            };
        }

        //private void BuildDefaultClient()
        //{
        //        smtpClient = new SmtpClient()
        //        {
        //            Host = "smtp.gmail.com",
        //            Port = 587,
        //            UseDefaultCredentials = false,
        //            Credentials = new NetworkCredential("NoReply.concesionariamg@gmail.com", "Ingarx13"),
        //            EnableSsl = true
        //        };
        //}

        private void BuildDefaultClient()
        {
            smtpClient = new SmtpClient()
            {
                Host = "smtp-relay.sendinblue.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("noreply.mgconcesionaria@gmail.com", "yEINx5GdLB4DOgb0"),
                EnableSsl = true
            };
        }

    }
}
