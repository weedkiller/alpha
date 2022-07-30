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
    public class CashierVm : BaseVm
    {
        public int CashierId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool Individual { get; set; }


        public Cashier ToModelObject()
        {
            return new Cashier
            {
                CashierId = CashierId,
                Name = Name,
                Active = Active,
                Individual = Individual
            };
        }

        public static CashierVm FromDomainModel(Cashier entity)
        {
            return new CashierVm
            {
                CashierId = entity.CashierId,
                Name = entity.Name,
                Active = entity.Active,
                Individual = entity.Individual
            };
        }
    }
}