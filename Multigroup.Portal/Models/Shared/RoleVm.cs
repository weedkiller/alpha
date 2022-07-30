using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Shared
{
    public class RoleVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        public string Name { get; set; }

        public string Description { get; set; }

        public Role ToModelObject()
        {
            return new Role
            {
                Name = Name,
                Description = Description,
                RoleId = Id
            };
        }

        public RoleVm FromDomainModel(Role entity)
        {
            return new RoleVm
            {
                Name = entity.Name,
                Description = entity.Description,
                Id = entity.RoleId
            };
        }
    }
}