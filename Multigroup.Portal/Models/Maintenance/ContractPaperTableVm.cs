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
    public class ContractPaperFilterVm
    {
        public IEnumerable<string> SelectedStatus { get; set; }
        public IEnumerable<SelectListItem> ListStatus { get; set; }
        public SingleSelectVm ddlContractType { get; set; }
    }
    public class ContractPaperTableVm
    {
        public IEnumerable<ContractPaperListVm> ContractPaperList { get; set; }

        public static ContractPaperTableVm ToViewModel(IEnumerable<ContractPaperSummary> entities)
        {
            return new ContractPaperTableVm
            {
                ContractPaperList = ContractPaperListVm.ToViewModelList(entities),
            };
        }
    }

    public class ContractPaperListVm
    {
        public int PaperContractId { get; set; }
        public int Number  { get; set; }
        public string Status { get; set; }
        public string User { get; set; }

        public string ContractType { get; set; }


        public static IEnumerable<ContractPaperListVm> ToViewModelList(IEnumerable<ContractPaperSummary> entities)
        {
            return entities.Select(x => new ContractPaperListVm
            {
                PaperContractId = x.PaperContractId,
                Number = x.Number,
                Status = x.Status,
                User = x.User,
                ContractType = x.ContractType,
            });
        }
    }
}