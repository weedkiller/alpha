using Multigroup.Common.Logging;
using Multigroup.Portal.Filters;
using Multigroup.Portal.Models.Maintenance;
using Multigroup.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multigroup.Portal.Helpers;
using Multigroup.DomainModel.Shared;

namespace Multigroup.Portal.Controllers.UserType
{
    public class UserTypeController : Controller
    {
        private UserTypeService _userTypeService;
        private IAuditLogHelper _auditLogHelper;

        public UserTypeController()
        {
            _userTypeService = new UserTypeService();
            _auditLogHelper = new AuditLogHelper();
        }

        // GET: /UserType/
        [LogActionFilter]
        [Description("UserTypes")]
        public ActionResult Index()
        {
            var model = UserTypeTableVm.ToViewModel(_userTypeService.getUserTypes());
            return PartialView("~/Views/Maintenance/UserType/UserType.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear UserTypes")]
        public ActionResult Create()
        {
            var model = new UserTypeVm();
            var collections = new CollectionHelper();
            return PartialView("~/Views/Maintenance/UserType/UserTypeCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(UserTypeVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _userTypeService.InsertUserType(model.ToModelObject());
                return Index();
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = model,
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
        [LogActionFilter]
        [Description("Editar UserTypes")]
        public ActionResult Edit(int id)
        {
            var entity = _userTypeService.GetUserType(id);
            var model = UserTypeVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            return PartialView("~/Views/Maintenance/UserType/UserTypeEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(UserTypeVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _userTypeService.UpdateUserType(model.ToModelObject());

                return Index();
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = model,
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
	}
}