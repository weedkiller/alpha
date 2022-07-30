using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Providers;
using Multigroup.Services.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Multigroup.Portal.Filters
{
    public class LogActionFilter : ActionFilterAttribute, IActionFilter
    {
        GeneralService _generalService = new GeneralService();
        
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            NavigationLog _navigation = new NavigationLog(); 
            ProfileProvider  _profileProvider = new ProfileProvider();

            _navigation.UserId = _profileProvider.CurrentUserId;
            _navigation.ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            _navigation.ActionDescription = getActionDescription(filterContext.ActionDescriptor);
            _navigation.ControllerFullName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;
            _navigation.ControllerNameSpace = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.Namespace;
            _navigation.ActionName = filterContext.ActionDescriptor.ActionName;
            _navigation.IP = filterContext.HttpContext.Request.UserHostAddress; 
            _navigation.HttpContextTimestamp = filterContext.HttpContext.Timestamp;
   
             StringBuilder parameters = new StringBuilder();
             foreach (var p in filterContext.ActionDescriptor.GetParameters())
             {
                 if (filterContext.Controller.ValueProvider.GetValue(p.ParameterName) != null)
                 {
                     parameters.AppendFormat("\n {0} : {1}", p.ParameterName,
                                             filterContext.Controller.ValueProvider.GetValue(p.ParameterName).AttemptedValue);
                 }
             }
             _navigation.Parameters = parameters.ToString();

             _navigation.SessionId = filterContext.HttpContext.Session.SessionID;
             _generalService.InsertNavigationLog(_navigation);
             base.OnActionExecuted(filterContext);
        //this.OnActionExecuted(filterContext);
        }

        private string getActionDescription(ActionDescriptor _actionDescriptor)
        {
            object [] attrList = _actionDescriptor.GetCustomAttributes(typeof(DescriptionAttribute), true);
            foreach (DescriptionAttribute attr in attrList)
            {
                return attr.Title;
            }
            // Si la acción no tiene el tag Description entonces guardo en ese campo
            // Nombre Controlador + Acción
            return  _actionDescriptor.ControllerDescriptor.ControllerName + " : " +  _actionDescriptor.ActionName;
        }

    }
}