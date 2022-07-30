using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Maintenance
{
    public class CashierTableVm
    {
        public IEnumerable<CashierListVm> CashierList { get; set; }

        public static CashierTableVm ToViewModel(IEnumerable<Cashier> entities)
        {
            return new CashierTableVm
            {
                CashierList = CashierListVm.ToViewModelList(entities),
            };
        }
    }

    public class CashierListVm
    {
        public int CashierId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool Individual { get; set; }
        public static IEnumerable<CashierListVm> ToViewModelList(IEnumerable<Cashier> entities)
        {
            return entities.Select(x => new CashierListVm
            {
                CashierId = x.CashierId,
                Name = x.Name,
                Active = x.Active,
                Individual = x.Individual
            });
        }
    }
}