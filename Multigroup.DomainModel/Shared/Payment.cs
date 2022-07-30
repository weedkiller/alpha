using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class Payment
    {
        public int PaymentId { get; set; } 
        public DateTime? PaymentDate { get; set; }
        public string type { get; set; }
        public string PaymentMethod { get; set; }
        public string Observations { get; set; }
        public string ReceipNumber { get; set; }
        public double Amount { get; set; } 
        public int ContractNumber { get; set; }
        public int? MPSellerAccountId { get; set; }
    }

    public class PaymentSummary
    {
        public int PaymentId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string type { get; set; }
        public string PaymentMethod { get; set; }
        public string Observations { get; set; }
        public string ReceipNumber { get; set; }
        public double Amount { get; set; }
        public int ContractNumber { get; set; }
        public int? MPSellerAccountId { get; set; }
        public string Customer { get; set; }
        public string DNI { get; set; }
        public string User { get; set; }

    }

    public class PaymentResume
    {
        public string Description { get; set; }
        public double Value { get; set; }
    }

    public class AssignPaymentPreference
    {
        public int AssignPaymentPreferenceId { get; set; }
        public int InstallmentId { get; set; }
        public int? PaymentPreferenceId { get; set; }
        public string Observations { get; set; }
        public string Follow { get; set; }
        public DateTime? FollowDate { get; set; }
        public int UserId { get; set; }
        public int? PaymentMethodId { get; set; }
        public DateTime? AssignPaymentMethodDate { get; set; }
        public string Channel { get; set; }
        public bool Warned { get; set; }
        public DateTime? WarnedDate { get; set; }
        public string WarnedCommentary { get; set; }
        public int AssignStateId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public double Amount { get; set; }
    }

    public class AssignPaymentPreferenceSummary
    {
        public int AssignPaymentPreferenceId { get; set; }
        public int InstallmentId { get; set; }
        public int Cuota { get; set; }
        public string PaymentPreference { get; set; }
        public string Observations { get; set; }
        public string Follow { get; set; }
        public DateTime? FollowDate { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? AssignPaymentMethodDate { get; set; }
        public string Channel { get; set; }
        public bool Warned { get; set; }
        public DateTime? WarnedDate { get; set; }
        public string WarnedCommentary { get; set; }
        public string AssignState { get; set; }
        public string Actual { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Customer { get; set; }
        public string Number { get; set; }
        public double Amount { get; set; }
    }

    public class AssignPaymentPreferenceWithVoucher
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
        public DateTime? WarnedDate { get; set; }
        public string Voucher { get; set; }
        public DateTime? PaymentDate { get; set; }
        public float PaymentAmount { get; set; }
        public string Telemarketer { get; set; }
        public string Number { get; set; }
        public string Saldado { get; set; }
    }

    public class PaymentVoucher
    {
        public int PaymentVoucherId { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime? VoucherDate { get; set; }
        public float Amount { get; set; }
        public string Code { get; set; }
        public string Commentary { get; set; }
        public int UserId { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public int ConfirmationUserId { get; set; }
    }

    public class PaymentVoucherInstallment
    {
        public int PaymentVoucherInstallmentId { get; set; }
        public int InstallmentId { get; set; }
        public int PaymentVoucherId { get; set; }
        public double Amount { get; set; }
        public int AssignPaymentPreferenceId { get; set; }
        public bool IsSamePaymentMethod { get; set; }    
    }

    public class PaymentVoucherPayment
    {
        public int PaymentVoucherPaymentId { get; set; }
        public int PaymentVoucherInstallmentId { get; set; }
        public int PaymentId { get; set; }
    }

    public class AssignState
    {
        public int AssignStateId { get; set; }
        public string Description { get; set; }
    }

    public class PaymentVoucherSummary
    {

        public int PaymentVoucherId { get; set; }
        public int Cuotas { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime? VoucherDate { get; set; }
        public string Confirmado { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string Telemarketer { get; set; }
        public string ConfirmationUser { get; set; }
        public string Customer { get; set; }
        public string Contract { get; set; }

    }

    public class PaymentVoucherDetail
    {
        public int Cuota { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentMethodOrigin { get; set; }
    }

    public class PaymentPaymentMethod
    {
        public int PaymentPaymentMethodId { get; set; }
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public string Saving { get; set; }
        public string Cycle { get; set; }
        public string ReconciledId { get; set; }
    }

}
