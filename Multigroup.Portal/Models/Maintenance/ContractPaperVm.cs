using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Enums;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Multigroup.Portal.Models.Shared;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Maintenance
{
    public class ContractPaperVm : BaseVm
    {
        public int PaperContractId { get; set; }
        public List<int> Ids { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public int Number { get; set; }
        public int NumberFrom { get; set; }
        public int NumberTo { get; set; }

        public int? User { get; set; }
        public int? Status { get; set; }
        public SingleSelectVm ddlStatus { get; set; }
        public SingleSelectVm ddlUser { get; set; }
        public SingleSelectVm ddlContractType { get; set; }

        public ContractPaper ToModelObject()
        {
            return new ContractPaper
            {
                PaperContractId = PaperContractId,
                Status = (ddlStatus == null) ? 1 : GetNullableValue(ddlStatus),
                User = (ddlUser == null) ? 1 : GetNullableValue(ddlUser),
                Number = Number,
            };
        }
        public static ContractPaperVm FromDomainModel(ContractPaper entity)
        {
            return new ContractPaperVm
            {
                PaperContractId = entity.PaperContractId,
                Number = entity.Number,
                Status = entity.Status,
                User = entity.User
            };
        }
    }
}