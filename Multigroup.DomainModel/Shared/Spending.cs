using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class Spending
    {
        public int SpendingId { get; set; }
        public DateTime ExecutionDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Receipt { get; set; }
        public int? ProveedorId { get; set; }
        public int SpendingTypeId { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public string Commentary { get; set; }
        public string Description { get; set; }
        public string Period { get; set; }
        public IEnumerable<SpendingDetail> Details { get; set; }
        public IEnumerable<SpendingPaymentMethod> PaymentMethods { get; set; }
    }

    public class SpendingSummary
    {
        public int SpendingId { get; set; }
        public DateTime ExecutionDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Receipt { get; set; }
        public string Proveedor { get; set; }
        public string User { get; set; }
        public double Balance { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public string Description { get; set; }
        public string Period { get; set; }
        public int UserId { get; set; }
    }

    public class SpendingDetail
    {
        public int SpendingDetailId { get; set; }
        public int SpendingId { get; set; }
        public Spending Spending { get; set; }
        public int ArticleId { get; set; }
        public int? ScheduledExpenseId { get; set; }
        public int? PurchaseRequestId { get; set; }
        public int? ExpenseAuthorizationId { get; set; }
        public string Article { get; set; }
        public string Origin { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public string Description { get; set; }
        public int? SalaryImputationId { get; set; }
    }

    public class SpendingDetailSummary
    {
        public int SpendingDetailId { get; set; }
        public int SpendingId { get; set; }
        public string Article { get; set; }
        public string Origin { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public string Description { get; set; }
        public string Receipt { get; set; }
        public int? SalaryImputationId { get; set; }
    }
    public class SpendingPaymentMethod
    {
        public int SpendingPaymentMethodId { get; set; }
        public int SpendingId { get; set; }
        public Spending Spending { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public string Saving { get; set; }
        public string Cycle { get; set; }
        public string ReconciledId { get; set; }
    }

    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int SpendingId { get; set; }
        public string CUIT { get; set; }
        public string BusinessName { get; set; }
        public int IvaPositionId { get; set; }
        public int PurchaseDocumentId { get; set; }
        public int PurchasePerceptionId { get; set; }
        public double? PerceptionAmount { get; set; }
        public string Letter { get; set; }
        public int Prefix { get; set; }
        public int Number { get; set; }
        public DateTime ImputDate { get; set; }
        public double? Net { get; set; }
        public double? Exempt { get; set; }
        public double? IVA21 { get; set; }
        public double? IVA105 { get; set; }
        public double? IVA22 { get; set; }
        public double? IVA5 { get; set; }
        public double? IVA25 { get; set; }
        public double? OtherTaxes { get; set; }
    }

    public enum ArticlesEspecial
    {
        Salary = -1,
    }

    public enum SpendingType
    {
        Salary = 2,
        Spending = 1
    }
}
