using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared.Mappers;
using Multigroup.Repositories.Shared.Resources;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;

using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared
{
    public class UserTypeRepositorio : BaseRepository
    {
        public UserType GetUserTypeById(int UserTypeId)
        {
            var data = Db.ExecuteSprocAccessor("pub_UserType_Get", UserTypeMapper.Mapper, UserTypeId);

            return GetFirstElement(data);
        }

        public IEnumerable<UserType> GetUserTypes()
        {
            var data = Db.ExecuteSprocAccessor("pub_UserType_GetList", UserTypeMapper.Mapper);

            return data;
        }


        public int InsertUserType(UserType UserType)
        {
            var command = Db.GetStoredProcCommand("pub_UserType_Insert");

            Db.AddInParameter(command, "@Commission", DbType.String, UserType.Commission);
            Db.AddInParameter(command, "@Description", DbType.String, UserType.Description);
            Db.AddInParameter(command, "@IsCommission", DbType.String, UserType.IsCommission);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }
        public void DeleteUserType(int UserTypeId)
        {
            var command = Db.GetStoredProcCommand("pub_UserType_Delete");

            Db.AddInParameter(command, "@UserTypeId", DbType.Int32, UserTypeId);

            Db.ExecuteScalar(command);
        }

        public void UpdateUserType(UserType UserType)
        {
            var command = Db.GetStoredProcCommand("pub_UserType_Update");

            Db.AddInParameter(command, "@UserTypeId", DbType.Int32, UserType.UserTypeId);
            Db.AddInParameter(command, "@Commission", DbType.String, UserType.Commission);
            Db.AddInParameter(command, "@Description", DbType.String, UserType.Description);
            Db.AddInParameter(command, "@IsCommission", DbType.String, UserType.IsCommission);

            Db.ExecuteScalar(command);
        }



    }
}
