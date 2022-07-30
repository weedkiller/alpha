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
    public class IncomePartnerController : Controller
    {
        private IncomePartnerService _incomePartnerService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProfileProvider _profileProvider;
        private PaymentMethodsService _paymentMethodsServvice;
        private PartnerService _partnerService;
        private ContributionAllocationService _contributionAllocationService;      
        private CashierService _cashierService;

        public IncomePartnerController()
        {
            _incomePartnerService = new IncomePartnerService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _profileProvider = new ProfileProvider();
            _paymentMethodsServvice = new PaymentMethodsService();
            _partnerService = new PartnerService();
            _contributionAllocationService = new ContributionAllocationService();
            _cashierService = new CashierService();
        }

        // GET: /IncomePartner/
        [LogActionFilter]
        [Description("IncomePartners")]
        public ActionResult Index()
        {
            IncomePartnerFilterVm model = new IncomePartnerFilterVm();
            var collections = new CollectionHelper();
            model.ListPartner = new SelectList(_partnerService.GetPartners(), "PartnerId", "Name");
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");           
            return PartialView("~/Views/IncomePartner/IncomePartner.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedPartner, int? SelectedUser, string DateFromSystem, string DateToSystem, string DateFrom, string DateTo, string AmountFrom, string AmountTo)
        {
            var model = IncomePartnerTableVm.ToViewModel(_incomePartnerService.GetIncomePartnersSummary(SelectedPartner, SelectedUser, DateFromSystem, DateToSystem, DateFrom, DateTo, AmountFrom, AmountTo));
            return PartialView("~/Views/IncomePartner/IncomePartnerTable.cshtml", model);
        }


        [LogActionFilter]
        [Description("Crear IncomePartners")]
        public ActionResult Create()
        {
            var model = new IncomePartnerVm();
            var collections = new CollectionHelper();
            List<IncomePartnerAllocation> lists = new List<IncomePartnerAllocation>();
            model.Allocations = lists;
            List<IncomePartnerPaymentMethod> payment = new List<IncomePartnerPaymentMethod>();
            model.PaymentMethods = payment;
            model.Date = DateTime.Now.ToShortDateString();
            model.ddlPartner = collections.GetPartnersSingleSelectVm();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            model.Saving = _cashierService.GetCashiersById(_userService.GetUserCashierByUserId(_profileProvider.CurrentUserId).CashierId).Name;
            Session["SessionIncomePartner"] = model;
            Session["PMs"] = new List<int>();
            return PartialView("~/Views/IncomePartner/IncomePartnerCreate.cshtml", model);
        }


        public ActionResult IndexAllocations()
        {
            IncomePartnerVm model = (IncomePartnerVm)Session["SessionIncomePartner"];
            ViewBag.ErrorMessageTrabajo = "false";
            model.AllocationsAll = new List<ContributionAllocationPartner>(); 

            return PartialView("~/Views/IncomePartner/AllocationsTable.cshtml", model);
        }

        public ActionResult AllocationsByPartner(int partnerId)
        {
            IncomePartnerVm model = (IncomePartnerVm)Session["SessionIncomePartner"];
            ViewBag.ErrorMessageTrabajo = "false";
            model.AllocationsAll = _contributionAllocationService.GetContributionAllocationPartnersByParnerId(partnerId);
            return PartialView("~/Views/IncomePartner/AllocationsTable.cshtml", model);
        }

        public ActionResult IndexPaymentMethod()
        {
            IncomePartnerVm model = (IncomePartnerVm)Session["SessionIncomePartner"];
            ViewBag.ErrorMessagePayment = "false";
            return PartialView("~/Views/IncomePartner/PaymentMethodTable.cshtml", model);
        }

        public ActionResult PaymentMethod(int? PaymentMethod, double Amount)
        {
            bool existe = false;
            IncomePartnerVm principal = (IncomePartnerVm)Session["SessionIncomePartner"];
            if (PaymentMethod.HasValue)
            {
                IncomePartnerPaymentMethod payment = new IncomePartnerPaymentMethod();
                payment.PaymentMethodId = PaymentMethod.Value;
                payment.PaymentMethod = _paymentMethodsServvice.GetPaymentMethodssById(PaymentMethod.Value).Name;
                payment.Amount = Amount;

                List<int> listPM = (List<int>)Session["PMs"];
                listPM.Add(PaymentMethod.Value);
                Session["PMs"] = listPM;

                List<IncomePartnerPaymentMethod> list = (List<IncomePartnerPaymentMethod>)principal.PaymentMethods;
                Int32 id = PaymentMethod.Value;
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                if (item != null)
                    existe = true;
                else
                    list.Add(payment);
                principal.PaymentMethods = list;

                Session["SessionIncomePartner"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessagePayment = "false";
                    return PartialView("~/Views/IncomePartner/PaymentMethodTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessagePayment = "true";
                    return PartialView("~/Views/IncomePartner/PaymentMethodTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un método de pago");
            }
        }

        public ActionResult RemovePaymentMethod(int Id)
        {
            IncomePartnerVm principal = (IncomePartnerVm)Session["SessionIncomePartner"];
            List<IncomePartnerPaymentMethod> list = (List<IncomePartnerPaymentMethod>)principal.PaymentMethods;
            var item = list.FirstOrDefault(x => x.PaymentMethodId == Id);
            list.Remove(item);

            principal.PaymentMethods = list;

            Session["SessionIncomePartner"] = principal;

            Session["esNuevo"] = false;
            ViewBag.ErrorMessagePayment = "false";
            return PartialView("~/Views/IncomePartner/PaymentMethodTable.cshtml", principal);
        }
        public ActionResult PaymentMethods()
        {
            IncomePartnerVm principal = (IncomePartnerVm)Session["SessionIncomePartner"];

            Session["PMs"] = new List<int>();
            return PartialView("~/Views/IncomePartner/PaymentMethodTable.cshtml", principal);
        }

        public ActionResult CancelPaymentMethods()
        {
            IncomePartnerVm principal = (IncomePartnerVm)Session["SessionIncomePartner"];


            List<IncomePartnerPaymentMethod> list = (List<IncomePartnerPaymentMethod>)principal.PaymentMethods;
            foreach(int id in (List<int>)Session["PMs"])
            {
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                list.Remove(item);
            }
           
            principal.PaymentMethods = list;
            Session["PMs"] = new List<int>();
            Session["SessionIncomePartner"] = principal;

            ViewBag.ErrorMessagePayment = "false";

            return PartialView("~/Views/IncomePartner/PaymentMethodTable.cshtml", principal);
        }

       
        [HttpPost]
        public ActionResult Create(IncomePartnerVm model)
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
                IncomePartnerVm principal = (IncomePartnerVm)Session["SessionIncomePartner"];
                model.Allocations = principal.Allocations;
                model.PaymentMethods = principal.PaymentMethods;

                if (model.Allocations.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos una asignación.");
                }

                if(model.PaymentMethods.Count() == 0 && model.ddlPaymentMethods == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar al menos un medio de Pago.");
                }
                else if (model.PaymentMethods.Count() == 0)
                {
                    IncomePartnerPaymentMethod pm = new IncomePartnerPaymentMethod();
                    pm.Amount = model.Amount;
                    pm.PaymentMethodId = int.Parse(model.ddlPaymentMethods.SelectedItem);
                    List<IncomePartnerPaymentMethod> listPm = (List<IncomePartnerPaymentMethod>)model.PaymentMethods;
                    listPm.Add(pm);
                    model.PaymentMethods = listPm;                 
                }

                var incomePartner = model.ToModelObject();
                double totalpago = 0;
                foreach (IncomePartnerPaymentMethod pay in model.PaymentMethods)
                {
                    totalpago = totalpago + pay.Amount;
                }

                if (totalpago != model.Amount)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La suma de los totales de los medios de pago debe ser igual al total del Gasto.");

                incomePartner.UserId = _profileProvider.CurrentUserId;

                _incomePartnerService.AddIncomePartner(incomePartner);

                return Index();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        public ActionResult CalculateAmount(int[] list)
        {
            bool existe = false;
            IncomePartnerVm principal = (IncomePartnerVm)Session["SessionIncomePartner"];
            if (list != null && list.Length != 0)
            {
                double total = 0;
                List<IncomePartnerAllocation> lista = new List<IncomePartnerAllocation>();
                for (int i = 0; i < list.Length; i++)
                {
                    IncomePartnerAllocation prom = new IncomePartnerAllocation();
                    prom.ContributionAllocationPartnerId = list[i];
                        lista.Add(prom);
                }

                principal.Allocations = lista;

                principal.Amount = total;
                Session["Total"] = total;
                principal.AmountPayment = total;
                Session["SessionIncomePartner"] = principal;
            }
            return PartialView("~/Views/IncomePartner/PaymentMethodTable.cshtml", principal);
        }

        public ActionResult View(int id)
        {
            var entity = _incomePartnerService.GetIncomePartnersById(id);
            var model = IncomePartnerVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlPartner = collections.GetPartnersSingleSelectVm();
            model.ddlPartner.SelectedItem = entity.PartnerId.ToString();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();

            return PartialView("~/Views/IncomePartner/IncomePartnerView.cshtml", model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _incomePartnerService.DeleteIncomePartner(id);
                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
    }
}