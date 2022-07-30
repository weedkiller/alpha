using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{

    public class IncomePartner
    {
        public int IncomePartnerId { get; set; }
        public int PartnerId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public DateTime SystemDate { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public IEnumerable<IncomePartnerPaymentMethod> PaymentMethods { get; set; }
        public IEnumerable<IncomePartnerAllocation> Allocations { get; set; }
        public IEnumerable<ContributionAllocationPartner> AllocationsAll { get; set; }
    }

    public class IncomePartnerSummary
    {
        public int IncomePartnerId { get; set; }
        public string Partner { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }
        public DateTime SystemDate { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
    }

    public class IncomePartnerPaymentMethod
    {
        public int IncomePartnerPaymentMethodId { get; set; }
        public int IncomePartnerId { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public string Saving { get; set; }
        public string Cycle { get; set; }
    }

    public class IncomePartnerAllocation
    {
        public int IncomePartnerAllocationId { get; set; }
        public int ContributionAllocationPartnerId { get; set; }
        public int IncomePartnerId { get; set; }
        public double Amount { get; set; }
    }
    
}
