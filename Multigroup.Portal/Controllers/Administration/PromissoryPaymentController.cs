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
    public class PromissoryPaymentController : Controller
    {
        private PromissoryPaymentService _promissoryPaymentService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProfileProvider _profileProvider;
        private PaymentMethodsService _paymentMethodsServvice;
        private CustomerService _customerService;
        private PromissoryService _promissoryService;
        private PromissorySurrenderService _promissorySurrenderService;
        private CashierService _cashierService;

        public PromissoryPaymentController()
        {
            _promissoryPaymentService = new PromissoryPaymentService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _profileProvider = new ProfileProvider();
            _paymentMethodsServvice = new PaymentMethodsService();
            _customerService = new CustomerService();
            _promissoryService = new PromissoryService();
            _promissorySurrenderService = new PromissorySurrenderService();
            _cashierService = new CashierService();
        }

        // GET: /PromissoryPayment/
        [LogActionFilter]
        [Description("PromissoryPayments")]
        public ActionResult Index()
        {
            PromissoryPaymentFilterVm model = new PromissoryPaymentFilterVm();
            var collections = new CollectionHelper();
            model.ListCustomer = new SelectList(_customerService.GetCustomersComplete(), "CustomerId", "Name");
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");           
            return PartialView("~/Views/PromissoryPayment/PromissoryPayment.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedClient, int? SelectedUser, string DateFromSystem, string DateToSystem, string DateFromPayment, string DateToPayment, string AmountFrom, string AmountTo)
        {
            var model = PromissoryPaymentTableVm.ToViewModel(_promissoryPaymentService.GetPromissoryPaymentsSummary(SelectedClient, SelectedUser, DateFromSystem, DateToSystem, DateFromPayment, DateToPayment, AmountFrom, AmountTo));
            return PartialView("~/Views/PromissoryPayment/PromissoryPaymentTable.cshtml", model);
        }

        public ActionResult GetTableDetail(int? SelectedClient, int? SelectedUser, string DateFromSystem, string DateToSystem, string DateFromPayment, string DateToPayment, string AmountFrom, string AmountTo)
        {
            var model = PromissoryPaymentDetailTableVm.ToViewModel(_promissoryPaymentService.GetPromissoryPaymentPromissories(SelectedClient, SelectedUser, DateFromSystem, DateToSystem, DateFromPayment, DateToPayment, AmountFrom, AmountTo));
            return PartialView("~/Views/PromissoryPayment/PromissoryPaymentTableDetail.cshtml", model);
        }


        [LogActionFilter]
        [Description("Crear PromissoryPayments")]
        public ActionResult Create()
        {
            var model = new PromissoryPaymentVm();
            var collections = new CollectionHelper();
            List<PromissoryPaymentPromissory> lists = new List<PromissoryPaymentPromissory>();
            model.Promissories = lists;
            List<PromissoryPaymentMethod> payment = new List<PromissoryPaymentMethod>();
            model.PaymentMethods = payment;
            model.PaymentDate = DateTime.Now.ToShortDateString();
            model.ddlClient = collections.GetCustomersSingleSelectVm();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            model.Saving = _cashierService.GetCashiersById(_userService.GetUserCashierByUserId(_profileProvider.CurrentUserId).CashierId).Name;
            Session["SessionPromissoryPayment"] = model;
            Session["PMs"] = new List<int>();
            return PartialView("~/Views/PromissoryPayment/PromissoryPaymentCreate.cshtml", model);
        }


        public ActionResult IndexPromissories()
        {
            PromissoryPaymentVm model = (PromissoryPaymentVm)Session["SessionPromissoryPayment"];
            ViewBag.ErrorMessageTrabajo = "false";
            model.PromissoriesAll = new List<Promissory>(); 
            // model.PromissoriesAll = _promissoryService.GetPromissoriesNotPaid();
            return PartialView("~/Views/PromissoryPayment/PromissoriesTable.cshtml", model);
        }

        public ActionResult PromissoriesByClient(int clientId)
        {
            PromissoryPaymentVm model = (PromissoryPaymentVm)Session["SessionPromissoryPayment"];
            ViewBag.ErrorMessageTrabajo = "false";
            model.PromissoriesAll = _promissoryService.GetPromissoriesNotPaid(clientId);
            return PartialView("~/Views/PromissoryPayment/PromissoriesTable.cshtml", model);
        }

        public ActionResult IndexPaymentMethod()
        {
            PromissoryPaymentVm model = (PromissoryPaymentVm)Session["SessionPromissoryPayment"];
            ViewBag.ErrorMessagePayment = "false";
            return PartialView("~/Views/PromissoryPayment/PaymentMethodTable.cshtml", model);
        }

        public ActionResult PaymentMethod(int? PaymentMethod, double Amount)
        {
            bool existe = false;
            PromissoryPaymentVm principal = (PromissoryPaymentVm)Session["SessionPromissoryPayment"];
            if (PaymentMethod.HasValue)
            {
                PromissoryPaymentMethod payment = new PromissoryPaymentMethod();
                payment.PaymentMethodId = PaymentMethod.Value;
                payment.PaymentMethod = _paymentMethodsServvice.GetPaymentMethodssById(PaymentMethod.Value).Name;
                payment.Amount = Amount;

                List<int> listPM = (List<int>)Session["PMs"];
                listPM.Add(PaymentMethod.Value);
                Session["PMs"] = listPM;

                List<PromissoryPaymentMethod> list = (List<PromissoryPaymentMethod>)principal.PaymentMethods;
                Int32 id = PaymentMethod.Value;
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                if (item != null)
                    existe = true;
                else
                    list.Add(payment);
                principal.PaymentMethods = list;

                Session["SessionPromissoryPayment"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessagePayment = "false";
                    return PartialView("~/Views/PromissoryPayment/PaymentMethodTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessagePayment = "true";
                    return PartialView("~/Views/PromissoryPayment/PaymentMethodTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un método de pago");
            }
        }

        public ActionResult RemovePaymentMethod(int Id)
        {
            PromissoryPaymentVm principal = (PromissoryPaymentVm)Session["SessionPromissoryPayment"];
            List<PromissoryPaymentMethod> list = (List<PromissoryPaymentMethod>)principal.PaymentMethods;
            var item = list.FirstOrDefault(x => x.PaymentMethodId == Id);
            list.Remove(item);

            principal.PaymentMethods = list;

            Session["SessionPromissoryPayment"] = principal;

            Session["esNuevo"] = false;
            ViewBag.ErrorMessagePayment = "false";
            return PartialView("~/Views/PromissoryPayment/PaymentMethodTable.cshtml", principal);
        }
        public ActionResult PaymentMethods()
        {
            PromissoryPaymentVm principal = (PromissoryPaymentVm)Session["SessionPromissoryPayment"];

            Session["PMs"] = new List<int>();
            return PartialView("~/Views/PromissoryPayment/PaymentMethodTable.cshtml", principal);
        }

        public ActionResult CancelPaymentMethods()
        {
            PromissoryPaymentVm principal = (PromissoryPaymentVm)Session["SessionPromissoryPayment"];


            List<PromissoryPaymentMethod> list = (List<PromissoryPaymentMethod>)principal.PaymentMethods;
            foreach(int id in (List<int>)Session["PMs"])
            {
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                list.Remove(item);
            }
           
            principal.PaymentMethods = list;
            Session["PMs"] = new List<int>();
            Session["SessionPromissoryPayment"] = principal;

            ViewBag.ErrorMessagePayment = "false";

            return PartialView("~/Views/PromissoryPayment/PaymentMethodTable.cshtml", principal);
        }

       
        [HttpPost]
        public ActionResult Create(PromissoryPaymentVm model)
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
                PromissoryPaymentVm principal = (PromissoryPaymentVm)Session["SessionPromissoryPayment"];
                model.Promissories = principal.Promissories;
                model.PaymentMethods = principal.PaymentMethods;

                if (model.Promissories.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Pagaré.");
                }

                if(model.PaymentMethods.Count() == 0 && model.ddlPaymentMethods == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar al menos un medio de Pago.");
                }
                else if (model.PaymentMethods.Count() == 0)
                {
                    PromissoryPaymentMethod pm = new PromissoryPaymentMethod();
                    pm.Amount = model.Amount;
                    pm.PaymentMethodId = int.Parse(model.ddlPaymentMethods.SelectedItem);
                    List<PromissoryPaymentMethod> listPm = (List<PromissoryPaymentMethod>)model.PaymentMethods;
                    listPm.Add(pm);
                    model.PaymentMethods = listPm;                 
                }

                var promissoryPayment = model.ToModelObject();
                double totalpago = 0;
                foreach (PromissoryPaymentMethod pay in model.PaymentMethods)
                {
                    totalpago = totalpago + pay.Amount;
                }

                if (totalpago != model.Amount)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La suma de los totales de los medios de pago debe ser igual al total del Gasto.");

                promissoryPayment.UserId = _profileProvider.CurrentUserId;

                _promissoryPaymentService.AddPromissoryPayment(promissoryPayment);

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
            PromissoryPaymentVm principal = (PromissoryPaymentVm)Session["SessionPromissoryPayment"];
            if (list != null && list.Length != 0)
            {
                double total = 0;
                List<PromissoryPaymentPromissory> lista = new List<PromissoryPaymentPromissory>();
                for (int i = 0; i < list.Length; i++)
                {
                    PromissoryPaymentPromissory prom = new PromissoryPaymentPromissory();
                    prom.PromissoryId = list[i];
                        lista.Add(prom);
                }

                principal.Promissories = lista;

                principal.Amount = total;
                Session["Total"] = total;
                principal.AmountPayment = total;
                Session["SessionPromissoryPayment"] = principal;
            }
            return PartialView("~/Views/PromissoryPayment/PaymentMethodTable.cshtml", principal);
        }

        public ActionResult View(int id)
        {
            var entity = _promissoryPaymentService.GetPromissoryPaymentsById(id);
            var model = PromissoryPaymentVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlClient = collections.GetCustomersSingleSelectVm();
            model.ddlClient.SelectedItem = entity.ClientId.ToString();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();

            return PartialView("~/Views/PromissoryPayment/PromissoryPaymentView.cshtml", model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var surrenderPartner = _promissoryPaymentService.GetPromissorySurrenderPartnerPaidByPayment(id);
                if(surrenderPartner.Count() > 1)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El cobro ya tiene una rendición");

                _promissoryPaymentService.DeletePromissoryPayment(id);
                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
    }
}