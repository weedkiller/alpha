using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public double Percentage { get; set; }
        public bool Consolidated { get; set; }
        public bool Verifiable { get; set; }
        public bool Automatic { get; set; }
        public bool Asignation { get; set; }
        public int? PaymentPreferenceId { get; set; }
        public string Channel { get; set; }
    }

    public class PaymentMethodSummary
    {
        public int PaymentMethodId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public double Percentage { get; set; }
        public bool Consolidated { get; set; }
        public bool Verifiable { get; set; }
        public bool Automatic { get; set; }
        public bool Asignation { get; set; }
        public string PaymentPreference { get; set; }
        public string Channel { get; set; }
    }

    public class PaymentMethodVerification
    {
        public int PaymentMethodVerificationId { get; set; }
        public int MovementID { get; set; }
        public DateTime VerificationDate { get; set; }
        public string Time { get; set; }
        public string MovementType { get; set; }
    }

    public class PaymentMethodReconciled
    {
        public int PaymentMethodReconciledId { get; set; }
        public int PaymentMethodId { get; set; }
        public int UserId { get; set; }
        public DateTime ReconciledDate { get; set; }
        public string Time { get; set; }
        public DateTime SystemDate { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
    }

    public class PaymentMethodReconciledSummary
    {
        public int PaymentMethodReconciledId { get; set; }
        public string PaymentMethod { get; set; }
        public string User { get; set; }
        public DateTime ReconciledDate { get; set; }
        public string Time { get; set; }
        public DateTime SystemDate { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }

    }

    public class PaymentMethodReconciledLast
    {
        public int PaymentMethodReconciledId { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? ReconciledDate { get; set; }
    }

    public class PaymentPreferencePaymentMethods
    {
        public int PaymentPreferencePMId { get; set; }
        public int PaymentPreferenceId { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentPreference { get; set; }
    }
}
