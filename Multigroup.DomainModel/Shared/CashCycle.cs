using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class CashCycle
    {
        public int CashCycleId { get; set; }
        public int CashierId { get; set; }
        public int CycleNumber { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public int? UserId { get; set; }
        public bool Validate { get; set; }
        public int? UserValidateId { get; set; }
        public bool Closed { get; set; }
        public IEnumerable<CashCyclePaymentMethod> PaymentMethods { get; set; }
    }

    public class CashCycleSummary
    {
        public int CashCycleId { get; set; }
        public int CashierId { get; set; }
        public string Cashier { get; set; }
        public int CycleNumber { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public string User { get; set; }
        public bool Validate { get; set; }
        public string UserValidate { get; set; }
        public bool Closed { get; set; }
        public IEnumerable<CashCyclePaymentMethod> PaymentMethods { get; set; }
    }

    public class CashCyclePaymentMethod
    {
        public int CashCyclePaymentMethodId { get; set; }
        public int CashCycleId { get; set; }
        public int PaymentMethodId { get; set; }
        public double Balance { get; set; }
    }

    public class CashCyclePaymentMethodSummary
    {
        public int CashCyclePaymentMethodId { get; set; }
        public int CashCycleId { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public double OLD { get; set; }
        public double Entry { get; set; }
        public double Egress { get; set; }
        public double Balance { get; set; }
    }

    public class CashCyclePayment
    {
        public string PaymentMethod { get; set; }
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; }
        public double Amount { get; set; }
    }

    public class Cycle
    {
        public int CycleId { get; set; }
        public int CycleNumber { get; set; }
    }

}
