using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class PurchaseRequest
    {
        public int PurchaseRequestId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? NeedDate { get; set; }
        public int? ProveedorId { get; set; }
        public int RequestTypeId { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public int UrgencyId { get; set; }
        public bool Active { get; set; }
        public bool Processed { get; set; }
        public int UserAuthorizedId { get; set; }
        public bool Managed { get; set; }
        public IEnumerable<PurchaseRequestDetail> Details { get; set; }
    }

    public class PurchaseRequestSummary
    {
        public int PurchaseRequestId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? NeedDate { get; set; }
        public string Proveedor { get; set; }
        public string User { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public string Urgency { get; set; }
        public string Processed { get; set; }
        public bool Active { get; set; }
        public bool Managed { get; set; }
        public string UserAuthorized { get; set; }
        public int UserAuthorizedId { get; set; }
    }

    public class PurchaseRequestDetail
    {
        public int PurchaseRequestDetailId { get; set; }
        public int PurchaseRequestId { get; set; }
        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public string Article { get; set; }
        public string Description { get; set; }
    }

    public class Urgency
    {
        public int UrgencyId { get; set; }
        public string Description { get; set; }
    }
}
