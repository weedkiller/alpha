using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class Promissory
    {
        public int PromissoryId { get; set; }
        public int Number { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public int PercentageDefinitionId { get; set; }
        public DateTime BroadcastDate { get; set; }
        public DateTime CollectionDate { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public string Description { get; set; }
        public bool isPaid { get; set; }
    }

    public class PromissorySummary
    {
        public int PromissoryId { get; set; }
        public int Number { get; set; }
        public string Client { get; set; }
        public string User { get; set; }
        public DateTime BroadcastDate { get; set; }
        public DateTime CollectionDate { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public string Description { get; set; }
        public string isPaid { get; set; }
    }

    public class PromissoryPartner
    {
        public int PromissoryPartnerId { get; set; }
        public int PromissoryId { get; set; }
        public int PartnerId { get; set; }
        public double Amount { get; set; }
    }

    public class PromissoryPayment
    {
        public int PromissoryPaymentId { get; set; }
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime SystemDate { get; set; }
        public double Amount { get; set; }
        public IEnumerable<PromissoryPaymentMethod> PaymentMethods { get; set; }
        public IEnumerable<PromissoryPaymentPromissory> Promissories { get; set; }
        public IEnumerable<Promissory> PromissoriesAll { get; set; }
    }

    public class PromissoryPaymentSummary
    {
        public int PromissoryPaymentId { get; set; }
        public string Client { get; set; }
        public string User { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime SystemDate { get; set; }
        public double Amount { get; set; }
    }

    public class PromissoryPaymentMethod
    {
        public int PromissoryPaymentMethodId { get; set; }
        public int PromissoryPaymentId { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public string Saving { get; set; }
        public string Cycle { get; set; }
    }

    public class PromissoryPaymentPromissory
    {
        public int PromissoryPaymentPromissoryId { get; set; }
        public int PromissoryId { get; set; }
        public int PromissoryPaymentId { get; set; }
    }

    public class PromissorySurrenderPartner
    {
        public int PromissorySurrenderPartnerId { get; set; }
        public int PromissoryPaymentId { get; set; }
        public int PromissoryId { get; set; }
        public int PartnerId { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; }
    }

    public class PromissorySurrender
    {
        public int PromissorySurrenderId { get; set; }
        public int UserId { get; set; }
        public int PartnerId { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime SystemDate { get; set; }
        public double Amount { get; set; }
        public IEnumerable<PromissorySurrenderMethod> PaymentMethods { get; set; }
        public IEnumerable<PromissoryRendered> rendereds { get; set; }
        public IEnumerable<Promissory> PromissoriesAll { get; set; }
    }

    public class PromissorySurrenderPartnerSummary
    {
        public int PromissorySurrenderPartnerId { get; set; }
        public int PromissorySurrenderId { get; set; }
        public int PromissoryPaymentId { get; set; }
        public int Number { get; set; }
        public string Partner { get; set; }
        public string User { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    public class PromissorySurrenderMethod
    {
        public int PromissorySurrenderMethodId { get; set; }
        public int PromissorySurrenderId { get; set; }
        public int PaymentMethodId { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Saving { get; set; }
        public string Cycle { get; set; }
    }

    public class PromissoryRendered
    {
        public int PromissoryRenderedId { get; set; }
        public int PromissorySurrenderId { get; set; }
        public int PromissoryId { get; set; }
    }
}
