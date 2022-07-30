using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class SpendingPaymentFilterVm
    {
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public IEnumerable<string> SelectedProvider { get; set; }
        public IEnumerable<SelectListItem> ListProvider { get; set; }
        public double AmountFrom { get; set; }
        public double AmountTo { get; set; }
        public double BalanceFrom { get; set; }
        public double BalanceTo { get; set; }
        public string Receipt { get; set; }
        public int UserId { get; set; }
    }
    public class SpendingPaymentTableVm
    {
        public IEnumerable<SpendingPaymentListVm> SpendingPaymentList { get; set; }

        public static SpendingPaymentTableVm ToViewModel(IEnumerable<SpendingPaymentSummary> entities)
        {
            return new SpendingPaymentTableVm
            {
                SpendingPaymentList = SpendingPaymentListVm.ToViewModelList(entities),
            };
        }
    }

    public class SpendingPaymentListVm
    {
        public int SpendingPaymentId { get; set; }
        public string ExecutionDate { get; set; }
        public string SystemDate { get; set; }
        public string Proveedor { get; set; }
        public string User { get; set; }
        public double Balance { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public string Receipt { get; set; }
        public int UserId { get; set; }

        public static IEnumerable<SpendingPaymentListVm> ToViewModelList(IEnumerable<SpendingPaymentSummary> entities)
        {
            return entities.Select(x => new SpendingPaymentListVm
            {
                SpendingPaymentId = x.SpendingPaymentId,
                ExecutionDate = x.ExecutionDate.ToShortDateString(),
                SystemDate = (x.SystemDate.HasValue) ? x.SystemDate.Value.ToShortDateString() : " - ",
                User = x.User,
                Balance = x.Balance,
                Amount = x.Amount,
                Commentary = x.Commentary,
                Proveedor = x.Proveedor,
                Receipt = x.Receipt,
                UserId = x.UserId
            });
        }
    }

    public class SpendingPaymentDetailTableVm
    {
        public IEnumerable<SpendingPaymentDetailListVm> SpendingPaymentList { get; set; }

        public static SpendingPaymentDetailTableVm ToViewModel(IEnumerable<SpendingPaymentDetailSummary> entities)
        {
            return new SpendingPaymentDetailTableVm
            {
                SpendingPaymentList = SpendingPaymentDetailListVm.ToViewModelList(entities),
            };
        }
    }

    public class SpendingPaymentDetailListVm
    {
        public int SpendingPaymentDetailId { get; set; }
        public int SpendingPaymentId { get; set; }
        public string DescSpending { get; set; }
        public string ReceiptSpending { get; set; }
        public double Amount { get; set; }

        public static IEnumerable<SpendingPaymentDetailListVm> ToViewModelList(IEnumerable<SpendingPaymentDetailSummary> entities)
        {
            return entities.Select(x => new SpendingPaymentDetailListVm
            {
                SpendingPaymentId = x.SpendingPaymentId,
                SpendingPaymentDetailId = x.SpendingPaymentDetailId,
                DescSpending = x.DescSpending,
                ReceiptSpending = x.ReceiptSpending,
                Amount = x.Amount,
            });
        }
    }
}