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
    public class ContributionAllocationController : Controller
    {
        private ContributionAllocationService _contributionAllocationService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProfileProvider _profileProvider;
        private CustomerService _customerService;
        private PartnerService _partnerService;

        public ContributionAllocationController()
        {
            _contributionAllocationService = new ContributionAllocationService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _profileProvider = new ProfileProvider();
            _customerService = new CustomerService();
            _partnerService = new PartnerService();
        }

        // GET: /ContributionAllocation/
        [LogActionFilter]
        [Description("ContributionAllocations")]
        public ActionResult Index()
        {
            ContributionAllocationFilterVm model = new ContributionAllocationFilterVm();
            var collections = new CollectionHelper();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            return PartialView("~/Views/ContributionAllocation/ContributionAllocation.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedUser, string DateFrom, string DateTo, string AmountFrom, string AmountTo)
        {
            var model = ContributionAllocationTableVm.ToViewModel(_contributionAllocationService.GetContributionAllocationSummary(SelectedUser, DateFrom, DateTo, AmountFrom, AmountTo));
            return PartialView("~/Views/ContributionAllocation/ContributionAllocationTable.cshtml", model);
        }

        public ActionResult IndexPartners()
        {
            ContributionAllocationVm model = (ContributionAllocationVm)Session["SessionContributionAllocation"];
            ViewBag.ErrorMessageTrabajo = "false";
            return PartialView("~/Views/ContributionAllocation/PartnersTable.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear ContributionAllocations")]
        public ActionResult Create()
        {
            var model = new ContributionAllocationVm();
            var collections = new CollectionHelper();
            List<ContributionAllocationPartner> lists = new List<ContributionAllocationPartner>();
            model.Partners = lists;
            model.Date = DateTime.Now.ToShortDateString();
            Session["SessionContributionAllocation"] = model;
            return PartialView("~/Views/ContributionAllocation/ContributionAllocationCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(ContributionAllocationVm model)
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

                ContributionAllocationVm principal = (ContributionAllocationVm)Session["SessionContributionAllocation"];
                model.Partners = principal.Partners;
                double total = 0;

                foreach (ContributionAllocationPartner part in model.Partners)
                {
                    total = total + part.Amount;
                }

                if (Math.Abs((model.Amount - total)) >= 0.1)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al calcular los totales de cada socio. Por favor recalcular");

                var contributionAllocation = model.ToModelObject();
                contributionAllocation.SystemDate = DateTime.Now;
                contributionAllocation.UserId = _profileProvider.CurrentUserId;
              
                _contributionAllocationService.AddContributionAllocation(contributionAllocation);
            
                return Index();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        public ActionResult View(int id)
        {
            var entity = _contributionAllocationService.GetContributionAllocationsById(id);
            var model = ContributionAllocationVm.FromDomainModel(entity);

            return PartialView("~/Views/ContributionAllocation/ContributionAllocationView.cshtml", model);
        }

        public ActionResult Edit(int id)
        {
            var entity = _contributionAllocationService.GetContributionAllocationsById(id);
            var model = ContributionAllocationVm.FromDomainModel(entity);

            Session["SessionContributionAllocation"] = model;
            return PartialView("~/Views/ContributionAllocation/ContributionAllocationEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(ContributionAllocationVm model)
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

                ContributionAllocationVm principal = (ContributionAllocationVm)Session["SessionContributionAllocation"];
                model.Partners = principal.Partners;
                double total = 0;

                foreach (ContributionAllocationPartner part in model.Partners)
                {
                    total = total + part.Amount;
                }

                if (Math.Abs((model.Amount - total)) >= 0.1)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al calcular los totales de cada socio. Por favor recalcular");

                ContributionAllocation contributionAllocation = model.ToModelObject();
                contributionAllocation.SystemDate = DateTime.Now;
                contributionAllocation.UserId = _profileProvider.CurrentUserId;
                _contributionAllocationService.UpdateContributionAllocation(contributionAllocation);
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
            ContributionAllocationVm principal = (ContributionAllocationVm)Session["SessionContributionAllocation"];

            List<ContributionAllocationPartner> list = new List<ContributionAllocationPartner>();
            IEnumerable<Multigroup.DomainModel.Shared.Partner> partners = _partnerService.GetPartners();


            foreach(Multigroup.DomainModel.Shared.Partner partner in partners)
            {
                ContributionAllocationPartner part = new ContributionAllocationPartner();
                part.Partner = partner.Name;
                part.PartnerId = partner.PartnerId;
                part.Percentage = partner.Percentage;
               part.Amount = partner.Percentage * amount / 100;
                list.Add(part);
            }

                principal.Partners = list;

                Session["SessionContributionAllocation"] = principal;

                return PartialView("~/Views/ContributionAllocation/PartnersTable.cshtml", principal);
      
        }

    }
}