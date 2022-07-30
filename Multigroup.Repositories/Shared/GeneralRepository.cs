
using Multigroup.DomainModel.Shared;
using System.Data;
namespace Multigroup.Repositories.Shared
{
    public class GeneralRepository : BaseRepository
    {
        public void InsertComponentStatusDataLog(int? SiteId)
        {
            var command = Db.GetStoredProcCommand("pub_ComponentStatusDataLog_Insert");

            Db.AddInParameter(command, "@SiteId", DbType.Int32, SiteId);
            Db.ExecuteScalar(command);
        }

        public void InsertRoutineStatusDataLog(int? SiteId)
        {
            var command = Db.GetStoredProcCommand("pub_RoutineStatusDataLog_Insert");

            Db.AddInParameter(command, "@SiteId", DbType.Int32, SiteId);
            Db.ExecuteScalar(command);
        }

        public void InsertNavigationLog(NavigationLog _navigation)
        {
            var command = Db.GetStoredProcCommand("pub_NavigationLog_Insert");

            Db.AddInParameter(command, "@UserId", DbType.Int32, _navigation.UserId);
            Db.AddInParameter(command, "@ControllerName", DbType.String, _navigation.ControllerName);
            Db.AddInParameter(command, "@ControllerFullName", DbType.String, _navigation.ControllerFullName);
            Db.AddInParameter(command, "@ControllerNameSpace", DbType.String, _navigation.ControllerNameSpace);
            Db.AddInParameter(command, "@ActionName", DbType.String, _navigation.ActionName);
            Db.AddInParameter(command, "@ActionDescription", DbType.String, _navigation.ActionDescription);
            Db.AddInParameter(command, "@Parameters", DbType.String, _navigation.Parameters);
            Db.AddInParameter(command, "@IP", DbType.String, _navigation.IP);
            Db.AddInParameter(command, "@HttpContextTimestamp", DbType.DateTime, _navigation.HttpContextTimestamp);
            Db.AddInParameter(command, "@SessionId", DbType.String, _navigation.SessionId);
            Db.ExecuteScalar(command);
        }

    }
}
