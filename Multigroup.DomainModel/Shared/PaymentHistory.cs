using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class PaymentHistory
    {
        public int Number { get; set; }
        public string Customer { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public string InstallmentNumber { get; set; }
        public string Seller { get; set; }
        public string Telemarketer { get; set; }
        public string Supervisor { get; set; }
        public string Amount { get; set; }
    }
}
