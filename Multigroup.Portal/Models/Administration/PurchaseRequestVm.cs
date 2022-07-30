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
    public class PurchaseRequestVm : BaseVm
    {
        public int PurchaseRequestId { get; set; }
        [Required(ErrorMessage = "El campo Fecha Necesidad es requerido")]
        public string NeedDate { get; set; }
        public string CreateDate { get; set; }
        public SingleSelectVm ddlProvider { get; set; }
        [Required(ErrorMessage = "El campo Usuario es requerido")]
        public SingleSelectVm ddlUser { get; set; }
        [Required(ErrorMessage = "El campo Usuario Autorizador / Comprador es requerido")]
        public SingleSelectVm ddlUserAuthorized { get; set; }
        public SingleSelectVm ddlArticles { get; set; }
        public SingleSelectVm ddlHeadings { get; set; }
        public bool Active { get; set; }
        public bool Processed { get; set; }
        public bool Managed { get; set; }
        public double Amount { get; set; }
        public int QuantityArticle { get; set; }
        public double PriceArticle { get; set; }
        public string DescriptionArticle { get; set; }
        public string Commentary { get; set; }
        [Required(ErrorMessage = "El campo Urgencia es requerido")]
        public SingleSelectVm ddlUrgency { get; set; }
        public IEnumerable<PurchaseRequestDetail> Details { get; set; }


        public PurchaseRequest ToModelObject()
        {
            return new PurchaseRequest
            {
                PurchaseRequestId = PurchaseRequestId,
                NeedDate = Util.ParseDateTime(NeedDate),
                ProveedorId = (ddlProvider == null) ? null : Util.ParseIntNullable(ddlProvider.SelectedItem),
                UserId = GetNullableValue(ddlUser).Value,
                Active = Active,
                Amount = Amount,
                Commentary = Commentary,
                UrgencyId = GetNullableValue(ddlUrgency).Value,
                Details = Details,
                UserAuthorizedId = GetNullableValue(ddlUserAuthorized).Value,
            };
        }

        public static PurchaseRequestVm FromDomainModel(PurchaseRequest entity)
        {
            return new PurchaseRequestVm
            {
                PurchaseRequestId = entity.PurchaseRequestId,
                Commentary = entity.Commentary,
                Amount = entity.Amount,
                Active = entity.Active,
                NeedDate = entity.NeedDate.Value.ToShortDateString(),
                CreateDate = entity.CreateDate.ToShortDateString(),
                Details = entity.Details,
                Processed = entity.Processed,
                Managed = entity.Managed
            };
        }
    }
}