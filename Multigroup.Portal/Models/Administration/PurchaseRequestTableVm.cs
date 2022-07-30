using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class PurchaseRequestFilterVm
    {
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public IEnumerable<string> SelectedUserAuthorized { get; set; }
        public IEnumerable<SelectListItem> ListUserAuthorized { get; set; }
        public IEnumerable<string> SelectedProvider { get; set; }
        public IEnumerable<SelectListItem> ListProvider { get; set; }
        public IEnumerable<string> SelectedUrgency { get; set; }
        public IEnumerable<SelectListItem> ListUrgency { get; set; }
        public double AmountFrom { get; set; }
        public double AmountTo { get; set; }
        public bool Active { get; set; }
        public int UserId { get; set; }
    }
    public class PurchaseRequestTableVm
    {
        public IEnumerable<PurchaseRequestListVm> PurchaseRequestList { get; set; }

        public static PurchaseRequestTableVm ToViewModel(IEnumerable<PurchaseRequestSummary> entities)
        {
            return new PurchaseRequestTableVm
            {
                PurchaseRequestList = PurchaseRequestListVm.ToViewModelList(entities),
            };
        }
    }

    public class PurchaseRequestListVm
    {
        public int PurchaseRequestId { get; set; }
        public string CreateDate { get; set; }
        public string NeedDate { get; set; }
        public string Proveedor { get; set; }
        public string User { get; set; }
        public string Urgency { get; set; }
        public bool Active { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public string Processed { get; set; }
        public string UserAuthorized { get; set; }
        public bool Managed { get; set; }
        public int UserAuthorizedId { get; set; }

        public static IEnumerable<PurchaseRequestListVm> ToViewModelList(IEnumerable<PurchaseRequestSummary> entities)
        {
            return entities.Select(x => new PurchaseRequestListVm
            {
                PurchaseRequestId = x.PurchaseRequestId,
                CreateDate = x.CreateDate.ToShortDateString(),
                NeedDate = x.NeedDate.Value.ToShortDateString(),
                User = x.User,
                Active = x.Active,
                Amount = x.Amount,
                Commentary = x.Commentary,
                Proveedor = x.Proveedor,
                Urgency = x.Urgency,
                Processed = x.Processed,
                UserAuthorized = x.UserAuthorized,
                Managed = x.Managed,
                UserAuthorizedId = x.UserAuthorizedId,
            });
        }
    }
}