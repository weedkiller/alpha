using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class CashCycleFilterVm
    {
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public IEnumerable<string> SelectedUserValidate { get; set; }
        public IEnumerable<SelectListItem> ListUserValidate { get; set; }
        public IEnumerable<string> SelectedCashier { get; set; }
        public IEnumerable<SelectListItem> ListCashier { get; set; }
        public int CycleNumber { get; set; }
    }
    public class CashCycleTableVm
    {
        public IEnumerable<CashCycleListVm> CashCycleList { get; set; }
        public static CashCycleTableVm ToViewModel(IEnumerable<CashCycleSummary> entities)
        {
            return new CashCycleTableVm
            {
                CashCycleList = CashCycleListVm.ToViewModelList(entities),
            };
        }
    }

    public class CashCycleListVm
    {
        public int CashCycleId { get; set; }
        public int CashierId { get; set; }
        public string Date { get; set; }
        public bool Validate { get; set; }
        public bool Closed { get; set; }
        public string Time { get; set; }
        public string User { get; set; }
        public string UserValidate { get; set; }
        public string Cashier { get; set; }
        public int CycleNumber { get; set; }

        public static IEnumerable<CashCycleListVm> ToViewModelList(IEnumerable<CashCycleSummary> entities)
        {
            return entities.Select(x => new CashCycleListVm
            {
                CashCycleId = x.CashCycleId,
                Date = (x.Date.HasValue) ? x.Date.Value.ToShortDateString() : null,               
                User = x.User,
                Validate = x.Validate,
                Closed = x.Closed,
                Time = x.Time,
                UserValidate = x.UserValidate,
                Cashier = x.Cashier,
                CycleNumber = x.CycleNumber,
                CashierId = x.CashierId
            });
        }
    }

    public class CashCyclePaymentTableVm
    {
        public IEnumerable<CashCyclePaymentListVm> CashCyclePaymentList { get; set; }
        public static CashCyclePaymentTableVm ToViewModel(IEnumerable<CashCyclePayment> entities)
        {
            return new CashCyclePaymentTableVm
            {
                CashCyclePaymentList = CashCyclePaymentListVm.ToViewModelList(entities),
            };
        }
    }

    public class CashCyclePaymentListVm
    {
        public string PaymentMethod { get; set; }
        public string Tipo { get; set; }
        public string Fecha { get; set; }
        public double Amount { get; set; }

        public static IEnumerable<CashCyclePaymentListVm> ToViewModelList(IEnumerable<CashCyclePayment> entities)
        {
            return entities.Select(x => new CashCyclePaymentListVm
            {
                PaymentMethod = x.PaymentMethod,
                Fecha = x.Fecha.ToShortDateString(),
                Tipo = x.Tipo,
                Amount = x.Amount,
            });
        }
    }

    public class CashCyclePaymentMethodSummaryTableVm
    {
        public IEnumerable<CashCyclePaymentMethodSummaryListVm> CashCyclePaymentList { get; set; }
        public static CashCyclePaymentMethodSummaryTableVm ToViewModel(IEnumerable<CashCyclePaymentMethodSummary> entities)
        {
            return new CashCyclePaymentMethodSummaryTableVm
            {
                CashCyclePaymentList = CashCyclePaymentMethodSummaryListVm.ToViewModelList(entities),
            };
        }
    }

    public class CashCyclePaymentMethodSummaryListVm
    {
        public int CashCyclePaymentMethodId { get; set; }
        public int CashCycleId { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public double OLD { get; set; }
        public double Entry { get; set; }
        public double Egress { get; set; }
        public double Balance { get; set; }

        public static IEnumerable<CashCyclePaymentMethodSummaryListVm> ToViewModelList(IEnumerable<CashCyclePaymentMethodSummary> entities)
        {
            return entities.Select(x => new CashCyclePaymentMethodSummaryListVm
            {
                CashCycleId = x.CashCycleId,
                PaymentMethodId = x.PaymentMethodId,
                CashCyclePaymentMethodId = x.CashCyclePaymentMethodId,
                OLD = x.OLD,
                PaymentMethod = x.PaymentMethod,
                Entry = x.Entry,
                Egress = x.Egress,
                Balance = x.Balance,
            });
        }
    }
}