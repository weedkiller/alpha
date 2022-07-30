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
    public class PromissorySurrenderController : Controller
    {
        private PromissorySurrenderService _promissorySurrenderService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProfileProvider _profileProvider;
        private PaymentMethodsService _paymentMethodsServvice;
        private PartnerService _partnerService;
        private PromissoryService _promissoryService;
        private CashierService _cashierService;

        public PromissorySurrenderController()
        {
            _promissorySurrenderService = new PromissorySurrenderService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _profileProvider = new ProfileProvider();
            _paymentMethodsServvice = new PaymentMethodsService();
            _partnerService = new PartnerService();
            _promissoryService = new PromissoryService();
            _cashierService = new CashierService();
        }

        // GET: /PromissorySurrender/
        [LogActionFilter]
        [Description("PromissorySurrenders")]
        public ActionResult Index()
        {
            PromissorySurrenderFilterVm model = new PromissorySurrenderFilterVm();
            var collections = new CollectionHelper();
            model.ListPartner = new SelectList(_partnerService.GetPartners(), "PartnerId", "Name");
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");           
            return PartialView("~/Views/PromissorySurrender/PromissorySurrender.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedPartner, int? SelectedUser, string DateFromPayment, string DateToPayment, string AmountFrom, string AmountTo, string isPaid)
        {
            var model = PromissorySurrenderTableVm.ToViewModel(_promissorySurrenderService.GetPromissorySurrendersPartnerSummary(SelectedPartner, SelectedUser, DateFromPayment, DateToPayment, AmountFrom, AmountTo, isPaid));
            return PartialView("~/Views/PromissorySurrender/PromissorySurrenderTable.cshtml", model);
        }



        [LogActionFilter]
        [Description("Crear PromissorySurrenders")]
        public ActionResult Create()
        {
            var model = new PromissorySurrenderVm();
            var collections = new CollectionHelper();
            List<PromissoryRendered> lists = new List<PromissoryRendered>();
            model.Promissories = lists;
            List<PromissorySurrenderMethod> payment = new List<PromissorySurrenderMethod>();
            model.PaymentMethods = payment;
            model.PaymentDate = DateTime.Now.ToShortDateString();
            model.ddlPartner = collections.GetPartnersSingleSelectVm();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            model.Saving = _cashierService.GetCashiersById(_userService.GetUserCashierByUserId(_profileProvider.CurrentUserId).CashierId).Name;
            Session["SessionPromissorySurrender"] = model;
            Session["PMs"] = new List<int>();
            return PartialView("~/Views/PromissorySurrender/PromissorySurrenderCreate.cshtml", model);
        }


        public ActionResult IndexPromissories()
        {
            PromissorySurrenderVm model = (PromissorySurrenderVm)Session["SessionPromissorySurrender"];
            ViewBag.ErrorMessageTrabajo = "false";
            model.PromissoriesAll = new List<Promissory>(); 
            // model.PromissoriesAll = _promissoryService.GetPromissoriesNotPaid();
            return PartialView("~/Views/PromissorySurrender/PromissoriesTable.cshtml", model);
        }

        public ActionResult PromissoriesByPartner(int partnerId)
        {
            PromissorySurrenderVm model = (PromissorySurrenderVm)Session["SessionPromissorySurrender"];
            ViewBag.ErrorMessageTrabajo = "false";
            model.PromissoriesAll = _promissoryService.GetPromissoriesByPartnerNotPaid(partnerId);
            return PartialView("~/Views/PromissorySurrender/PromissoriesTable.cshtml", model);
        }

        public ActionResult IndexPaymentMethod()
        {
            PromissorySurrenderVm model = (PromissorySurrenderVm)Session["SessionPromissorySurrender"];
            ViewBag.ErrorMessagePayment = "false";
            return PartialView("~/Views/PromissorySurrender/PaymentMethodTable.cshtml", model);
        }

        public ActionResult PaymentMethod(int? PaymentMethod, double Amount)
        {
            bool existe = false;
            PromissorySurrenderVm principal = (PromissorySurrenderVm)Session["SessionPromissorySurrender"];
            if (PaymentMethod.HasValue)
            {
                PromissorySurrenderMethod payment = new PromissorySurrenderMethod();
                payment.PaymentMethodId = PaymentMethod.Value;
                payment.PaymentMethod = _paymentMethodsServvice.GetPaymentMethodssById(PaymentMethod.Value).Name;
                payment.Amount = Amount;

                List<int> listPM = (List<int>)Session["PMs"];
                listPM.Add(PaymentMethod.Value);
                Session["PMs"] = listPM;

                List<PromissorySurrenderMethod> list = (List<PromissorySurrenderMethod>)principal.PaymentMethods;
                Int32 id = PaymentMethod.Value;
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                if (item != null)
                    existe = true;
                else
                    list.Add(payment);
                principal.PaymentMethods = list;

                Session["SessionPromissorySurrender"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessagePayment = "false";
                    return PartialView("~/Views/PromissorySurrender/PaymentMethodTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessagePayment = "true";
                    return PartialView("~/Views/PromissorySurrender/PaymentMethodTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un método de pago");
            }
        }

        public ActionResult RemovePaymentMethod(int Id)
        {
            PromissorySurrenderVm principal = (PromissorySurrenderVm)Session["SessionPromissorySurrender"];
            List<PromissorySurrenderMethod> list = (List<PromissorySurrenderMethod>)principal.PaymentMethods;
            var item = list.FirstOrDefault(x => x.PaymentMethodId == Id);
            list.Remove(item);

            principal.PaymentMethods = list;

            Session["SessionPromissorySurrender"] = principal;

            Session["esNuevo"] = false;
            ViewBag.ErrorMessagePayment = "false";
            return PartialView("~/Views/PromissorySurrender/PaymentMethodTable.cshtml", principal);
        }
        public ActionResult PaymentMethods()
        {
            PromissorySurrenderVm principal = (PromissorySurrenderVm)Session["SessionPromissorySurrender"];

            Session["PMs"] = new List<int>();
            return PartialView("~/Views/PromissorySurrender/PaymentMethodTable.cshtml", principal);
        }

        public ActionResult CancelPaymentMethods()
        {
            PromissorySurrenderVm principal = (PromissorySurrenderVm)Session["SessionPromissorySurrender"];


            List<PromissorySurrenderMethod> list = (List<PromissorySurrenderMethod>)principal.PaymentMethods;
            foreach(int id in (List<int>)Session["PMs"])
            {
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                list.Remove(item);
            }
           
            principal.PaymentMethods = list;
            Session["PMs"] = new List<int>();
            Session["SessionPromissorySurrender"] = principal;

            ViewBag.ErrorMessagePayment = "false";

            return PartialView("~/Views/PromissorySurrender/PaymentMethodTable.cshtml", principal);
        }

       
        [HttpPost]
        public ActionResult Create(PromissorySurrenderVm model)
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
                PromissorySurrenderVm principal = (PromissorySurrenderVm)Session["SessionPromissorySurrender"];
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
                    PromissorySurrenderMethod pm = new PromissorySurrenderMethod();
                    pm.Amount = model.Amount;
                    pm.PaymentMethodId = int.Parse(model.ddlPaymentMethods.SelectedItem);
                    List<PromissorySurrenderMethod> listPm = (List<PromissorySurrenderMethod>)model.PaymentMethods;
                    listPm.Add(pm);
                    model.PaymentMethods = listPm;                 
                }

                var promissorySurrender = model.ToModelObject();
                double totalpago = 0;
                foreach (PromissorySurrenderMethod pay in model.PaymentMethods)
                {
                    totalpago = totalpago + pay.Amount;
                }

                if (totalpago != model.Amount)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La suma de los totales de los medios de pago debe ser igual al total de la rendición.");

                promissorySurrender.UserId = _profileProvider.CurrentUserId;

                _promissorySurrenderService.AddPromissorySurrender(promissorySurrender);

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
            PromissorySurrenderVm principal = (PromissorySurrenderVm)Session["SessionPromissorySurrender"];
            if (list != null && list.Length != 0)
            {
                double total = 0;
                List<PromissoryRendered> lista = new List<PromissoryRendered>();
                for (int i = 0; i < list.Length; i++)
                {
                    PromissoryRendered prom = new PromissoryRendered();
                    prom.PromissoryId = list[i];
                        lista.Add(prom);
                }

                principal.Promissories = lista;

                principal.Amount = total;
                Session["Total"] = total;
                principal.AmountPayment = total;
                Session["SessionPromissorySurrender"] = principal;
            }
            return PartialView("~/Views/PromissorySurrender/PaymentMethodTable.cshtml", principal);
        }

        public ActionResult View(int id)
        {
            var entity = _promissorySurrenderService.GetPromissorySurrendersById(id);
            var model = PromissorySurrenderVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlPartner = collections.GetPartnersSingleSelectVm();
            model.ddlPartner.SelectedItem = entity.PartnerId.ToString();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();


            return PartialView("~/Views/PromissorySurrender/PromissorySurrenderView.cshtml", model);
        }      
    }
}