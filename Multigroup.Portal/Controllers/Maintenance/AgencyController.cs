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

namespace Multigroup.Portal.Controllers.Agency
{
    public class AgencyController : Controller
    {
        private AgencyService _agencyService;
        private IAuditLogHelper _auditLogHelper;

        public AgencyController()
        {
            _agencyService = new AgencyService();
            _auditLogHelper = new AuditLogHelper();
        }

        // GET: /Agency/
        [LogActionFilter]
        [Description("Agencys")]
        public ActionResult Index()
        {
            var model = AgencyTableVm.ToViewModel(_agencyService.GetAgencys());
            return PartialView("~/Views/Maintenance/Agency/Agency.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Agencys")]
        public ActionResult Create()
        {
            var model = new AgencyVm();
            var collections = new CollectionHelper();
            model.ddlUsers = collections.GetUsersAgencySingleSelectVm();
            model.ddlTypes = collections.GetTypesAgencySingleSelectVm();
            return PartialView("~/Views/Maintenance/Agency/AgencyCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(AgencyVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _agencyService.AddAgency(model.ToModelObject());
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
        [Description("Editar Agencys")]
        public ActionResult Edit(int id)
        {
            _agencyService.GetCarteraClientes();

            var entity = _agencyService.GetAgency(id);
            var model = AgencyVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlUsers = collections.GetUsersAgencySingleSelectVm();
            model.ddlUsers.SelectedItem = entity.ManagerId.ToString();
            model.ddlTypes = collections.GetTypesAgencySingleSelectVm();
            model.ddlTypes.SelectedItem = entity.Type.ToString(); 
            return PartialView("~/Views/Maintenance/Agency/AgencyEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(AgencyVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _agencyService.UpdateAgencys(model.ToModelObject());

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