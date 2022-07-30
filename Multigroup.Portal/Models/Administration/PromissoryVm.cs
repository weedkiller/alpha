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
    public class PromissoryVm : BaseVm
    {
        public int PromissoryId { get; set; }
        [Required(ErrorMessage = "El campo Fecha Emisión es requerido")]
        public string BroadcastDate { get; set; }
        [Required(ErrorMessage = "El campo Fecha Pago es requerido")]
        public string CollectionDate { get; set; }
        [Required(ErrorMessage = "El campo Cliente es requerido")]
        public SingleSelectVm ddlClient{ get; set; }
        [Required(ErrorMessage = "El campo Porcetaje Socios es requerido")]
        public SingleSelectVm ddlPercentages { get; set; }
        [Required(ErrorMessage = "El campo Número Pagare es requerido")]
        public int Number { get; set; }
        [Required(ErrorMessage = "El campo Monto es requerido")]
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public string Description { get; set; }
        public bool isPaid { get; set; }
        public int Quantity { get; set; }
        public IEnumerable<PercentageDefinitionPartnerSummary> Partners { get; set; }

        public Promissory ToModelObject()
        {
            return new Promissory
            {
                PromissoryId = PromissoryId,
                BroadcastDate = Util.ParseDateTime(BroadcastDate).Value,
                CollectionDate = Util.ParseDateTime(CollectionDate).Value,
                ClientId = GetNullableValue(ddlClient).Value,
                PercentageDefinitionId = GetNullableValue(ddlPercentages).Value,
                Number = Number,
                Amount = Amount,
                Commentary = Commentary,
                isPaid = isPaid,
                Description = Description,
            };
        }

        public static PromissoryVm FromDomainModel(Promissory entity)
        {
            return new PromissoryVm
            {
                PromissoryId = entity.PromissoryId,
                Commentary = entity.Commentary,
                Amount = entity.Amount,
                Number = entity.Number,
                Description = entity.Description,
                isPaid = entity.isPaid,
                BroadcastDate = entity.BroadcastDate.ToShortDateString(),
                CollectionDate = entity.CollectionDate.ToShortDateString(),              
            };
        }
    }
}