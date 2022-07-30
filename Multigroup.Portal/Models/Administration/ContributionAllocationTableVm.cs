using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class ContributionAllocationFilterVm
    {
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public double AmountFrom { get; set; }
        public double AmountTo { get; set; }
        public IEnumerable<ContributionAllocationPartner> Partners { get; set; }
    }
    public class ContributionAllocationTableVm
    {
        public IEnumerable<ContributionAllocationListVm> ContributionAllocationList { get; set; }

        public static ContributionAllocationTableVm ToViewModel(IEnumerable<ContributionAllocationSummary> entities)
        {
            return new ContributionAllocationTableVm
            {
                ContributionAllocationList = ContributionAllocationListVm.ToViewModelList(entities),
            };
        }
    }

    public class ContributionAllocationListVm
    {
        public int ContributionAllocationId { get; set; }
        public string Date { get; set; }
        public string SystemDate { get; set; }
        public string Concept { get; set; }
        public string User { get; set; }
        public double Amount { get; set; }

        public static IEnumerable<ContributionAllocationListVm> ToViewModelList(IEnumerable<ContributionAllocationSummary> entities)
        {
            return entities.Select(x => new ContributionAllocationListVm
            {
                ContributionAllocationId = x.ContributionAllocationId,
                Date = x.Date.ToShortDateString(),
                SystemDate = x.SystemDate.ToShortDateString(),
                User = x.User,
                Concept = x.Concept,
                Amount = x.Amount
            });
        }
    }  
}