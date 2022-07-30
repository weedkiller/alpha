using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Multigroup.Portal.Utilities;

namespace Multigroup.Portal.Models.Collection
{

    public class VoucherTableVm
    {
        public IEnumerable<VoucherListVm> VoucherList { get; set; }

        public static VoucherTableVm ToViewModel(IEnumerable<AssignPaymentPreferenceWithVoucher> entities)
        {
            return new VoucherTableVm
            {
                VoucherList = VoucherListVm.ToViewModelList(entities),
            };
        }
    }

    public class VoucherListVm
    {

        public int AssignPaymentPreferenceId { get; set; }
        public int Cuota { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Channel { get; set; }
        public string Color { get; set; }
        public double PaymentAmount { get; set; }
        public int InstallmentId { get; set; }
        public int PaymentMethodId { get; set; }

        public static IEnumerable<VoucherListVm> ToViewModelList(IEnumerable<AssignPaymentPreferenceWithVoucher> entities)
        {
            return entities.Select(x => new VoucherListVm
            {
                AssignPaymentPreferenceId = x.AssignPaymentPreferenceId,
                PaymentMethod = x.PaymentMethod,
                Cuota = x.Cuota,
                Channel = x.Channel,
                Amount = x.Amount,
                PaymentAmount = x.PaymentAmount,
                InstallmentId = x.InstallmentId,
                PaymentMethodId = x.PaymentMethodId
            });
        }
    }


    public class PaymentVoucherSummaryTableVm
    {
        public IEnumerable<PaymentVoucherSummaryListVm> PaymentVoucherSummaryList { get; set; }

        public static PaymentVoucherSummaryTableVm ToViewModel(IEnumerable<PaymentVoucherSummary> entities)
        {
            return new PaymentVoucherSummaryTableVm
            {
                PaymentVoucherSummaryList = PaymentVoucherSummaryListVm.ToViewModelList(entities),
            };
        }
    }

    public class PaymentVoucherSummaryListVm
    {

        public int PaymentVoucherId { get; set; }
        public int Cuotas { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentMethodId { get; set; }
        public string VoucherDate { get; set; }
        public string Confirmado { get; set; }
        public string ConfirmationDate { get; set; }
        public string Telemarketer { get; set; }
        public string ConfirmationUser { get; set; }
        public string Customer { get; set; }
        public string Contract { get; set; }

        public static IEnumerable<PaymentVoucherSummaryListVm> ToViewModelList(IEnumerable<PaymentVoucherSummary> entities)
        {
            return entities.Select(x => new PaymentVoucherSummaryListVm
            {
                PaymentVoucherId = x.PaymentVoucherId,
                PaymentMethod = x.PaymentMethod,
                Cuotas = x.Cuotas,
                PaymentMethodId = x.PaymentMethodId,
                Amount = x.Amount,
                VoucherDate = Util.GetFrenchFormatDate(x.VoucherDate),
                Confirmado = x.Confirmado,
                Telemarketer = x.Telemarketer,
                ConfirmationUser = x.ConfirmationUser,
                ConfirmationDate = Util.GetFrenchFormatDate(x.ConfirmationDate),
                Customer = x.Customer,
                Contract = x.Contract
            });
        }
    }

    public class PaymentVoucherDetailTableVm
    {
        public IEnumerable<PaymentVoucherDetailListVm> PaymentVoucherDetailList { get; set; }

        public static PaymentVoucherDetailTableVm ToViewModel(IEnumerable<PaymentVoucherDetail> entities)
        {
            return new PaymentVoucherDetailTableVm
            {
                PaymentVoucherDetailList = PaymentVoucherDetailListVm.ToViewModelList(entities),
            };
        }
    }

    public class PaymentVoucherDetailListVm
    {

        public int Cuota { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentMethodOrigin { get; set; }      

        public static IEnumerable<PaymentVoucherDetailListVm> ToViewModelList(IEnumerable<PaymentVoucherDetail> entities)
        {
            return entities.Select(x => new PaymentVoucherDetailListVm
            {
                PaymentMethod = x.PaymentMethod,
                Cuota = x.Cuota,
                PaymentMethodOrigin = x.PaymentMethodOrigin,
                Amount = x.Amount,              
            });
        }
    }
}