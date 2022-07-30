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
    public class ContributionAllocationVm : BaseVm
    {
        public int ContributionAllocationId { get; set; }
        [Required(ErrorMessage = "El campo Fecha es requerido")]
        public string Date { get; set; }
        public string SystemDate { get; set; }
        [Required(ErrorMessage = "El campo Concepto es requerido")]
        public string Concept { get; set; }
        [Required(ErrorMessage = "El campo Monto es requerido")]
        public double Amount { get; set; }       
         public IEnumerable<ContributionAllocationPartner> Partners { get; set; }
       
        public ContributionAllocation ToModelObject()
        {
            return new ContributionAllocation
            {
                ContributionAllocationId = ContributionAllocationId,
                Date = Util.ParseDateTime(Date).Value,         
                Concept = Concept,
                Amount = Amount,
                Partners = Partners,
            };
        }

        public static ContributionAllocationVm FromDomainModel(ContributionAllocation entity)
        {
            return new ContributionAllocationVm
            {
                ContributionAllocationId = entity.ContributionAllocationId,
                Amount = entity.Amount,
                Concept = entity.Concept,
                SystemDate = entity.SystemDate.ToShortDateString(),
                Date = entity.Date.ToShortDateString(),
                Partners = entity.Partners,
            };
        }
    }
}