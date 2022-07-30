using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Enums;
using Multigroup.Portal.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Maintenance
{
    public class ReceipFilterVm
    {
        public IEnumerable<string> SelectedStatus { get; set; }
        public IEnumerable<SelectListItem> ListStatus { get; set; }
        public SingleSelectVm ddlContractType { get; set; }
    }
    public class ReceipTableVm
    {
        public IEnumerable<ReceipListVm> ReceipList { get; set; }

        public static ReceipTableVm ToViewModel(IEnumerable<ReceipSummary> entities)
        {
            return new ReceipTableVm
            {
                ReceipList = ReceipListVm.ToViewModelList(entities),
            };
        }
    }

    public class ReceipListVm
    {
        public int ReceipId { get; set; }
        public long Number  { get; set; }
        public string Status { get; set; }
        public string User { get; set; }

        public string ContractType { get; set; }


        public static IEnumerable<ReceipListVm> ToViewModelList(IEnumerable<ReceipSummary> entities)
        {
            return entities.Select(x => new ReceipListVm
            {
                ReceipId = x.ReceipId,
                Number = x.Number,
                Status = x.Status,
                User = x.User,
            });
        }
    }
}