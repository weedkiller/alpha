using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class Receip
    {
        public int ReceipId { get; set; }
        public int? User { get; set; }
        public int? Status { get; set; }
        public string Number { get; set; }
    }

    public class ReceipSummary
    {
        public int ReceipId { get; set; }
        public long Number { get; set; }
        public string Status { get; set; }
        public string User { get; set; }
        
    }
}
