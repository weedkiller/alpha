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

namespace Multigroup.Portal.Models.ContactCallModel
{
    public class ContactCallVm : BaseVm
    {
        public int ContactCallId { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo Apellido es requerido")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "El campo Telefono es requerido")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "El campo Vehículo es requerido")]
        public string Vehicle { get; set;}
        public string ContactDate { get; set; }
        public string SystemDate { get; set; }
        public string ContactTime { get; set; }
        public string Commentary { get; set; }
        public SingleSelectVm ddlContactCallState { get; set; }
        public SingleSelectVm ddlManager { get; set; }
        public SingleSelectVm ddlSeller { get; set; }
        public SingleSelectVm ddlSupervisor { get; set; }

        public ContactCall ToModelObject()
        {
            return new ContactCall
            {
            ContactCallId = ContactCallId,
            ContactDate = Util.ParseDateTime(ContactDate),
            Name = Name,
            Surname = Surname,
            Vehicle = Vehicle,
            Commentary  = Commentary,
            Phone = Phone,
            SellerId = GetNullableValue(ddlSeller),
            ManagerId = GetNullableValue(ddlManager),
            SupervisorId = GetNullableValue(ddlSupervisor),
            ContactCallStateId = GetNullableValue(ddlContactCallState),          
            };
        }

        public static ContactCallVm FromDomainModel(ContactCall entity)
        {
            return new ContactCallVm
            {
                ContactCallId = entity.ContactCallId,
                SystemDate = entity.SystemDate.ToString(),
                ContactDate = entity.ContactDate.ToString(),
                Name = entity.Name,
                Surname = entity.Surname,
                Phone = entity.Phone,
                Vehicle = entity.Vehicle,
                Commentary = entity.Commentary,
                ContactTime = entity.ContactTime,             
            };
        }
    }
}