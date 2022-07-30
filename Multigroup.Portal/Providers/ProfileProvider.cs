using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;
//using WebMatrix.WebData;

namespace Multigroup.Portal.Providers
{
    public class ProfileProvider
    {

        public UserFilter Filters
        {
            get
            {
                return UserLoggedProvider.GetFilters();
            }
            set
            {
                UserLoggedProvider.AddFilters(value);
            }
        }

        public int CurrentUserId
        {
            get
            {
                return GetCurrentUserId();
            }
        }

        public string CurrentHour
        {
            get
            {
                return UserLoggedProvider.GetCurrentHour();
            }
        }

        public bool Authenticated
        {
            get
            {
                return WebSecurity.IsAuthenticated;
            }
        }

        public string CurrentUserName
        {
            get
            {
                return GetCurrentUserName();
            }
            set
            {
                UserLoggedProvider.AddUserName(value);
            }
        }
        public int CurrenSucursalId
        {
            get
            {
                return UserLoggedProvider.GetCurrentSucursalId();
            }
            set
            {
                UserLoggedProvider.AddSucursalId(value);
            }
        }

     

        private static string GetCurrentUserName()
        {
            if (WebSecurity.CurrentUserName == null)
            {
                throw new Exception("The profile Name is not accesible");
            }

            return WebSecurity.CurrentUserName;
        }
        private int GetCurrentUserId()
        {
            if (WebSecurity.IsAuthenticated)
            {
                if (WebSecurity.CurrentUserId <= 0)
                {
                    throw new Exception("The user Id is not accesible");
                }
                return WebSecurity.CurrentUserId;
            }
            else
            {
                return 0;
            }
        }
    }
}