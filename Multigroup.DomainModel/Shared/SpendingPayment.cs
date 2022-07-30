using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class SpendingPayment
    {
        public int SpendingPaymentId { get; set; }
        public DateTime ExecutionDate { get; set; }
        public DateTime? SystemDate { get; set; }
        public string Receipt { get; set; }
        public int? ProveedorId { get; set; }
        public int SpendingPaymentTypeId { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public string Commentary { get; set; }
        public IEnumerable<SpendingPaymentDetail> Details { get; set; }
        public IEnumerable<SpendingPaymentPaymentMethod> PaymentMethods { get; set; }
    }

    public class SpendingPaymentSummary
    {
        public int SpendingPaymentId { get; set; }
        public DateTime ExecutionDate { get; set; }
        public DateTime? SystemDate { get; set; }
        public string Receipt { get; set; }
        public string Proveedor { get; set; }
        public string User { get; set; }
        public double Balance { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public int UserId { get; set; }
    }

    public class SpendingPaymentDetail
    {
        public int SpendingPaymentDetailId { get; set; }
        public int SpendingPaymentId { get; set; }
        public int SpendingId { get; set; }
        public string Spending { get; set; }
        public double Total { get; set; }
    }

    public class SpendingPaymentDetailSummary
    {
        public int SpendingPaymentDetailId { get; set; }
        public int SpendingPaymentId { get; set; }
        public int SpendingId { get; set; }
        public string ReceiptSpending { get; set; }
        public string DescSpending { get; set; }
        public double Amount { get; set; }
    }

    public class SpendingPaymentPaymentMethod
    {
        public int SpendingPaymentPaymentMethodId { get; set; }
        public int SpendingPaymentId { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public string Saving { get; set; }
        public string Cycle { get; set; }
        public int? ReconciledId { get; set; }
    }

    public enum SpendingPaymentType
    {
        SpendingPayment = 1,
        SallaryPayment = 2,
    }

}
