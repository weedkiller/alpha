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
    public class EmployeeVm : BaseVm
    {
        public int ProviderId { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo DNI es requerido")]
        public string Document { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail no tiene un formato válido")]
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Commentary { get; set; }
        public SingleSelectVm ddlProviderType { get; set; }
        public bool IsEmployee { get; set; }
        public SingleSelectVm ddlUser { get; set; }
        public int UserId { get; set; }
        public double Balance { get; set; }
        public string Date { get; set; }
        public bool Active { get; set; }


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
                UserId = UserId,
            };
        }

        public static EmployeeVm FromDomainModel(Provider entity)
        {
            return new EmployeeVm
            {
                ProviderId = entity.ProviderId,
                Name = entity.Name,
                Document = entity.Document,
                Email = entity.Email,
                Telephone = entity.Telephone,
                Commentary = entity.Commentary,
                Balance = entity.Balance,
                Active = entity.Active,
                UserId = entity.UserId,
                Date = entity.Date.Value.ToShortDateString(),
            };
        }
    }
}