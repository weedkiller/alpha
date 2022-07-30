using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class EarningsAllocationFilterVm
    {
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public double AmountFrom { get; set; }
        public double AmountTo { get; set; }
        public IEnumerable<EarningsAllocationPartner> Partners { get; set; }
    }
    public class EarningsAllocationTableVm
    {
        public IEnumerable<EarningsAllocationListVm> EarningsAllocationList { get; set; }

        public static EarningsAllocationTableVm ToViewModel(IEnumerable<EarningsAllocationSummary> entities)
        {
            return new EarningsAllocationTableVm
            {
                EarningsAllocationList = EarningsAllocationListVm.ToViewModelList(entities),
            };
        }
    }

    public class EarningsAllocationListVm
    {
        public int EarningsAllocationId { get; set; }
        public string Date { get; set; }
        public string SystemDate { get; set; }
        public string Concept { get; set; }
        public string User { get; set; }
        public double Amount { get; set; }

        public static IEnumerable<EarningsAllocationListVm> ToViewModelList(IEnumerable<EarningsAllocationSummary> entities)
        {
            return entities.Select(x => new EarningsAllocationListVm
            {
                EarningsAllocationId = x.EarningsAllocationId,
                Date = x.Date.ToShortDateString(),
                SystemDate = x.SystemDate.ToShortDateString(),
                User = x.User,
                Concept = x.Concept,
                Amount = x.Amount
            });
        }
    }
}