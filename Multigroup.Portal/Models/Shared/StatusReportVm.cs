using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Shared
{
    public class StatusReportFilterVm
    {
        public IEnumerable<string> SelectedStatus { get; set; }
        public IEnumerable<SelectListItem> ListStatus { get; set; }
        
    }
    public class StatusReportTableVm
    {
        public IEnumerable<StatusReportListVm> StatusReportList { get; set; }

        public static StatusReportTableVm ToViewModel(IEnumerable<StatusReport> entities)
        {
            return new StatusReportTableVm
            {
                StatusReportList = StatusReportListVm.ToViewModelList(entities),
            };
        }
    }

    public class StatusReportListVm
    {
        public int Number { get; set; }
        public string Customer { get; set; }
        public string Status { get; set; }
        public string UserChange { get; set; }
        public string StatusDate { get; set; }
        public string ActualStatus { get; set; }
        public string ActualStatusDate { get; set; }
     
        public static IEnumerable<StatusReportListVm> ToViewModelList(IEnumerable<StatusReport> entities)
        {
            return entities.Select(x => new StatusReportListVm
            {
                Number = x.Number,
                Customer = x.Customer,
                Status = x.Status,
                StatusDate = Util.GetFrenchFormatDate(x.StatusDate),
                ActualStatusDate = Util.GetFrenchFormatDate(x.ActualStatusDate),
                ActualStatus = x.ActualStatus,
                UserChange = x.UserChange
            });
        }
    }
}