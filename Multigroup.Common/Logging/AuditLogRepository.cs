using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace Multigroup.Common.Logging
{
    public class AuditLogRepository : BaseRepository, IAuditLogRepository
    {
        public int Save(string integrationType, string inputParam, string response, string description, string logLevel, int? referenceId)
        {
            var command = Db.GetStoredProcCommand("pub_AuditLog_Put");

            Db.AddInParameter(command, "@Type", DbType.String, integrationType);
            Db.AddInParameter(command, "@InputParam", DbType.String, inputParam);
            Db.AddInParameter(command, "@Response", DbType.String, response);
            Db.AddInParameter(command, "@Description", DbType.String, description);
            Db.AddInParameter(command, "@LogLevel", DbType.String, logLevel);
            Db.AddInParameter(command, "@ReferenceId", DbType.Int32, referenceId);

            return Convert.ToInt32(Db.ExecuteScalar(command));
        }

        public ConfigurationParam GetByKey(string key)
        {
            var configurationParam = Db.ExecuteSprocAccessor("pub_AuditLogParam_Get", ConfigurationParamMapper.Mapper, key);
            return configurationParam.FirstOrDefault();
        }
    }
}
