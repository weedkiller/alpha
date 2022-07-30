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

namespace Multigroup.Portal.Controllers.Partner
{
    public class PartnerController : Controller
    {
        private PartnerService _partnerService;
        private IAuditLogHelper _auditLogHelper;

        public PartnerController()
        {
            _partnerService = new PartnerService();
            _auditLogHelper = new AuditLogHelper();
        }

        // GET: /Partner/
        [LogActionFilter]
        [Description("Partners")]
        public ActionResult Index()
        {
            var model = PartnerTableVm.ToViewModel(_partnerService.GetPartners());
            return PartialView("~/Views/Maintenance/Partner/Partner.cshtml", model);
        }

        // GET: /Partner/
        [LogActionFilter]
        [Description("IndexPercentage")]
        public ActionResult IndexPercentage()
        {
            var model = PercentageDefinitionTableVm.ToViewModel(_partnerService.GetPercentageDefinitions());
            return PartialView("~/Views/Maintenance/PercentageDefinition/PercentageDefinition.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Partners")]
        public ActionResult Create()
        {
            var model = new PartnerVm();
            var collections = new CollectionHelper();
            return PartialView("~/Views/Maintenance/Partner/PartnerCreate.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Percentage")]
        public ActionResult CreatePercentage()
        {
            var model = new PercentageDefinitionVm();
            var collections = new CollectionHelper();
            List<PercentageDefinitionPartner> lists = new List<PercentageDefinitionPartner>();
            model.Partners = lists;
            model.ddlPartners = collections.GetPartnersSingleSelectVm();
            Session["Percentage"] = model;
            return PartialView("~/Views/Maintenance/PercentageDefinition/PercentageDefinitionCreate.cshtml", model);
        }

        public ActionResult IndexPartners()
        {
            PercentageDefinitionVm model = (PercentageDefinitionVm)Session["Percentage"];
            ViewBag.ErrorMessageTrabajo = "false";
            return PartialView("~/Views/Maintenance/PercentageDefinition/PartnersTable.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(PartnerVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }
                model.Active = true;
                _partnerService.AddPartner(model.ToModelObject());
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
        [Description("Editar Partners")]
        public ActionResult Edit(int id)
        {
            var entity = _partnerService.GetPartner(id);
            var model = PartnerVm.FromDomainModel(entity);
            var collections = new CollectionHelper();         
            return PartialView("~/Views/Maintenance/Partner/PartnerEdit.cshtml", model);
        }

        [Description("Editar Partners")]
        public ActionResult EditPercentage(int id)
        {
            var entity = _partnerService.GetPercentageDefinitionById(id);
            var model = PercentageDefinitionVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlPartners = collections.GetPartnersSingleSelectVm();
            Session["Percentage"] = model;
            return PartialView("~/Views/Maintenance/PercentageDefinition/PercentageDefinitionEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(PartnerVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _partnerService.UpdatePartners(model.ToModelObject());

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

        public ActionResult Partners(int? PartnerId, double Percentage)
        {
            bool existe = false;
            PercentageDefinitionVm principal = (PercentageDefinitionVm)Session["Percentage"];
            if (PartnerId.HasValue)
            {
                PercentageDefinitionPartner detail = new PercentageDefinitionPartner();
                detail.PartnerId = PartnerId.Value;
                detail.Percentage = Percentage;
                detail.Name = _partnerService.GetPartner(PartnerId.Value).Name;

                List<PercentageDefinitionPartner> list = (List<PercentageDefinitionPartner>)principal.Partners;
                Int32 id = PartnerId.Value;
                var item = list.FirstOrDefault(x => x.PartnerId == id);
                if (item != null)
                    existe = true;
                else
                    list.Add(detail);
                principal.Partners = list;


                Session["Percentage"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessageTrabajo = "false";
                    return PartialView("~/Views/Maintenance/PercentageDefinition/PartnersTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessageTrabajo = "true";
                    return PartialView("~/Views/Maintenance/PercentageDefinition/PartnersTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un articulo");
            }
        }

        public ActionResult RemovePartners(int Id)
        {
            PercentageDefinitionVm principal = (PercentageDefinitionVm)Session["Percentage"];
            List<PercentageDefinitionPartner> list = (List<PercentageDefinitionPartner>)principal.Partners;
            var item = list.FirstOrDefault(x => x.PartnerId == Id);
            list.Remove(item);

            principal.Partners = list;

            Session["Percentage"] = principal;

            Session["esNuevo"] = false;
            ViewBag.ErrorMessageTrabajo = "false";
            return PartialView("~/Views/Maintenance/PercentageDefinition/PartnersTable.cshtml", principal);
        }

        public ActionResult ViewPercentage(int id)
        {
            var entity = _partnerService.GetPercentageDefinitionById(id);
            var model = PercentageDefinitionVm.FromDomainModel(entity);
            return PartialView("~/Views/Maintenance/PercentageDefinition/PercentageDefinitionView.cshtml", model);
        }

        [HttpPost]
        public ActionResult CreatePercentage(PercentageDefinitionVm model)
        {
            try
            {

                if (model.Name == null ||model.Name == "")
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe ingresar Nombre.");
                }


                PercentageDefinitionVm principal = (PercentageDefinitionVm)Session["Percentage"];
                model.Partners = principal.Partners;

                if (model.Partners.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Socio.");
                }


                var percentage = model.ToModelObject();
                double total = 0;
                foreach (PercentageDefinitionPartner per in model.Partners)
                {
                    total = total + per.Percentage;
                }

                if (total != 100)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La suma de los totales de los socios debe ser igual a 100.");

                _partnerService.AddPercentageDefinition(percentage);

                return Index();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        [HttpPost]
        public ActionResult EditPercentage(PercentageDefinitionVm model)
        {
            try
            {
                if (model.Name == null || model.Name == "")
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe ingresar Nombre.");
                }


                PercentageDefinitionVm principal = (PercentageDefinitionVm)Session["Percentage"];
                model.Partners = principal.Partners;

                if (model.Partners.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Socio.");
                }


                var percentage = model.ToModelObject();
                double total = 0;
                foreach (PercentageDefinitionPartner per in model.Partners)
                {
                    total = total + per.Percentage;
                }

                if (total != 100)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La suma de los totales de los socios debe ser igual a 100.");

                _partnerService.UpdatePercentageDefinition(percentage);

                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        [Description("Balance")]
        public ActionResult IndexBalance()
        {
            BalancePartnerFilterVm model = new BalancePartnerFilterVm();
            var collections = new CollectionHelper();
            model.ListPartner = new SelectList(_partnerService.GetPartners(), "PartnerId", "Name");
          
            return PartialView("~/Views/Maintenance/Partner/Balance.cshtml", model);
        }

        public ActionResult GetTableBalance(int? SelectedPartner, string isAssigned)
        {
            if (SelectedPartner == null)
                SelectedPartner = 0;    
            IEnumerable<BalancePartner> resumen = _partnerService.GetBalancePartner(SelectedPartner.Value.ToString(), isAssigned);
            var model = BalancePartnerTableVm.ToViewModel(resumen);
            double total = 0;

            foreach (BalancePartner mov in resumen)
            {
                if (mov.TransactionType.Equals("Asignación de Ganancias") || mov.TransactionType.Equals("Ingreso Socios"))
                    total = total + mov.Amount;
                else
                    total = total - mov.Amount;
            }

            ViewBag.Total = total.ToString();

            return PartialView("~/Views/Maintenance/Partner/BalanceTable.cshtml", model);
        }
    }
}