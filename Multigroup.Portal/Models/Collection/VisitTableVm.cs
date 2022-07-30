using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Models.Shared;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Collection
{
    public class VisitFilterVm
    {
        public IEnumerable<string> SelectedZones { get; set; }
        public IEnumerable<SelectListItem> ListZones { get; set; }
        public IEnumerable<string> SelectedCollector { get; set; }
        public IEnumerable<SelectListItem> ListCollector { get; set; }
        public IEnumerable<string> SelectedStatus { get; set; }
        public IEnumerable<SelectListItem> ListStatus { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
    public class VisitTableVm
    {
        public IEnumerable<VisitListVm> VisitList { get; set; }
        public IEnumerable<string> SelectedStatus { get; set; }
        public IEnumerable<SelectListItem> ListStatus { get; set; }

        public static VisitTableVm ToViewModel(IEnumerable<VisitSummary> entities)
        {
            return new VisitTableVm
            {
                VisitList = VisitListVm.ToViewModelList(entities),
            };
        }
    }

    public class VisitListVm
    {
        public int VisitId { get; set; }
        public string Customer { get; set; }
        public string ScheduledVisitDate { get; set; }
        public string ConcretedVisitDate { get; set; }
        public string Collector { get; set; }
        public double CollectorComission { get; set; }
        public double AmountOwed { get; set; }
        public double? AmountCollected { get; set; }
        public double? TotalComission { get; set; }
        public string Status { get; set; }
        public string CollectorExpensesDesc { get; set; }
        public double? CollectorExpensesAmount { get; set; }
        public string Observations { get; set; }
        public static IEnumerable<VisitListVm> ToViewModelList(IEnumerable<VisitSummary> entities)
        {
            return entities.Select(x => new VisitListVm
            {
                VisitId = x.VisitId,
                Customer = x.Customer,
                ScheduledVisitDate = Util.GetFrenchFormatDate(x.ScheduledVisitDate),
                ConcretedVisitDate = Util.GetFrenchFormatDate(x.ConcretedVisitDate),
                Collector = x.Collector,
                CollectorComission = x.CollectorComission,
                AmountOwed = x.AmountOwed,
                AmountCollected = x.AmountCollected,
                TotalComission = x.TotalComission,
                Status = x.Status,
                CollectorExpensesDesc = x.CollectorExpensesDesc,
                CollectorExpensesAmount = x.CollectorExpensesAmount,
                Observations = x.Observations,
            });
        }
    }
}