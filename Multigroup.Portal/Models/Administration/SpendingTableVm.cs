using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class SpendingFilterVm
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
        public double Balance { get; set; }
        public double Imput { get; set; }
        public int SpendingPaymentId { get; set; }
        public int UserId { get; set; }
        public IEnumerable<SpendingPaymentDetail> Details { get; set; }
    }
    public class SpendingTableVm
    {
        public IEnumerable<SpendingListVm> SpendingList { get; set; }

        public static SpendingTableVm ToViewModel(IEnumerable<SpendingSummary> entities)
        {
            return new SpendingTableVm
            {
                SpendingList = SpendingListVm.ToViewModelList(entities),
            };
        }
    }

    public class SpendingListVm
    {
        public int SpendingId { get; set; }
        public string ExecutionDate { get; set; }
        public string ExpirationDate { get; set; }
        public string Proveedor { get; set; }
        public string User { get; set; }
        public double Balance { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public string Period { get; set; }
        public string Receipt { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }

        public static IEnumerable<SpendingListVm> ToViewModelList(IEnumerable<SpendingSummary> entities)
        {
            return entities.Select(x => new SpendingListVm
            {
                SpendingId = x.SpendingId,
                ExecutionDate = x.ExecutionDate.ToShortDateString(),
                ExpirationDate = (x.ExpirationDate.HasValue) ? x.ExpirationDate.Value.ToShortDateString() : " - ",
                User = x.User,
                Balance = x.Balance,
                Amount = x.Amount,
                Commentary = x.Commentary,
                Proveedor = x.Proveedor,
                Receipt = x.Receipt,
                Description = x.Description,
                Period = x.Period,
                UserId = x.UserId
            });
        }
    }

    public class SpendingDetailTableVm
    {
        public IEnumerable<SpendingDetailListVm> SpendingList { get; set; }

        public static SpendingDetailTableVm ToViewModel(IEnumerable<SpendingDetailSummary> entities)
        {
            return new SpendingDetailTableVm
            {
                SpendingList = SpendingDetailListVm.ToViewModelList(entities),
            };
        }
    }

    public class SpendingDetailListVm
    {
        public int SpendingDetailId { get; set; }
        public int SpendingId { get; set; }
        public string Article { get; set; }
        public string Origin { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public string Description { get; set; }
        public string Receipt { get; set; }

        public static IEnumerable<SpendingDetailListVm> ToViewModelList(IEnumerable<SpendingDetailSummary> entities)
        {
            return entities.Select(x => new SpendingDetailListVm
            {
                SpendingId = x.SpendingId,
                SpendingDetailId = x.SpendingDetailId,
                Article = x.Article,
                Origin = x.Origin,
                Quantity = x.Quantity,
                Price = x.Price,
                Total = x.Total,
                Receipt = x.Receipt,
                Description = x.Description,
            });
        }
    }
}