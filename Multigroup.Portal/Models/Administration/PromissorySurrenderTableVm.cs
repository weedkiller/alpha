using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class PromissorySurrenderFilterVm
    {
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public IEnumerable<string> SelectedPartner { get; set; }
        public IEnumerable<SelectListItem> ListPartner { get; set; }
        public double AmountFrom { get; set; }
        public double AmountTo { get; set; }
    }
    public class PromissorySurrenderTableVm
    {
        public IEnumerable<PromissorySurrenderListVm> PromissorySurrenderList { get; set; }

        public static PromissorySurrenderTableVm ToViewModel(IEnumerable<PromissorySurrenderPartnerSummary> entities)
        {
            return new PromissorySurrenderTableVm
            {
                PromissorySurrenderList = PromissorySurrenderListVm.ToViewModelList(entities),
            };
        }
    }

    public class PromissorySurrenderListVm
    {
        public int PromissorySurrenderPartnerId { get; set; }
        public int PromissorySurrenderId { get; set; }
        public int PromissoryPaymentId { get; set; }
        public int Number { get; set; }
        public string Partner { get; set; }
        public string User { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; }
        public string PaymentDate { get; set; }


        public static IEnumerable<PromissorySurrenderListVm> ToViewModelList(IEnumerable<PromissorySurrenderPartnerSummary> entities)
        {
            return entities.Select(x => new PromissorySurrenderListVm
            {
                PromissorySurrenderId = x.PromissorySurrenderId,
                PromissorySurrenderPartnerId = x.PromissorySurrenderPartnerId,
                PaymentDate = x.PaymentDate.ToShortDateString(),
                PromissoryPaymentId = x.PromissoryPaymentId,
                Number = x.Number,
                User = x.User,
                Partner = x.Partner,
                Amount = x.Amount,
                IsPaid = x.IsPaid,
            });
        }
    }

    public class PromissorySurrenderDetailTableVm
    {
        public IEnumerable<PromissorySurrenderDetailListVm> PromissorySurrenderList { get; set; }

        public static PromissorySurrenderDetailTableVm ToViewModel(IEnumerable<PromissorySummary> entities)
        {
            return new PromissorySurrenderDetailTableVm
            {
                PromissorySurrenderList = PromissorySurrenderDetailListVm.ToViewModelList(entities),
            };
        }
    }

    public class PromissorySurrenderDetailListVm
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

        public static IEnumerable<PromissorySurrenderDetailListVm> ToViewModelList(IEnumerable<PromissorySummary> entities)
        {
            return entities.Select(x => new PromissorySurrenderDetailListVm
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