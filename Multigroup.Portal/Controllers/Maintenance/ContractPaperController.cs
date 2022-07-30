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
    public class ContractPaperController : Controller
    {
        private ContractPaperService _contractPaperService;
        private ContractService _contractService;
        private IAuditLogHelper _auditLogHelper;

        public ContractPaperController()
        {
            _contractPaperService = new ContractPaperService();
            _contractService = new ContractService();
            _auditLogHelper = new AuditLogHelper();
        }
        
        // GET: /ContractPaper/
        [LogActionFilter]
        [Description("ContractPapers")]
        public ActionResult Index()
        {
            ContractPaperFilterVm model = new ContractPaperFilterVm();
            var collections = new CollectionHelper();
            model.ddlContractType = collections.GetAllContractTypeSingleSelectVm();
            model.ListStatus = new SelectList(_contractService.GetStatusContract("PaperStatus"), "StatusContractId", "Description");
            return PartialView("~/Views/Maintenance/ContractPaper/ContractPaper.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedStatus, int? SelectedContractType)
        {
            var model = ContractPaperTableVm.ToViewModel(_contractPaperService.GetContractByFilter(SelectedStatus, SelectedContractType));
            return PartialView("~/Views/Maintenance/ContractPaper/ContractPaperTable.cshtml", model);
        }
        [LogActionFilter]
        [Description("Crear ContractPapers")]
        public ActionResult Create()
        {
            var model = new ContractPaperVm();
            var collections = new CollectionHelper();
            model.ddlStatus = collections.GetPaperStatusSingleSelectVm();
            model.ddlContractType = collections.GetAllContractTypeSingleSelectVm();
            return PartialView("~/Views/Maintenance/ContractPaper/ContractPaperCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(ContractPaperVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                bool existe = _contractPaperService.AddContractPaper(model.ToModelObject(), model.NumberFrom, model.NumberTo, model.ddlContractType.SelectedItem);
                if (!existe)
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
        [Description("Crear ContractPapers")]
        public ActionResult ChangeStatus()
        {
            var model = new ContractPaperVm();
            var collections = new CollectionHelper();

            model.ddlStatus = collections.GetPaperStatusSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();

            return PartialView("~/Views/Maintenance/ContractPaper/StatusModal.cshtml", model);
        }

        [HttpPost]
        public void ChangeStatus(ContractPaperVm model)
        {
            try
            {
                // TODO: Add update logic here
                if (model.ddlUser.SelectedItem == null)
                {
                    foreach (var item in model.Ids)
                    {
                        _contractPaperService.UpdateContractPaperStatus(item, int.Parse(model.ddlStatus.SelectedItem), null);
                    }
                }
                else
                {
                    foreach (var item in model.Ids)
                    {
                        _contractPaperService.UpdateContractPaperStatus(item, int.Parse(model.ddlStatus.SelectedItem), int.Parse(model.ddlUser.SelectedItem));
                    }
                }
                
            }
            catch
            {
            }
        }

        [LogActionFilter]
        [Description("Editar ContractPapers")]
        public ActionResult Edit(int id)
        {
            var entity = _contractPaperService.GetContractPaper(id);
            var model = ContractPaperVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlContractType = collections.GetContractTypeSingleSelectVm();
            model.ddlContractType.SelectedItem = entity.ContractType.ToString();
            return PartialView("~/Views/Maintenance/ContractPaper/ContractPaperEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(ContractPaperVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }
                if (model.PaperContractId < 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _contractPaperService.UpdateContractPaper(model.ToModelObject());

                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
	}
}