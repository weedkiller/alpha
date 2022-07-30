using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Maintenance
{
    public class PaymentPreferenceTableVm
    {
        public IEnumerable<PaymentPreferenceListVm> PaymentPreferenceList { get; set; }

        public static PaymentPreferenceTableVm ToViewModel(IEnumerable<PaymentPreference> entities)
        {
            return new PaymentPreferenceTableVm
            {
                PaymentPreferenceList = PaymentPreferenceListVm.ToViewModelList(entities),
            };
        }
    }

    public class PaymentPreferenceListVm
    {
        public int PaymentPreferenceId { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public bool Assing { get; set; }

        public static IEnumerable<PaymentPreferenceListVm> ToViewModelList(IEnumerable<PaymentPreference> entities)
        {
            return entities.Select(x => new PaymentPreferenceListVm
            {
                PaymentPreferenceId = x.PaymentPreferenceId,
                Description = x.Description,
                Active = x.Active,
                Assing = x.Assing,
            });
        }
    }
}