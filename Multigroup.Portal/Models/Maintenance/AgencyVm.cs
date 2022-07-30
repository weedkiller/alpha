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
    public class AgencyVm : BaseVm
    {
        public int AgencyId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string Description { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public SingleSelectVm ddlUsers { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public SingleSelectVm ddlTypes { get; set; }


    public Agency ToModelObject()
        {
            return new Agency
            {
                AgencyId = AgencyId,
                Description = Description,
                Adress = Adress,
                Phone = Phone,
                ManagerId = GetNullableValue(ddlUsers).Value,
                Type = GetNullableValue(ddlTypes).Value,
            };
        }

        public static AgencyVm FromDomainModel(Agency entity)
        {
            return new AgencyVm
            {
                AgencyId = entity.AgencyId,
                Description = entity.Description,
                Adress = entity.Adress,
                Phone = entity.Phone,
            };
        }
    }
}