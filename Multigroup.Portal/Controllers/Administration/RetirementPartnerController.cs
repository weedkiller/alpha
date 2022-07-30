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
    public class RetirementPartnerController : Controller
    {
        private RetirementPartnerService _retirementPartnerService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProfileProvider _profileProvider;
        private PaymentMethodsService _paymentMethodsServvice;
        private PartnerService _partnerService;
        private EarningsAllocationService _earningsAllocationService;      
        private CashierService _cashierService;

        public RetirementPartnerController()
        {
            _retirementPartnerService = new RetirementPartnerService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _profileProvider = new ProfileProvider();
            _paymentMethodsServvice = new PaymentMethodsService();
            _partnerService = new PartnerService();
            _earningsAllocationService = new EarningsAllocationService();
            _cashierService = new CashierService();
        }

        // GET: /RetirementPartner/
        [LogActionFilter]
        [Description("RetirementPartners")]
        public ActionResult Index()
        {
            RetirementPartnerFilterVm model = new RetirementPartnerFilterVm();
            var collections = new CollectionHelper();
            model.ListPartner = new SelectList(_partnerService.GetPartners(), "PartnerId", "Name");
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");           
            return PartialView("~/Views/RetirementPartner/RetirementPartner.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedPartner, int? SelectedUser, string DateFromSystem, string DateToSystem, string DateFrom, string DateTo, string AmountFrom, string AmountTo)
        {
            var model = RetirementPartnerTableVm.ToViewModel(_retirementPartnerService.GetRetirementPartnersSummary(SelectedPartner, SelectedUser, DateFromSystem, DateToSystem, DateFrom, DateTo, AmountFrom, AmountTo));
            return PartialView("~/Views/RetirementPartner/RetirementPartnerTable.cshtml", model);
        }


        [LogActionFilter]
        [Description("Crear RetirementPartners")]
        public ActionResult Create()
        {
            var model = new RetirementPartnerVm();
            var collections = new CollectionHelper();
            List<RetirementPartnerAllocation> lists = new List<RetirementPartnerAllocation>();
            model.Allocations = lists;
            List<RetirementPartnerPaymentMethod> payment = new List<RetirementPartnerPaymentMethod>();
            model.PaymentMethods = payment;
            model.Date = DateTime.Now.ToShortDateString();
            model.ddlPartner = collections.GetPartnersSingleSelectVm();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            model.Saving = _cashierService.GetCashiersById(_userService.GetUserCashierByUserId(_profileProvider.CurrentUserId).CashierId).Name;
            Session["SessionRetirementPartner"] = model;
            Session["PMs"] = new List<int>();
            return PartialView("~/Views/RetirementPartner/RetirementPartnerCreate.cshtml", model);
        }


        public ActionResult IndexAllocations()
        {
            RetirementPartnerVm model = (RetirementPartnerVm)Session["SessionRetirementPartner"];
            ViewBag.ErrorMessageTrabajo = "false";
            model.AllocationsAll = new List<EarningsAllocationPartner>(); 

            return PartialView("~/Views/RetirementPartner/AllocationsTable.cshtml", model);
        }

        public ActionResult AllocationsByPartner(int partnerId)
        {
            RetirementPartnerVm model = (RetirementPartnerVm)Session["SessionRetirementPartner"];
            ViewBag.ErrorMessageTrabajo = "false";
            model.AllocationsAll = _earningsAllocationService.GetEarningsAllocationPartnersByParnerId(partnerId);
            return PartialView("~/Views/RetirementPartner/AllocationsTable.cshtml", model);
        }

        public ActionResult IndexPaymentMethod()
        {
            RetirementPartnerVm model = (RetirementPartnerVm)Session["SessionRetirementPartner"];
            ViewBag.ErrorMessagePayment = "false";
            return PartialView("~/Views/RetirementPartner/PaymentMethodTable.cshtml", model);
        }

        public ActionResult PaymentMethod(int? PaymentMethod, double Amount)
        {
            bool existe = false;
            RetirementPartnerVm principal = (RetirementPartnerVm)Session["SessionRetirementPartner"];
            if (PaymentMethod.HasValue)
            {
                RetirementPartnerPaymentMethod payment = new RetirementPartnerPaymentMethod();
                payment.PaymentMethodId = PaymentMethod.Value;
                payment.PaymentMethod = _paymentMethodsServvice.GetPaymentMethodssById(PaymentMethod.Value).Name;
                payment.Amount = Amount;

                List<int> listPM = (List<int>)Session["PMs"];
                listPM.Add(PaymentMethod.Value);
                Session["PMs"] = listPM;

                List<RetirementPartnerPaymentMethod> list = (List<RetirementPartnerPaymentMethod>)principal.PaymentMethods;
                Int32 id = PaymentMethod.Value;
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                if (item != null)
                    existe = true;
                else
                    list.Add(payment);
                principal.PaymentMethods = list;

                Session["SessionRetirementPartner"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessagePayment = "false";
                    return PartialView("~/Views/RetirementPartner/PaymentMethodTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessagePayment = "true";
                    return PartialView("~/Views/RetirementPartner/PaymentMethodTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un método de pago");
            }
        }

        public ActionResult RemovePaymentMethod(int Id)
        {
            RetirementPartnerVm principal = (RetirementPartnerVm)Session["SessionRetirementPartner"];
            List<RetirementPartnerPaymentMethod> list = (List<RetirementPartnerPaymentMethod>)principal.PaymentMethods;
            var item = list.FirstOrDefault(x => x.PaymentMethodId == Id);
            list.Remove(item);

            principal.PaymentMethods = list;

            Session["SessionRetirementPartner"] = principal;

            Session["esNuevo"] = false;
            ViewBag.ErrorMessagePayment = "false";
            return PartialView("~/Views/RetirementPartner/PaymentMethodTable.cshtml", principal);
        }
        public ActionResult PaymentMethods()
        {
            RetirementPartnerVm principal = (RetirementPartnerVm)Session["SessionRetirementPartner"];

            Session["PMs"] = new List<int>();
            return PartialView("~/Views/RetirementPartner/PaymentMethodTable.cshtml", principal);
        }

        public ActionResult CancelPaymentMethods()
        {
            RetirementPartnerVm principal = (RetirementPartnerVm)Session["SessionRetirementPartner"];


            List<RetirementPartnerPaymentMethod> list = (List<RetirementPartnerPaymentMethod>)principal.PaymentMethods;
            foreach(int id in (List<int>)Session["PMs"])
            {
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                list.Remove(item);
            }
           
            principal.PaymentMethods = list;
            Session["PMs"] = new List<int>();
            Session["SessionRetirementPartner"] = principal;

            ViewBag.ErrorMessagePayment = "false";

            return PartialView("~/Views/RetirementPartner/PaymentMethodTable.cshtml", principal);
        }

       
        [HttpPost]
        public ActionResult Create(RetirementPartnerVm model)
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
                RetirementPartnerVm principal = (RetirementPartnerVm)Session["SessionRetirementPartner"];
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
                    RetirementPartnerPaymentMethod pm = new RetirementPartnerPaymentMethod();
                    pm.Amount = model.Amount;
                    pm.PaymentMethodId = int.Parse(model.ddlPaymentMethods.SelectedItem);
                    List<RetirementPartnerPaymentMethod> listPm = (List<RetirementPartnerPaymentMethod>)model.PaymentMethods;
                    listPm.Add(pm);
                    model.PaymentMethods = listPm;                 
                }

                var retirementPartner = model.ToModelObject();
                double totalpago = 0;
                foreach (RetirementPartnerPaymentMethod pay in model.PaymentMethods)
                {
                    totalpago = totalpago + pay.Amount;
                }

                if (totalpago != model.Amount)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La suma de los totales de los medios de pago debe ser igual al total del Gasto.");

                retirementPartner.UserId = _profileProvider.CurrentUserId;

                _retirementPartnerService.AddRetirementPartner(retirementPartner);

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
            RetirementPartnerVm principal = (RetirementPartnerVm)Session["SessionRetirementPartner"];
            if (list != null && list.Length != 0)
            {
                double total = 0;
                List<RetirementPartnerAllocation> lista = new List<RetirementPartnerAllocation>();
                for (int i = 0; i < list.Length; i++)
                {
                    RetirementPartnerAllocation prom = new RetirementPartnerAllocation();
                    prom.EarningsAllocationPartnerId = list[i];
                        lista.Add(prom);
                }

                principal.Allocations = lista;

                principal.Amount = total;
                Session["Total"] = total;
                principal.AmountPayment = total;
                Session["SessionRetirementPartner"] = principal;
            }
            return PartialView("~/Views/RetirementPartner/PaymentMethodTable.cshtml", principal);
        }

        public ActionResult View(int id)
        {
            var entity = _retirementPartnerService.GetRetirementPartnersById(id);
            var model = RetirementPartnerVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlPartner = collections.GetPartnersSingleSelectVm();
            model.ddlPartner.SelectedItem = entity.PartnerId.ToString();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();

            return PartialView("~/Views/RetirementPartner/RetirementPartnerView.cshtml", model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _retirementPartnerService.DeleteRetirementPartner(id);
                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
    }
}