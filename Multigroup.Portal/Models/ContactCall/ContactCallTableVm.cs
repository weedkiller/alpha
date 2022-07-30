using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Enums;
using Multigroup.Portal.Models.Shared;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.ContactCallModel
{
    public class ContactCallFilterVm
    {
        public IEnumerable<string> SelectedManager { get; set; }
        public IEnumerable<SelectListItem> ListManager { get; set; }
        public IEnumerable<string> SelectedSupervisor { get; set; }
        public IEnumerable<SelectListItem> ListSupervisor { get; set; }
        public IEnumerable<string> SelectedSeller { get; set; }
        public IEnumerable<SelectListItem> ListSeller { get; set; }
        public IEnumerable<string> SelectedStatus { get; set; }
        public IEnumerable<SelectListItem> ListStatus { get; set; }
        public IEnumerable<string> SelectedClient { get; set; }
        public IEnumerable<SelectListItem> ListClient { get; set; }
        public SingleSelectVm ddlManager { get; set; }
        public SingleSelectVm ddlSeller { get; set; }
        public SingleSelectVm ddlSupervisor { get; set; }

    }
    public class ContactCallTableVm
    {
        public IEnumerable<ContactCallListVm> ContactCallList { get; set; }

        public static ContactCallTableVm ToViewModel(IEnumerable<ContactCallSummary> entities)
        {
            return new ContactCallTableVm
            {
                ContactCallList = ContactCallListVm.ToViewModelList(entities),
            };
        }
    }

    public class ContactCallListVm
    {
        public int ContactCallId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Vehicle { get; set; }
        public string SystemDate { get; set; }
        public string ContactDate { get; set; }
        public string ContactTime { get; set; }
        public string Commentary { get; set; }
        public int ContactCallStateId { get; set; }
        public string ContactCallState { get; set; }
        public string Color { get; set; }
        public int? ManagerId { get; set; }
        public string Manager { get; set; }
        public int? SupervisorId { get; set; }
        public string Supervisor { get; set; }
        public int? SellerId { get; set; }
        public string Seller { get; set; }

        public static IEnumerable<ContactCallListVm> ToViewModelList(IEnumerable<ContactCallSummary> entities)
        {
            return entities.Select(x => new ContactCallListVm
            {
                ContactCallId = x.ContactCallId,
                Name = x.Name,
                Surname = x.Surname,
                Phone = x.Phone,
                Vehicle = x.Vehicle,
                ContactDate = Util.GetFrenchFormatDate(x.ContactDate),
                SystemDate = (x.SystemDate == null) ? "" : Util.GetFrenchFormatDate(x.SystemDate),
                ContactTime = x.ContactTime,
                Commentary = x.Commentary,
                ContactCallStateId = x.ContactCallStateId,
                ContactCallState = x.ContactCallState,
                Color = x.Color,
                Manager = x.Manager,
                ManagerId = x.ManagerId,
                SupervisorId = x.SupervisorId,
                Supervisor = x.Supervisor,
                SellerId = x.SellerId,
                Seller = x.Seller,
                });
        }
    }
}