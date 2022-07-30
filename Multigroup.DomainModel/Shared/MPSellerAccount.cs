using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class MPSellerAccount
    {
        public int MPSellerAccountId { get; set; }
        public string MPAccountId { get; set; }
        public int UserId { get; set; }
        public string Pass { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public string Token { get; set; }
    }

    public class MPPaymentInstallment
    {
        public int MPPaymentInstallmentId { get; set; }
        public int InstallmentId { get; set; }
        public int MPSellerAccountId { get; set; }
        public double amount { get; set; }
        public DateTime date { get; set; }
    }

    public class MPSellerAccountSummary
    {
        public int MPSellerAccountId { get; set; }
        public string MPAccountId { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Pass { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public string Token { get; set; }
        public double Amount { get; set; }
        public string Color { get; set; }
    }
}
