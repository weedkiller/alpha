using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class Contract
    {
        public int ContractId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public int Number { get; set; }
        public int? AgencyId { get; set; }
        public int CustomerId { get; set; }
        public string GoodRequested { get; set; }
        public string GoodRequestedColour { get; set; }
        public int? PaymentDateId { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleModel { get; set; }
        public int VehicleMileage { get; set; }
        public string VehicleRegistrationPlate { get; set; }
        public int? SellerId { get; set; }
        public int? SupervisorId { get; set; }
        public int? ManagerId { get; set; }
        public double SubscriptionAmount { get; set; }
        public double FirstAdvanceAmount { get; set; }
        public double AdvancesAmount { get; set; }
        public string Observations { get; set; }
        public int? Status { get; set; }
        public int? StatusDetail { get; set; }
        public int? PaperStatus { get; set; }
        public double? Discount { get; set; }
        public double? FirstAdvanceDiscount { get; set; }
        public double? SecondAdvanceDiscount { get; set; }
        public double? ThirdAdvanceDiscount { get; set; }
        public int? PaymentPreference { get; set; }
        public int? PaymentPlace { get; set; }
        public int? ContractType { get; set; }
        //Contrato de casa
        public double Expenses { get; set; }
        public double Total { get; set; }
        public double Salary { get; set; }
        public string Code { get; set; }
        public string Vehicle { get; set; }
        public string Money { get; set; }
        public string Description { get; set; }
        public string Product { get; set; }
        public string Measure { get; set; }
        public double Advance { get; set; }
    }

    public class StatusContract
    {
        public int StatusContractId { get; set; }
        public string Description { get; set; }
    }

    public class QuotationContract
    {
        public int QuotationContractId { get; set; }
        public int ContractId { get; set; }
        public DateTime? QuotationDate { get; set; }
        public double? Quotation { get; set; }
        public double? QuotationDolar { get; set; }
    }

    public class PaymentDate
    {
        public int PaymentDateId { get; set; }
        public string Description { get; set; }
    }

    public class PaymentPreference
    {
        public int PaymentPreferenceId { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public bool Active { get; set; }
        public bool Assing { get; set; }
        public string Class { get; set; }
    }

    public class PaymentPlace
    {
        public int PaymentPlaceId { get; set; }
        public string Description { get; set; }
    }

    public class ContractType
    {
        public int ContractTypeId { get; set; }
        public string Description { get; set; }
    }

    public class ContractSummary
    {
        public int ContractId { get; set; }
        public int Number { get; set; }
        public string Zone { get; set; }
        public string Agency { get; set; }
        public string Customer { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; }
        public string PaperStatus { get; set; }
        public string Color { get; set; }
        public string DNI { get; set; }
        public string ContractType { get; set; }
        public string User { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? InitDate { get; set; }
        public double FirstInstallmentAmount { get; set; }
        public double InstallmentAmount { get; set; }
    }

    public class ContractSummaryTotal
    {
        public int Amount1 { get; set; }
        public int Amount2 { get; set; }
        public double Quantity { get; set; }
    }

    public class ContractResume
    {
        public string Customer { get; set; }
        public string ContractNumber { get; set; }
        public string Agency { get; set; }
        public string GoodRequested { get; set; }
        public int ContractTerm { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalInstallments { get; set; }
        public int PaidInstallments { get; set; }
        public int InstallmentsToExpire { get; set; }
        public int PartialInstallments { get; set; }
        public int OwedInstallments { get; set; }
        public double Advance { get; set; }
        public double Bonification { get; set; }
        public double Monto { get; set; }
    }

    public class ContractCuotas
    {
        public int Number { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }
        public string User { get; set; }
        public double Anio { get; set; }
        public double Mes { get; set; }
    }

    public class ContractAmount
    {
        public double Amount { get; set; }
    }
    public class ContractChart
    {
        public String Valor { get; set; }
        public int Cantidad { get; set; }
    }

    public class CuotasContrato
    {
        public int CuotasContratoId { get; set; }
        public int NumeroContrato { get; set; }
        public int Cuotas { get; set; }
        public bool Migrado { get; set; }
        public double Monto { get; set; }

    }

    public class SupervisorReport
    {
        public string Supervisor { get; set; }
        public string State { get; set; }
        public double Quantity { get; set; }
        public double amount { get; set; }
    }

    public class PortfolioReport
    {
        public string Telemarketer { get; set; }
        public string State { get; set; }
        public double Quantity { get; set; }
        public double amount { get; set; }
    }

    public class StatusContractHistory
    {

        public int StatusContractHistoryId { get; set; }
        public string Date { get; set; }
        public string StatusOld { get; set; }
        public string StatusNew { get; set; }
        public string Observations { get; set; }
        public string User { get; set; }
    }

    public class StatusReport
    {

        public int Number { get; set; }
        public string Customer { get; set; }
        public string Status { get; set; }
        public string UserChange { get; set; }
        public DateTime StatusDate { get; set; }
        public string ActualStatus { get; set; }
        public DateTime ActualStatusDate { get; set; }
    }
}
