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
    public class IncomePartnerVm : BaseVm
    {
        public int IncomePartnerId { get; set; }
        [Required(ErrorMessage = "El campo Fecha Pago es requerido")]
        public string Date { get; set; }
        [Required(ErrorMessage = "El campo Socio es requerido")]
        public SingleSelectVm ddlPartner { get; set; }
        public string SystemDate { get; set; }
        public SingleSelectVm ddlPaymentMethods{ get; set; }
        public SingleSelectVm ddlPaymentMethodPM { get; set; }
         public IEnumerable<IncomePartnerPaymentMethod> PaymentMethods { get; set; }
        public IEnumerable<IncomePartnerAllocation> Allocations { get; set; }
        public IEnumerable<ContributionAllocationPartner> AllocationsAll { get; set; }
        public double Amount { get; set; }
        public double AmountPayment { get; set; }
        public double AmountByPaymentMethod { get; set; }
        public string Saving { get; set; }
        public string Commentary { get; set; }

        public IncomePartner ToModelObject()
        {
            return new IncomePartner
            {
                IncomePartnerId = IncomePartnerId,
                Date = Util.ParseDateTime(Date).Value,
                SystemDate = DateTime.Now,
                PartnerId = GetNullableValue(ddlPartner).Value,             
                PaymentMethods = PaymentMethods,
                Allocations = Allocations,
                Amount = Amount,
                Commentary = Commentary,
            };
        }

        public static IncomePartnerVm FromDomainModel(IncomePartner entity)
        {
            return new IncomePartnerVm
            {
                IncomePartnerId = entity.IncomePartnerId,
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