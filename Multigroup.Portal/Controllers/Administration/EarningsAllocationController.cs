using Multigroup.Common.Logging;
using Multigroup.Portal.Filters;
using Multigroup.Portal.Models.Administration;
using Multigroup.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multigroup.Portal.Helpers;
using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Models.Maintenance;
using Multigroup.Portal.Utilities;
using Multigroup.Portal.Providers;
using System.Web.UI.WebControls;

namespace Multigroup.Portal.Controllers.Administration
{
    public class EarningsAllocationController : Controller
    {
        private EarningsAllocationService _earningsAllocationService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProfileProvider _profileProvider;
        private CustomerService _customerService;
        private PartnerService _partnerService;

        public EarningsAllocationController()
        {
            _earningsAllocationService = new EarningsAllocationService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _profileProvider = new ProfileProvider();
            _customerService = new CustomerService();
            _partnerService = new PartnerService();
        }

        // GET: /EarningsAllocation/
        [LogActionFilter]
        [Description("EarningsAllocations")]
        public ActionResult Index()
        {
            EarningsAllocationFilterVm model = new EarningsAllocationFilterVm();
            var collections = new CollectionHelper();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            return PartialView("~/Views/EarningsAllocation/EarningsAllocation.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedUser, string DateFrom, string DateTo, string AmountFrom, string AmountTo)
        {
            var model = EarningsAllocationTableVm.ToViewModel(_earningsAllocationService.GetEarningsAllocationSummary(SelectedUser, DateFrom, DateTo, AmountFrom, AmountTo));
            return PartialView("~/Views/EarningsAllocation/EarningsAllocationTable.cshtml", model);
        }

        public ActionResult IndexPartners()
        {
            EarningsAllocationVm model = (EarningsAllocationVm)Session["SessionEarningsAllocation"];
            ViewBag.ErrorMessageTrabajo = "false";
            return PartialView("~/Views/EarningsAllocation/PartnersTable.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear EarningsAllocations")]
        public ActionResult Create()
        {
            var model = new EarningsAllocationVm();
            var collections = new CollectionHelper();
            List<EarningsAllocationPartner> lists = new List<EarningsAllocationPartner>();
            model.Partners = lists;
            model.Date = DateTime.Now.ToShortDateString();
            Session["SessionEarningsAllocation"] = model;
            return PartialView("~/Views/EarningsAllocation/EarningsAllocationCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(EarningsAllocationVm model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage));

                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, message);
                }
                if (model.Amount <= 0)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El monto debe ser mayor a 0");

                EarningsAllocationVm principal = (EarningsAllocationVm)Session["SessionEarningsAllocation"];
                model.Partners = principal.Partners;
                double total = 0;
                foreach (EarningsAllocationPartner part in model.Partners)
                {
                    total = total + part.Amount;
                }

                if (Math.Abs((model.Amount - total)) >= 0.1)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al calcular los totales de cada socio. Por favor recalcular");
                
                var earningsAllocation = model.ToModelObject();
                earningsAllocation.SystemDate = DateTime.Now;
                earningsAllocation.UserId = _profileProvider.CurrentUserId;

                _earningsAllocationService.AddEarningsAllocation(earningsAllocation);

                return Index();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        public ActionResult View(int id)
        {
            var entity = _earningsAllocationService.GetEarningsAllocationsById(id);
            var model = EarningsAllocationVm.FromDomainModel(entity);

            return PartialView("~/Views/EarningsAllocation/EarningsAllocationView.cshtml", model);
        }

        public ActionResult Edit(int id)
        {
            var entity = _earningsAllocationService.GetEarningsAllocationsById(id);
            var model = EarningsAllocationVm.FromDomainModel(entity);

            Session["SessionEarningsAllocation"] = model;
            return PartialView("~/Views/EarningsAllocation/EarningsAllocationEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(EarningsAllocationVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, message);
                }

                if (model.Amount <= 0)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El monto debe ser mayor a 0");

                EarningsAllocationVm principal = (EarningsAllocationVm)Session["SessionEarningsAllocation"];
                model.Partners = principal.Partners;
                double total = 0;
                foreach (EarningsAllocationPartner part in model.Partners)
                {
                    total = total + part.Amount;
                }

                if (Math.Abs((model.Amount - total)) >= 0.1)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al calcular los totales de cada socio. Por favor recalcular");

                EarningsAllocation earningsAllocation = model.ToModelObject();
                earningsAllocation.SystemDate = DateTime.Now;
                earningsAllocation.UserId = _profileProvider.CurrentUserId;
                _earningsAllocationService.UpdateEarningsAllocation(earningsAllocation);
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

        public ActionResult Partners(double amount)
        {
            EarningsAllocationVm principal = (EarningsAllocationVm)Session["SessionEarningsAllocation"];

            List<EarningsAllocationPartner> list = new List<EarningsAllocationPartner>();
            IEnumerable<Multigroup.DomainModel.Shared.Partner> partners = _partnerService.GetPartners();


            foreach (Multigroup.DomainModel.Shared.Partner partner in partners)
            {
                EarningsAllocationPartner part = new EarningsAllocationPartner();
                part.Partner = partner.Name;
                part.PartnerId = partner.PartnerId;
                part.Percentage = partner.Percentage;
                part.Amount = partner.Percentage * amount / 100;
                list.Add(part);
            }

            principal.Partners = list;

            Session["SessionEarningsAllocation"] = principal;

            return PartialView("~/Views/EarningsAllocation/PartnersTable.cshtml", principal);

        }

    }
}