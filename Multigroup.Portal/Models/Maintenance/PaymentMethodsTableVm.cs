using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Maintenance
{
    public class PaymentMethodsTableVm
    {
        public IEnumerable<PaymentMethodsListVm> PaymentMethodsList { get; set; }

        public static PaymentMethodsTableVm ToViewModel(IEnumerable<PaymentMethodSummary> entities)
        {
            return new PaymentMethodsTableVm
            {
                PaymentMethodsList = PaymentMethodsListVm.ToViewModelList(entities),
            };
        }
    }

    public class PaymentMethodsListVm
    {
        public int PaymentMethodId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public double Percentage { get; set; }
        public bool Consolidated { get; set; }
        public bool Verifiable { get; set; }
        public bool Automatic { get; set; }
        public bool Asignation { get; set; }
        public string PaymentPreference { get; set; }
        public string Channel { get; set; }

        public static IEnumerable<PaymentMethodsListVm> ToViewModelList(IEnumerable<PaymentMethodSummary> entities)
        {
            return entities.Select(x => new PaymentMethodsListVm
            {
                PaymentMethodId = x.PaymentMethodId,
                Name = x.Name,
                Active = x.Active,
                Percentage = x.Percentage,
                Consolidated = x.Consolidated,
                Verifiable = x.Verifiable,
                Automatic = x.Automatic,
                Asignation = x.Asignation,
                PaymentPreference = x.PaymentPreference,
                Channel = x.Channel
            });
        }
    }
}