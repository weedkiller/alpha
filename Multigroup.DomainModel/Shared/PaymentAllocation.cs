using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class PaymentAllocation
    {
        public int PaymentAllocationId { get; set; }
        public int PaymentId { get; set; }
        public int InstallmentId { get; set; }
        public double amount { get; set; }
    }
}
