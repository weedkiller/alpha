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

namespace Multigroup.Portal.Models.ContractModel
{
    public class ContractVm : BaseVm
    {
        public int ContractId { get; set; }
        [Required(ErrorMessage = "El campo Feha Registro es requerido")]
        public string RegistrationDate { get; set; }
        public string AdmissionDate { get; set; }
        public string QuotationDate { get; set; }
        [Required(ErrorMessage = "El campo Número es requerido")]
        public int Number { get; set; }
        public int? AgencyId { get; set;}
        [Required(ErrorMessage = "El campo es requerido")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string GoodRequested { get; set; }
        [Required(ErrorMessage = "El campo Bien Requerido es requerido")]
        public string GoodRequestedColour { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleModel { get; set; }
        public int VehicleMileage { get; set; }
        public string VehicleRegistrationPlate { get; set; }
        [Required(ErrorMessage = "El campo Monto de Subscripción es requerido")]
        public double SubscriptionAmount { get; set; }
        [Required(ErrorMessage = "El campo Monto de primer cuota es requerido")]
        public double FirstAdvanceAmount { get; set; }
        public double? Quotation { get; set; }
        public double? QuotationDolar { get; set; }
        public string QuotationString { get; set; }
        public string QuotationDolarString { get; set; }
        public double? FirstAdvanceDiscount { get; set; }
        public double? SecondAdvanceDiscount { get; set; }
        public double? ThirdAdvanceDiscount { get; set; }
        [Required(ErrorMessage = "El campo Monto de cuotas es requerido")]
        public double AdvancesAmount { get; set; }
        public string Observations { get; set; }
        [Required(ErrorMessage = "El campo Fecha de pago es requerido")]
        public SingleSelectVm ddlPaymentDate { get; set; }
        [Required(ErrorMessage = "El campo Agencia es requerido")]
        public SingleSelectVm ddlAgency { get; set; }
        [Required(ErrorMessage = "El campo Vendedor es requerido")]
        public SingleSelectVm ddlSeller { get; set; }
        public SingleSelectVm ddlSupervisor { get; set; }
        public SingleSelectVm ddlManager { get; set; }
        public SingleSelectVm ddlStatus { get; set; }
        public SingleSelectVm ddlState { get; set; }
        public SingleSelectVm ddlStatusDetail { get; set; }
        public SingleSelectVm ddlPaperStatus { get; set; }
        public SingleSelectVm ddlIdentificationType { get; set; }
        public SingleSelectVm ddlMaritalStatus { get; set; }
        public SingleSelectVm ddlCity { get; set; }
        public SingleSelectVm ddlSpouseIdentificationType { get; set; }
        public SingleSelectVm ddlStatusClient { get; set; }
        public SingleSelectVm ddlPaymentPreference { get; set; }
        public SingleSelectVm ddlPaymentPlace { get; set; }
        public SingleSelectVm ddlContractType { get; set; }
        public string Seller { get; set; }
        public string Supervisor { get; set; }
        public string Manager { get; set; }
        public string Status { get; set; }
        public string StatusDetail { get; set; }
        public string PaperStatus { get; set; }
        public string IdentificationType { get; set; }
        public string MaritalStatus { get; set; }
        public string City { get; set; }
        public string SpouseIdentificationType { get; set; }
        public string StatusClient { get; set; }
        public string PaymentPreference { get; set; }
        public string PaymentPlace { get; set; }
        public string PaymentDate { get; set; }
        public string ContractType { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IdentificationNumber { get; set; }
        public string Birthdate { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string ContactHours { get; set; }
        public string SpouseIdentificationNumber { get; set; }
        public string SpouseName { get; set; }
        public string SpouseSurname { get; set; }
        public string CUIT { get; set; }
        public string AddressDetail { get; set; }
        public string StartMonth { get; set; }
        public double? Discount { get; set; }
        public string ObservationsStatus { get; set; }
        public string htmlContract { get; set; }
        public string Clave { get; set; }

        public Contract ToModelObject()
        {
            return new Contract
            {
            ContractId = ContractId,
            RegistrationDate = Util.ParseDateTime(RegistrationDate),
            AdmissionDate  = DateTime.Now,
            Number = Number,
            AgencyId = GetNullableValue(ddlAgency),
            CustomerId =CustomerId,
            GoodRequested =GoodRequested,
            GoodRequestedColour  = GoodRequestedColour,
            PaymentDateId = GetNullableValue(ddlPaymentDate),
             VehicleBrand  = VehicleBrand,
             VehicleModel = VehicleModel,
            VehicleMileage  = VehicleMileage,
             VehicleRegistrationPlate = VehicleRegistrationPlate,
            SellerId = GetNullableValue(ddlSeller),
            ManagerId = GetNullableValue(ddlManager),
            SupervisorId = GetNullableValue(ddlSupervisor),
             SubscriptionAmount = SubscriptionAmount,
             FirstAdvanceAmount = FirstAdvanceAmount,
             AdvancesAmount = AdvancesAmount,
            Observations = Observations,
            Status = GetNullableValue(ddlStatus),
            StatusDetail = GetNullableValue(ddlStatusDetail),
            PaperStatus = GetNullableValue(ddlPaperStatus),
            Discount = Discount,
            FirstAdvanceDiscount = FirstAdvanceDiscount,
            SecondAdvanceDiscount = SecondAdvanceDiscount,
            ThirdAdvanceDiscount = ThirdAdvanceDiscount,
            PaymentPreference = GetNullableValue(ddlPaymentPreference),
            PaymentPlace = GetNullableValue(ddlPaymentPlace),
            ContractType = GetNullableValue(ddlContractType),
            };
        }

        public static ContractVm FromDomainModel(Contract entity)
        {
            return new ContractVm
            {
                ContractId = entity.ContractId,
                RegistrationDate = entity.RegistrationDate.ToString(),
                AdmissionDate = entity.AdmissionDate.ToString(),
                Number = entity.Number,
                AgencyId = entity.AgencyId,
                CustomerId = entity.CustomerId,
                GoodRequested = entity.GoodRequested,
                GoodRequestedColour = entity.GoodRequestedColour,
                VehicleBrand = entity.VehicleBrand,
                VehicleModel = entity.VehicleModel,
                VehicleMileage = entity.VehicleMileage,
                VehicleRegistrationPlate = entity.VehicleRegistrationPlate,
                SubscriptionAmount = entity.SubscriptionAmount,
                FirstAdvanceAmount = entity.FirstAdvanceAmount,
                AdvancesAmount = entity.AdvancesAmount,
                Observations = entity.Observations,
                Discount = entity.Discount,
                FirstAdvanceDiscount = entity.FirstAdvanceDiscount,
                SecondAdvanceDiscount = entity.SecondAdvanceDiscount,
                ThirdAdvanceDiscount = entity.ThirdAdvanceDiscount,
            };
        }
    }

    public class ContractWebSummaryVm : BaseVm
    {
        public int ContractId { get; set; }
        [Required(ErrorMessage = "El campo Fecha de ingreso es requerido")]
        public string RegistrationDate { get; set; }
        public string AdmissionDate { get; set; }
        public string QuotationDate { get; set; }
        public double? Quotation { get; set; }
        public double? QuotationDolar { get; set; }
        public int? AgencyId { get; set; }
        [Required(ErrorMessage = "El campo Cliente es requerido")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "El campo Bien requerido es requerido")]
        public string GoodRequested { get; set; }
        public string GoodRequestedColour { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleModel { get; set; }
        public int VehicleMileage { get; set; }
        public string VehicleRegistrationPlate { get; set; }
        [Required(ErrorMessage = "El campo Monto suscripción es requerido")]
        public double SubscriptionAmount { get; set; }
        [Required(ErrorMessage = "El campo  Primer cuota es requerido")]
        public double FirstAdvanceAmount { get; set; }
        public double? FirstAdvanceDiscount { get; set; }
        public double? SecondAdvanceDiscount { get; set; }
        public double? ThirdAdvanceDiscount { get; set; }
        [Required(ErrorMessage = "El campo cuotas siguientes es requerido")]
        public double AdvancesAmount { get; set; }
        public string Observations { get; set; }
        [Required(ErrorMessage = "El campo Fecha de Pago es requerido")]
        public SingleSelectVm ddlPaymentDate { get; set; }
        [Required(ErrorMessage = "El campo Agencia es requerido")]
        public SingleSelectVm ddlAgency { get; set; }
        public SingleSelectVm ddlSupervisor { get; set; }
        public SingleSelectVm ddlManager { get; set; }
        public SingleSelectVm ddlStatus { get; set; }
        public SingleSelectVm ddlState { get; set; }
        public SingleSelectVm ddlStatusDetail { get; set; }
        public SingleSelectVm ddlPaperStatus { get; set; }
        public SingleSelectVm ddlIdentificationType { get; set; }
        public SingleSelectVm ddlMaritalStatus { get; set; }
        public SingleSelectVm ddlCity { get; set; }
        public SingleSelectVm ddlSpouseIdentificationType { get; set; }
        public SingleSelectVm ddlStatusClient { get; set; }
        public SingleSelectVm ddlPaymentPreference { get; set; }
        public SingleSelectVm ddlPaymentPlace { get; set; }
        public SingleSelectVm ddlContractType { get; set; }
        public string Seller { get; set; }
        public string Supervisor { get; set; }
        public string Manager { get; set; }
        public string Status { get; set; }
        public string StatusDetail { get; set; }
        public string PaperStatus { get; set; }
        public string IdentificationType { get; set; }
        public string MaritalStatus { get; set; }
        public string City { get; set; }
        public string SpouseIdentificationType { get; set; }
        public string StatusClient { get; set; }
        public string PaymentPreference { get; set; }
        public string PaymentPlace { get; set; }
        public string PaymentDate { get; set; }
        public string ContractType { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IdentificationNumber { get; set; }
        public string Birthdate { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string ContactHours { get; set; }
        public string SpouseIdentificationNumber { get; set; }
        public string SpouseName { get; set; }
        public string SpouseSurname { get; set; }
        public string CUIT { get; set; }
        public string AddressDetail { get; set; }
        public string StartMonth { get; set; }
        public double? Discount { get; set; }
        public string ObservationsStatus { get; set; }
        public string htmlContract { get; set; }


        public Contract ToModelObject()
        {
            return new Contract
            {
                ContractId = ContractId,
                RegistrationDate = Util.ParseDateTime(RegistrationDate),
                AdmissionDate = DateTime.Now,
                AgencyId = GetNullableValue(ddlAgency),
                CustomerId = CustomerId,
                GoodRequested = GoodRequested,
                GoodRequestedColour = GoodRequestedColour,
                PaymentDateId = GetNullableValue(ddlPaymentDate),
                VehicleBrand = VehicleBrand,
                VehicleModel = VehicleModel,
                VehicleMileage = VehicleMileage,
                VehicleRegistrationPlate = VehicleRegistrationPlate,
                ManagerId = GetNullableValue(ddlManager),
                SupervisorId = GetNullableValue(ddlSupervisor),
                SubscriptionAmount = SubscriptionAmount,
                FirstAdvanceAmount = FirstAdvanceAmount,
                AdvancesAmount = AdvancesAmount,
                Observations = Observations,
                Status = GetNullableValue(ddlStatus),
                StatusDetail = GetNullableValue(ddlStatusDetail),
                PaperStatus = GetNullableValue(ddlPaperStatus),
                Discount = Discount,
                FirstAdvanceDiscount = FirstAdvanceDiscount,
                SecondAdvanceDiscount = SecondAdvanceDiscount,
                ThirdAdvanceDiscount = ThirdAdvanceDiscount,
                PaymentPreference = GetNullableValue(ddlPaymentPreference),
                PaymentPlace = GetNullableValue(ddlPaymentPlace),
                ContractType = GetNullableValue(ddlContractType),
            };
        }

        public static ContractVm FromDomainModel(Contract entity)
        {
            return new ContractVm
            {
                ContractId = entity.ContractId,
                RegistrationDate = entity.RegistrationDate.ToString(),
                AdmissionDate = entity.AdmissionDate.ToString(),
                Number = entity.Number,
                AgencyId = entity.AgencyId,
                CustomerId = entity.CustomerId,
                GoodRequested = entity.GoodRequested,
                GoodRequestedColour = entity.GoodRequestedColour,
                VehicleBrand = entity.VehicleBrand,
                VehicleModel = entity.VehicleModel,
                VehicleMileage = entity.VehicleMileage,
                VehicleRegistrationPlate = entity.VehicleRegistrationPlate,
                SubscriptionAmount = entity.SubscriptionAmount,
                FirstAdvanceAmount = entity.FirstAdvanceAmount,
                AdvancesAmount = entity.AdvancesAmount,
                Observations = entity.Observations,
                Discount = entity.Discount,
                FirstAdvanceDiscount = entity.FirstAdvanceDiscount,
                SecondAdvanceDiscount = entity.SecondAdvanceDiscount,
                ThirdAdvanceDiscount = entity.ThirdAdvanceDiscount,
            };
        }
    }


    public class ContractWebVm : BaseVm
    {
        public int ContractId { get; set; }
        public string htmlContract { get; set; }

    }
}