using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class ExpenseAuthorizationFilterVm
    {
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public IEnumerable<string> SelectedUserBuyer { get; set; }
        public IEnumerable<SelectListItem> ListUserBuyer { get; set; }
        public IEnumerable<string> SelectedProvider { get; set; }
        public IEnumerable<SelectListItem> ListProvider { get; set; }
        public double AmounFrom { get; set; }
        public double AmountTo { get; set; }
        public int PurchaseRequest { get; set; }
        public bool Authorized { get; set; }
        public int UserCreateId { get; set; }
    }
    public class ExpenseAuthorizationTableVm
    {
        public IEnumerable<ExpenseAuthorizationListVm> ExpenseAuthorizationList { get; set; }

        public static ExpenseAuthorizationTableVm ToViewModel(IEnumerable<ExpenseAuthorizationSummary> entities)
        {
            return new ExpenseAuthorizationTableVm
            {
                ExpenseAuthorizationList = ExpenseAuthorizationListVm.ToViewModelList(entities),
            };
        }
    }

    public class ExpenseAuthorizationListVm
    {
        public int ExpenseAuthorizationId { get; set; }
        public string CreateDate { get; set; }
        public string PurchaseDate { get; set; }
        public string Proveedor { get; set; }
        public string User { get; set; }
        public string UserBuyer { get; set; }
        public bool IsAuthorized { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public string Processed { get; set; }
        public bool Managed { get; set; }
        public int? PurchaseRequestId { get; set; }
        public int? ScheduledExpenseId { get; set; }
        public int UserBuyerId { get; set; }
        public static IEnumerable<ExpenseAuthorizationListVm> ToViewModelList(IEnumerable<ExpenseAuthorizationSummary> entities)
        {
            return entities.Select(x => new ExpenseAuthorizationListVm
            {
                ExpenseAuthorizationId = x.ExpenseAuthorizationId,
                CreateDate = x.CreateDate.ToShortDateString(),
                PurchaseDate = x.PurchaseDate.Value.ToShortDateString(),
                User = x.User,
                UserBuyer = x.UserBuyer,
                IsAuthorized = x.IsAuthorized,
                Amount = x.Amount,
                Commentary = x.Commentary,
                Proveedor = x.Proveedor,
                PurchaseRequestId = x.PurchaseRequestId,
                ScheduledExpenseId = x.ScheduledExpenseId,
                Processed = x.Processed,
                UserBuyerId = x.UserBuyerId,
                Managed = x.Managed
            });
        }
    }
}