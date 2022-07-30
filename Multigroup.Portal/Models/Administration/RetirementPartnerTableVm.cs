using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class RetirementPartnerFilterVm
    {
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public IEnumerable<string> SelectedPartner { get; set; }
        public IEnumerable<SelectListItem> ListPartner { get; set; }
        public double AmountFrom { get; set; }
        public double AmountTo { get; set; }
    }
    public class RetirementPartnerTableVm
    {
        public IEnumerable<RetirementPartnerListVm> RetirementPartnerList { get; set; }

        public static RetirementPartnerTableVm ToViewModel(IEnumerable<RetirementPartnerSummary> entities)
        {
            return new RetirementPartnerTableVm
            {
                RetirementPartnerList = RetirementPartnerListVm.ToViewModelList(entities),
            };
        }
    }

    public class RetirementPartnerListVm
    {
        public int RetirementPartnerId { get; set; }
        public string Partner { get; set; }
        public string User { get; set; }
        public string Date { get; set; }
        public string SystemDate { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }


        public static IEnumerable<RetirementPartnerListVm> ToViewModelList(IEnumerable<RetirementPartnerSummary> entities)
        {
            return entities.Select(x => new RetirementPartnerListVm
            {
                RetirementPartnerId = x.RetirementPartnerId,
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