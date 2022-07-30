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
    public class ContractHouseVm : BaseVm
    {
        public int ContractId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string RegistrationDate { get; set; }
        public string AdmissionDate { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public int Number { get; set; }
        public int? AgencyId { get; set;}
        [Required(ErrorMessage = "El campo es requerido")]
        public int CustomerId { get; set; }
        public string Vehicle { get; set; }
        public string Code { get; set; }
        public string Money { get; set; }
        public string Observations { get; set; }
        public string PaymentDate { get; set; }
        public int RequestNumber { get; set; }
        public double Advance { get; set; }
        public double Expenses { get; set; }
        public double Total { get; set; }
        public double Salary { get; set; }
        public string Description { get; set; }
        public string Product { get; set; }
        public string Measure { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public SingleSelectVm ddlPaymentDate { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public SingleSelectVm ddlAgency { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
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

        public string StartMonth { get; set; }

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
            PaymentDateId = GetNullableValue(ddlPaymentDate),
            SellerId = GetNullableValue(ddlSeller),
            ManagerId = GetNullableValue(ddlManager),
            SupervisorId = GetNullableValue(ddlSupervisor),
            SubscriptionAmount = Total,
            FirstAdvanceAmount = Advance,
            AdvancesAmount = Advance,
            Advance = Advance,
            Observations = Observations,
            Status = GetNullableValue(ddlStatus),
            StatusDetail = GetNullableValue(ddlStatusDetail),
            PaperStatus = GetNullableValue(ddlPaperStatus),
            PaymentPreference = GetNullableValue(ddlPaymentPreference),
            PaymentPlace = GetNullableValue(ddlPaymentPlace),
            ContractType = 3,
            Expenses = Expenses,
            Total = Total,
            Salary = Salary,
            Code = Code,
            Vehicle = Vehicle,
            Money = Money, 
            Description = Description, 
            Product = Product, 
            Measure = Measure
            };
        }

        public static ContractHouseVm FromDomainModel(Contract entity)
        {
            return new ContractHouseVm
            {
                ContractId = entity.ContractId,
                RegistrationDate = entity.RegistrationDate.ToString(),
                AdmissionDate = entity.AdmissionDate.ToString(),
                Number = entity.Number,
                AgencyId = entity.AgencyId,
                CustomerId = entity.CustomerId,
                Advance = entity.Advance,
                Observations = entity.Observations,
                Expenses = entity.Expenses,
                Total = entity.Total,
                Salary = entity.Salary,
                Code = entity.Code,
                Vehicle = entity.Vehicle,
                Money = entity.Money,
                Description = entity.Description,
                Product = entity.Product,
                Measure = entity.Measure
            };
        }
    }
}