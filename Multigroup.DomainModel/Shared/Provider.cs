using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{

    public class ProviderType
    {
        public int ProviderTypeId { get; set; }
        public string Name { get; set; }
    }

    public class Provider
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Commentary { get; set; }
        public ProviderType ProviderType { get; set; }
        public int ProviderTypeId { get; set; }
        public bool IsEmployee { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public double Balance { get; set; }
        public DateTime? Date { get; set; }
        public bool Active { get; set; }
        public int IVAPositionId { get; set; }
        public string BusinessName { get; set; }
        public string CUIT { get; set; }
    }

    public class ProviderSummary
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Commentary { get; set; }
        public string ProviderType { get; set; }
        public double Balance { get; set; }
        public DateTime Date { get; set; }
        public bool Active { get; set; }
        public string IVAPosition { get; set; }
        public string BusinessName { get; set; }
        public string CUIT { get; set; }
    }

    public class EmployeeCurrentAcount
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime SpendingDate { get; set; }
        public double BalanceNotImput { get; set; }

    }

    public class EmployeeCurrentAcountDetail
    {
        public int MovementId { get; set; }
        public string Movement { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public double BalanceNotImput { get; set; }

    }

    public class ProviderCurrentAcount
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime SpendingDate { get; set; }
        public double BalanceNotImput { get; set; }

    }

    public class ProviderCurrentAcountDetail
    {
        public int MovementId { get; set; }
        public string Movement { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public double BalanceNotImput { get; set; }

    }

    public class IVAPosition
    {
        public int IVAPositionId { get; set; }
        public string Description { get; set; }
    }
}
