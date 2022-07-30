using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class ProviderFilterVm
    {
        public IEnumerable<string> SelectedProviderType { get; set; }
        public IEnumerable<SelectListItem> ListProviderType { get; set; }
        public double BalanceFrom { get; set; }
        public double BalanceTo { get; set; }
        public bool Active { get; set; }
    }
    public class ProviderTableVm
    {
        public IEnumerable<ProviderListVm> ProviderList { get; set; }

        public static ProviderTableVm ToViewModel(IEnumerable<ProviderSummary> entities)
        {
            return new ProviderTableVm
            {
                ProviderList = ProviderListVm.ToViewModelList(entities),
            };
        }
    }

    public class ProviderListVm
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
        public string IVAPosition { get; set; }
        public string BusinessName { get; set; }
        public string CUIT { get; set; }

        public static IEnumerable<ProviderListVm> ToViewModelList(IEnumerable<ProviderSummary> entities)
        {
            return entities.Select(x => new ProviderListVm
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
                IVAPosition = x.IVAPosition,
                BusinessName = x.BusinessName,
                CUIT = x.CUIT
            });
        }
    }

    public class ProviderCAFilterVm
    {
        public IEnumerable<string> SelectedProvider { get; set; }
        public IEnumerable<SelectListItem> ListProvider { get; set; }
        public double BalanceFrom { get; set; }
        public double BalanceTo { get; set; }
        public bool NotImput { get; set; }
    }
    public class ProviderCATableVm
    {
        public IEnumerable<ProviderCAListVm> ProviderCAList { get; set; }

        public static ProviderCATableVm ToViewModel(IEnumerable<ProviderCurrentAcount> entities)
        {
            return new ProviderCATableVm
            {
                ProviderCAList = ProviderCAListVm.ToViewModelList(entities),
            };
        }
    }

    public class ProviderCAListVm
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public string PaymentDate { get; set; }
        public string SpendingDate { get; set; }
        public double BalanceNotImput { get; set; }
        public static IEnumerable<ProviderCAListVm> ToViewModelList(IEnumerable<ProviderCurrentAcount> entities)
        {
            return entities.Select(x => new ProviderCAListVm
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

    public class ProviderCADetailTableVm
    {
        public IEnumerable<ProviderCADetailListVm> ProviderCADetailList { get; set; }
        public string name { get; set; }
        public double balance { get; set; }

        public static ProviderCADetailTableVm ToViewModel(IEnumerable<ProviderCurrentAcountDetail> entities)
        {
            return new ProviderCADetailTableVm
            {
                ProviderCADetailList = ProviderCADetailListVm.ToViewModelList(entities),
            };
        }
    }

    public class ProviderCADetailListVm
    {
        public int MovementId { get; set; }
        public string Movement { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string Date { get; set; }
        public double BalanceNotImput { get; set; }
        public static IEnumerable<ProviderCADetailListVm> ToViewModelList(IEnumerable<ProviderCurrentAcountDetail> entities)
        {
            return entities.Select(x => new ProviderCADetailListVm
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