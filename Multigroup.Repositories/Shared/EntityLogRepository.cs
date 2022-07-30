using System;
using System.Data;

namespace Multigroup.Repositories.Shared
{
    public class EntityLogRepository : BaseRepository
    {
        public void Insert(DateTime date, string _action, int UserId, string EntityNew, string EntityOld, string AdditionalData)
        {
            var command = Db.GetStoredProcCommand("pub_EntityLog_Insert");

            Db.AddInParameter(command, "@date", DbType.DateTime, date);
            Db.AddInParameter(command, "@action", DbType.String, _action);
            Db.AddInParameter(command, "@userId", DbType.Int32, UserId);
            Db.AddInParameter(command, "@EntityNew", DbType.String, EntityNew);
            Db.AddInParameter(command, "@EntityOld", DbType.String, EntityOld);
            Db.AddInParameter(command, "@AdditionalData", DbType.String, AdditionalData);

            Db.ExecuteScalar(command);
        }
    }
}