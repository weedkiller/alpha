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
    public class UserTypeVm : BaseVm
    {
        public int UserTypeId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string Description { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public bool IsCommission { get; set; }

        public Double Commission { get; set; }


        public UserType ToModelObject()
        {
            return new UserType
            {
                UserTypeId = UserTypeId,
                Description = Description,
                IsCommission = IsCommission,
                Commission = Commission,

            };
        }

        public static UserTypeVm FromDomainModel(UserType entity)
        {
            return new UserTypeVm
            {
                UserTypeId = entity.UserTypeId,
                Description = entity.Description,
                IsCommission = entity.IsCommission,
                Commission = entity.Commission,
            };
        }
    }
}