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

namespace Multigroup.Portal.Controllers.Administration
{
    public class CashCycleController : Controller
    {
        private CashCycleService _cashCycleService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProfileProvider _profileProvider;
        private CashierService _cashierService;
        private PaymentMethodsService _paymentMethodsService;
        private MovementService _movementService;

        public CashCycleController()
        {
            _cashCycleService = new CashCycleService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _profileProvider = new ProfileProvider();
            _cashierService = new CashierService();
            _paymentMethodsService = new PaymentMethodsService();
            _movementService = new MovementService();
        }

        [Description("CashCyclesOpen")]
        public ActionResult IndexOpenCash()
        {
            var model = CashCycleTableVm.ToViewModel(_cashCycleService.GetCashCycleSummaryOpen());
            return PartialView("~/Views/CashCycle/CashCycleOpenTable.cshtml", model);
        }
        [LogActionFilter]
        [Description("CashCyclesClose")]
        public ActionResult IndexCloseCash()
        {
            CashCycleFilterVm model = new CashCycleFilterVm();
            var collections = new CollectionHelper();
            model.ListCashier = new SelectList(_cashierService.GetCashiers(), "CashierId", "Name");
            return PartialView("~/Views/CashCycle/CashCycleCloses.cshtml", model);
        }

        public ActionResult GetTableCloseCash(int SelectedCashier)
        {
            var model = CashCycleTableVm.ToViewModel(_cashCycleService.GetCashCycleSummaryClose(SelectedCashier));
            return PartialView("~/Views/CashCycle/CashCycleClosesTable.cshtml", model);
        }



        [LogActionFilter]
        [Description("Crear CashCycles")]
        public ActionResult CloseCycle()
        {
            var model = new CashCycleVm();
            var collections = new CollectionHelper();
            List<CashCyclePaymentMethod> lists = new List<CashCyclePaymentMethod>();
            model.PaymentMethods = lists;
            Cashier cashier = _cashierService.GetCashiersByUserId(_profileProvider.CurrentUserId);
            model.Cashier = cashier.Name;
            model.CashierId = cashier.CashierId;
            CashCycle cashCycleClose = _cashCycleService.GetCashCycleByCashierId(cashier.CashierId);
            model.CycleNumber = cashCycleClose.CycleNumber;
            model.ddlUserValidate = collections.GetUsersSingleSelectVm();
            model.ddlUserValidate.SelectedItem = _profileProvider.CurrentUserId.ToString();
            model.CashCycleId = cashCycleClose.CashCycleId;
            model.CashierId = cashCycleClose.CashierId;
            CashCycle cashCycle = _cashCycleService.LastCashCycleByCashier(cashier.CashierId);
            if (cashCycle != null)
                model.lastDate = cashCycle.Date.Value.ToShortDateString();
            else
                model.lastDate = "Sin cierres anteriores";
            model.Date = DateTime.Now.ToShortDateString();
            Session["CashCycle"] = model;
            return PartialView("~/Views/CashCycle/CashCycleClose.cshtml", model);
        }

        [HttpPost]
        public ActionResult Close(CashCycleVm model)
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
                CashCycleVm principal = (CashCycleVm)Session["CashCycle"];
                principal.Validate = false;
                principal.ddlUserValidate = model.ddlUserValidate;
                var cashCycle = principal.ToModelObject();
                cashCycle.UserId = _profileProvider.CurrentUserId;
                if (cashCycle.UserValidateId == _profileProvider.CurrentUserId)
                    cashCycle.Validate = true;

                _cashCycleService.CloseCashCycle(cashCycle);

                return CloseCycle();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        [LogActionFilter]
        [Description("Crear CashCycles")]
        public ActionResult Transfer()
        {
            var model = new CashCycleTransferVm();
            var collections = new CollectionHelper();

            model.ddlOriginCashier = collections.GetCashierSingleSelectVm();
            model.ddlDestinyCashier = collections.GetCashierSingleSelectVm();
            model.ddlOriginPaymentMethods = collections.GetPaymentMethodNCSingleSelectVm();
            model.Amount = 0;
            return PartialView("~/Views/CashCycle/CashCycleTransfer.cshtml", model);
        }

        [Description("BuscarBalance")]
        public ActionResult GetBalance(int OriginCashierId, int PaymentMethodId, int? DestinyCashierId, double Amount, string Commentary)
        {
            CashCycleTransferVm model = new CashCycleTransferVm();
            var collections = new CollectionHelper();
            model.ddlOriginCashier = collections.GetCashierSingleSelectVm();
            model.ddlOriginCashier.SelectedItem = OriginCashierId.ToString();
            model.ddlOriginPaymentMethods = collections.GetPaymentMethodNCSingleSelectVm();
            model.ddlOriginPaymentMethods.SelectedItem = PaymentMethodId.ToString();
            model.ddlDestinyCashier = collections.GetCashierSingleSelectVm();
            if (DestinyCashierId.HasValue)
                model.ddlDestinyCashier.SelectedItem = DestinyCashierId.Value.ToString();
            model.Balance = _cashCycleService.GetAmountByCashierPM(OriginCashierId, PaymentMethodId);
            model.Commentary = Commentary;
            model.Amount = Amount;
            return PartialView("~/Views/CashCycle/CashCycleTransfer.cshtml", model);
        }

        [HttpPost]
        public ActionResult CreateTransfer(CashCycleTransferVm model)
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
                var transferCashier = model.ToModelObject();

                transferCashier.UserId = _profileProvider.CurrentUserId;
                if(model.Amount > _cashCycleService.GetAmountByCashierPM(int.Parse(model.ddlOriginCashier.SelectedItem), int.Parse(model.ddlOriginPaymentMethods.SelectedItem)))
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El monto a transferir debe ser menor o igual al balance del medio de pago");

                if (model.ddlOriginPaymentMethods == null || model.ddlOriginPaymentMethods.SelectedItem == null || model.ddlOriginPaymentMethods.SelectedItem.Equals(""))
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar medio de pago");

                if (model.ddlDestinyCashier == null || model.ddlDestinyCashier.SelectedItem == null || model.ddlDestinyCashier.SelectedItem.Equals(""))
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar Caja Destino");
                if (model.ddlOriginCashier == null || model.ddlOriginCashier.SelectedItem == null || model.ddlOriginCashier.SelectedItem.Equals(""))
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar Caja Origen");

                _cashCycleService.Transfer(transferCashier, model.ddlOriginPaymentMethods.SelectedItem);

                return CloseCycle();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        [LogActionFilter]
        [Description("Crear CashCycles")]
        public ActionResult TransferPM()
        {
            var model = new CashCycleTransferVm();
            var collections = new CollectionHelper();

            model.ddlOriginCashier = collections.GetCashierSingleSelectVm();
            model.ddlDestinyCashier = collections.GetCashierSingleSelectVm();
            model.ddlOriginPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlDestinyPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.Origin = 0;
            model.Destiny = 0;
            model.Amount = 0;
            return PartialView("~/Views/CashCycle/CashCycleTransferPM.cshtml", model);
        }

        [Description("BuscarBalancePM")]
        public ActionResult GetCashiers(int? OriginCashierId, int? OriginPaymentMethodId, int? DestinyCashierId, int? DestinyPaymentMethodId, double Amount, string Commentary)
        {
            CashCycleTransferVm model = new CashCycleTransferVm();
            var collections = new CollectionHelper();
            model.ddlOriginCashier = collections.GetCashierSingleSelectVm();
            if (OriginCashierId.HasValue)
                model.ddlOriginCashier.SelectedItem = OriginCashierId.Value.ToString();
            model.ddlOriginPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            if (OriginPaymentMethodId.HasValue)
            {
                model.ddlOriginPaymentMethods.SelectedItem = OriginPaymentMethodId.Value.ToString();
                PaymentMethod payment = _paymentMethodsService.GetPaymentMethods(OriginPaymentMethodId.Value);
                if (!payment.Consolidated)
                    model.Origin = 1;
                else
                    model.Origin = 0;
            }
            model.ddlDestinyCashier = collections.GetCashierSingleSelectVm();
            if (DestinyCashierId.HasValue)
                model.ddlDestinyCashier.SelectedItem = DestinyCashierId.Value.ToString();
            model.ddlDestinyPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            if (DestinyPaymentMethodId.HasValue)
            {
                model.ddlDestinyPaymentMethods.SelectedItem = DestinyPaymentMethodId.Value.ToString();
                PaymentMethod payment = _paymentMethodsService.GetPaymentMethods(DestinyPaymentMethodId.Value);
                if (!payment.Consolidated)
                    model.Destiny = 1;
                else
                    model.Destiny = 0;
            }

            model.Commentary = Commentary;
            model.Amount = Amount;
            return PartialView("~/Views/CashCycle/CashCycleTransferPM.cshtml", model);
        }

        [HttpPost]
        public ActionResult CreateTransferPM(CashCycleTransferVm model)
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

                var transferCashier = new TransferCashier();

                if (model.ddlOriginPaymentMethods == null || model.ddlOriginPaymentMethods.SelectedItem == null || model.ddlOriginPaymentMethods.SelectedItem.Equals(""))
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar medio de pago Origen");
                else
                {
                    PaymentMethod payment = _paymentMethodsService.GetPaymentMethods(int.Parse(model.ddlOriginPaymentMethods.SelectedItem));
                    if (!payment.Consolidated)
                    {
                        if (model.ddlOriginCashier == null || model.ddlOriginCashier.SelectedItem == null || model.ddlOriginCashier.SelectedItem.Equals(""))
                            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar Caja Origen");
                        else
                            transferCashier.OriginCashierId = int.Parse(model.ddlOriginCashier.SelectedItem);
                    }
                    else
                        transferCashier.OriginCashierId = 0;
                }

                if (model.ddlDestinyPaymentMethods == null || model.ddlDestinyPaymentMethods.SelectedItem == null || model.ddlDestinyPaymentMethods.SelectedItem.Equals(""))
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar medio de pago Destino");
                else
                {
                    PaymentMethod payment = _paymentMethodsService.GetPaymentMethods(int.Parse(model.ddlDestinyPaymentMethods.SelectedItem));
                    if (!payment.Consolidated)
                    {
                        if (model.ddlDestinyCashier == null || model.ddlDestinyCashier.SelectedItem == null || model.ddlDestinyCashier.SelectedItem.Equals(""))
                            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar Caja Destino");
                        else
                            transferCashier.DestinyCashierId = int.Parse(model.ddlDestinyCashier.SelectedItem);
                    }
                    else
                        transferCashier.DestinyCashierId = 0;
                }

                if (0 > model.Amount)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El monto a transferir debe ser mayor a 0");
                else
                    transferCashier.Amount = model.Amount;
                transferCashier.Commentary = model.Commentary;
                transferCashier.UserId = _profileProvider.CurrentUserId;

                _cashCycleService.TransferPM(transferCashier, model.ddlOriginPaymentMethods.SelectedItem, model.ddlDestinyPaymentMethods.SelectedItem);

                return CloseCycle();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        [Description("Verification")]
        public ActionResult IndexMovement()
        {
            MovementFilterVm model = new MovementFilterVm();
            var collections = new CollectionHelper();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListCashier = new SelectList(_cashierService.GetCashiers(), "CashierId", "Name");
            model.ListMovementType = new SelectList(_movementService.GetMovementTypes(), "MovementTypeId", "Description");
            model.ListPaymentMethod = new SelectList(_paymentMethodsService.GetPaymentMethodss(), "PaymentMethodId", "Name");
            model.ListVerified = new SelectList(_movementService.GetVerified(), "VerifiedId", "Description");
            model.ListCycle = new SelectList(_cashCycleService.GetCycles(), "CycleId", "CycleNumber");
            return PartialView("~/Views/CashCycle/Movement.cshtml", model);
        }

        public ActionResult GetTableMovement(int? SelectedUser, int? SelectedPaymentMethod, List<string> SelectedMovementType, List<string> SelectedCashier, List<string> SelectedVerified, string DateFrom, string DateTo, string DateSystemFrom, string DateSystemTo, List<string> SelectedCycle, string AmountFrom, string AmountTo)
        {
            if (DateFrom == "")
                DateFrom = "01/01/1900";
            if (DateTo == "")
                DateTo = "01/01/2100";
            if (DateSystemFrom == "")
                DateSystemFrom = "01/01/1900";
            if (DateSystemTo == "")
                DateSystemTo = "01/01/2100";
            if (SelectedUser == null)
                SelectedUser = 0;
            if (SelectedPaymentMethod == null)
                SelectedPaymentMethod = 0;
            if (AmountFrom == "")
                AmountFrom = "-10000000";
            if (AmountTo == "")
                AmountTo = "10000000";
            string _selectedCashier = (SelectedCashier[0] != "" && SelectedCashier.Count() > 0) ? String.Join(",", SelectedCashier) : "0";
            string _selectedCycle = (SelectedCycle[0] != "" && SelectedCycle.Count() > 0) ? String.Join(",", SelectedCycle) : "0";
            string _selectedMovementType = (SelectedMovementType[0] != "" && SelectedMovementType.Count() > 0) ? String.Join(",", SelectedMovementType) : "0";
            string _selectedVerified = (SelectedVerified[0] != "" && SelectedVerified.Count() > 0) ? String.Join(",", SelectedVerified) : "0";

            IEnumerable <MovementResume> resumen = _movementService.GetMovementResume(DateFrom, DateTo, DateSystemFrom, DateSystemTo, SelectedUser.ToString(), SelectedPaymentMethod.ToString(), _selectedCashier, _selectedCycle, _selectedMovementType, _selectedVerified, AmountFrom, AmountTo);
            var model = MovementTableVm.ToViewModel(resumen);

            double total = 0;

            foreach(MovementResume mov in resumen)
            {
                total = total + mov.Amount;
            }

            ViewBag.Total = total.ToString();

            return PartialView("~/Views/CashCycle/MovementTable.cshtml", model);
        }

        [Description("Balances")]
        public ActionResult IndexBalances()
        {
            MovementBalanceTableVm model = new MovementBalanceTableVm();
            model = MovementBalanceTableVm.ToViewModel(_movementService.GetMovementBalance());
            return PartialView("~/Views/CashCycle/Balance.cshtml", model);
        }

        public ActionResult MediosPagoList(int idCycle, int idCashier)
        {
            var model = CashCyclePaymentMethodSummaryTableVm.ToViewModel(_cashCycleService.GetCashCyclePMSummaryOpen(idCycle, idCashier));
            return PartialView("~/Views/CashCycle/PaymentMethodsTable.cshtml", model);
        }

        public ActionResult OperacionesList(int idCycle, int idCashier)
        {
            var model = CashCyclePaymentTableVm.ToViewModel(_cashCycleService.GetCashCyclePayments(idCycle, idCashier));
            return PartialView("~/Views/CashCycle/TransferTable.cshtml", model);
        }

    }
}