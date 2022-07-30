using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Enums;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Multigroup.Portal.Models.Shared;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{
    public class ExpenseAuthorizationVm : BaseVm
    {
        public int ExpenseAuthorizationId { get; set; }
        public string PurchaseDate { get; set; }
        public string CreateDate { get; set; }
        public SingleSelectVm ddlProvider { get; set; }
        [Required(ErrorMessage = "El campo Usuario es requerido")]
        public SingleSelectVm ddlUser { get; set; }
        [Required(ErrorMessage = "El campo Comprador es requerido")]
        public SingleSelectVm ddlUserBuyer { get; set; }
        public SingleSelectVm ddlArticles { get; set; }
        public SingleSelectVm ddlHeadings { get; set; }
        public bool IsAuthorized { get; set; }
        public bool Active { get; set; }
        public bool Managed { get; set; }
        public bool Processed { get; set; }
        public double Amount { get; set; }
        public int QuantityArticle { get; set; }
        public double PriceArticle { get; set; }
        public string DescriptionArticle { get; set; }
        public string Commentary { get; set; }
        public int? PurchaseRequestId { get; set; }
        public int? ScheduledExpenseId { get; set; }
        public IEnumerable<ExpenseAuthorizationDetail> Details { get; set; }
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public IEnumerable<string> SelectedUserSE { get; set; }
        public IEnumerable<SelectListItem> ListUserSE { get; set; }


        public ExpenseAuthorization ToModelObject()
        {
            return new ExpenseAuthorization
            {
                ExpenseAuthorizationId = ExpenseAuthorizationId,
                PurchaseDate = Util.ParseDateTime(PurchaseDate),
                ProveedorId = (ddlProvider == null) ? null : Util.ParseIntNullable(ddlProvider.SelectedItem),
                UserId = GetNullableValue(ddlUser).Value,
                UserBuyerId = GetNullableValue(ddlUserBuyer).Value,
                IsAuthorized = IsAuthorized,
                Amount = Amount,
                Commentary = Commentary,
                PurchaseRequestId = PurchaseRequestId,
                ScheduledExpenseId = ScheduledExpenseId,
                Details = Details,
            };
        }

        public static ExpenseAuthorizationVm FromDomainModel(ExpenseAuthorization entity)
        {
            return new ExpenseAuthorizationVm
            {
                ExpenseAuthorizationId = entity.ExpenseAuthorizationId,
                Commentary = entity.Commentary,
                Amount = entity.Amount,
                IsAuthorized = entity.IsAuthorized,
                PurchaseDate = entity.PurchaseDate.Value.ToShortDateString(),
                CreateDate = entity.CreateDate.ToShortDateString(),
                Details = entity.Details,
                PurchaseRequestId = entity.PurchaseRequestId,
                ScheduledExpenseId = entity.ScheduledExpenseId,
                Active = entity.Active,
                Processed = entity.Processed,
                Managed = entity.Managed
            };
        }
    }
}