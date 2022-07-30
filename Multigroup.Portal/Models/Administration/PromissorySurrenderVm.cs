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
    public class PromissorySurrenderVm : BaseVm
    {
        public int PromissorySurrenderId { get; set; }
        [Required(ErrorMessage = "El campo Fecha Pago es requerido")]
        public string PaymentDate { get; set; }
        [Required(ErrorMessage = "El campo Cliente es requerido")]
        public SingleSelectVm ddlPartner { get; set; }
        public SingleSelectVm ddlPaymentMethods{ get; set; }
        public SingleSelectVm ddlPaymentMethodPM { get; set; }
        public IEnumerable<PromissorySurrenderMethod> PaymentMethods { get; set; }
        public IEnumerable<PromissoryRendered> Promissories { get; set; }
        public IEnumerable<Promissory> PromissoriesAll { get; set; }
        public double Amount { get; set; }
        public double AmountPayment { get; set; }
        public double AmountBySurrenderMethod { get; set; }
        public string Saving { get; set; }


        public PromissorySurrender ToModelObject()
        {
            return new PromissorySurrender
            {
                PromissorySurrenderId = PromissorySurrenderId,
                PaymentDate = Util.ParseDateTime(PaymentDate).Value,
                PartnerId = GetNullableValue(ddlPartner).Value,
                PaymentMethods = PaymentMethods,
                rendereds = Promissories,
                Amount = Amount,
            };
        }

        public static PromissorySurrenderVm FromDomainModel(PromissorySurrender entity)
        {
            return new PromissorySurrenderVm
            {
                PromissorySurrenderId = entity.PromissorySurrenderId,
                PaymentDate = entity.PaymentDate.ToShortDateString(),
                Promissories = entity.rendereds,
                PaymentMethods = entity.PaymentMethods,
                Amount = entity.Amount,
                PromissoriesAll = entity.PromissoriesAll,
            };
        }
    }
}