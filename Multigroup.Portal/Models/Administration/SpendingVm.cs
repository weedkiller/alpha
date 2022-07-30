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
    public class SpendingVm : BaseVm
    {
        public int SpendingId { get; set; }
        [Required(ErrorMessage = "El campo Fecha Necesidad es requerido")]
        public string ExecutionDate { get; set; }
        public string ExpirationDate { get; set; }
        public SingleSelectVm ddlProvider { get; set; }
        public SingleSelectVm ddlUser { get; set; }
        public SingleSelectVm ddlArticles { get; set; }
        public SingleSelectVm ddlHeadings { get; set; }
        public SingleSelectVm ddlPaymentMethods { get; set; }
        public SingleSelectVm ddlPaymentMethodPM { get; set; }
        public double Amount { get; set; }
        public double AmountPayment { get; set; }
        public double AmountByPaymentMethod { get; set; }
        public double Balance { get; set; }
        public string Saving { get; set; }
        public string Receipt { get; set; }
        [Required(ErrorMessage = "El campo Descripción es requerido")]
        [MaxLength(25, ErrorMessage = "La descripción debe tener un máximo de 25 caracteres")]
        public string Description { get; set; }
        public int QuantityArticle { get; set; }
        public double PriceArticle { get; set; }
        public string DescriptionArticle { get; set; }
        public string Commentary { get; set; }
        public IEnumerable<SpendingDetail> Details { get; set; }
        public IEnumerable<SpendingPaymentMethod> PaymentMethods { get; set; }
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public IEnumerable<string> SelectedUserSE { get; set; }
        public IEnumerable<SelectListItem> ListUserSE { get; set; }
        public IEnumerable<string> SelectedUserEA { get; set; }
        public IEnumerable<SelectListItem> ListUserEA { get; set; }
        public int? PurchaseRequestId { get; set; }
        public int? ScheduledExpenseId { get; set; }
        public int? ExpenseAuthorizationId { get; set; }
        public bool chkCurrentAccount { get; set; }

        public bool chkImputed { get; set; }

        public int IInvoiceId { get; set; }
        public int ISpendingId { get; set; }
        public string ICUIT { get; set; }
        public string IBusinessName { get; set; }
        public SingleSelectVm ddlIVAPosition { get; set; }
        public SingleSelectVm ddlPurchaseDocument { get; set; }
        public SingleSelectVm ddlPurchasePerception { get; set; }
        public double? IPerceptionAmount { get; set; }
        public string ILetter { get; set; }
        public int IPrefix { get; set; }
        public int INumber { get; set; }
        public string IImputDate { get; set; }
        public double? INet { get; set; }
        public double? IExempt { get; set; }
        public double? IIVA21 { get; set; }
        public double? IIVA105 { get; set; }
        public double? IIVA22 { get; set; }
        public double? IIVA5 { get; set; }
        public double? IIVA25 { get; set; }
        public double? IOtherTaxes { get; set; }

        public Invoice invoice { get; set; }
        public int IvaPositionInt { get; set; }


        public Spending ToModelObject()
        {
            return new Spending
            {
                SpendingId = SpendingId,
                ExecutionDate = Util.ParseDateTime(ExecutionDate).Value,
                ExpirationDate = Util.ParseDateTime(ExpirationDate),
                ProveedorId = (ddlProvider == null) ? null : Util.ParseIntNullable(ddlProvider.SelectedItem),
                Receipt = Receipt,
                Amount = Amount,
                Commentary = Commentary,
                Balance = Balance,
                Details = Details,
                PaymentMethods = PaymentMethods,
                Description = Description,
            };
        }

        public Invoice ToModelObjectInvoice()
        {
            return new Invoice
            {
                InvoiceId = IInvoiceId,
                SpendingId = ISpendingId,
                CUIT = ICUIT,
                BusinessName = IBusinessName,
                IvaPositionId = int.Parse(ddlIVAPosition.SelectedItem),
                PurchaseDocumentId = int.Parse(ddlPurchaseDocument.SelectedItem),
                PurchasePerceptionId = int.Parse(ddlPurchasePerception.SelectedItem),
                PerceptionAmount = IPerceptionAmount,
                Letter = ILetter,
                Prefix = IPrefix,
                Number = INumber,
                ImputDate = Util.ParseDateTime(IImputDate).Value,
                Net = INet,
                Exempt = IExempt,
                IVA21 = IIVA21,
                IVA105 = IIVA105,
                IVA22 = IIVA22,
                IVA25 = IIVA25,
                IVA5 = IIVA5,
                OtherTaxes = IOtherTaxes,
            };
        }

        public static SpendingVm FromDomainModel(Spending entity)
        {
            return new SpendingVm
            {
                SpendingId = entity.SpendingId,
                Commentary = entity.Commentary,
                Amount = entity.Amount,
                Receipt = entity.Receipt,
                ExpirationDate = (!entity.ExpirationDate.HasValue) ? null : entity.ExpirationDate.Value.ToShortDateString(),
                ExecutionDate = entity.ExecutionDate.ToShortDateString(),
                Details = entity.Details,
                PaymentMethods = entity.PaymentMethods,
                Balance = entity.Balance,
                Description = entity.Description,
            };
        }
    }
}