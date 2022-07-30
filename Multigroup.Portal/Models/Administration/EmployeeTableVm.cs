using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class EmployeeFilterVm
    {
        public IEnumerable<string> SelectedProviderType { get; set; }
        public IEnumerable<SelectListItem> ListProviderType { get; set; }
        public double BalanceFrom { get; set; }
        public double BalanceTo { get; set; }
        public bool Active { get; set; }
    }
    public class EmployeeTableVm
    {
        public IEnumerable<EmployeeListVm> ProviderList { get; set; }

        public static EmployeeTableVm ToViewModel(IEnumerable<ProviderSummary> entities)
        {
            return new EmployeeTableVm
            {
                ProviderList = EmployeeListVm.ToViewModelList(entities),
            };
        }
    }

    public class EmployeeListVm
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Commentary { get; set; }
        public string ProviderType { get; set; }
        public double Balance { get; set; }
        public string Date { get; set; }
        public bool Active { get; set; }
        public static IEnumerable<EmployeeListVm> ToViewModelList(IEnumerable<ProviderSummary> entities)
        {
            return entities.Select(x => new EmployeeListVm
            {
                ProviderId = x.ProviderId,
                Name = x.Name,
                Document = x.Document,
                Email = x.Email,
                Telephone = x.Telephone,
                Commentary = x.Commentary,
                ProviderType = x.ProviderType,
                Balance = x.Balance,
                Date = x.Date.ToShortDateString(),
                Active = x.Active,
            });
        }
    }

    public class EmployeeCAFilterVm
    {
        public IEnumerable<string> SelectedProvider { get; set; }
        public IEnumerable<SelectListItem> ListProvider { get; set; }
        public double BalanceFrom { get; set; }
        public double BalanceTo { get; set; }
        public bool NotImput { get; set; }
    }
    public class EmployeeCATableVm
    {
        public IEnumerable<EmployeeCAListVm> ProviderCAList { get; set; }

        public static EmployeeCATableVm ToViewModel(IEnumerable<EmployeeCurrentAcount> entities)
        {
            return new EmployeeCATableVm
            {
                ProviderCAList = EmployeeCAListVm.ToViewModelList(entities),
            };
        }
    }

    public class EmployeeCAListVm
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public string PaymentDate { get; set; }
        public string SpendingDate { get; set; }
        public double BalanceNotImput { get; set; }
        public static IEnumerable<EmployeeCAListVm> ToViewModelList(IEnumerable<EmployeeCurrentAcount> entities)
        {
            return entities.Select(x => new EmployeeCAListVm
            {
                Balance = x.Balance,
                Name = x.Name,
                BalanceNotImput = x.BalanceNotImput,
                PaymentDate = x.PaymentDate.ToShortDateString(),
                SpendingDate = x.SpendingDate.ToShortDateString(),
                ProviderId = x.ProviderId
            });
        }
    }

    public class EmployeeCADetailTableVm
    {
        public IEnumerable<EmployeeCADetailListVm> ProviderCADetailList { get; set; }
        public string name { get; set; }
        public double balance { get; set; }

        public static EmployeeCADetailTableVm ToViewModel(IEnumerable<EmployeeCurrentAcountDetail> entities)
        {
            return new EmployeeCADetailTableVm
            {
                ProviderCADetailList = EmployeeCADetailListVm.ToViewModelList(entities),
            };
        }
    }

    public class EmployeeCADetailListVm
    {
        public int MovementId { get; set; }
        public string Movement { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string Date { get; set; }
        public double BalanceNotImput { get; set; }
        public static IEnumerable<EmployeeCADetailListVm> ToViewModelList(IEnumerable<EmployeeCurrentAcountDetail> entities)
        {
            return entities.Select(x => new EmployeeCADetailListVm
            {
                MovementId = x.MovementId,
                Movement = x.Movement,
                BalanceNotImput = x.BalanceNotImput,
                Date = x.Date.ToShortDateString(),
                Description = x.Description,
                Amount = x.Amount
            });
        }
    }
}