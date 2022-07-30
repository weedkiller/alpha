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

namespace Multigroup.Portal.Controllers
{
    public class CollectorController : Controller
    {
        private ZoneService _zoneService;
        private IAuditLogHelper _auditLogHelper;
        private VisitService _visitService;
        private UserService _userService;
        private ContractService _contractService;
        private ProfileProvider _profileProvider;
        private CustomerService _customerService;
        private PaymentService _paymentService;
        private PaymentMethodsService _paymentMethodService;

        public CollectorController()
        {
            _auditLogHelper = new AuditLogHelper();
            _zoneService = new ZoneService();
            _visitService = new VisitService();
            _userService = new UserService();
            _contractService = new ContractService();
            _profileProvider = new ProfileProvider();
            _customerService = new CustomerService();
            _paymentService = new PaymentService();
            _paymentMethodService = new PaymentMethodsService();
        }
        // GET: Collector
        public ActionResult Index()
        {
            AssignVisitFilterVm model = new AssignVisitFilterVm();
            var collections = new CollectionHelper();
            var collector = _profileProvider.CurrentUserId;
            var zones = _zoneService.GetZonesByCollectorId(collector);
            model.ListZones = new SelectList(zones, "ZoneId", "Name");
            return PartialView("~/Views/Collections/Collector/Collector.cshtml", model);
        }

        // GET: Collector/Details/5
        public ActionResult CollectorTable(List<string> ZoneId)
        {
            var collector = _profileProvider.CurrentUserId;
            var zone = _zoneService.GetZoneByCollectorId(collector);
            DateTime dateFrom = DateTime.Today;

            DateTime fechaInicio = new DateTime(dateFrom.Year, dateFrom.Month, 1);

            DateTime dateTo = fechaInicio.AddMonths(1).AddDays(-1);

            string status = "10";
            string collectors = collector.ToString();
            string selectedZone = (ZoneId[0] != "" && ZoneId.Count() > 0) ? String.Join(",", ZoneId) : zone;

            var visits = _visitService.GetVisitsByFilters(status, selectedZone, collectors, dateFrom.ToShortDateString(), dateTo.ToShortDateString());
            var model = VisitTableVm.ToViewModel(visits);
            model.ListStatus = new SelectList(_contractService.GetStatusContractFilter("visit"), "StatusContractId", "Description");
            return PartialView("~/Views/Collections/Collector/CollectorTable.cshtml", model);
        }

        // GET: Collector/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Collector/Create
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

        // GET: Collector/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Collector/Edit/5
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

        // GET: Collector/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Collector/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // Nuevo sistema 

        public ActionResult IndexManagement()
        {
            AssignPaymentPreferenceFiltersVm model = new AssignPaymentPreferenceFiltersVm();
            var collections = new CollectionHelper();
            model.ListUser = new SelectList(_userService.GetUsersTelemarketer(), "Id", "Name");
            model.ListCollector = new SelectList(_userService.GetUsersByType(2), "Id", "Name");
            model.ListCustomers = new SelectList(_customerService.GetCustomersComplete(), "CustomerId", "Name");
            model.Surrendered = false;
            model.Assigned = false;
            model.Register = false;
            model.ListZone = new SelectList(_zoneService.GetZones(), "ZoneId", "Name") ;
            return PartialView("~/Views/Collections/Collector/CollectorManagement.cshtml", model);
        }

        // GET: Collector/Details/5
        public ActionResult IndexManagementTable(List<string> Telemarketer, List<string> Collector, List<string> Zone, List<string> Customer, string DateVisitFrom, string DateVisitTo, string DatePaymentTo, string DatePaymentFrom, string IsSurrender, string IsAssign, string IsRegister)
        {
            string Telemarketers = (Telemarketer[0] != "" && Telemarketer.Count() > 0) ? String.Join(",", Telemarketer) : "0";
            string Collectors = (Collector[0] != "" && Collector.Count() > 0) ? String.Join(",", Collector) : "0";
            string Zones = (Zone[0] != "" && Zone.Count() > 0) ? String.Join(",", Zone) : "0";
            string Customers = (Customer[0] != "" && Customer.Count() > 0) ? String.Join(",", Customer) : "0";
            if (DateVisitFrom == "")
                DateVisitFrom = "01/01/1900";
            if (DateVisitTo == "")
                DateVisitTo = "01/01/2100";
            if (DatePaymentFrom == "")
                DatePaymentFrom = "01/01/1900";
            if (DatePaymentTo == "")
                DatePaymentTo = "01/01/2100";
            var assign = _visitService.GetAssignPaymentPreferenceSummaryPaymentCollector(Telemarketers, Collectors, Zones, Customers, DateVisitFrom, DateVisitTo, DatePaymentFrom, DatePaymentTo, IsSurrender, IsAssign, IsRegister);
            var model = AssignPaymentPreferenceCollectorTableVm.ToViewModel(assign);
            return PartialView("~/Views/Collections/Collector/CollectorManagementTable.cshtml", model);
        }


        public ActionResult Assign(int id)
        {
            var model = new CollectorAssigmmentVm();
            var collections = new CollectionHelper();
            model.AssignPaymentPreferenceId = id;
            model.ddlCollector = collections.GetUsersByTypeSingleSelectVm(2);


            model.Asignaciones = _visitService.GetCollectorAssigmmentsByAssignPaymentAssign(id);


            return PartialView("~/Views/Collections/Collector/AssignCollector.cshtml", model);
        }

        [HttpPost]
        public ActionResult AssignCollector(CollectorAssigmmentVm model)
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

                // TODO: Add update logic here
                CollectorAssigmment collector = new CollectorAssigmment();
                collector.Active = true;
                collector.AssignPaymentPreferenceId = model.AssignPaymentPreferenceId;
                collector.Date = DateTime.Now;
                collector.UserId = int.Parse(model.ddlCollector.SelectedItem);
                collector.VisitDate = Util.ParseDateTime(model.VisitDate);

                _visitService.AddCollectorAssigmment(collector);
               
                return IndexManagement();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        public ActionResult IndexSurrender()
        {
            AssignPaymentPreferenceFiltersVm model = new AssignPaymentPreferenceFiltersVm();
            var collections = new CollectionHelper();
            model.ListUser = new SelectList(_userService.GetUsersTelemarketer(), "Id", "Name");
            model.ListCustomers = new SelectList(_customerService.GetCustomersComplete(), "CustomerId", "Name");
            model.Surrendered = false;
            model.Assigned = false;
            model.Register = false;
            model.ListZone = new SelectList(_zoneService.GetZones(), "ZoneId", "Name");
            return PartialView("~/Views/Collections/Collector/CollectorSurrender.cshtml", model);
        }

        // GET: Collector/Details/5
        public ActionResult IndexSurrenderTable(List<string> Telemarketer, List<string> Zone, List<string> Customer, string DateVisitFrom, string DateVisitTo, string DatePaymentTo, string DatePaymentFrom, string IsSurrender, string IsRegister)
        {
            string Telemarketers = (Telemarketer[0] != "" && Telemarketer.Count() > 0) ? String.Join(",", Telemarketer) : "0";
            string Zones = (Zone[0] != "" && Zone.Count() > 0) ? String.Join(",", Zone) : "0";
            string Customers = (Customer[0] != "" && Customer.Count() > 0) ? String.Join(",", Customer) : "0";
            if (DateVisitFrom == "")
                DateVisitFrom = "01/01/1900";
            if (DateVisitTo == "")
                DateVisitTo = "01/01/2100";
            if (DatePaymentFrom == "")
                DatePaymentFrom = "01/01/1900";
            if (DatePaymentTo == "")
                DatePaymentTo = "01/01/2100";
            var assign = _visitService.GetAssignPaymentPreferenceSummaryPaymentCollector(Telemarketers, _profileProvider.CurrentUserId.ToString(), Zones, Customers, DateVisitFrom, DateVisitTo, DatePaymentFrom, DatePaymentTo, IsSurrender, "1", IsRegister);
            var model = AssignPaymentPreferenceCollectorTableVm.ToViewModel(assign);
            return PartialView("~/Views/Collections/Collector/CollectorSurrenderTable.cshtml", model);
        }

        public ActionResult SurrenderSummary(List<string> cuotas)
        {
                CollectorSurrenderVm model = new CollectorSurrenderVm();
                var collections = new CollectionHelper();
                List<AssignPaymentPreferenceCollector> lists = new List<AssignPaymentPreferenceCollector>();
                model.Asignaciones = lists;
                List<CollectorSurrenderPM> payment = new List<CollectorSurrenderPM>();
                model.PaymentMethods = payment;
                List<CollectorSurrenderReceipt> receipts = new List<CollectorSurrenderReceipt>();
                model.Recibos = receipts;
                model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
                model.ddlPaymentMethodsPM = collections.GetPaymentMethodSingleSelectVm();
                model.Amount = 0;
                model.AmountReceipt = 0;
                foreach (var cuota in cuotas)
                {
                    AssignPaymentPreferenceCollector cu = _visitService.GetAssignPaymentPreferenceCollectorById(int.Parse(cuota));
                    if (cu.Amount > 0)
                    {
                        lists.Add(cu);
                        model.Amount = model.Amount + cu.Amount;
                    }
                }
                Session["SessionSurrenderSummary"] = model;
                Session["PMs"] = new List<int>();
            Session["Total"] = model.Amount.ToString();
            Session["TotalReceio"] = "0";
            return PartialView("~/Views/Collections/Collector/SurrenderSummary.cshtml", model);
        }

        public ActionResult SurrenderSummaryTable()
        {
            CollectorSurrenderVm model = (CollectorSurrenderVm)Session["SessionSurrenderSummary"];
            return PartialView("~/Views/Collections/Collector/SurrenderSummaryTable.cshtml", model);
        }

        public ActionResult IndexPaymentMethod()
        {
            CollectorSurrenderVm model = (CollectorSurrenderVm)Session["SessionSurrenderSummary"];
            ViewBag.ErrorMessagePayment = "false";
            return PartialView("~/Views/Collections/Collector/PaymentMethodTable.cshtml", model);
        }

        public ActionResult PaymentMethod(int? PaymentMethod, double Amount)
        {
            bool existe = false;
            CollectorSurrenderVm principal = (CollectorSurrenderVm)Session["SessionSurrenderSummary"];
            if (PaymentMethod.HasValue)
            {
                CollectorSurrenderPM payment = new CollectorSurrenderPM();
                payment.PaymentMethodId = PaymentMethod.Value;
                payment.PaymentMethod = _paymentMethodService.GetPaymentMethodssById(PaymentMethod.Value).Name;
                payment.Amount = Amount;

                List<int> listPM = (List<int>)Session["PMs"];
                listPM.Add(PaymentMethod.Value);
                Session["PMs"] = listPM;

                List<CollectorSurrenderPM> list = (List<CollectorSurrenderPM>)principal.PaymentMethods;
                Int32 id = PaymentMethod.Value;
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                if (item != null)
                    existe = true;
                else
                    list.Add(payment);
                principal.PaymentMethods = list;

                Session["SessionSurrenderSummary"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessagePayment = "false";
                    return PartialView("~/Views/Collections/Collector/PaymentMethodTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessagePayment = "true";
                    return PartialView("~/Views/Collections/Collector/PaymentMethodTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un método de pago");
            }
        }

        public ActionResult RemovePaymentMethod(int Id)
        {
            CollectorSurrenderVm principal = (CollectorSurrenderVm)Session["SessionSurrenderSummary"];
            List<CollectorSurrenderPM> list = (List<CollectorSurrenderPM>)principal.PaymentMethods;
            var item = list.FirstOrDefault(x => x.PaymentMethodId == Id);
            list.Remove(item);

            principal.PaymentMethods = list;

            Session["SessionSurrenderSummary"] = principal;

            Session["esNuevo"] = false;
            ViewBag.ErrorMessagePayment = "false";
            return PartialView("~/Views/Collections/Collector/PaymentMethodTable.cshtml", principal);
        }
        public ActionResult PaymentMethods()
        {
            CollectorSurrenderVm principal = (CollectorSurrenderVm)Session["SessionSurrenderSummary"];

            Session["PMs"] = new List<int>();
            return PartialView("~/Views/Collections/Collector/PaymentMethodTable.cshtml", principal);
        }

        public ActionResult CancelPaymentMethods()
        {
            CollectorSurrenderVm principal = (CollectorSurrenderVm)Session["SessionSurrenderSummary"];


            List<CollectorSurrenderPM> list = (List<CollectorSurrenderPM>)principal.PaymentMethods;
            foreach (int id in (List<int>)Session["PMs"])
            {
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                list.Remove(item);
            }

            principal.PaymentMethods = list;
            Session["PMs"] = new List<int>();
            Session["SessionSurrenderSummary"] = principal;

            ViewBag.ErrorMessagePayment = "false";

            return PartialView("~/Views/Collections/Collector/PaymentMethodTable.cshtml", principal);
        }

        public ActionResult InstallmentUpdate(int? InstallmentId,double Monto)
        {
            CollectorSurrenderVm principal = (CollectorSurrenderVm)Session["SessionSurrenderSummary"];
            if (InstallmentId.HasValue)
            {
                AssignPaymentPreferenceCollector detail = new AssignPaymentPreferenceCollector();
                detail.AssignPaymentPreferenceId = InstallmentId.Value;

                List<AssignPaymentPreferenceCollector> list = (List<AssignPaymentPreferenceCollector>)principal.Asignaciones;
                Int32 id = InstallmentId.Value;

                var item = list.FirstOrDefault(x => x.AssignPaymentPreferenceId == id );
                detail.Amount = Monto;
                detail.Assign = item.Assign;
                detail.Collector = item.Collector;
                detail.Customer = item.Customer;
                detail.Cuota = item.Cuota;
                detail.Number = item.Number;
                list.Remove(item);

                list.Add(detail);
                principal.Asignaciones = list;


                double total = 0;

                foreach (AssignPaymentPreferenceCollector det in list)
                {
                    total = total + det.Amount;
                }
                ViewBag.Total = total.ToString();

                Session["Total"] = total.ToString();
                principal.AmountPayment = total;
                Session["SessionSurrenderSummary"] = principal;
                Session["esNuevo"] = false;


                    return PartialView("~/Views/Collections/Collector/SurrenderSummaryTable.cshtml", principal);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un articulo");
            }
        }

        public ActionResult RemoveInstallments(int Id)
        {
            CollectorSurrenderVm principal = (CollectorSurrenderVm)Session["SessionSurrenderSummary"];
            List<AssignPaymentPreferenceCollector> list = (List<AssignPaymentPreferenceCollector>)principal.Asignaciones;
            var item = list.FirstOrDefault(x => x.AssignPaymentPreferenceId == Id);
            list.Remove(item);

            principal.Asignaciones = list;
            double total = 0;

            foreach (AssignPaymentPreferenceCollector det in list)
            {
                total = total + det.Amount;
            }
            ViewBag.Total = total.ToString();

            Session["Total"] = total.ToString();

            Session["SessionSurrenderSummary"] = principal;


            return PartialView("~/Views/Collections/Collector/SurrenderSummaryTable.cshtml", principal);
        }

        public ActionResult RemoveReceips(string number)
        {
            CollectorSurrenderVm principal = (CollectorSurrenderVm)Session["SessionSurrenderSummary"];
            List<CollectorSurrenderReceipt> list = (List<CollectorSurrenderReceipt>)principal.Recibos;
            var item = list.FirstOrDefault(x => x.Number == number);
            list.Remove(item);

            principal.Recibos = list;
            double totalReceip = 0;

            foreach (CollectorSurrenderReceipt det in list)
            {
                totalReceip = totalReceip + det.Amount;
            }
            ViewBag.TotalReceip = totalReceip.ToString();

            Session["TotalReceip"] = totalReceip.ToString();

            Session["SessionSurrenderSummary"] = principal;

            Session["esNuevo"] = false;

            return PartialView("~/Views/Collections/Collector/ReceipTable.cshtml", principal);
        }

        public ActionResult AddReceipt(string Number, double Amount)
        {
            bool existe = false;
            CollectorSurrenderVm principal = (CollectorSurrenderVm)Session["SessionSurrenderSummary"];

            CollectorSurrenderReceipt detail = new CollectorSurrenderReceipt();
                detail.Number = Number;
                detail.Amount = Amount;

                List<CollectorSurrenderReceipt> list = (List<CollectorSurrenderReceipt>)principal.Recibos;
                var item = list.FirstOrDefault(x => x.Number == Number);
                if (item != null)
                    existe = true;
                else
                    list.Add(detail);
                principal.Recibos = list;


                double total = 0;

            double totalReceip = 0;

            foreach (CollectorSurrenderReceipt det in list)
            {
                totalReceip = totalReceip + det.Amount;
            }
            ViewBag.TotalReceip = totalReceip.ToString();

            Session["TotalReceip"] = totalReceip.ToString();

            Session["SessionSurrenderSummary"] = principal;
            Session["esNuevo"] = false;


           return PartialView("~/Views/Collections/Collector/ReceipTable.cshtml", principal);
        }


        [HttpPost]
        public ActionResult SurrenderSummaryCreate(CollectorSurrenderVm model)
        {
            try
            {
                if (model.SurrenderDate == null || model.SurrenderDate.Equals(""))
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Rendición Es obligatoria");


                CollectorSurrenderVm principal = (CollectorSurrenderVm)Session["SessionSurrenderSummary"];
                if (principal.Asignaciones.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un cobro.");
                }


                if (model.Amount != model.AmountReceipt)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El monto de recibos no coincide con el rendido");
               
                

                if (Util.ParseDateTime(model.SurrenderDate) > DateTime.Now)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Rendición no puede ser mayor a la actual.");


                if (principal.PaymentMethods.Count() == 0 && model.ddlPaymentMethods != null)
                {
                    CollectorSurrenderPM pm = new CollectorSurrenderPM();
                    pm.Amount = model.Amount;
                    pm.PaymentMethodId = int.Parse(model.ddlPaymentMethods.SelectedItem);
                    List<CollectorSurrenderPM> listPm = (List<CollectorSurrenderPM>)principal.PaymentMethods;
                    listPm.Add(pm);
                    principal.PaymentMethods = listPm;
                }


                if (principal.PaymentMethods.Count() > 0)
                {
                    double totalpago = 0;
                    foreach (CollectorSurrenderPM pay in principal.PaymentMethods)
                    {
                        totalpago = totalpago + pay.Amount;
                    }

                    if(totalpago != model.Amount)
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El total rendido no se corresponden con los medios de pago ingresados.");
                }

                CollectorSurrender rendicion = new CollectorSurrender();
                rendicion.UserId = _profileProvider.CurrentUserId;
                rendicion.Date = Util.ParseDateTime(model.SurrenderDate);
                rendicion.Amount = model.Amount;
                rendicion.Commentary = model.Commentary;
                rendicion.InstallmentQuantity = principal.Asignaciones.Count();
                rendicion.ReceiptQuantity = principal.Recibos.Count();
                rendicion.Balance = (principal.PaymentMethods.Count() > 0) ? 0 : model.Amount;

                _visitService.AddCollectorSurrender(rendicion, principal.Asignaciones, principal.PaymentMethods);

                return IndexSurrender();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        public ActionResult IndexState()
        {
            var model = CollectorStateTableVm.ToViewModel(_visitService.GetCollectorState());
            return PartialView("~/Views/Collections/Collector/CollectorState.cshtml", model);
        }


        public ActionResult EntryAdvancement(int id)
        {
            var collections = new CollectionHelper();
            var model = new CollectorAdvancementVm();
            model.CollectorId = id;
            model.rendiciones = _visitService.GetCollectorSurrendorByUser(id);
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();

            return PartialView("~/Views/Collections/Collector/EntryAdvancement.cshtml", model);
        }


        public ActionResult ImputSurrender(int id)
        {
            var collections = new CollectionHelper();
            var model = new CollectorAdvancementVm();
            model.CollectorId = id;
            model.rendiciones = _visitService.GetCollectorSurrendorByUser(id);
            model.avances = _visitService.GetCollectorAdvancementByUser(id);
            model.Amount = 0;
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            double total = 0;
            double totalr = 0;
            foreach (CollectorAdvancement avance in model.avances)
            {
                total = total + avance.Balance;
            }
            foreach (CollectorSurrender su in model.rendiciones)
            {
                totalr = totalr + su.Balance;
            }

            if (totalr > total)
                model.Amount = total;
            else
                model.Amount = totalr;

            return PartialView("~/Views/Collections/Collector/ImputSurrender.cshtml", model);
        }

        [HttpPost]
        public ActionResult EntryAdvancement(CollectorAdvancementVm model)
        {
            try
            {
                // TODO: Add update logic here
                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, message);
                }
                CollectorAdvancement avance = model.ToModelObject();

                _visitService.AddCollectorAdvanced(avance, int.Parse(model.ddlPaymentMethods.SelectedItem));        
               
                return IndexState();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        [HttpPost]
        public ActionResult ImputSurrender(CollectorAdvancementVm model)
        {
            try
            {
                // TODO: Add update logic here
                if (model.Amount == 0)
                {

                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe ingresar un Monto válido");
                }

                var avances = _visitService.GetCollectorAdvancementByUser(model.CollectorId);
                double total = 0;
                foreach(CollectorAdvancement av in avances)
                {
                    total = total + av.Balance;
                }

                var rendiciones = _visitService.GetCollectorSurrendorByUser(model.CollectorId);
                double totalr = 0;
                foreach (CollectorSurrender su in rendiciones)
                {
                    totalr = totalr + su.Balance;
                }
                // TODO: Add update logic here
                if (model.Amount > total || model.Amount > totalr)
                {

                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El Monto no puede ser mayor al total de avances ni al total de dinero sin rendir");
                }

                CollectorAdvancement imputacion = new CollectorAdvancement();
                imputacion.Amount = model.Amount;
                imputacion.CollectorId = model.CollectorId;
                _visitService.AddImputSurrender(imputacion);

                return IndexState();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
    }
}
