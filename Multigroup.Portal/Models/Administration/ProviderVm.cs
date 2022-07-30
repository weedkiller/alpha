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
    public class ProviderVm : BaseVm
    {
        public int ProviderId { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Name { get; set; }
        public string Document { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail no tiene un formato válido")]
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Commentary { get; set; }
        [Required(ErrorMessage = "El campo Tipo Proveedor es requerido")]
        public SingleSelectVm ddlProviderType { get; set; }
        [Required(ErrorMessage = "El campo Posición IVA es requerido")]
        public SingleSelectVm ddlIVAPosition { get; set; }
        public bool IsEmployee { get; set; }
        public SingleSelectVm ddlUser { get; set; }
        public double Balance { get; set; }
        public string Date { get; set; }
        public bool Active { get; set; }
        [Required(ErrorMessage = "El campo Posición ante el IVA es requerido")]
        public int IVAPositionId { get; set; }
        public string BusinessName { get; set; }
        public string CUIT { get; set; }


        public Provider ToModelObject()
        {
            return new Provider
            {
                ProviderId = ProviderId,
                Name = Name,
                Document = Document,
                Email = Email,
                Telephone = Telephone,
                Commentary = Commentary,
                ProviderTypeId = GetNullableValue(ddlProviderType).Value,
                Balance = Balance,
                Active = Active,
                Date = Util.ParseDateTime(Date),
                BusinessName = BusinessName,
                CUIT = CUIT,
                IVAPositionId = GetNullableValue(ddlIVAPosition).Value,
            };
        }

        public static ProviderVm FromDomainModel(Provider entity)
        {
            return new ProviderVm
            {
                ProviderId = entity.ProviderId,
                Name = entity.Name,
                Document = entity.Document,
                Email = entity.Email,
                Telephone = entity.Telephone,
                Commentary = entity.Commentary,
                Balance = entity.Balance,
                Active = entity.Active,
                Date = entity.Date.Value.ToShortDateString(),
                BusinessName = entity.BusinessName,
                CUIT = entity.CUIT
            };
        }
    }
}