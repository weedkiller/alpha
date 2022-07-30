using Multigroup.Common.Logging;
using Multigroup.Portal.Filters;
using Multigroup.Portal.Models.Shared;
using Multigroup.Services.Shared;
using Resources;
using System;
using System.Net;
using System.Web.Mvc;

namespace Multigroup.Portal.Controllers.Shared
{
    public class RoleController : Controller
    {
        private RoleService _roleService;
        private IAuditLogHelper _auditLogHelper;

        public RoleController()
        {
            _roleService = new RoleService();
            _auditLogHelper = new AuditLogHelper();
        }

        public JsonResult GetRoles()
        {
            _roleService.GetRoles();
            return Json(_roleService.GetRoles(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        [LogActionFilter]
        [Description("Roles")]
        public ActionResult Index()
        {
            var model = RoleTableVm.ToViewModel(_roleService.GetRoles());
            return PartialView("~/Views/Security/Role/Role.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Roles")]
        public ActionResult Create()
        {
            var model = new RoleVm();
            return PartialView("~/Views/Security/Role/RoleCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(RoleVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _roleService.AddRole(model.ToModelObject());
                return Index();
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = model,
                    IntegrationType = AuditLogType.RoleService
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
        [LogActionFilter]
        [Description("Editar Roles")]
        public ActionResult Edit(int id)
        {
            var model = new RoleVm();

            return PartialView("~/Views/Security/Role/RoleEdit.cshtml", model.FromDomainModel(_roleService.GetRolesById(id)));
        }

        [HttpPost]
        public ActionResult Edit(RoleVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }
                if (model.Id < 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _roleService.UpdateRoles(model.ToModelObject());

                return Index();
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = model,
                    IntegrationType = AuditLogType.RoleService
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
    }
}
