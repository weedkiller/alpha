using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Common.Logging
{
    public class AuditLog
    {
        public AuditLogType IntegrationType { get; set; }
        public object InputParam { get; set; }
        public object Response { get; set; }
        public string Description { get; set; }
        public AuditLogLevel LogLevel { get; set; }
        public int? ReferenceId { get; set; }
    }
}
