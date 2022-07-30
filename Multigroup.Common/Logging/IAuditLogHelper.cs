using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Common.Logging
{
    public interface IAuditLogHelper
    {
        void LogAuditFail(AuditLog param);

        void LogAuditInfo(AuditLog param);

        void LogAudit(AuditLog param, bool sendEmail);
    }
}
