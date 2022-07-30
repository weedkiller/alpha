using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Collection
{
    public class AssignVisitFilterVm
    {
        public IEnumerable<string> SelectedZones { get; set; }
        public IEnumerable<SelectListItem> ListZones { get; set; }
        public SingleSelectVm ddlPaymentDate { get; set; }
        public SingleSelectVm ddlStatus { get; set; }

    }
    public class AssignVisitTableVm
    {
        public IEnumerable<AssignVisitListVm> AssignVisitList { get; set; }

        public static AssignVisitTableVm ToViewModel(IEnumerable<AssignVisit> entities)
        {
            return new AssignVisitTableVm
            {
                AssignVisitList = AssignVisitListVm.ToViewModelList(entities),
            };
        }
    }

    public class AssignVisitListVm
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactHours { get; set; }
        public string StatusClient { get; set; }
        public string Zone { get; set; }
        public string AmountOwed { get; set; }
        public string InstallmentNumber { get; set; }
        public string InstallmentsOwed { get; set; }
        public int ZoneId { get; set; }
        public double CommissionRate { get; set; }
        public int PaymentPreference { get; set; }
        public string ScheduledPaymentDate { get; set; }
        public string Observations { get; set; }
        public string StatusInstallent { get; set; }
        public int Number { get; set; }
        public string Color { get; set; }       


        public static IEnumerable<AssignVisitListVm> ToViewModelList(IEnumerable<AssignVisit> entities)
        {
            return entities.Select(x => new AssignVisitListVm
            {
                CustomerId = x.CustomerId,
                Name = x.Name + " " + x.Surname,
                Address = x.Address + " - " + x.City,
                Phone = x.Phone + " - " + x.CellPhone,
                Email = x.Email,
                ContactHours = x.ContactHours,
                Zone = x.Zone,
                StatusClient = x.StatusClient,
                InstallmentNumber = x.InstallmentNumber,
                AmountOwed = x.AmountOwed,
                InstallmentsOwed = x.InstallmentsOwed,
                CommissionRate = x.CommissionRate,
                ZoneId = x.ZoneId,
                PaymentPreference = x.PaymentPreference,
                Observations = x.Observations,
                ScheduledPaymentDate = x.ScheduledPaymentDate,
                StatusInstallent = x.StatusInstallent,
                Number = x.Number,
                Color = x.Color,
            });
        }
    }
}