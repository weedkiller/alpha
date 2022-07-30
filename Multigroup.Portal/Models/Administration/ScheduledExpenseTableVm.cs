using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class ScheduledExpenseFilterVm
    {
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public IEnumerable<string> SelectedUserAuth { get; set; }
        public IEnumerable<SelectListItem> ListUserAuth { get; set; }
        public IEnumerable<string> SelectedProvider { get; set; }
        public IEnumerable<SelectListItem> ListProvider { get; set; }
        public IEnumerable<string> SelectedAuthorized { get; set; }
        public IEnumerable<SelectListItem> ListAuthorized { get; set; }
        public double AmounFrom { get; set; }
        public double AmountTo { get; set; }
        public int PurchaseRequestId { get; set; }
        public bool Authorized { get; set; }
        public int UserCreateId { get; set; }
    }
    public class ScheduledExpenseTableVm
    {
        public IEnumerable<ScheduledExpenseListVm> ScheduledExpenseList { get; set; }

        public static ScheduledExpenseTableVm ToViewModel(IEnumerable<ScheduledExpenseSummary> entities)
        {
            return new ScheduledExpenseTableVm
            {
                ScheduledExpenseList = ScheduledExpenseListVm.ToViewModelList(entities),
            };
        }
    }

    public class ScheduledExpenseListVm
    {
        public int ScheduledExpenseId { get; set; }
        public string CreateDate { get; set; }
        public string PurchaseDate { get; set; }
        public string Proveedor { get; set; }
        public string User { get; set; }
        public int UserId { get; set; }
        public int UserAuthorizedId { get; set; }
        public string UserAuthorized { get; set; }
        public bool IsAuthorized { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public string Processed { get; set; }
        public int? PurchaseRequestId { get; set; }
        public bool Managed { get; set; }
        public static IEnumerable<ScheduledExpenseListVm> ToViewModelList(IEnumerable<ScheduledExpenseSummary> entities)
        {
            return entities.Select(x => new ScheduledExpenseListVm
            {
                ScheduledExpenseId = x.ScheduledExpenseId,
                CreateDate = x.CreateDate.ToShortDateString(),
                PurchaseDate = x.PurchaseDate.Value.ToShortDateString(),
                User = x.User,
                UserAuthorized = x.UserAuthorized,
                IsAuthorized = x.IsAuthorized,
                Amount = x.Amount,
                Commentary = x.Commentary,
                Proveedor = x.Proveedor,
                PurchaseRequestId = x.PurchaseRequestId,
                Processed = x.Processed,
                UserId = x.UserId,
                Managed = x.Managed,
                UserAuthorizedId = x.UserAuthorizedId
            });
        }
    }
}