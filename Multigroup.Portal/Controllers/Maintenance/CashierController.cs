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

namespace Multigroup.Portal.Controllers.Maintenance
{
    public class CashierController : Controller
    {
        private CashierService _cashierService;
        private IAuditLogHelper _auditLogHelper;

        public CashierController()
        {
            _cashierService = new CashierService();
            _auditLogHelper = new AuditLogHelper();
        }

        // GET: /Cashier/
        [LogActionFilter]
        [Description("Cashiers")]
        public ActionResult Index()
        {
            var model = CashierTableVm.ToViewModel(_cashierService.GetCashiers());
            return PartialView("~/Views/Maintenance/Cashier/Cashier.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Cashiers")]
        public ActionResult Create()
        {
            var model = new CashierVm();
            var collections = new CollectionHelper();
            return PartialView("~/Views/Maintenance/Cashier/CashierCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(CashierVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _cashierService.AddCashier(model.ToModelObject());
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
        [Description("Editar Cashiers")]
        public ActionResult Edit(int id)
        {
            var entity = _cashierService.GetCashiersById(id);
            var model = CashierVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            return PartialView("~/Views/Maintenance/Cashier/CashierEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(CashierVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _cashierService.UpdateCashier(model.ToModelObject());

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