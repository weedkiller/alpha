using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Multigroup.Services.Shared
{
    public class RoleService
    {
        private RoleRepository _roleRepository;

        public RoleService()
        {
            _roleRepository = new RoleRepository();
        }

        public Role GetRole(int RoleId)
        {
            return _roleRepository.GetRoleById(RoleId);
        }

        public IEnumerable<Role> GetRolesByUser(int userId)
        {
            return _roleRepository.GetRolesByUserId(userId);
        }

        public Role GetRolesById(int roleId)
        {
            return _roleRepository.GetRoleById(roleId);
        }

        public IEnumerable<Role> GetRoles()
        {
            return _roleRepository.GetRoles();
        }

        public IEnumerable<Role> GetRolesByRol(int roleId)
        {
            return _roleRepository.GetRolesByRol(roleId);
        }

        public void InsertRoles(int roleId, int userId)
        {
            _roleRepository.Insert(roleId, userId);
        }

        public void AddRole(Role role)
        {
            _roleRepository.AddRole(role);
        }

        public void DeleteRoles(int userId)
        {
            _roleRepository.Delete(userId);
        }

        public void UpdateRoles(Role role)
        {
            _roleRepository.Update(role);
        }
    }
}
