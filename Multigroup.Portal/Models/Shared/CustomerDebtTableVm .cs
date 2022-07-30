using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Shared
{
    public class CustomerDebtTableVm
    {
        public IEnumerable<CustomerDebtListVm> CustomerDebtList { get; set; }

        public static CustomerDebtTableVm ToViewModel(IEnumerable<CustomerDebt> entities)
        {
            return new CustomerDebtTableVm
            {
                CustomerDebtList = CustomerDebtListVm.ToViewModelList(entities),
            };
        }
    }

    public class CustomerDebtListVm
    {
        public int Number { get; set; }
        public string IdentificationNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string StatusContract { get; set; }
        public string Color { get; set; }
        public string Count { get; set; }
        public string Amount { get; set; }
        public string Interest { get; set; }
        public string Total { get; set; }
        public string User { get; set; }

        public static IEnumerable<CustomerDebtListVm> ToViewModelList(IEnumerable<CustomerDebt> entities)
        {
            return entities.Select(x => new CustomerDebtListVm
            {
                Number = x.Number,
                IdentificationNumber = x.IdentificationNumber,
                Name = x.Name,
                Surname = x.Surname,
                Color = x.Color,
                Count = x.Count,
                Amount = x.Amount,
                StatusContract = x.StatusContract,
                Interest = x.Interest,
                Total = x.Total,
                User = x.User,
            });
        }
    }
}