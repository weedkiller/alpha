using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Maintenance
{
    public class CustomerFilterVm
    {
        public IEnumerable<string> SelectedZones { get; set; }
        public IEnumerable<SelectListItem> ListZones { get; set; }
        public IEnumerable<string> SelectedStatus { get; set; }
        public IEnumerable<SelectListItem> ListStatus { get; set; }
    }
    public class CustomerTableVm
    {
        public IEnumerable<CustomerListVm> CustomerList { get; set; }

        public static CustomerTableVm ToViewModel(IEnumerable<Customer> entities)
        {
            return new CustomerTableVm
            {
                CustomerList = CustomerListVm.ToViewModelList(entities),
            };
        }
    }

    public class CustomerListVm
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
        public string StatusClient { get; set; }
        public string Color { get; set; }


        public static IEnumerable<CustomerListVm> ToViewModelList(IEnumerable<Customer> entities)
        {
            return entities.Select(x => new CustomerListVm
            {
                CustomerId = x.CustomerId,
                Name = x.Name + " " + x.Surname,
                IdentificationNumber = x.IdentificationNumber,
                Phone = x.Phone,
                CellPhone = x.CellPhone,
                Email = x.Email,
                ContactHours = x.ContactHours,
                CUIT= x.CUIT,
                Color = x.Color,
            });
        }
    }
}