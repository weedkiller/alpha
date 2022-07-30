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

namespace Multigroup.Portal.Models.Administration
{
    public class SalaryAllocationVm : BaseVm
    {
        public int SalaryAllocationId { get; set; }
        [Required(ErrorMessage = "El campo Fecha Inicio Compra es requerido")]
        public string StartDate { get; set; }
        [Required(ErrorMessage = "El campo Fecha Fin Compra es requerido")]
        public string EndDate { get; set; }
        [Required(ErrorMessage = "El campo Proveedor es requerido")]
        public SingleSelectVm ddlProvider { get; set; }
        [Required(ErrorMessage = "El campo Usuario Autoriza es requerido")]
        public SingleSelectVm ddlUserAuthorized { get; set; }
        public bool IsAuthorized { get; set; }
        [Required(ErrorMessage = "El campo Monto es requerido")]
        public double Amount { get; set; }
        public string Description { get; set; }
        public string LastMonth { get; set; }


        public SalaryAllocation ToModelObject()
        {
            return new SalaryAllocation
            {
                SalaryAllocationId = SalaryAllocationId,
                StartDate = Util.ParseDateTime(StartDate).Value,
                EndDate = Util.ParseDateTime(EndDate).Value,
                ProveedorId = GetNullableValue(ddlProvider).Value,
                UserAuthorizedId = GetNullableValue(ddlUserAuthorized).Value,
                Amount = Amount,
                Description = Description,
            };
        }

        public static SalaryAllocationVm FromDomainModel(SalaryAllocation entity)
        {
            return new SalaryAllocationVm
            {
                SalaryAllocationId = entity.SalaryAllocationId,
                Description = entity.Description,
                Amount = entity.Amount,
                IsAuthorized = entity.IsAuthorized,
                StartDate = entity.StartDate.ToShortDateString(),
                EndDate = entity.EndDate.ToShortDateString(),
                LastMonth = entity.LastMonth,               
            };
        }
    }
}