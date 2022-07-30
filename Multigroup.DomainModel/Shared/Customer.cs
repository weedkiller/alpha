using Multigroup.DomainModel.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime? Birthdate { get; set; }
        public int? MaritalStatus { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string ContactHours { get; set; }
        public int? SpouseIdentificationType { get; set; }
        public string SpouseIdentificationNumber { get; set; }
        public string SpouseName { get; set; }
        public string SpouseSurname { get; set; }
        public int? StatusClient { get; set; }
        public string CUIT { get; set; }
        public string Color { get; set; }
        public string AddressDetail { get; set; }
    }

    public class CustomerPortfolio
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime? Birthdate { get; set; }
        public int? MaritalStatus { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string ContactHours { get; set; }
        public int? SpouseIdentificationType { get; set; }
        public string SpouseIdentificationNumber { get; set; }
        public string SpouseName { get; set; }
        public string SpouseSurname { get; set; }
        public int? StatusClient { get; set; }
        public string CUIT { get; set; }
        public string Color { get; set; }
        public string AddressDetail { get; set; }
        public double FirstAdvanceAmount { get; set; }
        public double AdvancesAmount { get; set; }
        public double Monto { get; set; }
        public double Cantidad { get; set; }
    }

    public class CustomerSummary
    {
        public int CustomerId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string UserName { get; set; }
        public string Zone { get; set; }
        public string Color { get; set; }
    }

    public class CustomerPortfolioTotal
    {
        public int Amount1 { get; set; }
        public int Amount2 { get; set; }
        public double Quantity { get; set; }
    }


    public class CustomerDebt
    {
        public int Number { get; set; }
        public string IdentificationNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string StatusContract { get; set; }
        public string Color { get; set; }
        public string Count { get; set; }
        public string Amount { get; set; }
        public string Interest { get; set; }
        public string Total { get; set; }
        public string User { get; set; }

    }

    public class MaritalStatus
    {
        public int MaritalStatusId { get; set; }
        public string Description { get; set; }
    }

    public class IdentificationType
    {
        public int IdentificationTypeId { get; set; }
        public string Description { get; set; }
    }

    public class StatusClient
    {
        public int StatusClientId { get; set; }
        public string Description { get; set; }
    }

    public class CustomerUserResume
    {
        public string Usuario { get; set; }
        public int Cantidad { get; set; }
    }

    public class InstallmentReport
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IdentificationNumber { get; set; }       
        public string State { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public int CuotaActual { get; set; }
        public int CuotasDebe { get; set; }
        public string Color { get; set; }
        public string Status { get; set; }
        public double CuotaValor { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
