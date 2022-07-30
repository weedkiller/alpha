using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Common.Logging
{
    public interface IAuditLogRepository
    {
        int Save(string integrationType, string inputParam, string response, string description, string resultKind, int? referenceId);
        ConfigurationParam GetByKey(string key);
    }
}
