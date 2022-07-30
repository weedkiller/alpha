using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Multigroup.Portal.Utilities;

namespace Multigroup.Portal.Models.Collection
{
    public class AssignPaymentPreferenceTableVm
    {
        public IEnumerable<AssignPaymentPreferenceListVm> AssignPaymentPreferenceList { get; set; }

        public static AssignPaymentPreferenceTableVm ToViewModel(IEnumerable<AssignPaymentPreferenceSummary> entities)
        {
            return new AssignPaymentPreferenceTableVm
            {
                AssignPaymentPreferenceList = AssignPaymentPreferenceListVm.ToViewModelList(entities),
            };
        }
    }

    public class AssignPaymentPreferenceListVm
    {
        public int AssignPaymentPreferenceId { get; set; }
        public int InstallmentId { get; set; }
        public int Cuota { get; set; }
        public string PaymentPreference { get; set; }
        public string Observations { get; set; }
        public string Follow { get; set; }
        public string FollowDate { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        public string PaymentMethod { get; set; }
        public string AssignPaymentMethodDate { get; set; }
        public string Channel { get; set; }
        public bool Warned { get; set; }
        public string WarnedDate { get; set; }
        public string WarnedCommentary { get; set; }
        public string AssignState { get; set; }
        public string Actual { get; set; }
        public string PaymentDate { get; set; }
        public string Customer { get; set; }
        public string Number { get; set; }
        public string Voucher { get; set; }
        public string Settled { get; set; }
        public double Amount { get; set; }

        public static IEnumerable<AssignPaymentPreferenceListVm> ToViewModelList(IEnumerable<AssignPaymentPreferenceSummary> entities)
        {
            return entities.Select(x => new AssignPaymentPreferenceListVm
            {
                AssignPaymentPreferenceId = x.AssignPaymentPreferenceId,
                FollowDate = Util.GetFrenchFormatDate(x.FollowDate),
                AssignPaymentMethodDate = Util.GetFrenchFormatDate(x.AssignPaymentMethodDate),
                WarnedDate = Util.GetFrenchFormatDate(x.WarnedDate),
                InstallmentId = x.InstallmentId,
                PaymentMethod = x.PaymentMethod,
                Observations = x.Observations,
                PaymentPreference = x.PaymentPreference,
                Follow = x.Follow,
                UserId = x.UserId,
                Channel = x.Channel,
                Warned = x.Warned,
                WarnedCommentary = x.WarnedCommentary,
                AssignState = x.AssignState,
                Actual = x.Actual,
                User = x.User,
                Cuota = x.Cuota,
                PaymentDate = Util.GetFrenchFormatDate(x.PaymentDate),
                Customer = x.Customer,
                Number = x.Number,
                Amount = x.Amount,
            });
        }
    }

    public class AssignPaymentPreferenceWithVoucherTableVm
    {
        public IEnumerable<AssignPaymentPreferenceWithVoucherListVm> AssignPaymentPreferenceWithVoucherList { get; set; }

        public static AssignPaymentPreferenceWithVoucherTableVm ToViewModel(IEnumerable<AssignPaymentPreferenceWithVoucher> entities)
        {
            return new AssignPaymentPreferenceWithVoucherTableVm
            {
                AssignPaymentPreferenceWithVoucherList = AssignPaymentPreferenceWithVoucherListVm.ToViewModelList(entities),
            };
        }
    }

    public class AssignPaymentPreferenceWithVoucherListVm
    {
        public int AssignPaymentPreferenceId { get; set; }
        public int InstallmentId { get; set; }
        public int Cuota { get; set; }
        public float Amount { get; set; }
        public string PaymentPreference { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public string Customer { get; set; }
        public string CustomerMobile { get; set; }
        public string Channel { get; set; }
        public string WarnedDate { get; set; }
        public string Voucher { get; set; }
        public string PaymentDate { get; set; }
        public float PaymentAmount { get; set; }
        public string Telemarketer { get; set; }
        public string Number { get; set; }
        public string Saldado { get; set; }

        public static IEnumerable<AssignPaymentPreferenceWithVoucherListVm> ToViewModelList(IEnumerable<AssignPaymentPreferenceWithVoucher> entities)
        {
            return entities.Select(x => new AssignPaymentPreferenceWithVoucherListVm
            {
                AssignPaymentPreferenceId = x.AssignPaymentPreferenceId,
                PaymentDate = Util.GetFrenchFormatDate(x.PaymentDate),
                WarnedDate = Util.GetFrenchFormatDate(x.WarnedDate),
                Cuota = x.Cuota,
                Amount = x.Amount,
                InstallmentId = x.InstallmentId,
                PaymentMethodId = x.PaymentMethodId,
                PaymentMethod = x.PaymentMethod,
                PaymentPreference = x.PaymentPreference,
                Channel = x.Channel,
                CustomerMobile = x.CustomerMobile,
                Customer = x.Customer,
                Voucher = x.Voucher,
                PaymentAmount = x.PaymentAmount,
                Telemarketer = x.Telemarketer,
                Number = x.Number,
                Saldado = x.Saldado
            });
        }
    }

    public class AssignPaymentPreferenceCollectorTableVm
    {
        public IEnumerable<AssignPaymentPreferenceCollectorListVm> AssignPaymentPreferenceCollectorList { get; set; }

        public static AssignPaymentPreferenceCollectorTableVm ToViewModel(IEnumerable<AssignPaymentPreferenceCollector> entities)
        {
            return new AssignPaymentPreferenceCollectorTableVm
            {
                AssignPaymentPreferenceCollectorList = AssignPaymentPreferenceCollectorListVm.ToViewModelList(entities),
            };
        }
    }

    public class AssignPaymentPreferenceCollectorListVm
    {
        public int AssignPaymentPreferenceId { get; set; }
        public int InstallmentId { get; set; }
        public int Cuota { get; set; }
        public double Amount { get; set; }
        public string PaymentPreference { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public string Zone { get; set; }
        public int ZoneId { get; set; }
        public string Customer { get; set; }
        public string CustomerMobile { get; set; }
        public string PaymentDate { get; set; }
        public string VisitDate { get; set; }
        public string Collector { get; set; }
        public string Telemarketer { get; set; }
        public bool Assign { get; set; }
        public bool Surrender { get; set; }
        public bool Register { get; set; }
        public int Number { get; set; }

        public static IEnumerable<AssignPaymentPreferenceCollectorListVm> ToViewModelList(IEnumerable<AssignPaymentPreferenceCollector> entities)
        {
            return entities.Select(x => new AssignPaymentPreferenceCollectorListVm
            {
                AssignPaymentPreferenceId = x.AssignPaymentPreferenceId,
                PaymentDate = Util.GetFrenchFormatDate(x.PaymentDate),
                VisitDate = Util.GetFrenchFormatDate(x.VisitDate),
                Collector = x.Collector,
                Cuota = x.Cuota,
                Amount = x.Amount,
                InstallmentId = x.InstallmentId,
                PaymentMethodId = x.PaymentMethodId,
                PaymentMethod = x.PaymentMethod,
                PaymentPreference = x.PaymentPreference,
                CustomerMobile = x.CustomerMobile,
                Customer = x.Customer,
                Zone = x.Zone,
                ZoneId = x.ZoneId,
                Assign = x.Assign,
                Surrender = x.Surrender,
                Register = x.Register,
                Number = x.Number,
                Telemarketer = x.Telemarketer
            });
        }
    }
}