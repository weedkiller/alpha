using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class Partner
    {
        public int PartnerId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public double Percentage { get; set; }
    }

    public class PercentageDefinition
    {
        public int PercentageDefinitionId { get; set; }
        public string Name { get; set; }
        public IEnumerable<PercentageDefinitionPartner> Partners { get; set; }
    }

    public class PercentageDefinitionPartner
    {
        public int PercentageDefinitionPartnerId { get; set; }
        public int PercentageDefinitionId { get; set; }
        public int PartnerId { get; set; }
        public double Percentage { get; set; }
        public string Name { get; set; }
    }

    public class PercentageDefinitionPartnerSummary
    {
        public int PercentageDefinitionPartnerId { get; set; }
        public int PercentageDefinitionId { get; set; }
        public int PartnerId { get; set; }
        public double Percentage { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
    }

    public class BalancePartner
    {
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
