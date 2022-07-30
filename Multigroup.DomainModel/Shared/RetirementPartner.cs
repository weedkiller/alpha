using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{

    public class RetirementPartner
    {
        public int RetirementPartnerId { get; set; }
        public int PartnerId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime SystemDate { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public IEnumerable<RetirementPartnerPaymentMethod> PaymentMethods { get; set; }
        public IEnumerable<RetirementPartnerAllocation> Allocations { get; set; }
        public IEnumerable<EarningsAllocationPartner> AllocationsAll { get; set; }
    }

    public class RetirementPartnerSummary
    {
        public int RetirementPartnerId { get; set; }
        public string Partner { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }
        public DateTime SystemDate { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
    }

    public class RetirementPartnerPaymentMethod
    {
        public int RetirementPartnerPaymentMethodId { get; set; }
        public int RetirementPartnerId { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public string Saving { get; set; }
        public string Cycle { get; set; }
    }

    public class RetirementPartnerAllocation
    {
        public int RetirementPartnerAllocationId { get; set; }
        public int EarningsAllocationPartnerId { get; set; }
        public int RetirementPartnerId { get; set; }
        public double Amount { get; set; }
    }
    
}
