using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class ContractPaper
    {
        public int PaperContractId { get; set; }
        public int? User { get; set; }
        public int? Status { get; set; }
        public int Number { get; set; }

        public int ContractType { get; set; }
    }

    public class ContractPaperSummary
    {
        public int PaperContractId { get; set; }
        public int Number { get; set; }
        public string Status { get; set; }
        public string User { get; set; }

        public string ContractType { get; set; }
    }
}
