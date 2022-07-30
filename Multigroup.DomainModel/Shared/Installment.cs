using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class Installment
    {
        public int InstallmentId { get; set; }
        public int ContractId { get; set; }
        public int Number { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime DueDate { get; set; }
        public double OriginalAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; }

    }

    public class InstallmentInsterests
    {
        public int InstallmentInterestsId { get; set; }
        public int ContractId { get; set; }
        public int InstallmentId { get; set; }
        public int PaymentId { get; set; }
        public double Amount { get; set; }

    }

    public class InstallmentInsterestsSummary
    {
        public int InstallmentInterestsId { get; set; }
        public int ContractId { get; set; }
        public int InstallmentId { get; set; }
        public int PaymentId { get; set; }
        public double Amount { get; set; }
        public DateTime InitialDate { get; set; }
        public int Number { get; set; }
    }

    public class InstallmentReminderSummary
    {
        public int InstallmentId { get; set; }
        public int Number { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime DueDate { get; set; }
        public double OriginalAmount { get; set; }
        public int ContractId { get; set; }
        public int ContractNumber { get; set; }
        public String CustomerName { get; set; }
        public String CustomerSurname { get; set; }
        public String CustomerEmail { get; set; }
        public int PaymentPreference { get; set; }


    }
}
