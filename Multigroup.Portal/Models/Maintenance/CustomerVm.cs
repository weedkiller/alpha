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
    public class CustomerVm : BaseVm
    {
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo Apellido es requerido")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "El campo Tipo de DNI es requerido")]
        public SingleSelectVm ddlIdentificationType { get; set; }
        [Required(ErrorMessage = "El campo Número de DNI es requerido")]
        public string IdentificationNumber { get; set; }
        [Required(ErrorMessage = "El campo Fecha de Nacimiento es requerido")]
        public string Birthdate { get; set; }
        [Required(ErrorMessage = "El campo Estado Civil es requerido")]
        public SingleSelectVm ddlMaritalStatus { get; set; }
        [Required(ErrorMessage = "El campo Dirección es requerido")]
        public string Address { get; set; }
        [Required(ErrorMessage = "El campo  Ciudad es requerido")]
        public SingleSelectVm ddlCity { get; set; }
        [Required(ErrorMessage = "El campo Provincia es requerido")]
        public SingleSelectVm ddlState { get; set; }
        public string State { get; set; }
        [Required(ErrorMessage = "El campo CP es requerido")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "El campo Telefono es requerido")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "El campo  Celular es requerido")]
        public string CellPhone { get; set; }
        [Required(ErrorMessage = "El campo Email es requerido")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail no tiene un formato válido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo Ocupación es requerido")]
        public string Occupation { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string ContactHours { get; set; }
        public SingleSelectVm ddlSpouseIdentificationType { get; set; }
        public string SpouseIdentificationNumber { get; set; }
        public string SpouseName { get; set; }
        public string SpouseSurname { get; set; }
        public SingleSelectVm ddlStatusClient { get; set; }
        [Required(ErrorMessage = "El campo CUIT es requerido")]
        public string CUIT { get; set; }
        public string AddressDetail { get; set; }

        public Customer ToModelObject()
        {
            return new Customer
            {
                CustomerId = CustomerId,
                Name = Name,
                Surname = Surname,
                IdentificationType = GetNullableValue(ddlIdentificationType),
                IdentificationNumber = IdentificationNumber,
                Birthdate  = Util.ParseDateTime(Birthdate),
                MaritalStatus = GetNullableValue(ddlMaritalStatus),
                Address = Address,
                CityId  = GetNullableValue(ddlCity),
                State = GetNullableValue(ddlState).ToString(),
                ZipCode = ZipCode,
                Phone = Phone,
                CellPhone = CellPhone,
                Email = Email,
                Occupation = Occupation,
                ContactHours = ContactHours,
                SpouseIdentificationType = GetNullableValue(ddlSpouseIdentificationType),
                SpouseIdentificationNumber  = SpouseIdentificationNumber,
                SpouseName = SpouseName, 
                SpouseSurname  = SpouseSurname,
                StatusClient = (ddlStatusClient == null) ? 1 : GetNullableValue(ddlStatusClient),
                CUIT = CUIT,
                AddressDetail = AddressDetail,

            };
        }
        public static CustomerVm FromDomainModel(Customer entity)
        {
            return new CustomerVm
            {
                CustomerId = entity.CustomerId,
                Name = entity.Name,
                Surname = entity.Surname,
                IdentificationNumber = entity.IdentificationNumber,
                Birthdate = entity.Birthdate.ToString(),
                Address = entity.Address,
                State = entity.State,
                ZipCode = entity.ZipCode,
                Phone = entity.Phone,
                CellPhone = entity.CellPhone,
                Email = entity.Email,
                Occupation = entity.Occupation,
                ContactHours = entity.ContactHours,
                SpouseIdentificationNumber = entity.SpouseIdentificationNumber,
                SpouseName = entity.SpouseName,
                SpouseSurname = entity.SpouseSurname,
                CUIT = entity.CUIT,
                AddressDetail = entity.AddressDetail,
            };
        }
    }
}