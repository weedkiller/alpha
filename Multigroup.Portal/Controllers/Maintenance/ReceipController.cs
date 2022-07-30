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
using Multigroup.Portal.Models.Collection;

namespace Multigroup.Portal.Controllers.Maintenance
{
    public class ReceipController : Controller
    {
        private ReceipService _ReceipService;
        private ContractService _contractService;
        private IAuditLogHelper _auditLogHelper;

        public ReceipController()
        {
            _ReceipService = new ReceipService();
            _contractService = new ContractService();
            _auditLogHelper = new AuditLogHelper();
        }
        
        // GET: /Receip/
        [LogActionFilter]
        [Description("Receips")]
        public ActionResult Index()
        {
            ReceipFilterVm model = new ReceipFilterVm();
            var collections = new CollectionHelper();
            model.ddlContractType = collections.GetAllContractTypeSingleSelectVm();
            model.ListStatus = new SelectList(_contractService.GetStatusContract("PaperStatus"), "StatusContractId", "Description");
            return PartialView("~/Views/Maintenance/Receip/Receip.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedStatus)
        {
            var model = ReceipTableVm.ToViewModel(_ReceipService.GetContractByFilter(SelectedStatus));
            return PartialView("~/Views/Maintenance/Receip/ReceipTable.cshtml", model);
        }
        [LogActionFilter]
        [Description("Crear Receips")]
        public ActionResult Create()
        {
            var model = new ReceipVm();
            var collections = new CollectionHelper();
            model.ddlStatus = collections.GetPaperStatusSingleSelectVm();
            return PartialView("~/Views/Maintenance/Receip/ReceipCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(ReceipVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                else if (!_ReceipService.AddReceip(model.ToModelObject(), model.NumberFrom, model.NumberTo))
                {
                    return Index();
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.PartialContent, "Algunos contratos no se cargaron porque ya existían");
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }


        [LogActionFilter]
        [Description("Crear Receips")]
        public ActionResult ChangeStatus()
        {
            var model = new ReceipVm();
            var collections = new CollectionHelper();

            model.ddlStatus = collections.GetPaperStatusSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();

            return PartialView("~/Views/Maintenance/Receip/StatusModal.cshtml", model);
        }

        [HttpPost]
        public void ChangeStatus(ReceipVm model)
        {
            try
            {
                // TODO: Add update logic here
                if (model.ddlUser.SelectedItem == null)
                {
                    foreach (var item in model.Ids)
                    {
                        _ReceipService.UpdateReceipStatus(item, int.Parse(model.ddlStatus.SelectedItem), null);
                    }
                }
                else
                {
                    foreach (var item in model.Ids)
                    {
                        _ReceipService.UpdateReceipStatus(item, int.Parse(model.ddlStatus.SelectedItem), int.Parse(model.ddlUser.SelectedItem));
                    }
                }
                
            }
            catch
            {
            }
        }

        [LogActionFilter]
        [Description("Editar Receips")]
        public ActionResult Edit(int id)
        {
            var entity = _ReceipService.GetReceip(id);
            var model = ReceipVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            return PartialView("~/Views/Maintenance/Receip/ReceipEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(ReceipVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }
                if (model.ReceipId < 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _ReceipService.UpdateReceip(model.ToModelObject());

                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
	}
}