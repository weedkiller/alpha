using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class CollectorAssigmment
    {
        public int CollectorAssigmmentId { get; set; } 
        public int AssignPaymentPreferenceId { get; set; }
        public int UserId { get; set; }
        public DateTime? VisitDate { get; set; }
        public bool Active { get; set; }
        public DateTime? Date { get; set; }
    }

    public class CollectorAssigmmentSummary
    {
        public int CollectorAssigmmentId { get; set; }
        public int AssignPaymentPreferenceId { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        public DateTime? VisitDate { get; set; }
        public bool Active { get; set; }
        public DateTime? Date { get; set; }
    }

    public class CollectorSurrender
    {
        public int CollectorSurrenderId { get; set; }
        public int UserId { get; set; }
        public DateTime? Date { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public string Commentary { get; set; }
        public int ReceiptQuantity { get; set; }
        public int InstallmentQuantity { get; set; }
    }

    public class InstallmentSurrender
    {
        public int InstallmentSurrenderId { get; set; }
        public int CollectorSurrenderId { get; set; }
        public double Amount { get; set; }
        public int AssignPaymentPreferenceId { get; set; }
    }

    public class AssignPaymentPreferenceCollector
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
        public DateTime? PaymentDate { get; set; }
        public DateTime? VisitDate { get; set; }
        public string Collector { get; set; }
        public string Telemarketer { get; set; }
        public bool Assign { get; set; }
        public bool Surrender { get; set; }
        public bool Register { get; set; }
        public int Number { get; set; }
    }

    public class CollectorSurrenderPM
    {
        public int CollectorSurrenderPMId { get; set; }
        public int CollectorSurrenderId { get; set; }
        public int PaymentMethodId { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Saving { get; set; }
        public string Cycle { get; set; }
    }
    public class CollectorSurrenderReceipt
    {
        public string Number { get; set; }
        public double Amount { get; set; }
    }

    public class CollectorState
    {
        public int CollectorId { get; set; }
        public string Collector { get; set; }
        public double PositiveBalance { get; set; }
        public double NegativeBalance { get; set; }
        public double Balance { get; set; }
    }

    public class CollectorAdvancementPM
    {
        public int CollectorAdvancementPMId { get; set; }
        public int CollectorAdvancementId { get; set; }
        public int PaymentMethodId { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Saving { get; set; }
        public string Cycle { get; set; }
    }

    public class CollectorAdvancement
    {
        public int CollectorAdvancementId { get; set; }
        public int CollectorId { get; set; }
        public DateTime Date { get; set; }
        public string Hour { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public string Commentary { get; set; }
    }

    public class InstallmentSurrenderSummary
    {
        public int InstallmentSurrenderId { get; set; }
        public int CollectorSurrenderId { get; set; }
        public double Amount { get; set; }
        public int Cuota { get; set; }
        public int Contrato { get; set; }
        public string Cliente { get; set; }
    }
}
