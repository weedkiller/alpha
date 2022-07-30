using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Administration
{

    public class SalaryAllocationFilterVm
    {
        public IEnumerable<string> SelectedUserAuth { get; set; }
        public IEnumerable<SelectListItem> ListUserAuth { get; set; }
        public IEnumerable<string> SelectedProvider { get; set; }
        public IEnumerable<SelectListItem> ListProvider { get; set; }
        public double AmounFrom { get; set; }
        public double AmountTo { get; set; }
        public bool Authorized { get; set; }
        public string Period { get; set; }
        public string Description { get; set; }
    }
    public class SalaryAllocationTableVm
    {
        public IEnumerable<SalaryAllocationListVm> SalaryAllocationList { get; set; }

        public static SalaryAllocationTableVm ToViewModel(IEnumerable<SalaryAllocationSummary> entities)
        {
            return new SalaryAllocationTableVm
            {
                SalaryAllocationList = SalaryAllocationListVm.ToViewModelList(entities),
            };
        }
    }

    public class SalaryAllocationListVm
    {
        public int SalaryAllocationId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Proveedor { get; set; }
        public string UserAuthorized { get; set; }
        public string Description { get; set; }
        public bool IsAuthorized { get; set; }
        public double Amount { get; set; }
        public string LastMonth { get; set; }
        public static IEnumerable<SalaryAllocationListVm> ToViewModelList(IEnumerable<SalaryAllocationSummary> entities)
        {
            return entities.Select(x => new SalaryAllocationListVm
            {
                SalaryAllocationId = x.SalaryAllocationId,
                StartDate = x.StartDate.ToShortDateString(),
                EndDate = x.EndDate.ToShortDateString(),
                UserAuthorized = x.UserAuthorized,
                IsAuthorized = x.IsAuthorized,
                Amount = x.Amount,
                Description = x.Description,
                Proveedor = x.Proveedor,
                LastMonth = x.LastMonth
            });
        }
    }
}