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
    public class RetirementPartnerVm : BaseVm
    {
        public int RetirementPartnerId { get; set; }
        [Required(ErrorMessage = "El campo Fecha Pago es requerido")]
        public string Date { get; set; }
        [Required(ErrorMessage = "El campo Socio es requerido")]
        public SingleSelectVm ddlPartner { get; set; }
        public string SystemDate { get; set; }
        public SingleSelectVm ddlPaymentMethods{ get; set; }
        public SingleSelectVm ddlPaymentMethodPM { get; set; }
         public IEnumerable<RetirementPartnerPaymentMethod> PaymentMethods { get; set; }
        public IEnumerable<RetirementPartnerAllocation> Allocations { get; set; }
        public IEnumerable<EarningsAllocationPartner> AllocationsAll { get; set; }
        public double Amount { get; set; }
        public double AmountPayment { get; set; }
        public double AmountByPaymentMethod { get; set; }
        public string Saving { get; set; }
        public string Commentary { get; set; }

        public RetirementPartner ToModelObject()
        {
            return new RetirementPartner
            {
                RetirementPartnerId = RetirementPartnerId,
                Date = Util.ParseDateTime(Date).Value,
                SystemDate = DateTime.Now,
                PartnerId = GetNullableValue(ddlPartner).Value,             
                PaymentMethods = PaymentMethods,
                Allocations = Allocations,
                Amount = Amount,
                Commentary = Commentary,
            };
        }

        public static RetirementPartnerVm FromDomainModel(RetirementPartner entity)
        {
            return new RetirementPartnerVm
            {
                RetirementPartnerId = entity.RetirementPartnerId,
                Date = entity.Date.ToShortDateString(),
                SystemDate = entity.SystemDate.ToShortDateString(),
                Allocations = entity.Allocations,
                PaymentMethods = entity.PaymentMethods,
                AllocationsAll = entity.AllocationsAll,
                Amount = entity.Amount,
                Commentary = entity.Commentary,
            };
        }
    }
}