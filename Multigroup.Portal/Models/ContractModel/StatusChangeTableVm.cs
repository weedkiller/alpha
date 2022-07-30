using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Enums;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.ContractModel
{
    public class StatusChangeTableVm
    {
        public IEnumerable<StatusChangeListVm> StatusChangeList { get; set; }

        public static StatusChangeTableVm ToViewModel(IEnumerable<StatusContractHistory> entities)
        {
            return new StatusChangeTableVm
            {
                StatusChangeList = StatusChangeListVm.ToViewModelList(entities),
            };
        }
    }

    public class StatusChangeListVm
    {
        public int StatusContractHistoryId { get; set; }
        public string Date { get; set; }
        public string StatusOld { get; set; }
        public string StatusNew { get; set; }
        public string Observations { get; set; }
        public string User { get; set; }

        public static IEnumerable<StatusChangeListVm> ToViewModelList(IEnumerable<StatusContractHistory> entities)
        {
            return entities.Select(x => new StatusChangeListVm
            {
                StatusContractHistoryId = x.StatusContractHistoryId,
                Date = x.Date,
                StatusOld = x.StatusOld,
                StatusNew = x.StatusNew,
                Observations = x.Observations,
                User = x.User
            });
        }
    }
}