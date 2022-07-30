using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Shared
{
    public class PaymentHistoryFilterVm
    {
        public IEnumerable<string> SelectedCustomer { get; set; }
        public IEnumerable<SelectListItem> ListCustomers { get; set; }
        public IEnumerable<string> SelectedPaymentMethod { get; set; }
        public IEnumerable<SelectListItem> ListPaymentMethods { get; set; }
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUsers { get; set; }
        public IEnumerable<string> SelectedProvince { get; set; }
        public IEnumerable<SelectListItem> ListProvince { get; set; }
        public IEnumerable<string> SelectedCities { get; set; }
        public IEnumerable<SelectListItem> ListCities { get; set; }
        public IEnumerable<string> SelectedSupervisor { get; set; }
        public IEnumerable<SelectListItem> ListSupervisor { get; set; }
        public int InstallmentNumber { get; set; }
    }
    public class PaymentHistoryTableVm
    {
        public IEnumerable<PaymentHistoryListVm> PaymentHistoryList { get; set; }

        public static PaymentHistoryTableVm ToViewModel(IEnumerable<PaymentHistory> entities)
        {
            return new PaymentHistoryTableVm
            {
                PaymentHistoryList = PaymentHistoryListVm.ToViewModelList(entities),
            };
        }
    }

    public class PaymentHistoryListVm
    {
        public int Number { get; set; }
        public string Customer { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentDate { get; set; }
        public string InstallmentNumber { get; set; }
        public string Seller { get; set; }
        public string Telemarketer { get; set; }
        public string Supervisor { get; set; }
        public string Amount { get; set; }
        public static IEnumerable<PaymentHistoryListVm> ToViewModelList(IEnumerable<PaymentHistory> entities)
        {
            return entities.Select(x => new PaymentHistoryListVm
            {
                Number = x.Number,
                Customer = x.Customer,
                PaymentMethod = x.PaymentMethod,
                PaymentDate = Util.GetFrenchFormatDate(x.PaymentDate),
                InstallmentNumber = x.InstallmentNumber,
                Seller = x.Seller,
                Telemarketer = x.Telemarketer,
                Supervisor = x.Supervisor,
                Amount = x.Amount,
            });
        }
    }
}