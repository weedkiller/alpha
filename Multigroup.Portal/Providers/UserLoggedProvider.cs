using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Multigroup.Portal.Providers
{
    public class UserLoggedProvider
    {
        /// <summary>
        /// Devuelve los filtros de dashboard guardados en session
        /// </summary>
        /// <returns></returns>
        public static UserFilter GetFilters()
        {
            var session = HttpContext.Current.Session;
            var currentFilters = session[GetSessionFiltersObject()] as UserFilter;

            if (currentFilters != null)
            {
                return currentFilters;
            }
            else
            {
                var filter = new UserFilter();
                session[GetSessionFiltersObject()] = filter;
                filter.SelectedFleets = Enumerable.Empty<string>();
                return filter;
            }
        }

        public static void AddFilters(UserFilter filters)
        {
            var session = HttpContext.Current.Session;
            session[GetSessionFiltersObject()] = filters;
        }

        /// <summary>
        /// Devuelve el Id del usuario logueado
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentUserId()
        {
            var session = HttpContext.Current.Session;
            var currentUserId = session[GetSessionUserId()] as int?;

            if (currentUserId != null)
            {
                return (int)currentUserId;
            }

            return 58;//usuario zapata
            //throw new Exception("Usuario no loguedo");        
        }

        public static int GetCurrentSucursalId()
        {
            var session = HttpContext.Current.Session;
            var currentSucursalId = session[GetSessionSucursalId()] as int?;

            if (currentSucursalId != null)
            {
                return (int)currentSucursalId;
            }

            return 58;//usuario zapata
            //throw new Exception("Usuario no loguedo");
        }

        /// <summary>
        /// Devuelve el userName del usuario logueado
        /// </summary>
        /// <returns></returns>
        public static String GetCurrentUserName()
        {
            var session = HttpContext.Current.Session;
            var currentUserName = session[GetSessionUserName()];

            if (currentUserName != null)
            {
                return (String)currentUserName;
            }

            //return "NZAPATA";
            throw new Exception("Usuario no loguedo");
        }

        public static void SetAuthenticated(bool isAuthenticated)
        {
            var session = HttpContext.Current.Session;
            session["Authenticated"] = isAuthenticated;
        }

        public static bool GetAuthenticated()
        {
            var session = HttpContext.Current.Session;
            var authenticated = session["Authenticated"] as bool?;

            if (authenticated != null)
            {
                return (bool)authenticated;
            }

            return false;
        }

        public static void AddUserId(int userId)
        {
            var session = HttpContext.Current.Session;

            session[GetSessionUserId()] = userId;
        }

        public static void AddSucursalId(int sucursalId)
        {
            var session = HttpContext.Current.Session;

            session[GetSessionSucursalId()] = sucursalId;
        }

        public static void AddUserName(String userName)
        {
            var session = HttpContext.Current.Session;

            session[GetSessionUserName()] = userName;
        }

        public static string GetCurrentHour()
        {
            return DateTime.Now.ToShortTimeString();
        }

        public void RefreshInstance()
        {
            var session = HttpContext.Current.Session;
            session[GetSessionFiltersObject()] = null;
        }

        private static string GetSessionFiltersObject()
        {
            return "FiltersObject";
        }

        private static string GetSessionUserId()
        {
            return "userId";
        }

        private static string GetSessionSucursalId()
        {
            return "sucursalId";
        }

        private static string GetSessionUserName()
        {
            return "userName";
        }
    }
}