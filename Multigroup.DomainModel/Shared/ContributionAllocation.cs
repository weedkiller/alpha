using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class ContributionAllocation
    {
        public int ContributionAllocationId { get; set; }
        public DateTime Date { get; set; }
        public DateTime SystemDate { get; set; }
        public string Concept { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public IEnumerable<ContributionAllocationPartner> Partners { get; set; }
    }

    public class ContributionAllocationSummary
    {
        public int ContributionAllocationId { get; set; }
        public DateTime Date { get; set; }
        public DateTime SystemDate { get; set; }
        public string Concept { get; set; }
        public string User { get; set; }
        public double Amount { get; set; }
    }

    public class ContributionAllocationPartner
    {
        public int ContributionAllocationPartnerId { get; set; }
        public int ContributionAllocationId { get; set; }
        public int PartnerId { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public string Partner { get; set; }
        public double Percentage { get; set; }
        public string Concept { get; set; }
    }
}
