using Multigroup.Common.Logging;
using Multigroup.Portal.Helpers;
using Multigroup.Portal.Models.Collection;
using Multigroup.Portal.Providers;
using Multigroup.Portal.Utilities;
using Multigroup.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Filters;

namespace Multigroup.Portal.Controllers
{
    public class VisitsController : Controller
    {
        private ZoneService _zoneService;
        private IAuditLogHelper _auditLogHelper;
        private VisitService _visitService;
        private UserService _userService;
        private ContractService _contractService;
        private ProfileProvider _profileProvider;
        private PaymentService _paymentService;
        private PaymentMethodsService _paymentMethodsService;
        private CustomerService _customerService;

        public VisitsController()
        {
            _auditLogHelper = new AuditLogHelper();
            _zoneService = new ZoneService();
            _visitService = new VisitService();
            _userService = new UserService();
            _contractService = new ContractService();
            _profileProvider = new ProfileProvider();
            _paymentService = new PaymentService();
            _paymentMethodsService = new PaymentMethodsService();
            _customerService = new CustomerService();
        }
        public ActionResult Index()
        {
            VisitFilterVm model = new VisitFilterVm();
            var collections = new CollectionHelper();
            model.ListZones = new SelectList(_zoneService.GetZones(), "ZoneId", "Name");
            model.ListCollector = new SelectList(_userService.GetUsersCollector(), "Id", "Name");
            model.ListStatus = new SelectList(_contractService.GetStatusContract("visit"), "StatusContractId", "Description");
            return PartialView("~/Views/Collections/Visits/Visits.cshtml", model);
        }
        public ActionResult Collector()
        {
            var collector = _profileProvider.CurrentUserId;
            var zone = _zoneService.GetZoneByCollectorId(collector);
            DateTime dateFrom = DateTime.Today;

            DateTime fechaInicio = new DateTime(dateFrom.Year, dateFrom.Month, 1);

            DateTime dateTo = fechaInicio.AddMonths(1).AddDays(-1);

            string status = "10";
            string collectors = collector.ToString();
            var visits = _visitService.GetVisitsByFilters(status, zone, collectors, dateFrom.ToShortDateString(), dateTo.ToShortDateString());
            var model = VisitTableVm.ToViewModel(visits);
            model.ListStatus = new SelectList(_contractService.GetStatusContractFilter("visit"), "StatusContractId", "Description");
            return PartialView("~/Views/Collections/Collector/Collector.cshtml", model);
        }
        public ActionResult ConfirmStatus(int VisitId)
        {
            try
            {
                _visitService.ConfirmStatus(VisitId);
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Visita confirmada con éxito.");
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = "visita",
                    IntegrationType = AuditLogType.RoleService
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al generar visita.");
            }
        }

        public ActionResult CancelStatus(int VisitId)
        {
            try
            {
                _visitService.CancelStatus(VisitId);
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Visita cancelada con éxito.");
            }
            catch (Exception ex)
            {            
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al generar visita.");
            }
        }
        public ActionResult ConfirmCollectorVisit(int VisitId, string ConcretedVisitDate, double AmountCollected, int StatusId, string CollectorExpensesDesc, double CollectorExpensesAmount, string Observations, string ReceipNumber)
        {
            try
            {
                DateTime date = DateTime.Parse(ConcretedVisitDate);
                _visitService.CollectorVisit(VisitId, date, AmountCollected, StatusId, CollectorExpensesDesc, CollectorExpensesAmount, Observations, ReceipNumber, null, _profileProvider.CurrentUserId);
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Visita confirmada con éxito.");
            }
            catch (Exception ex)
            {            

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al generar visita.");
            }
        }
        public ActionResult GetTable(List<string> Status, List<string> Zones, List<string> Collectors, string DateTo, string DateFrom)
        {
            if (DateFrom == "")
                DateFrom = "01/01/1900";
            if (DateTo == "")
                DateTo = "01/01/2100";
            string status = (Status[0] != "" && Status.Count() > 0) ? String.Join(",", Status) : "0";
            string zones = (Zones[0] != "" && Zones.Count() > 0) ? String.Join(",", Zones) : "0";
            string collectors = (Collectors[0] != "" && Collectors.Count() > 0) ? String.Join(",", Collectors) : "0";
            var visits = _visitService.GetVisitsByFilters(status, zones, collectors, DateFrom, DateTo);
            var model = VisitTableVm.ToViewModel(visits);
            return PartialView("~/Views/Collections/Visits/VisitsTable.cshtml", model);
        }
        public ActionResult AssignVisit()
        {
            AssignVisitFilterVm model = new AssignVisitFilterVm();
            var collections = new CollectionHelper();
            model.ListZones = new SelectList(_zoneService.GetZones(), "ZoneId", "Name");
            model.ddlPaymentDate = collections.GetPaymentDateSingleSelectVm();
            model.ddlStatus = collections.GetStatusSingleSelectVm();

            return PartialView("~/Views/Collections/Visits/AssignVisit.cshtml", model);
        }

        public ActionResult GetAssignVisitTable(int? ZoneId, int? PaymentDateId)
        {
            var assigVisit = _visitService.GetAssignVisitByZoneAndPaymentDate(ZoneId, PaymentDateId, _profileProvider.CurrentUserId);
            var model = AssignVisitTableVm.ToViewModel(assigVisit);
            return PartialView("~/Views/Collections/Visits/AssignVisitTable.cshtml", model);
        }

        public ActionResult GenerateVisit(int CustomerId, string ScheduledVisitDate, double AmountOwed, double CommissionRate, int ZoneId, string observationsCollector)
        {
            try
            {
                DateTime? date = Util.ParseDateTime(ScheduledVisitDate);
                _visitService.GenerateVisit(CustomerId, date, AmountOwed, CommissionRate, ZoneId, observationsCollector, _profileProvider.CurrentUserId);

                return RedirectToAction("AssignVisit");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al generar visita.");
            }
        }
        public ActionResult GenerateChangeStatus(int CustomerId, int Status, string observations)
        {
            try
            {
                if (Status == 18)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "No puede cambiar a scoring Ok.");
                }
                else { 
                int statusOld = _visitService.GetContractByCustomerId(CustomerId).Status.Value;
                _contractService.AddStatusContractHistory(_visitService.GetContractByCustomerId(CustomerId).ContractId, _profileProvider.CurrentUserId, statusOld, Status, observations);
                _contractService.UpdateContractStatus(_visitService.GetContractByCustomerId(CustomerId).ContractId, Status, _visitService.GetContractByCustomerId(CustomerId).StatusDetail.Value, _visitService.GetContractByCustomerId(CustomerId).PaperStatus.Value, null);
                return RedirectToAction("AssignVisit");
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al generar visita.");
            }
        }
        public ActionResult GenerateAgency(int CustomerId, string dateAgency, string observationsAgency)
        {
            try
            {
                DateTime? date = Util.ParseDateTime(dateAgency);
                _visitService.GenerateAgency(CustomerId, date, observationsAgency, _profileProvider.CurrentUserId);

                return RedirectToAction("AssignVisit");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al generar visita.");
            }
        }

        // GET: Visits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Visits/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Visits/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Visits/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Visits/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Visits/Delete/5
        [HttpPost]
        public void RepeatAsignation(int monthToRepeat, int yearToRepeat, int monthToApply, int yearToApply)
        {
            try
            {
                _visitService.RepeatAsignation(monthToRepeat, yearToRepeat, monthToApply, yearToApply);
            }
            catch (Exception ex)
            {
                int a = 1;
            }
        }

        [LogActionFilter]
        [Description("Crear Seguimiento")]
        public ActionResult Follow(int id)
        {

            AssignPaymentPreferenceVm assignFollow = new AssignPaymentPreferenceVm();
            Contract contrato = _visitService.GetContractByCustomerId(id);
            Installment installment = _contractService.GetInstallmentByContract(contrato.ContractId);
            assignFollow.InstallmentId = installment.InstallmentId;

            return PartialView("~/Views/Collections/Visits/FollowModal.cshtml", assignFollow);
        }

        [HttpPost]
        public ActionResult Follow(AssignPaymentPreferenceVm model)
        {
            try
            {
                var follow = model.ToModelObject();
                follow.FollowDate = DateTime.Now;
                follow.UserId = _profileProvider.CurrentUserId;
                follow.AssignStateId = 3;
                _paymentService.AddAssignPaymentPreference(follow);
                return AssignVisit();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }


        [LogActionFilter]
        [Description("Crear Asignación")]
        public ActionResult AssignPaymentPreference(int id)
        {
            AssignPaymentPreferenceVm assignFollow = new AssignPaymentPreferenceVm();
            Contract contrato = _visitService.GetContractByCustomerId(id);
            Installment installment = _contractService.GetInstallmentByContract(contrato.ContractId);
            assignFollow.InstallmentId = installment.InstallmentId;

            var collections = new CollectionHelper();

            assignFollow.ddlPaymentPreference = collections.GetPaymentPreferenceSingleSelectVm();

            return PartialView("~/Views/Collections/Visits/AssignPaymentPreferenceModal.cshtml", assignFollow);
        }

        [HttpPost]
        public ActionResult AssignPaymentPreference(AssignPaymentPreferenceVm model)
        {
            try
            {
                var follow = model.ToModelObject();
                if (!follow.PaymentPreferenceId.HasValue)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar Forma de Pago");
                }
                Multigroup.DomainModel.Shared.PaymentPreference paymentPreference = _contractService.GetPaymentPreferenceById(follow.PaymentPreferenceId.Value);
                if (!paymentPreference.Class.Equals("Virtual") && follow.PaymentDate == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe Seleccionar fecha de pago");
                }
                else
                {
                    follow.UserId = _profileProvider.CurrentUserId;
                    follow.AssignStateId = 1;
                    follow.FollowDate = DateTime.Now;
                    _paymentService.AddAssignPaymentPreference(follow);
                    return AssignVisit();
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        public ActionResult DetailCustomer(int id)
        {
            AssignPaymentPreferenceFiltersVm model = new AssignPaymentPreferenceFiltersVm();
            var collections = new CollectionHelper();
            model.SelectedCustomer = id;
            model.ListUser = new SelectList(_userService.GetUsersTelemarketer(), "Id", "Name");
            model.ListPaymentPreference = new SelectList(_contractService.GetPaymentPreference(), "PaymentPreferenceId", "Description");
            model.ListAssignState = new SelectList(_paymentService.GetAssignStates(), "AssignStateId", "Description");
            return PartialView("~/Views/Collections/Visits/AssignDetail.cshtml", model);
        }

        public ActionResult GetTableDetail(List<string> State, List<string> Telemarketer, List<string> PaymentPreference, string DateFrom, string DateTo, string customer, string cuota)
        {
            if (DateFrom == "")
                DateFrom = "01/01/1900";
            if (DateTo == "")
                DateTo = "01/01/2100";
            if (cuota == "")
                cuota = "0";
            string States = (State[0] != "" && State.Count() > 0) ? String.Join(",", State) : "0";
            string Telemarketers = (Telemarketer[0] != "" && Telemarketer.Count() > 0) ? String.Join(",", Telemarketer) : "0";
            string PaymentPreferences = (PaymentPreference[0] != "" && PaymentPreference.Count() > 0) ? String.Join(",", PaymentPreference) : "0";
            var assign = _paymentService.GetAssignPaymentPreferenceSummary(PaymentPreferences, Telemarketers, States, cuota, DateFrom, DateTo, customer);
            var model = AssignPaymentPreferenceTableVm.ToViewModel(assign);
            return PartialView("~/Views/Collections/Visits/AssignDetailTable.cshtml", model);
        }

        public ActionResult IndexAssignPaymentMethod()
        {
            AssignPaymentPreferenceFiltersVm model = new AssignPaymentPreferenceFiltersVm();
            var collections = new CollectionHelper();
            model.ListUser = new SelectList(_userService.GetUsersTelemarketer(), "Id", "Name");

            IEnumerable<Multigroup.DomainModel.Shared.PaymentPreference> paymentPreference = _contractService.GetPaymentPreference();
            paymentPreference = paymentPreference.Where(x => x.Class == "Virtual");
            model.ListPaymentPreference = new SelectList(paymentPreference, "PaymentPreferenceId", "Description");
            model.Advised = false;
            model.Processed = false;
            return PartialView("~/Views/Collections/Visits/AssignPaymentMethod.cshtml", model);
        }

        public ActionResult GetTableAssignPaymentMethod(List<string> Telemarketer, List<string> PaymentPreference, string IsProcessed, string IsAdvised)
        {
            string Telemarketers = (Telemarketer[0] != "" && Telemarketer.Count() > 0) ? String.Join(",", Telemarketer) : "0";
            string PaymentPreferences = (PaymentPreference[0] != "" && PaymentPreference.Count() > 0) ? String.Join(",", PaymentPreference) : "0";
            var assign = _paymentService.GetAssignPaymentPreferenceSummaryByAssignPaymentMethod(PaymentPreferences, Telemarketers, IsProcessed, IsAdvised);
            var model = AssignPaymentPreferenceTableVm.ToViewModel(assign);
            return PartialView("~/Views/Collections/Visits/AssignPaymentMethodTable.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Asignación Medio de Pago")]
        public ActionResult AssignPaymentMethod(int id)
        {
            AssignPaymentPreference assignFollow = _paymentService.GetAssignPaymentPreferenceById(id);
            AssignPaymentPreferenceVm model = new AssignPaymentPreferenceVm();
            model.AssignPaymentPreferenceId = id;
            var collections = new CollectionHelper();

            model.ddlPaymentMethod = collections.GetPaymentMethodByPPSingleSelectVm(assignFollow.PaymentPreferenceId.Value);

            return PartialView("~/Views/Collections/Visits/AssignPaymentMethodModal.cshtml", model);
        }

        [Description("BuscarBalance")]
        public ActionResult GetChannel(int PaymentMethodId, int id)
        {
            AssignPaymentPreference assignFollow = _paymentService.GetAssignPaymentPreferenceById(id);
            AssignPaymentPreferenceVm model = new AssignPaymentPreferenceVm();
            PaymentMethod pm = _paymentMethodsService.GetPaymentMethods(PaymentMethodId);
            model.Channel = pm.Channel;
            model.AssignPaymentPreferenceId = id;
            var collections = new CollectionHelper();

            model.ddlPaymentMethod = collections.GetPaymentMethodByPPSingleSelectVm(assignFollow.PaymentPreferenceId.Value);
            model.ddlPaymentMethod.SelectedItem = PaymentMethodId.ToString();
            return PartialView("~/Views/Collections/Visits/AssignPaymentMethodModal.cshtml", model);
        }

        [HttpPost]
        public ActionResult AssignPaymentMethod(AssignPaymentPreferenceVm model)
        {
            try
            {
                if (model.ddlPaymentMethod == null)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar Medio de Pago");

                PaymentMethod pm = _paymentMethodsService.GetPaymentMethods(int.Parse(model.ddlPaymentMethod.SelectedItem));
                if (!pm.Channel.Equals(""))
                    model.Channel = pm.Channel;
                if (model.Channel == "")
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar Canal de Pago");

                var follow = _paymentService.GetAssignPaymentPreferenceById(model.AssignPaymentPreferenceId);
                follow.Channel = model.Channel;
                follow.AssignStateId = 2;
                follow.AssignPaymentMethodDate = DateTime.Now;
                follow.PaymentMethodId = int.Parse(model.ddlPaymentMethod.SelectedItem);
                _paymentService.UpdateAssignPaymentPreference(follow);
                return IndexAssignPaymentMethod();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        public ActionResult DeleteAssignPaymentMethod(int id)
        {
            try
            {
                var follow = _paymentService.GetAssignPaymentPreferenceById(id);
                follow.AssignStateId = 1;
                follow.AssignPaymentMethodDate = null;
                follow.PaymentMethodId = null;
                follow.Channel = "";
                _paymentService.UpdateAssignPaymentPreference(follow);
                return IndexAssignPaymentMethod();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        public ActionResult IndexChannelWarned()
        {
            AssignPaymentPreferenceFiltersVm model = new AssignPaymentPreferenceFiltersVm();
            var collections = new CollectionHelper();
            model.ListUser = new SelectList(_userService.GetUsersTelemarketer(), "Id", "Name");
            model.ListPaymentMethod = new SelectList(_paymentMethodsService.GetPaymentMethodss(), "PaymentMethodId", "Name");
            model.ListCustomers = new SelectList(_customerService.GetCustomersComplete(), "CustomerId", "Name");
            model.Advised = false;
            return PartialView("~/Views/Collections/Visits/ChannelWarned.cshtml", model);
        }

        public ActionResult GetTableChannelWarned(List<string> Telemarketer, List<string> PaymentMethod, List<string> Customer, string DateFrom, string DateTo, string IsWarned)
        {
            var user = _profileProvider.CurrentUserId;
            var rol = _userService.GetUser(user).Roles;
            ViewBag.Rol = rol.FirstOrDefault().RoleId;
            string Telemarketers = (Telemarketer[0] != "" && Telemarketer.Count() > 0) ? String.Join(",", Telemarketer) : "0";
            string PaymentMethods = (PaymentMethod[0] != "" && PaymentMethod.Count() > 0) ? String.Join(",", PaymentMethod) : "0";
            string Customers = (Customer[0] != "" && Customer.Count() > 0) ? String.Join(",", Customer) : "0";
            if (DateFrom == "")
                DateFrom = "01/01/1900";
            if (DateTo == "")
                DateTo = "01/01/2100";
            var assign = _paymentService.GetAssignPaymentPreferenceSummaryChannelWarned(PaymentMethods, Telemarketers, Customers, DateFrom, DateTo, IsWarned);
            var model = AssignPaymentPreferenceTableVm.ToViewModel(assign);
            return PartialView("~/Views/Collections/Visits/ChannelWarnedTable.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Aviso Canal de Pago")]
        public ActionResult ChannelWarned(int id)
        {
            AssignPaymentPreference assignFollow = _paymentService.GetAssignPaymentPreferenceById(id);
            AssignPaymentPreferenceVm model = new AssignPaymentPreferenceVm();
            model.AssignPaymentPreferenceId = id;

            return PartialView("~/Views/Collections/Visits/ChannelWarnedModal.cshtml", model);
        }

        [HttpPost]
        public ActionResult ChannelWarned(AssignPaymentPreferenceVm model)
        {
            try
            {
                if (model.WarnedDate == null || model.WarnedDate.Equals(""))
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe ingresar Fecha de Aviso");

                var follow = _paymentService.GetAssignPaymentPreferenceById(model.AssignPaymentPreferenceId);
                follow.Warned = true;
                follow.WarnedCommentary = model.WarnedCommentary;
                follow.WarnedDate = Util.ParseDateTime(model.WarnedDate);
                _paymentService.UpdateAssignPaymentPreference(follow);
                return IndexChannelWarned();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        public ActionResult DeleteWarned(int id)
        {
            try
            {
                var follow = _paymentService.GetAssignPaymentPreferenceById(id);
                follow.Warned = false;
                follow.WarnedCommentary = null;
                follow.WarnedDate = null;
                _paymentService.UpdateAssignPaymentPreference(follow);
                return IndexChannelWarned();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
    }
}
