using Multigroup.Common.Logging;
using Multigroup.Portal.Filters;
using Multigroup.Portal.Models.Security;
using Multigroup.Portal.Models.Shared;
using Multigroup.Services.Menu;
using Multigroup.Services.Shared;
using Resources;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Multigroup.Portal.Controllers.Security
{
    public class NavigationAccessController : Controller
    {
        private RoleService _roleService;
        private MenuService _menuService;
        private IAuditLogHelper _auditLogHelper;

        public NavigationAccessController()
        {
            _roleService = new RoleService();
            _menuService = new MenuService();
            _auditLogHelper = new AuditLogHelper();
        }

        [LogActionFilter]
        [Description("Accesos de navegacion")]
        public ActionResult Index()
        {
            var model = new MenuRoleVm();
            model.ddlMenuTypes = GetMenuSelectVm();
            model.ddlRoles = GetRolesSelectVm();

            return PartialView("~/Views/Security/NavigationAccess/NavigationAccess.cshtml", model);
        }

        public ActionResult GetNavigation(MenuRoleVm model)
        {
            if (model.RoleId <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Faltan Campos requeridos");
            }

            return PartialView("~/Views/Security/NavigationAccess/NavigationTable.cshtml", MenuItemVm.ToViewModel(_menuService.GetMenuComponents(model.MenuType, model.RoleId)));
        }

        //[HttpPost]
        public ActionResult Create(MenuVm model)
        {
            try
            {
                if (model.RoleId <= 0 && model.Items != null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Faltan Campos requeridos");
                }

                _menuService.AddMenuComponentRole(model.ToModelObject());
                return PartialView("~/Views/Security/NavigationAccess/NavigationTable.cshtml", MenuItemVm.ToViewModel(_menuService.GetMenuComponents(model.MenuType, model.RoleId)));
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = model,
                    IntegrationType = AuditLogType.AlarmService
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al procesar la solicitud");
            }
        }

        public ActionResult Delete(MenuVm model)
        {
            try
            {
                if (model.RoleId <= 0 && model.Items != null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Faltan Campos requeridos");
                }

                _menuService.DeleteMenuComponentRole(model.ToModelObject());

                return PartialView("~/Views/Security/NavigationAccess/NavigationTable.cshtml", MenuItemVm.ToViewModel(_menuService.GetMenuComponents(model.MenuType, model.RoleId)));
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = model,
                    IntegrationType = AuditLogType.AlarmService
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al procesar la solicitud");
            }
        }

        private SingleSelectVm GetMenuSelectVm()
        {
            var types = _menuService.GetMenuTypes();
            var ddl = new SingleSelectVm();
            var items = types.Select(x => new ItemCode { Id = x.MenuTypeId, Name = x.Name });
            ddl.ListItems = new SelectList(items, "Id", "Name");

            return ddl;
        }

        private SingleSelectVm GetRolesSelectVm()
        {
            var ddl = new SingleSelectVm();
            ddl.ListItems = new SelectList(_roleService.GetRoles(), "RoleId", "Description");

            return ddl;
        }
    }
}
