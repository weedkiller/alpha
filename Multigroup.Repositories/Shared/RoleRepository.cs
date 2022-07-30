using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared.Mappers;
using Multigroup.Repositories.Shared.Resources;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Multigroup.Repositories.Shared
{
    public class RoleRepository : BaseRepository
    {
        public Role GetRoleById(int roleId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Roles_Get", RoleMapper.Mapper, roleId);

            return GetFirstElement(data);
        }

        public IEnumerable<Role> GetRolesByUserId(int userId)
        {
            var data = Db.ExecuteSprocAccessor("pub_UserRoles_GetList", RoleMapper.Mapper, userId);

            return data;
        }

        public IEnumerable<Role> GetRoles()
        {
            var data = Db.ExecuteSprocAccessor("pub_Roles_GetList", RoleMapper.Mapper);

            return data;
        }

        public IEnumerable<Role> GetRolesByRol(int roleId)
        {
            var data = Db.ExecuteSprocAccessor("pub_RolesByRol_GetList", RoleMapper.Mapper, roleId);

            return data;
        }

        public void Insert(int roleId, int userId)
        {
            var command = Db.GetStoredProcCommand("pub_UserRoles_Insert");

            Db.AddInParameter(command, "@RoleId", DbType.Int32, roleId);
            Db.AddInParameter(command, "@UserId", DbType.Int32, userId);

            Db.ExecuteScalar(command);
        }

        public void Delete(int userId)
        {
            var command = Db.GetStoredProcCommand("pub_UserRoles_Delete");
            Db.AddInParameter(command, "@UserId", DbType.Int32, userId);
            Db.ExecuteScalar(command);
        }

        public void AddRole(Role role)
        {
            var command = Db.GetStoredProcCommand("pub_Roles_Insert");

            Db.AddInParameter(command, "@Name", DbType.String, role.Name);
            Db.AddInParameter(command, "@Description", DbType.String, role.Description);
            Db.ExecuteScalar(command);
        }

        public void Update(Role role)
        {
            var command = Db.GetStoredProcCommand("pub_Roles_Update");

            Db.AddInParameter(command, "@RoleId", DbType.Int32, role.RoleId);
            Db.AddInParameter(command, "@Name", DbType.String, role.Name);
            Db.AddInParameter(command, "@Description", DbType.String, role.Description);
            Db.ExecuteScalar(command);
        }
    }
}
