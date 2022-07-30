using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class ExpenseAuthorization
    {
        public int ExpenseAuthorizationId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public int? ProveedorId { get; set; }
        public int PurchaseOrderTypeId { get; set; }
        public int UserId { get; set; }
        public int? PurchaseRequestId { get; set; }
        public int? ScheduledExpenseId { get; set; }
        public int UserBuyerId { get; set; }
        public bool IsAuthorized { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public IEnumerable<ExpenseAuthorizationDetail> Details { get; set; }
        public bool Processed { get; set; }
        public bool Managed { get; set; }
        public bool Active { get; set; }
    }

    public class ExpenseAuthorizationSummary
    {
        public int ExpenseAuthorizationId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string Proveedor { get; set; }
        public string User { get; set; }
        public string UserBuyer { get; set; }
        public int UserBuyerId { get; set; }
        public int? PurchaseRequestId { get; set; }
        public int? ScheduledExpenseId { get; set; }
        public bool IsAuthorized { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public string Processed { get; set; }
        public bool Managed { get; set; }
        public bool Active { get; set; }
    }

    public class ExpenseAuthorizationDetail
    {
        public int ExpenseAuthorizationDetailId { get; set; }
        public int ExpenseAuthorizationId { get; set; }
        public ExpenseAuthorization ExpenseAuthorization { get; set; }
        public int ArticleId { get; set; }
        public string Article { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public string Description { get; set; }
    }
}
