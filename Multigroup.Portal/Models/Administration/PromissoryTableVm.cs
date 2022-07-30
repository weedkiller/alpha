using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class PromissoryFilterVm
    {
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public IEnumerable<string> SelectedCustomer{ get; set; }
        public IEnumerable<SelectListItem> ListCustomer { get; set; }
        public double AmountFrom { get; set; }
        public double AmountTo { get; set; }
        public double BalanceFrom { get; set; }
        public double BalanceTo { get; set; }
        public string Number { get; set; }
        public bool IsPaid { get; set; }
    }
    public class PromissoryTableVm
    {
        public IEnumerable<PromissoryListVm> PromissoryList { get; set; }

        public static PromissoryTableVm ToViewModel(IEnumerable<PromissorySummary> entities)
        {
            return new PromissoryTableVm
            {
                PromissoryList = PromissoryListVm.ToViewModelList(entities),
            };
        }
    }

    public class PromissoryListVm
    {
        public int PromissoryId { get; set; }
        public int Number { get; set; }
        public string Client { get; set; }
        public string User { get; set; }
        public string BroadcastDate { get; set; }
        public string CollectionDate { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public string Description { get; set; }
        public string isPaid { get; set; }

        public static IEnumerable<PromissoryListVm> ToViewModelList(IEnumerable<PromissorySummary> entities)
        {
            return entities.Select(x => new PromissoryListVm
            {
                PromissoryId = x.PromissoryId,
                BroadcastDate = x.BroadcastDate.ToShortDateString(),
                CollectionDate = x.CollectionDate.ToShortDateString(),
                User = x.User,
                Amount = x.Amount,
                Commentary = x.Commentary,
                Client = x.Client,
                Number = x.Number,
                Description = x.Description,
                isPaid = x.isPaid,
            });
        }
    }
}