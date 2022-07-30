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
    public class SpendingPaymentController : Controller
    {
        private SpendingPaymentService _SpendingPaymentService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProviderService _providerService;
        private ProfileProvider _profileProvider;
        private PurchaseRequestService _purchaseRequestService;
        private ScheduledExpenseService _scheduledExpenseService;
        private ExpenseAuthorizationService _expenseAuthorizationService;
        private PaymentMethodsService _paymentMethodsServvice;
        private SpendingService _spendingService;
        private CashierService _cashierService;

        public SpendingPaymentController()
        {
            _SpendingPaymentService = new SpendingPaymentService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _providerService = new ProviderService();
            _profileProvider = new ProfileProvider();
            _purchaseRequestService = new PurchaseRequestService();
            _scheduledExpenseService = new ScheduledExpenseService();
            _expenseAuthorizationService = new ExpenseAuthorizationService();
            _paymentMethodsServvice = new PaymentMethodsService();
            _spendingService = new SpendingService();
            _cashierService = new CashierService();
        }

        // GET: /SpendingPayment/
        [LogActionFilter]
        [Description("SpendingPayments")]
        public ActionResult Index()
        {
            SpendingPaymentFilterVm model = new SpendingPaymentFilterVm();
            var collections = new CollectionHelper();
            model.ListProvider = new SelectList(_providerService.GetProvidersWithId(), "ProviderId", "Name");
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.UserId = _profileProvider.CurrentUserId;
            Session["ImputProvider"] = null;
            Session["SpendingImputModel"] = null;
            Session["ImputSpending"] = new List<int>();
            return PartialView("~/Views/SpendingPayment/SpendingPayment.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedProvider, int? SelectedUser, string DateFromExecution, string DateToExecution, string DateFromSystem, string DateToSystem, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance)
        {
            var model = SpendingPaymentTableVm.ToViewModel(_SpendingPaymentService.GetSpendingPaymentSummary(SelectedProvider, SelectedUser, DateFromExecution, DateToExecution, DateFromSystem, DateToSystem, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance));
            return PartialView("~/Views/SpendingPayment/SpendingPaymentTable.cshtml", model);
        }

        public ActionResult GetTableDetail(int? SelectedProvider, int? SelectedUser, string DateFromExecution, string DateToExecution, string DateFromSystem, string DateToSystem, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance)
        {
            var model = SpendingPaymentDetailTableVm.ToViewModel(_SpendingPaymentService.GetSpendingPaymentDetailSummary(SelectedProvider, SelectedUser, DateFromExecution, DateToExecution, DateFromSystem, DateToSystem, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance));
            return PartialView("~/Views/SpendingPayment/SpendingPaymentTableDetail.cshtml", model);
        }


        [LogActionFilter]
        [Description("Crear SpendingPayments")]
        public ActionResult Create()
        {
            ViewBag.provider = "-10";
            var model = new SpendingPaymentVm();
            var collections = new CollectionHelper();
            List<SpendingPaymentDetail> lists = new List<SpendingPaymentDetail>();
            model.Details = lists;
            List<SpendingPaymentPaymentMethod> payment = new List<SpendingPaymentPaymentMethod>();
            model.PaymentMethods = payment;
            model.ExecutionDate = DateTime.Now.ToShortDateString();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUser.SelectedItem = _profileProvider.CurrentUserId.ToString();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            model.ddlSpendings = collections.GetSpendingsByProviderSingleSelectVm(0);
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.Saving = _cashierService.GetCashiersById(_userService.GetUserCashierByUserId(_profileProvider.CurrentUserId).CashierId).Name;
            Session["SessionSpendingPayment"] = model;
            Session["PMs"] = new List<int>();
            return PartialView("~/Views/SpendingPayment/SpendingPaymentCreate.cshtml", model);
        }

        public JsonResult GetAmoutSpending(int SpendingId)
        {
            var response = _spendingService.GetSpendingsById(SpendingId);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IndexSpendings(int ProviderId)
        {
            SpendingPaymentVm model = (SpendingPaymentVm)Session["SessionSpendingPayment"];
            var collections = new CollectionHelper();
            model.ddlSpendings = collections.GetSpendingsByProviderSingleSelectVm(ProviderId);
            List<SpendingPaymentDetail> lists = new List<SpendingPaymentDetail>();
            model.Details = lists;
            Session["SessionSpendingPayment"] = model;
            return PartialView("~/Views/SpendingPayment/SpendingsTable.cshtml", model);
        }

        public ActionResult IndexPaymentMethod()
        {
            SpendingPaymentVm model = (SpendingPaymentVm)Session["SessionSpendingPayment"];
            ViewBag.ErrorMessagePayment = "false";
            return PartialView("~/Views/SpendingPayment/PaymentMethodTable.cshtml", model);
        }


        public ActionResult Spendings(int? SpendigId, double AmountSpending)
        {
            bool existe = false;
            SpendingPaymentVm principal = (SpendingPaymentVm)Session["SessionSpendingPayment"];
            if (SpendigId.HasValue)
            {
                SpendingPaymentDetail detail = new SpendingPaymentDetail();
                detail.SpendingId = SpendigId.Value;
                detail.Total = AmountSpending;
                detail.Spending = _spendingService.GetSpendingsById(SpendigId.Value).Description;
                List<SpendingPaymentDetail> list = (List<SpendingPaymentDetail>)principal.Details;
                Int32 id = SpendigId.Value;
                var item = list.FirstOrDefault(x => x.SpendingId == id);
                if (item != null)
                    existe = true;
                else
                    list.Add(detail);
                principal.Details = list;


                double total = 0;

                foreach (SpendingPaymentDetail det in list)
                {
                    total = total + det.Total;
                }
                ViewBag.Total = total.ToString();

                Session["Total"] = total.ToString();
                principal.AmountPayment = total;
                Session["SessionSpendingPayment"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessageTrabajo = "false";
                    return PartialView("~/Views/SpendingPayment/SpendingsTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessageTrabajo = "true";
                    return PartialView("~/Views/SpendingPayment/SpendingsTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un articulo");
            }
        }

        public ActionResult SpendingsCA()
        {
            SpendingPaymentVm principal = (SpendingPaymentVm)Session["SessionSpendingPayment"];
            ViewBag.ErrorMessageTrabajo = "true";
            return PartialView("~/Views/SpendingPayment/SpendingsTable.cshtml", principal);

        }

        public ActionResult PaymentMethod(int? PaymentMethod, double Amount)
        {
            bool existe = false;
            SpendingPaymentVm principal = (SpendingPaymentVm)Session["SessionSpendingPayment"];
            if (PaymentMethod.HasValue)
            {
                SpendingPaymentPaymentMethod payment = new SpendingPaymentPaymentMethod();
                payment.PaymentMethodId = PaymentMethod.Value;
                payment.PaymentMethod = _paymentMethodsServvice.GetPaymentMethodssById(PaymentMethod.Value).Name;
                payment.Amount = Amount;

                List<int> listPM = (List<int>)Session["PMs"];
                listPM.Add(PaymentMethod.Value);
                Session["PMs"] = listPM;

                List<SpendingPaymentPaymentMethod> list = (List<SpendingPaymentPaymentMethod>)principal.PaymentMethods;
                Int32 id = PaymentMethod.Value;
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                if (item != null)
                    existe = true;
                else
                    list.Add(payment);
                principal.PaymentMethods = list;

                Session["SessionSpendingPayment"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessagePayment = "false";
                    return PartialView("~/Views/SpendingPayment/PaymentMethodTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessagePayment = "true";
                    return PartialView("~/Views/SpendingPayment/PaymentMethodTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un articulo");
            }
        }

        public ActionResult RemovePaymentMethod(int Id)
        {
            SpendingPaymentVm principal = (SpendingPaymentVm)Session["SessionSpendingPayment"];
            List<SpendingPaymentPaymentMethod> list = (List<SpendingPaymentPaymentMethod>)principal.PaymentMethods;
            var item = list.FirstOrDefault(x => x.PaymentMethodId == Id);
            list.Remove(item);

            principal.PaymentMethods = list;

            Session["SessionSpendingPayment"] = principal;

            Session["esNuevo"] = false;
            ViewBag.ErrorMessagePayment = "false";
            return PartialView("~/Views/SpendingPayment/PaymentMethodTable.cshtml", principal);
        }

        public ActionResult PaymentMethods(string SystemDate)
        {
            SpendingPaymentVm principal = (SpendingPaymentVm)Session["SessionSpendingPayment"];
            Session["PMs"] = new List<int>();
            return PartialView("~/Views/SpendingPayment/SpendingsTable.cshtml", principal);
        }

        public ActionResult CancelPaymentMethods()
        {
            SpendingPaymentVm principal = (SpendingPaymentVm)Session["SessionSpendingPayment"];


            List<SpendingPaymentPaymentMethod> list = (List<SpendingPaymentPaymentMethod>)principal.PaymentMethods;
            foreach (int id in (List<int>)Session["PMs"])
            {
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                list.Remove(item);
            }

            principal.PaymentMethods = list;
            Session["PMs"] = new List<int>();
            Session["SessionSpendingPayment"] = principal;

            ViewBag.ErrorMessagePayment = "false";

            return PartialView("~/Views/SpendingPayment/PaymentMethodTable.cshtml", principal);
        }

        public ActionResult RemovePayments(int Id)
        {
            SpendingPaymentVm principal = (SpendingPaymentVm)Session["SessionSpendingPayment"];
            List<SpendingPaymentDetail> list = (List<SpendingPaymentDetail>)principal.Details;
            var item = list.FirstOrDefault(x => x.SpendingId == Id);
            list.Remove(item);

            principal.Details = list;
            double total = 0;

            foreach (SpendingPaymentDetail det in list)
            {
                total = total + det.Total;
            }
            ViewBag.Total = total.ToString();

            Session["Total"] = total.ToString();

            Session["SessionSpendingPayment"] = principal;

            Session["esNuevo"] = false;

            return PartialView("~/Views/SpendingPayment/PaymentsTable.cshtml", principal);
        }


        [HttpPost]
        public ActionResult Create(SpendingPaymentVm model)
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
                SpendingPaymentVm principal = (SpendingPaymentVm)Session["SessionSpendingPayment"];
                model.Details = principal.Details;
                model.PaymentMethods = principal.PaymentMethods;


                if (model.Details.Count() == 0 && !model.Advancement)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Gasto si no ha checkeado la casilla 'Sin Imputación'.");
                }

                if (model.PaymentMethods.Count() == 0 && model.ddlPaymentMethods == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar al menos un medio de Pago.");
                }
                else if (model.PaymentMethods.Count() == 0)
                {
                    SpendingPaymentPaymentMethod pm = new SpendingPaymentPaymentMethod();
                    pm.Amount = model.Amount + model.AdvancementAmount;
                    pm.PaymentMethodId = int.Parse(model.ddlPaymentMethods.SelectedItem);
                    List<SpendingPaymentPaymentMethod> listPm = (List<SpendingPaymentPaymentMethod>)model.PaymentMethods;
                    listPm.Add(pm);
                    model.PaymentMethods = listPm;

                    if (model.Details.Count() == 0 && model.AdvancementAmount == 0)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar un monto que sera cargado sin Imputación.");
                    }
                }

                var SpendingPayment = model.ToModelObject();
                double totalpago = 0;
                double totalimputado = 0;

                foreach (SpendingPaymentPaymentMethod pay in model.PaymentMethods)
                {
                    totalpago = totalpago + pay.Amount;
                }

                foreach (SpendingPaymentDetail det in model.Details)
                {
                    totalimputado = totalimputado + det.Total;
                }

                if (SpendingPayment.ExecutionDate.Date > DateTime.Now.Date)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Necesidad no puede ser anterior a la fecha de Creación.");


                if (!model.Advancement)
                {
                    SpendingPayment.Balance = 0;

                }
                else
                    SpendingPayment.Balance = model.AdvancementAmount;

                SpendingPayment.Amount = model.AdvancementAmount + totalimputado;




                if (totalpago != SpendingPayment.Amount)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La suma de los totales de los medios de pago debe ser igual al total del Pago.");

                SpendingPayment.UserId = _profileProvider.CurrentUserId;


                _SpendingPaymentService.AddSpendingPayment(SpendingPayment);

                return Index();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        public ActionResult View(int id)
        {
            var entity = _SpendingPaymentService.GetSpendingPaymentsById(id);
            var model = SpendingPaymentVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            if (entity.Balance > 0)
            {
                model.AdvancementAmount = entity.Balance;
                model.Advancement = true;
            }


            return PartialView("~/Views/SpendingPayment/SpendingPaymentView.cshtml", model);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.provider = "-10";
            var entity = _SpendingPaymentService.GetSpendingPaymentsById(id);
            var model = SpendingPaymentVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            Session["PMs"] = new List<int>();

            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            model.ddlSpendings = collections.GetSpendingsByProviderSingleSelectVm(entity.ProveedorId.Value);

            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.AmountPayment = model.Amount;
            model.Saving = _cashierService.GetCashiersById(_userService.GetUserCashierByUserId(_profileProvider.CurrentUserId).CashierId).Name;
            if (entity.Balance > 0)
            {
                model.AdvancementAmount = entity.Balance;
                model.Advancement = true;
            }
            Session["SessionSpendingPayment"] = model;

            if (_SpendingPaymentService.IsEditable(id))
                return PartialView("~/Views/SpendingPayment/SpendingPaymentEdit.cshtml", model);
            else
                return PartialView("~/Views/SpendingPayment/SpendingPaymentEditShort.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(SpendingPaymentVm model)
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
                SpendingPaymentVm principal = (SpendingPaymentVm)Session["SessionSpendingPayment"];
                model.Details = principal.Details;
                model.PaymentMethods = principal.PaymentMethods;

                if (model.Details.Count() == 0 && !model.Advancement)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Gasto si no ha checkeado la casilla 'Sin Imputación'.");
                }

                model.SystemDate = null;
                model.Balance = 0;

                if (model.PaymentMethods.Count() == 0 && model.ddlPaymentMethods == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar al menos un medio de Pago.");
                }
                else if (model.PaymentMethods.Count() == 0)
                {
                    SpendingPaymentPaymentMethod pm = new SpendingPaymentPaymentMethod();
                    pm.Amount = model.Amount + model.AdvancementAmount;
                    pm.PaymentMethodId = int.Parse(model.ddlPaymentMethods.SelectedItem);
                    List<SpendingPaymentPaymentMethod> listPm = (List<SpendingPaymentPaymentMethod>)model.PaymentMethods;
                    listPm.Add(pm);
                    model.PaymentMethods = listPm;

                    if (model.Details.Count() == 0 && model.AdvancementAmount == 0)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar un monto que sera cargado sin Imputación.");
                    }
                }

                SpendingPayment SpendingPayment = model.ToModelObject();
                SpendingPayment.UserId = _profileProvider.CurrentUserId;

                double totalpago = 0;
                double totalimputado = 0;

                foreach (SpendingPaymentPaymentMethod pay in model.PaymentMethods)
                {
                    totalpago = totalpago + pay.Amount;
                }
                foreach (SpendingPaymentDetail det in model.Details)
                {
                    totalimputado = totalimputado + det.Total;
                }

                if (!model.Advancement)
                {
                    model.Balance = 0;
                    model.AdvancementAmount = 0;
                }

                model.Amount = model.AdvancementAmount + totalimputado;

                if (totalpago != model.Amount)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La suma de los totales de los medios de pago debe ser igual al total del Pago.");
                SpendingPayment.SystemDate = DateTime.Now;
                _SpendingPaymentService.UpdateSpendingPayment(SpendingPayment);

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

        [HttpPost]
        public ActionResult EditShort(SpendingPaymentVm model)
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
                SpendingPaymentVm principal = (SpendingPaymentVm)Session["SessionSpendingPayment"];
                model.Details = principal.Details;
                model.PaymentMethods = principal.PaymentMethods;

                SpendingPayment SpendingPayment = model.ToModelObject();
                SpendingPayment.UserId = _profileProvider.CurrentUserId;


                _SpendingPaymentService.UpdateSpendingPaymentShort(SpendingPayment);

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

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                if (_SpendingPaymentService.IsEditable(id))
                {
                    SpendingPayment spendingPayment = _SpendingPaymentService.GetSpendingPaymentsById(id);

                    _SpendingPaymentService.DeleteSpendingPayment(spendingPayment);
                    return Index();
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "No se puede eliminar el pago del gasto");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        [Description("Imputar Pagos")]
        public ActionResult ImputPayment(int id)
        {
            var entity = _SpendingPaymentService.GetSpendingPaymentsById(id);
            Session["ImputProvider"] = entity.ProveedorId.Value.ToString();
            Session["ImputSpending"] = new List<int>();
            SpendingFilterVm model = new SpendingFilterVm();
            List<SpendingPaymentDetail> lists = new List<SpendingPaymentDetail>();
            model.Details = lists;
            model.Balance = entity.Balance;
            model.Imput = 0;
            model.SpendingPaymentId = id;
            Session["SpendingImputModel"] = model;
            Session["Balance"] = entity.Balance;
            ViewBag.provider = "-10";

            return PartialView("~/Views/SpendingPayment/Spending.cshtml", model);
        }

        public ActionResult GetTableImput(string DateFromExecution, string DateToExecution, string DateFromExpiration, string DateToExpiration, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt)
        {
            ViewBag.Balance = Session["Balance"].ToString();
            var model = SpendingTableVm.ToViewModel(_spendingService.GetSpendingSummary(int.Parse(Session["ImputProvider"].ToString()), null, DateFromExecution, DateToExecution, DateFromExpiration, DateToExpiration, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, "1", "0", "0"));
            return PartialView("~/Views/SpendingPayment/SpendingTable.cshtml", model);
        }

        public ActionResult GetTableImputAll()
        {
            ViewBag.Balance = Session["Balance"].ToString();
            var model = SpendingTableVm.ToViewModel(_spendingService.GetSpendingSummary(int.Parse(Session["ImputProvider"].ToString()), null, "", "", "", "", "", "", "", "", "", "1", "0", "0"));
            return PartialView("~/Views/SpendingPayment/SpendingTable.cshtml", model);
        }

        [Description("Imputar Pago")]
        public ActionResult AddImput(int SpendingId, double Amount)
        {
            SpendingFilterVm model = (SpendingFilterVm)Session["SpendingImputModel"];
            List<int> spe = (List<int>)Session["ImputSpending"];
            spe.Add(SpendingId);
            Session["ImputSpending"] = spe;
            List<SpendingPaymentDetail> list = (List<SpendingPaymentDetail>)model.Details;
            SpendingPaymentDetail spd = new SpendingPaymentDetail();
            spd.SpendingId = SpendingId;
            spd.Total = Amount;
            list.Add(spd);
            model.Details = list;
            model.Balance = model.Balance - Amount;
            model.Imput = model.Imput + Amount;
            ViewBag.Balance = model.Balance.ToString();
            ViewBag.Imput = model.Imput.ToString();
            Session["SpendingImputModel"] = model;
            var modeld = SpendingTableVm.ToViewModel(_spendingService.GetSpendingSummaryWithOutImput(int.Parse(Session["ImputProvider"].ToString()), null, "", "", "", "", "", "", "", "", "", "1", "0", "0", spe));
            return PartialView("~/Views/SpendingPayment/SpendingTable.cshtml", modeld);
        }

        [HttpPost]
        public ActionResult CreateImput(SpendingFilterVm model)
        {
            try
            {
                model = (SpendingFilterVm)Session["SpendingImputModel"];
                _SpendingPaymentService.AddImputSpending(model.SpendingPaymentId, model.Imput, (List<SpendingPaymentDetail>)model.Details);
                return Index();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
    }
}