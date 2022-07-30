using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Threading.Tasks;
using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;

namespace Multigroup.Services.Shared
{
    public class UserTypeService
    {
        private UserTypeRepositorio _UserTypeRepositorio;

        public UserTypeService()
        {
            _UserTypeRepositorio = new UserTypeRepositorio();
        }

        public UserType GetUserType(int UserTypeId)
        {
            return _UserTypeRepositorio.GetUserTypeById(UserTypeId);
        }

        public IEnumerable<UserType> getUserTypes()
        {
            return _UserTypeRepositorio.GetUserTypes();
        }


        public int InsertUserType(UserType UserType)
        {
            try
            {
                return _UserTypeRepositorio.InsertUserType(UserType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateUserType(UserType UserType)
        {
            try
            {
                _UserTypeRepositorio.UpdateUserType(UserType);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteUserType(int UserTypeId)
        {
            _UserTypeRepositorio.DeleteUserType(UserTypeId);
        }

    }
}
