using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class MovementType
    {
        public int MovementTypeId { get; set; }
        public string Description { get; set; }
    }

    public class Verified
    {
        public int VerifiedId { get; set; }
        public string Description { get; set; }
    }

    public class MovementResume
    {
        public int MovementId { get; set; }
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Movement { get; set; }
        public string Cashier { get; set; }
        public string Cycle { get; set; }
        public DateTime SystemDate { get; set; }
        public string User { get; set; }
        public string Verified { get; set; }
    }

    public class MovementBalance
    {
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public DateTime? ConciliationDate { get; set; }
        public string User { get; set; }
    }
}
