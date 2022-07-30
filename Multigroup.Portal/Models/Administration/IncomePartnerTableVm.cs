using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class IncomePartnerFilterVm
    {
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public IEnumerable<string> SelectedPartner { get; set; }
        public IEnumerable<SelectListItem> ListPartner { get; set; }
        public double AmountFrom { get; set; }
        public double AmountTo { get; set; }
    }
    public class IncomePartnerTableVm
    {
        public IEnumerable<IncomePartnerListVm> IncomePartnerList { get; set; }

        public static IncomePartnerTableVm ToViewModel(IEnumerable<IncomePartnerSummary> entities)
        {
            return new IncomePartnerTableVm
            {
                IncomePartnerList = IncomePartnerListVm.ToViewModelList(entities),
            };
        }
    }

    public class IncomePartnerListVm
    {
        public int IncomePartnerId { get; set; }
        public string Partner { get; set; }
        public string User { get; set; }
        public string Date { get; set; }
        public string SystemDate { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }


        public static IEnumerable<IncomePartnerListVm> ToViewModelList(IEnumerable<IncomePartnerSummary> entities)
        {
            return entities.Select(x => new IncomePartnerListVm
            {
                IncomePartnerId = x.IncomePartnerId,
                Date = x.Date.ToShortDateString(),
                SystemDate = x.SystemDate.ToShortDateString(),
                User = x.User,
                Partner = x.Partner,
                Amount = x.Amount,
                Commentary = x.Commentary
            });
        }
    }
  
}