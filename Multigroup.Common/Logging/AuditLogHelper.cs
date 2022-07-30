using Multigroup.Common.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Common.Logging
{
    public class AuditLogHelper : IAuditLogHelper
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IEmailSender _emailSender;

        public AuditLogHelper()
        {
            _auditLogRepository = new AuditLogRepository();
            _emailSender = new EmailSender();
        }

        /// <summary>
        /// Logger Constructor
        /// </summary>
        /// <param name="auditLogRepository"></param>
        /// <param name="emailSender"></param>
        public AuditLogHelper(IAuditLogRepository auditLogRepository, IEmailSender emailSender)
        {
            _auditLogRepository = auditLogRepository;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Write a exception log in database and send email.
        /// </summary>
        /// <param name="param">IntegrationType = Sap, Hro, etc</param>
        /// <param name="param">InputParam = parameters (object)</param>
        /// <param name="param">Response = Response, exception (object)</param>
        /// <param name="param">Description = Dev explanation of error or any other usefull information</param>
        /// <param name="param">LogLevel = Error</param>
        /// <param name="param">ReferenceId = Id value for example JobProfileId</param>
        public void LogAuditFail(AuditLog param)
        {
            param.LogLevel = AuditLogLevel.Error;
            var writeLog = new Task(() => WriteLog(param, true));
            //WriteLog(param, true);
            writeLog.Start();
        }

        /// <summary>
        /// Write a exception log in database without send email.
        /// </summary>
        /// <param name="param">IntegrationType = Sap, Hro, etc</param>
        /// <param name="param">InputParam = parameters (object)</param>
        /// <param name="param">Response = Response, exception (object)</param>
        /// <param name="param">Description = Any other usefull information</param>
        /// <param name="param">LogLevel = Info</param>
        /// <param name="param">ReferenceId = Id value for example JobProfileId</param>
        public void LogAuditInfo(AuditLog param)
        {
            param.LogLevel = AuditLogLevel.Info;
            var writeLog = new Task(() => WriteLog(param, false));
            writeLog.Start();
        }

        /// <summary>
        /// Write a exception log in database with the option of send email.
        /// </summary>
        /// <param name="param">IntegrationType = Sap, Hro, etc</param>
        /// <param name="param">InputParam = parameters (object)</param>
        /// <param name="param">Response = Response, exception (object)</param>
        /// <param name="param">Description = Any other usefull information</param>
        /// <param name="param">LogLevel = Info or Error</param>
        /// <param name="param">ReferenceId = Id value for example JobProfileId</param>
        /// <param name="param">sendEmail = true send email, false not send email</param>
        public void LogAudit(AuditLog param, bool sendEmail)
        {
            var writeLog = new Task(() => WriteLog(param, sendEmail));
            writeLog.Start();
        }

        private void WriteLog(AuditLog param, bool sendEmail)
        {
            var logLevel = _auditLogRepository.GetByKey("AuditLogLevel").Value;
            var value = (AuditLogLevel)Enum.Parse(typeof(AuditLogLevel), logLevel);
            int level = (int)value;

            if ((int)param.LogLevel >= level)
            {
                param.InputParam = param.InputParam != null ? JsonConvert.SerializeObject(param.InputParam) : string.Empty;
                param.Response = param.Response != null ? JsonConvert.SerializeObject(param.Response) : string.Empty;

                var auditLodId = _auditLogRepository.Save(param.IntegrationType.ToString(), param.InputParam != null ? param.InputParam.ToString() : string.Empty, param.Response != null ? param.Response.ToString() : string.Empty, param.Description, param.LogLevel.ToString(), param.ReferenceId);

                //if (sendEmail)
                //{
                //    string mailTo = _auditLogRepository.GetByKey("AuditLogEmail").Value;

                //    _emailSender.SendMail(string.Concat("Id IntegrationAuditLog Table: ", auditLodId, "\n\nInput Paramater: ", param.InputParam, "\n\nError: ", param.Response), mailTo.Split(';'), string.Concat("An error occurred in ", param.IntegrationType.ToString()), null, null);
                //}
            }
        }
    }
}
