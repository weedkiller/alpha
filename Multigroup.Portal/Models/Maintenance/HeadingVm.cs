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
    public class HeadingVm : BaseVm
    {
        public int HeadingId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string Name { get; set; }
     

    public Heading ToModelObject()
        {
            return new Heading
            {
                HeadingId = HeadingId,
                Name = Name,
            };
        }

        public static HeadingVm FromDomainModel(Heading entity)
        {
            return new HeadingVm
            {
                HeadingId = entity.HeadingId,
                Name = entity.Name,
            };
        }
    }
}