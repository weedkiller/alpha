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
using System.Text.RegularExpressions;

namespace Multigroup.Portal.Controllers.Administration
{
    public class ConciliationController : Controller
    {
        private MovementService _movementService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProfileProvider _profileProvider;
        private CashierService _cashierService;
        private PaymentMethodsService _paymentMethodsService;
        private CashCycleService _cashCycleService;
        private ConciliatioService _conciliationService;

        public ConciliationController()
        {
            _movementService = new MovementService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _profileProvider = new ProfileProvider();
            _cashierService = new CashierService();
            _paymentMethodsService = new PaymentMethodsService();
            _cashCycleService = new CashCycleService();
            _conciliationService = new ConciliatioService();
        }

        [Description("Verification")]
        public ActionResult IndexVerification()
        {
            MovementFilterVm model = new MovementFilterVm();
            var collections = new CollectionHelper();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListCashier = new SelectList(_cashierService.GetCashiers(), "CashierId","Name");
            model.ListMovementType = new SelectList(_movementService.GetMovementTypes(), "MovementTypeId","Description");
            model.ListPaymentMethod = new SelectList(_paymentMethodsService.GetPaymentMethodss(), "PaymentMethodId", "Name");
            model.ListVerified = new SelectList(_movementService.GetVerified(),"VerifiedId","Description");
            model.ListCycle = new SelectList(_cashCycleService.GetCycles(), "CycleId", "CycleNumber");
            return PartialView("~/Views/Conciliation/Verification.cshtml", model);
        }

        public ActionResult GetTableVerification(int? SelectedUser, int? SelectedPaymentMethod, List<string> SelectedMovementType, List<string> SelectedCashier, List<string> SelectedVerified, string DateFrom, string DateTo, string DateSystemFrom, string DateSystemTo, List<string> SelectedCycle, string AmountFrom, string AmountTo)
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

            IEnumerable<MovementResume> resumen = _movementService.GetMovementResume(DateFrom, DateTo, DateSystemFrom, DateSystemTo, SelectedUser.ToString(), SelectedPaymentMethod.ToString(), _selectedCashier, _selectedCycle, _selectedMovementType, _selectedVerified, AmountFrom, AmountTo);
            var model = MovementTableVm.ToViewModel(resumen);

            double total = 0;

            foreach (MovementResume mov in resumen)
            {
                total = total + mov.Amount;
            }

            ViewBag.Total = total.ToString();

            return PartialView("~/Views/Conciliation/VerificationTable.cshtml", model);
        }

        [HttpPost]
        public ActionResult VerifiedMovement(MovementVerifiedVm model)
        {
            try
            {

                _movementService.VerifiedMovement(model.MovementId, model.MovementType);
                return IndexVerification();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        [Description("Reconciled")]
        public ActionResult IndexReconciled()
        {
            PaymentMethodReconciledLastTableVm model = new PaymentMethodReconciledLastTableVm();
            model = PaymentMethodReconciledLastTableVm.ToViewModel(_paymentMethodsService.GetPaymentMethodReconciledLast());
            return PartialView("~/Views/Conciliation/Reconciled.cshtml", model);
        }

        public ActionResult GetTableConciliations(int PaymentMethodId)
        {
            IEnumerable<PaymentMethodReconciledSummary> conciliations = _paymentMethodsService.GetPaymentMethodReconciledSummary(PaymentMethodId);
            var model = PaymentMethodReconciledSummaryTableVm.ToViewModel(conciliations);

            return PartialView("~/Views/Conciliation/ConciliationsTable.cshtml", model);
        }

        public ActionResult CreateReconciled(int id)
        {
            var entity = _paymentMethodsService.GetPaymentMethodReconciledSummaryLast(id);
            var model = new PaymentMethodReconciledVm();
            if (entity != null)
                model = PaymentMethodReconciledVm.FromDomainModel(entity);
            else
            {
                model.PaymentMethod = _paymentMethodsService.GetPaymentMethodssById(id).Name;
                model.ReconciledDate = " - ";
                model.SystemTime = DateTime.Now.ToString("HH:mm");
                model.SystemDate = DateTime.Now.ToShortDateString();
            }
            model.Amount = _conciliationService.GetBalance(id);
            model.PaymentMethodId = id;
            return PartialView("~/Views/Conciliation/CreateReconciled.cshtml", model);
        }

        [HttpPost]
        public ActionResult CreateReconciled(PaymentMethodReconciledVm model)
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
                
                if(!IsValidTime(model.SystemTime))
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe Ingresar horario correcto en Formato HH:MM 24 Hs");

                PaymentMethodReconciled reconciled = model.ToModelObject();
                reconciled.Time = model.SystemTime;
                reconciled.SystemDate = DateTime.Now;
                reconciled.UserId = _profileProvider.CurrentUserId;

                _conciliationService.AddPaymentMethodReconciled(reconciled);

                return IndexReconciled();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        public bool IsValidTime(string thetime)
        {

            Regex checktime = new Regex("^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
            if (!checktime.IsMatch(thetime))
                return false;

            if (thetime.Trim().Length < 5)
                thetime = thetime = "0" + thetime;

            string hh = thetime.Substring(0, 2);
            string mm = thetime.Substring(3, 2);

            int hh_i, mm_i;
            if ((int.TryParse(hh, out hh_i)) && (int.TryParse(mm, out mm_i)))
            {
                if ((hh_i >= 0 && hh_i <= 23) && (mm_i >= 0 && mm_i <= 59))
                {
                    return true;
                }
            }
            return false;
        }

    }
}