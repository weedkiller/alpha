using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class MovementFilterVm
    {
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public IEnumerable<string> SelectedPaymentMethod { get; set; }
        public IEnumerable<SelectListItem> ListPaymentMethod { get; set; }
        public IEnumerable<string> SelectedMovementType { get; set; }
        public IEnumerable<SelectListItem> ListMovementType { get; set; }
        public IEnumerable<string> SelectedCashier { get; set; }
        public IEnumerable<SelectListItem> ListCashier { get; set; }
        public IEnumerable<string> SelectedVerified { get; set; }
        public IEnumerable<SelectListItem> ListVerified { get; set; }
        public IEnumerable<string> SelectedCycle { get; set; }
        public IEnumerable<SelectListItem> ListCycle { get; set; }
        public double AmountFrom { get; set; }
        public double AmountTo { get; set; }
    }
    public class MovementTableVm
    {
        public IEnumerable<MovementListVm> MovementList { get; set; }

        public static MovementTableVm ToViewModel(IEnumerable<MovementResume> entities)
        {
            return new MovementTableVm
            {
                MovementList = MovementListVm.ToViewModelList(entities),
            };
        }
    }

    public class MovementListVm
    {
        public int MovementId { get; set; }
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public string Date { get; set; }
        public string Movement { get; set; }
        public string Cashier { get; set; }
        public string Cycle { get; set; }
        public string SystemDate { get; set; }
        public string User { get; set; }
        public string Verified { get; set; }

        public static IEnumerable<MovementListVm> ToViewModelList(IEnumerable<MovementResume> entities)
        {
            return entities.Select(x => new MovementListVm
            {
                MovementId = x.MovementId,
                Date = x.Date.ToShortDateString(),
                SystemDate = x.SystemDate.ToShortDateString(),
                User = x.User,
                PaymentMethod = x.PaymentMethod,
                Amount = x.Amount,
                Movement = x.Movement,
                Cashier = x.Cashier,
                Cycle = x.Cycle,
                Verified = x.Verified
            });
        }
    }

    public class MovementVerifiedVm
    {
        public int MovementId { get; set; }
        public string MovementType { get; set; }
        
    }

    public class PaymentMethodReconciledLastTableVm
    {
        public IEnumerable<PaymentMethodReconciledLastListVm> MovementList { get; set; }

        public static PaymentMethodReconciledLastTableVm ToViewModel(IEnumerable<PaymentMethodReconciledLast> entities)
        {
            return new PaymentMethodReconciledLastTableVm
            {
                MovementList = PaymentMethodReconciledLastListVm.ToViewModelList(entities),
            };
        }
    }

    public class PaymentMethodReconciledLastListVm
    {
        public int PaymentMethodReconciledId { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public string ReconciledDate { get; set; }

        public static IEnumerable<PaymentMethodReconciledLastListVm> ToViewModelList(IEnumerable<PaymentMethodReconciledLast> entities)
        {
            return entities.Select(x => new PaymentMethodReconciledLastListVm
            {
                PaymentMethodReconciledId = x.PaymentMethodReconciledId,
                ReconciledDate = (x.ReconciledDate.HasValue) ? x.ReconciledDate.Value.ToShortDateString() : " - ",
                PaymentMethod = x.PaymentMethod,
                PaymentMethodId = x.PaymentMethodId,
            });
        }
    }

    public class MovementBalanceTableVm
    {
        public IEnumerable<MovementBalanceListVm> MovementBalanceList { get; set; }

        public static MovementBalanceTableVm ToViewModel(IEnumerable<MovementBalance> entities)
        {
            return new MovementBalanceTableVm
            {
                MovementBalanceList = MovementBalanceListVm.ToViewModelList(entities),
            };
        }
    }

    public class MovementBalanceListVm
    {
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public string ConciliationDate { get; set; }
        public string User { get; set; }

        public static IEnumerable<MovementBalanceListVm> ToViewModelList(IEnumerable<MovementBalance> entities)
        {
            return entities.Select(x => new MovementBalanceListVm
            {
                PaymentMethod = x.PaymentMethod,
                ConciliationDate = (x.ConciliationDate.HasValue) ? x.ConciliationDate.Value.ToShortDateString() : " - ",
                Amount = x.Amount,
                User = (!x.User.Equals("")) ? x.User : " - ",
            });
        }
    }

    public class PaymentMethodReconciledSummaryTableVm
    {
        public IEnumerable<PaymentMethodReconciledSummaryListVm> MovementList { get; set; }

        public static PaymentMethodReconciledSummaryTableVm ToViewModel(IEnumerable<PaymentMethodReconciledSummary> entities)
        {
            return new PaymentMethodReconciledSummaryTableVm
            {
                MovementList = PaymentMethodReconciledSummaryListVm.ToViewModelList(entities),
            };
        }
    }

    public class PaymentMethodReconciledSummaryListVm
    {
        public int PaymentMethodReconciledId { get; set; }
        public string PaymentMethod { get; set; }
        public string User { get; set; }
        public string ReconciledDate { get; set; }
        public string Time { get; set; }
        public string SystemDate { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }

        public static IEnumerable<PaymentMethodReconciledSummaryListVm> ToViewModelList(IEnumerable<PaymentMethodReconciledSummary> entities)
        {
            return entities.Select(x => new PaymentMethodReconciledSummaryListVm
            {
                PaymentMethodReconciledId = x.PaymentMethodReconciledId,
                ReconciledDate = x.ReconciledDate.ToShortDateString(),
                SystemDate = x.SystemDate.ToShortDateString(),
                PaymentMethod = x.PaymentMethod,
                User = x.User,
                Time = x.Time,
                Amount = x.Amount,
                Commentary = x.Commentary,
            });
        }
    }
}