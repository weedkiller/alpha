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
    public class AgencyPaymentHistoryTableVm
    {
        public IEnumerable<AgencyPaymentHistoryListVm> AgencyPaymentHistoryList { get; set; }

        public static AgencyPaymentHistoryTableVm ToViewModel(IEnumerable<AgencyPaymentHistory> entities)
        {
            return new AgencyPaymentHistoryTableVm
            {
                AgencyPaymentHistoryList = AgencyPaymentHistoryListVm.ToViewModelList(entities),
            };
        }
    }

    public class AgencyPaymentHistoryListVm
    {
        public int AgencyPaymentId { get; set; }
        public string Observations { get; set; }
        public string Date { get; set; }
        public string ScheduledPaymentDate { get; set; }
        public string User { get; set; }
        public static IEnumerable<AgencyPaymentHistoryListVm> ToViewModelList(IEnumerable<AgencyPaymentHistory> entities)
        {
            return entities.Select(x => new AgencyPaymentHistoryListVm
            {
                AgencyPaymentId = x.AgencyPaymentHistoryId,
                Date = Util.GetFrenchFormatDate(x.Date),
                Observations = x.Observations,
                ScheduledPaymentDate = Util.GetFrenchFormatDate(x.ScheduledPaymentDate),
                User = x.User,
            });
        }
    }
}