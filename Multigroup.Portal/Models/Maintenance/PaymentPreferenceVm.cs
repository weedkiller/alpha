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

namespace Multigroup.Portal.Models.Maintenance
{
    public class PaymentPreferenceVm : BaseVm
    {
        public int PaymentPreferenceId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string Description { get; set; }
        public bool Active { get; set; }
        public bool Assing { get; set; }
        public string Date { get; set; }
        public SingleSelectVm ddlPaymentMethod { get; set; }
        public IEnumerable<PaymentPreferencePaymentMethods> Details { get; set; }

        public PaymentPreference ToModelObject()
        {
            return new PaymentPreference
            {
                PaymentPreferenceId = PaymentPreferenceId,
                Assing = Assing,
                Active = Active,
                Description = Description
            };
        }

        public static PaymentPreferenceVm FromDomainModel(PaymentPreference entity)
        {
            return new PaymentPreferenceVm
            {
                PaymentPreferenceId = entity.PaymentPreferenceId,
                Description = entity.Description,
                Active = entity.Active,
                Assing = entity.Assing,
            };
        }
    }
}