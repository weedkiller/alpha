using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class Cashier
    {
        public int CashierId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool Individual { get; set; }
    }

    public class TransferCashierPaymentMethod
    {
        public int TransferCashierPaymentMethodId { get; set; }
        public int TransferCashierId { get; set; }
        public int PaymentMethodId { get; set; }
        public string Saving { get; set; }
        public string Cycle { get; set; }
        public bool Entry { get; set; }
        public double Amount { get; set; }
    }

    public class TransferCashier
    {
        public int TransferCashierId { get; set; }
        public int OriginCashierId { get; set; }
        public int DestinyCashierId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
    }
}
