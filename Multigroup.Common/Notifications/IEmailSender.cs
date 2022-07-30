using System.IO;

namespace Multigroup.Common.Notifications
{
    public interface IEmailSender
    {
        void Send(string content, string mailTo, string subject, string ccAddress = null);

        void SendMail(string content, string[] mailTo, string subject, Stream[] attachments, string[] attachmentFilenames);

        void SendHtmlMail(string content, string[] mailTo, string[] mailCc, string subject, Stream[] attachments, string[] attachmentFilenames);
    }
}