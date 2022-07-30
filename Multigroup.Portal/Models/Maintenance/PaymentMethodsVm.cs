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
    public class PaymentMethodsVm : BaseVm
    {
        public int PaymentMethodId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public bool Active { get; set; }
        public double Percentage { get; set; }
        public bool Consolidated { get; set; }
        public bool Verifiable { get; set; }
        public bool Automatic { get; set; }
        public bool Aisgnation { get; set; }
        public SingleSelectVm ddlPaymentPreference { get; set; }
        public String Channel { get; set; }

        public PaymentMethod ToModelObject()
        {
            return new PaymentMethod
            {
                PaymentMethodId = PaymentMethodId,
                Name = Name,
                Active = Active,
                Percentage = Percentage,
                Consolidated = Consolidated,
                Verifiable = Verifiable,
                Automatic = Automatic,
                Asignation = Aisgnation,
                Channel = Channel
            };
        }

        public static PaymentMethodsVm FromDomainModel(PaymentMethod entity)
        {
            return new PaymentMethodsVm
            {
                PaymentMethodId = entity.PaymentMethodId,
                Name = entity.Name,
                Active = entity.Active,
                Percentage = entity.Percentage,
                Consolidated = entity.Consolidated,
                Verifiable = entity.Verifiable,
                Automatic = entity.Automatic,
                Aisgnation = entity.Asignation,
                Channel = entity.Channel
            };
        }
    }
}