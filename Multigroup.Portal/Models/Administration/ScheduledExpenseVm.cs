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
    public class ScheduledExpenseVm : BaseVm
    {
        public int ScheduledExpenseId { get; set; }
        [Required(ErrorMessage = "El campo Fecha Compra es requerido")]
        public string PurchaseDate { get; set; }
        public string CreateDate { get; set; }
        [Required(ErrorMessage = "El campo Proveedor es requerido")]
        public SingleSelectVm ddlProvider { get; set; }
        [Required(ErrorMessage = "El campo Usuario es requerido")]
        public SingleSelectVm ddlUser { get; set; }
        [Required(ErrorMessage = "El campo Usuario Autoriza es requerido")]
        public SingleSelectVm ddlUserAuthorized { get; set; }
        public SingleSelectVm ddlArticles { get; set; }
        public SingleSelectVm ddlHeadings { get; set; }
        public bool IsAuthorized { get; set; }
        public bool Active { get; set; }
        public bool Processed { get; set; }
        public bool Managed { get; set; }
        public int? PurchaseRequestId { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public int QuantityArticle { get; set; }
        public double PriceArticle { get; set; }
        public string DescriptionArticle { get; set; }
        public bool MassCreation { get; set; }
        public bool MonthlyPeriod { get; set; }
        public string QuantityPeriod { get; set; }
        public string PeriodInterval { get; set; }
        public IEnumerable<ScheduledExpenseDetail> Details { get; set; }
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }


        public ScheduledExpense ToModelObject()
        {
            return new ScheduledExpense
            {
                ScheduledExpenseId = ScheduledExpenseId,
                PurchaseDate = Util.ParseDateTime(PurchaseDate),
                ProveedorId = GetNullableValue(ddlProvider).Value,
                UserId = GetNullableValue(ddlUser).Value,
                UserAuthorizedId = GetNullableValue(ddlUserAuthorized).Value,
                IsAuthorized = IsAuthorized,
                Amount = Amount,
                Commentary = Commentary,
                PurchaseRequestId = PurchaseRequestId,
                Details = Details,
            };
        }

        public static ScheduledExpenseVm FromDomainModel(ScheduledExpense entity)
        {
            return new ScheduledExpenseVm
            {
                ScheduledExpenseId = entity.ScheduledExpenseId,
                Commentary = entity.Commentary,
                Amount = entity.Amount,
                IsAuthorized = entity.IsAuthorized,
                PurchaseDate = entity.PurchaseDate.Value.ToShortDateString(),
                CreateDate = entity.CreateDate.ToShortDateString(),
                Details = entity.Details,
                PurchaseRequestId = entity.PurchaseRequestId,
                Processed = entity.Processed,
                Managed = entity.Managed,
                Active = entity.Active,
            };
        }
    }
}