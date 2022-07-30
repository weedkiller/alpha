using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace Multigroup.Portal.Providers
{
    public class UserLoginProvider
    {
        public bool Login(string userName, string password)
        {
            return WebSecurity.Login(userName, password);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }
        public string CreateAccount(User user)
        {
            return WebSecurity.CreateAccount(user.UserName, user.Password, false);
        }

        public bool UpdateAccount(User user, string pass)
        {
            return WebSecurity.ChangePassword(user.UserName, user.Password, pass);
        }

        public bool ExistsAccount(User user)
        {
            return WebSecurity.UserExists(user.UserName);
        }
        public void ResetPassword(User user)
        {
            var token = WebSecurity.GeneratePasswordResetToken(user.UserName);

            WebSecurity.ResetPassword(token, user.Password);
        }

        public string GeneratePassword()
        {
            var password = System.Web.Security.Membership.GeneratePassword(10, 1);
            return password;
        }

    }
}