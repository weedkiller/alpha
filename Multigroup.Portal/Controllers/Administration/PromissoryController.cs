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
    public class PromissoryController : Controller
    {
        private PromissoryService _promissoryService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProfileProvider _profileProvider;
        private CustomerService _customerService;
        private PartnerService _partnerService;

        public PromissoryController()
        {
            _promissoryService = new PromissoryService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _profileProvider = new ProfileProvider();
            _customerService = new CustomerService();
            _partnerService = new PartnerService();
        }

        // GET: /Promissory/
        [LogActionFilter]
        [Description("Promissorys")]
        public ActionResult Index()
        {
            PromissoryFilterVm model = new PromissoryFilterVm();
            var collections = new CollectionHelper();
            model.ListCustomer = new SelectList(_customerService.GetCustomersComplete(), "CustomerId", "Name");
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            return PartialView("~/Views/Promissory/Promissory.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedClient, int? SelectedUser, string DateFromBroadcast, string DateToBroadcast, string DateFromCollection, string DateToCollection, string AmountFrom, string AmountTo, string Number, string isPaid)
        {
            var model = PromissoryTableVm.ToViewModel(_promissoryService.GetPromissorysSummary(SelectedClient, SelectedUser, DateFromBroadcast, DateToBroadcast, DateFromCollection, DateToCollection, AmountFrom, AmountTo, isPaid, Number));
            return PartialView("~/Views/Promissory/PromissoryTable.cshtml", model);
        }

        public ActionResult IndexPartners()
        {
            PromissoryVm model = (PromissoryVm)Session["SessionPromissory"];
            ViewBag.ErrorMessageTrabajo = "false";
            return PartialView("~/Views/Promissory/PartnersTable.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Promissorys")]
        public ActionResult Create()
        {
            var model = new PromissoryVm();
            var collections = new CollectionHelper();
            List<PercentageDefinitionPartnerSummary> lists = new List<PercentageDefinitionPartnerSummary>();
            model.Partners = lists;
            model.BroadcastDate = DateTime.Now.ToShortDateString();
            model.CollectionDate = DateTime.Now.ToShortDateString();
            model.ddlClient = collections.GetCustomersSingleSelectVm();
            model.ddlPercentages = collections.GetPercentagesSingleSelectVm();
            Session["SessionPromissory"] = model;
            return PartialView("~/Views/Promissory/PromissoryCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(PromissoryVm model)
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

                var promissory = model.ToModelObject();

                promissory.UserId = _profileProvider.CurrentUserId;

                

                promissory.Description = "Cuota 1 de " + model.Quantity;
                _promissoryService.AddPromissory(promissory);

                for (int i = 1; i < model.Quantity; i++)
                {
                    Promissory promiss = promissory;
                    promiss.Number = promissory.Number + 1;
                    promiss.CollectionDate = promissory.CollectionDate.AddMonths(1);
                    promissory.Description = "Cuota " + (i + 1) + " de " + model.Quantity;
                    _promissoryService.AddPromissory(promiss);
                }
             

                return Index();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        public ActionResult View(int id)
        {
            var entity = _promissoryService.GetPromissorysById(id);
            var model = PromissoryVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlClient = collections.GetCustomersSingleSelectVm();
            model.ddlClient.SelectedItem = entity.ClientId.ToString();
            model.ddlPercentages = collections.GetPercentagesSingleSelectVm();     
            model.ddlPercentages.SelectedItem = entity.PercentageDefinitionId.ToString();

            PercentageDefinition percentage = _partnerService.GetPercentageDefinitionById(entity.PercentageDefinitionId);

            List<PercentageDefinitionPartnerSummary> list = new List<PercentageDefinitionPartnerSummary>();

            foreach (PercentageDefinitionPartner partner in percentage.Partners)
            {
                PercentageDefinitionPartnerSummary partnerSummary = new PercentageDefinitionPartnerSummary();
                partnerSummary.Name = partner.Name;
                partnerSummary.Percentage = partner.Percentage;
                partnerSummary.Amount = partner.Percentage * entity.Amount / 100;
                list.Add(partnerSummary);
            }

            model.Partners = list;

            return PartialView("~/Views/Promissory/PromissoryView.cshtml", model);
        }

        public ActionResult Edit(int id)
        {
            var entity = _promissoryService.GetPromissorysById(id);
            var model = PromissoryVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlClient = collections.GetCustomersSingleSelectVm();
            model.ddlClient.SelectedItem = entity.ClientId.ToString();
            model.ddlPercentages = collections.GetPercentagesSingleSelectVm();
            model.ddlPercentages.SelectedItem = entity.PercentageDefinitionId.ToString();

            PercentageDefinition percentage = _partnerService.GetPercentageDefinitionById(entity.PercentageDefinitionId);

            List<PercentageDefinitionPartnerSummary> list = new List<PercentageDefinitionPartnerSummary>();

            foreach (PercentageDefinitionPartner partner in percentage.Partners)
            {
                PercentageDefinitionPartnerSummary partnerSummary = new PercentageDefinitionPartnerSummary();
                partnerSummary.Name = partner.Name;
                partnerSummary.Percentage = partner.Percentage;
                partnerSummary.Amount = partner.Percentage * entity.Amount / 100;
                list.Add(partnerSummary);
            }

            model.Partners = list;
            Session["SessionPromissory"] = model;
            return PartialView("~/Views/Promissory/PromissoryEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(PromissoryVm model)
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

                PromissoryVm principal = (PromissoryVm)Session["SessionPromissory"];
                Promissory promissory = model.ToModelObject();
                promissory.UserId = _profileProvider.CurrentUserId;
                promissory.Description = principal.Description;
                _promissoryService.UpdatePromissory(promissory);
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

        public ActionResult Partners(int Percentage, double Amount)
        {
            PromissoryVm principal = (PromissoryVm)Session["SessionPromissory"];

            PercentageDefinition percentage = _partnerService.GetPercentageDefinitionById(Percentage);

            List<PercentageDefinitionPartnerSummary> list = new List<PercentageDefinitionPartnerSummary>();

            foreach(PercentageDefinitionPartner partner in percentage.Partners)
            {
                PercentageDefinitionPartnerSummary partnerSummary = new PercentageDefinitionPartnerSummary();
                partnerSummary.Name = partner.Name;
                partnerSummary.Percentage = partner.Percentage;
                partnerSummary.Amount = partner.Percentage * Amount / 100;
                list.Add(partnerSummary);
            }

                principal.Partners = list;

                Session["SessionPromissory"] = principal;

                return PartialView("~/Views/Promissory/PartnersTable.cshtml", principal);
      
        }

    }
}