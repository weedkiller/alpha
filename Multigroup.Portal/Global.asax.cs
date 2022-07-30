using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;
using Multigroup.Services.Shared;
using Multigroup.Portal.Filters;

namespace Multigroup.Portal
{
    public class MvcApplication : System.Web.HttpApplication
    {
  protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalFilters.Filters.Add(new AuthLogAttribute());
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            if (!WebMatrix.WebData.WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "User", "UserId", "UserName", true);
            }  
        }

        protected void Session_Start()
        {
            //Session.Add("userId", "1");
            //WebServicesService _webServicesService = new WebServicesService();
            //_webServicesService.Execute();
            //XlsData _xlsData = new XlsData();
            //_xlsData.importData();
        }

        //private void Application_BeginRequest(Object source, EventArgs e)
        //{
        //    HttpApplication application = (HttpApplication)source;
        //    HttpContext context = application.Context;

        //    string culture = null;
        //    if (context.Request.UserLanguages != null && Request.UserLanguages.Length > 0)
        //    {
        //        culture = "en-US";// Request.UserLanguages[0];
        //        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
        //        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        //    }
        //}
    }
    
}
