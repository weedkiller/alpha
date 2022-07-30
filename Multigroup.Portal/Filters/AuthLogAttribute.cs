using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Filters
{
    public class AuthLogAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!(filterContext.HttpContext.Request.RawUrl == "/"
                || filterContext.HttpContext.Request.RawUrl.ToUpper().StartsWith("/CONTRACT/SIGNCONTRACT")
                || filterContext.HttpContext.Request.RawUrl.ToUpper().StartsWith("/CONTRACT/SCORINGCONTRACT")
                || filterContext.HttpContext.Request.RawUrl == "/Account/LostPassword") && !filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    // Indicate to the remote caller that the session has expired and where to redirect
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    //Redirects user to login screen if session has timed out and request is non AJAX
                    HttpContext ctx = HttpContext.Current;
                    ctx.Response.Redirect("/");
                }
            }         
        }
    }
}