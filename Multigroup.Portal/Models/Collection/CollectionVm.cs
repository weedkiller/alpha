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

namespace Multigroup.Portal.Models.Collection
{
    public class CollectionsFiltersVm
    {
        public string SelectedPaymentMethod { get; set; }
        public IEnumerable<SelectListItem> ListPaymentMethod { get; set; }
        public string SelectedType { get; set; }
        public IEnumerable<SelectListItem> ListType { get; set; }
        public SingleSelectVm ddlPaymentPreference { get; set; }
        public string SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUsers { get; set; }
        public IEnumerable<string> SelectedCustomer { get; set; }
        public IEnumerable<SelectListItem> ListCustomers { get; set; }
    }
    public class CollectionVm : BaseVm
    {
        public int PaymentId { get; set; }
        public int ContractNumber { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string PaymentDate { get; set; }
        public string Observations { get; set; }
        public string ReceipNumber { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public double Amount { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string SelectedPaymentMethod { get; set; }
        public IEnumerable<SelectListItem> ListPaymentMethod { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string SelectedType { get; set; }
        public IEnumerable<SelectListItem> ListType { get; set; }
        public int? MPSellerAccountId { get; set; }
        public SingleSelectVm ddlMPSellerAccount { get; set; }

        public Payment ToModelObject()
        {
            return new Payment
            {
                PaymentId = PaymentId,
                PaymentDate = Util.ParseDateTime(PaymentDate),
                Observations = Observations,
                ReceipNumber = ReceipNumber,
                type = SelectedType,
                PaymentMethod = SelectedPaymentMethod,
                MPSellerAccountId = MPSellerAccountId,
                Amount = Amount,
                ContractNumber = ContractNumber,
            };
        }

        public static CollectionVm FromDomainModel(Payment entity)
        {
            return new CollectionVm
            {
                PaymentId = entity.PaymentId,
                PaymentDate = Util.GetFrenchFormatDate(entity.PaymentDate),
                Observations = entity.Observations,
                ReceipNumber = entity.ReceipNumber,
                Amount = entity.Amount,
            };
        }
    }
}