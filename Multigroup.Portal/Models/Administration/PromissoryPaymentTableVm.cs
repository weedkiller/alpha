using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class PromissoryPaymentFilterVm
    {
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public IEnumerable<string> SelectedCustomer { get; set; }
        public IEnumerable<SelectListItem> ListCustomer { get; set; }
        public double AmountFrom { get; set; }
        public double AmountTo { get; set; }
    }
    public class PromissoryPaymentTableVm
    {
        public IEnumerable<PromissoryPaymentListVm> PromissoryPaymentList { get; set; }

        public static PromissoryPaymentTableVm ToViewModel(IEnumerable<PromissoryPaymentSummary> entities)
        {
            return new PromissoryPaymentTableVm
            {
                PromissoryPaymentList = PromissoryPaymentListVm.ToViewModelList(entities),
            };
        }
    }

    public class PromissoryPaymentListVm
    {
        public int PromissoryPaymentId { get; set; }
        public string Client { get; set; }
        public string User { get; set; }
        public string PaymentDate { get; set; }
        public string SystemDate { get; set; }
        public double Amount { get; set; }


        public static IEnumerable<PromissoryPaymentListVm> ToViewModelList(IEnumerable<PromissoryPaymentSummary> entities)
        {
            return entities.Select(x => new PromissoryPaymentListVm
            {
                PromissoryPaymentId = x.PromissoryPaymentId,
                PaymentDate = x.PaymentDate.ToShortDateString(),
                SystemDate = x.SystemDate.ToShortDateString(),
                User = x.User,
                Client = x.Client,
                Amount = x.Amount,
            });
        }
    }

    public class PromissoryPaymentDetailTableVm
    {
        public IEnumerable<PromissoryPaymentDetailListVm> PromissoryPaymentList { get; set; }

        public static PromissoryPaymentDetailTableVm ToViewModel(IEnumerable<PromissorySummary> entities)
        {
            return new PromissoryPaymentDetailTableVm
            {
                PromissoryPaymentList = PromissoryPaymentDetailListVm.ToViewModelList(entities),
            };
        }
    }

    public class PromissoryPaymentDetailListVm
    {
        public int PromissoryId { get; set; }
        public int Number { get; set; }
        public string Client { get; set; }
        public string User { get; set; }
        public string BroadcastDate { get; set; }
        public string CollectionDate { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public string Description { get; set; }
        public string isPaid { get; set; }

        public static IEnumerable<PromissoryPaymentDetailListVm> ToViewModelList(IEnumerable<PromissorySummary> entities)
        {
            return entities.Select(x => new PromissoryPaymentDetailListVm
            {
                PromissoryId = x.PromissoryId,
                BroadcastDate = x.BroadcastDate.ToShortDateString(),
                CollectionDate = x.CollectionDate.ToShortDateString(),
                User = x.User,
                Amount = x.Amount,
                Commentary = x.Commentary,
                Client = x.Client,
                Number = x.Number,
                Description = x.Description,
                isPaid = x.isPaid,
            });
        }
    }
}