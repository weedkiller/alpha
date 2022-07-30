using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Enums;
using Multigroup.Portal.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Maintenance
{
    public class PortfolioCustomerFilterVm
    {
        public IEnumerable<string> SelectedZones { get; set; }
        public IEnumerable<SelectListItem> ListZones { get; set; }
        public IEnumerable<string> SelectedProvince { get; set; }
        public IEnumerable<SelectListItem> ListProvince { get; set; }
        public IEnumerable<string> SelectedCities { get; set; }
        public IEnumerable<SelectListItem> ListCities { get; set; }
        public IEnumerable<string> SelectedStatus { get; set; }
        public IEnumerable<SelectListItem> ListStatus { get; set; }
        public string SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUsers { get; set; }
        public string SelectedUserAsign { get; set; }
        public IEnumerable<SelectListItem> ListUsersAsign { get; set; }
        public int SelectedMonth { get; set; }
        public IEnumerable<SelectListItem> ListMonths { get; set; }
        public int Year { get; set; }
        public int Installment { get; set; }
        public SingleSelectVm ddlPaymentDate { get; set; }
        public SingleSelectVm ddlPaymentMethod{ get; set; }
        public IEnumerable<ResumeVm> Resume { get; set; }
        public IEnumerable<ResumeVm> ResumeNext { get; set; }

    }
   
    public class ResumeTableVm
    {
        public IEnumerable<ResumeVm> CustomerList { get; set; }

        public static ResumeTableVm ToViewModel(IEnumerable<BaseEntityText> entities)
        {
            return new ResumeTableVm
            {
                CustomerList = ResumeVm.ToViewModelList(entities),
            };
        }
    }
    public class ResumeVm
    {
        public string User { get; set; }
        public string Quantity { get; set; }

        public static IEnumerable<ResumeVm> ToViewModelList(IEnumerable<BaseEntityText> entities)
        {
            return entities.Select(x => new ResumeVm
            {
                User = x.Name,
                Quantity = x.Code,
            });
        }
    }

    public class PortfolioCustomerTableVm
    {
        public IEnumerable<PortfolioCustomerListVm> CustomerList { get; set; }

        public static PortfolioCustomerTableVm ToViewModel(IEnumerable<CustomerPortfolio> entities)
        {
            return new PortfolioCustomerTableVm
            {
                CustomerList = PortfolioCustomerListVm.ToViewModelList(entities),
            };
        }
    }

    public class PortfolioCustomerListVm
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IdentificationNumber { get; set; }
        public string CUIT { get; set; }
        public DateTime Birthdate { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Occupation { get; set; }
        public string ContactHours { get; set; }
        public string SpouseIdentificationType { get; set; }
        public string SpouseIdentificationNumber { get; set; }
        public string SpouseName { get; set; }
        public string SpouseSurname { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string StatusClient { get; set; }
        public string User { get; set; }
        public double FirstAdvanceAmount { get; set; }
        public double AdvancesAmount { get; set; }
        public double Monto { get; set; }
        public double Cantidad { get; set; }

        public static IEnumerable<PortfolioCustomerListVm> ToViewModelList(IEnumerable<CustomerPortfolio> entities)
        {
            return entities.Select(x => new PortfolioCustomerListVm
            {
                CustomerId = x.CustomerId,
                Name = x.Name + " " + x.Surname,
                IdentificationNumber = x.IdentificationNumber,
                Phone = x.Phone,
                CellPhone = x.CellPhone,
                Email = x.Email,
                ContactHours = x.ContactHours,
                CUIT = x.CUIT,
                User = x.Occupation,
                FirstAdvanceAmount = x.FirstAdvanceAmount,
                AdvancesAmount = x.AdvancesAmount,
                State = x.State,
                City = x.SpouseSurname,
                Monto = x.Monto,
                Cantidad = x.Cantidad
            });
        }
    }

        public class InstallmentReportTableVm
        {
            public IEnumerable<InstallmentReportListVm> CustomerList { get; set; }

            public static InstallmentReportTableVm ToViewModel(IEnumerable<InstallmentReport> entities)
            {
                return new InstallmentReportTableVm
                {
                    CustomerList = InstallmentReportListVm.ToViewModelList(entities),
                };
            }
        }

        public class InstallmentReportListVm
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
            public string PaymentDate { get; set; }

        public static IEnumerable<InstallmentReportListVm> ToViewModelList(IEnumerable<InstallmentReport> entities)
            {
                return entities.Select(x => new InstallmentReportListVm
                {
                    CustomerId = x.CustomerId,
                    Name = x.Name + " " + x.Surname,
                    IdentificationNumber = x.IdentificationNumber,
                    Phone = x.Phone,
                    CellPhone = x.CellPhone,
                    Email = x.Email,
                    State = x.State,
                    CuotaActual = x.CuotaActual,
                    CuotasDebe = x.CuotasDebe,
                    Number = x.Number,
                    Color = x.Color,
                    PaymentDate = x.PaymentDate.ToShortDateString(),
                    CuotaValor = x.CuotaValor,
                    Status = x.Status
                });
            }
        }
    
}