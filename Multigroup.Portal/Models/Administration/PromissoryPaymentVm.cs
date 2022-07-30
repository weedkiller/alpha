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
    public class PromissoryPaymentVm : BaseVm
    {
        public int PromissoryPaymentId { get; set; }
        [Required(ErrorMessage = "El campo Fecha Pago es requerido")]
        public string PaymentDate { get; set; }
        [Required(ErrorMessage = "El campo Cliente es requerido")]
        public SingleSelectVm ddlClient { get; set; }
        public string SystemDate { get; set; }
        public SingleSelectVm ddlPaymentMethods{ get; set; }
        public SingleSelectVm ddlPaymentMethodPM { get; set; }
         public IEnumerable<PromissoryPaymentMethod> PaymentMethods { get; set; }
        public IEnumerable<PromissoryPaymentPromissory> Promissories { get; set; }
        public IEnumerable<Promissory> PromissoriesAll { get; set; }
        public double Amount { get; set; }
        public double AmountPayment { get; set; }
        public double AmountByPaymentMethod { get; set; }
        public string Saving { get; set; }


        public PromissoryPayment ToModelObject()
        {
            return new PromissoryPayment
            {
                PromissoryPaymentId = PromissoryPaymentId,
                PaymentDate = Util.ParseDateTime(PaymentDate).Value,
                SystemDate = DateTime.Now,
                ClientId = GetNullableValue(ddlClient).Value,             
                PaymentMethods = PaymentMethods,
                Promissories = Promissories,
                Amount = Amount,
            };
        }

        public static PromissoryPaymentVm FromDomainModel(PromissoryPayment entity)
        {
            return new PromissoryPaymentVm
            {
                PromissoryPaymentId = entity.PromissoryPaymentId,
                PaymentDate = entity.PaymentDate.ToShortDateString(),
                SystemDate = entity.SystemDate.ToShortDateString(),
                Promissories = entity.Promissories,
                PaymentMethods = entity.PaymentMethods,
                PromissoriesAll = entity.PromissoriesAll,
                Amount = entity.Amount,
            };
        }
    }
}