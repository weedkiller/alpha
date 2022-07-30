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
    public class ReceipVm : BaseVm
    {
        public int ReceipId { get; set; }
        public List<int> Ids { get; set; }
        public string Number { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public long NumberFrom { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public long NumberTo { get; set; }

        public int? User { get; set; }
        public int? Status { get; set; }
        public SingleSelectVm ddlStatus { get; set; }
        public SingleSelectVm ddlUser { get; set; }

        public Receip ToModelObject()
        {
            return new Receip
            {
                ReceipId = ReceipId,
                Status = (ddlStatus == null) ? 1 : GetNullableValue(ddlStatus),
                User = (ddlUser == null) ? 1 : GetNullableValue(ddlUser),
                Number = Number,
            };
        }
        public static ReceipVm FromDomainModel(Receip entity)
        {
            return new ReceipVm
            {
                ReceipId = entity.ReceipId,
                Number = entity.Number,
                Status = entity.Status,
                User = entity.User
            };
        }
    }
}