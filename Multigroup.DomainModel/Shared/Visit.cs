using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class Visit
    {
        public int VisitId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? ScheduledVisitDate { get; set; }
        public DateTime? ConcretedVisitDate { get; set; }
        public int CollectorId { get; set; }
        public double CollectorComission { get; set; }
        public double AmountOwed { get; set; }
        public double? AmountCollected { get; set; }
        public double? TotalComission { get; set; }
        public int Status { get; set; }
        public string CollectorExpensesDesc { get; set; }
        public double? CollectorExpensesAmount { get; set; }
        public string Observations { get; set; }
    }

    public class VisitSummary
    {
        public int VisitId { get; set; }
        public string Customer { get; set; }
        public DateTime? ScheduledVisitDate { get; set; }
        public DateTime? ConcretedVisitDate { get; set; }
        public string Collector { get; set; }
        public double CollectorComission { get; set; }
        public double AmountOwed { get; set; }
        public double? AmountCollected { get; set; }
        public double? TotalComission { get; set; }
        public string Status { get; set; }
        public string CollectorExpensesDesc { get; set; }
        public double CollectorExpensesAmount { get; set; }
        public string Observations { get; set; }
    }

    public class AgencyPayment
    {
        public int AgencyPaymentId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? ScheduledPaymentDate { get; set; }
        public string Observations { get; set; }
    }


    public class AgencyPaymentHistory
    {
        public int AgencyPaymentHistoryId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? ScheduledPaymentDate { get; set; }
        public DateTime? Date { get; set; }
        public string Observations { get; set; }
        public string User { get; set; }
    }
}
