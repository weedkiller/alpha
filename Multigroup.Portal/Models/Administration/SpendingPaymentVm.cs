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
    public class SpendingPaymentVm : BaseVm
    {
        public int SpendingPaymentId { get; set; }
        [Required(ErrorMessage = "El campo Fecha Necesidad es requerido")]
        public string ExecutionDate { get; set; }
        public string SystemDate { get; set; }
        public SingleSelectVm ddlProvider { get; set; }
        public SingleSelectVm ddlUser { get; set; }
        public SingleSelectVm ddlSpendings { get; set; }
        public SingleSelectVm ddlPaymentMethods{ get; set; }
        public SingleSelectVm ddlPaymentMethodPM { get; set; }
        public double Amount { get; set; }
        public double AmountPayment { get; set; }
        public double AmountByPaymentMethod { get; set; }
        public double AmountSpending{ get; set; }
        public double Balance { get; set; }
        public string Receipt { get; set; }
        public string Commentary { get; set; }
        public bool Advancement { get; set; }
        public string Saving { get; set; }
        public double AdvancementAmount { get; set; }
        public string Provider { get; set; }
        public IEnumerable<SpendingPaymentDetail> Details { get; set; }
        public IEnumerable<SpendingPaymentPaymentMethod> PaymentMethods { get; set; }
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }



        public SpendingPayment ToModelObject()
        {
            return new SpendingPayment
            {
                SpendingPaymentId = SpendingPaymentId,
                ExecutionDate = Util.ParseDateTime(ExecutionDate).Value,
                SystemDate = Util.ParseDateTime(SystemDate),
                ProveedorId = (ddlProvider == null) ? null : Util.ParseIntNullable(ddlProvider.SelectedItem),             
                Receipt = Receipt,
                Amount = Amount,
                Commentary = Commentary,
                Balance = Balance,
                Details = Details,
                PaymentMethods = PaymentMethods,
            };
        }

        public static SpendingPaymentVm FromDomainModel(SpendingPayment entity)
        {
            return new SpendingPaymentVm
            {
                SpendingPaymentId = entity.SpendingPaymentId,
                Commentary = entity.Commentary,
                Amount = entity.Amount,
                Receipt = entity.Receipt,
                SystemDate = (!entity.SystemDate.HasValue) ? null : entity.SystemDate.Value.ToShortDateString(),
                ExecutionDate = entity.ExecutionDate.ToShortDateString(),
                Details = entity.Details,
                PaymentMethods = entity.PaymentMethods,
                Balance = entity.Balance,
            };
        }
    }
}