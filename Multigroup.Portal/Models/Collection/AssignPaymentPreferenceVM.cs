using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Enums;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Multigroup.Portal.Models.Shared;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Collection
{
    public class AssignPaymentPreferenceFiltersVm
    {
        public IEnumerable<string> SelectedAssignState { get; set; }
        public IEnumerable<SelectListItem> ListAssignState { get; set; }
        public IEnumerable<string> SelectedPaymentPreference { get; set; }
        public IEnumerable<SelectListItem> ListPaymentPreference { get; set; }
        public IEnumerable<string> SelectedPaymentMethod { get; set; }
        public IEnumerable<SelectListItem> ListPaymentMethod { get; set; }
        public IEnumerable<string> SelectedCustomers { get; set; }
        public IEnumerable<SelectListItem> ListCustomers { get; set; }
        public IEnumerable<string> SelectedUser { get; set; }
        public IEnumerable<SelectListItem> ListUser { get; set; }
        public IEnumerable<string> SelectedCollector { get; set; }
        public IEnumerable<SelectListItem> ListCollector { get; set; }
        public IEnumerable<string> SelectedZone { get; set; }
        public IEnumerable<SelectListItem> ListZone { get; set; }
        public int SelectedCustomer { get; set; }
        public int InstallmentNumber { get; set; }
        public bool Processed { get; set; }
        public bool Advised { get; set; }
        public bool Pending { get; set; }
        public bool Register { get; set; }
        public bool Surrendered { get; set; }
        public bool Assigned { get; set; }
    }
    public class AssignPaymentPreferenceVm : BaseVm
    {
        public int AssignPaymentPreferenceId { get; set; }
        public int InstallmentId { get; set; }
        public SingleSelectVm ddlPaymentPreference { get; set; }
        public string Observations { get; set; }
        public string Follow { get; set; }
        public string FollowDate { get; set; }
        public int UserId { get; set; }
        public SingleSelectVm ddlPaymentMethod { get; set; }
        public string AssignPaymentMethodDate { get; set; }
        public string Channel { get; set; }
        public bool Warned { get; set; }
        public string WarnedDate { get; set; }
        public string WarnedCommentary { get; set; }
        public int AssignStateId { get; set; }
        public double Amount { get; set; }
        public string PaymentDate { get; set; }

        public AssignPaymentPreference ToModelObject()
        {
            return new AssignPaymentPreference
            {
                AssignPaymentPreferenceId = AssignPaymentPreferenceId,
                InstallmentId = InstallmentId,
                PaymentPreferenceId = GetNullableValue(ddlPaymentPreference),
                Observations = Observations,
                Follow = Follow,
                FollowDate = Util.ParseDateTime(FollowDate),
                UserId = UserId,
                PaymentMethodId = GetNullableValue(ddlPaymentMethod),
                AssignPaymentMethodDate = Util.ParseDateTime(AssignPaymentMethodDate),
                Channel = Channel,
                Warned = Warned,
                WarnedDate = Util.ParseDateTime(WarnedDate),
                WarnedCommentary = WarnedCommentary,
                AssignStateId = AssignStateId,
                PaymentDate = Util.ParseDateTime(PaymentDate),
                Amount = Amount
            };
        }

        public static AssignPaymentPreferenceVm FromDomainModel(AssignPaymentPreference entity)
        {
            return new AssignPaymentPreferenceVm
            {
                AssignPaymentPreferenceId = entity.AssignPaymentPreferenceId,
                InstallmentId = entity.InstallmentId,
                Observations = entity.Observations,
                Follow = entity.Follow,
                FollowDate = Util.GetFrenchFormatDate(entity.FollowDate),
                AssignPaymentMethodDate = Util.GetFrenchFormatDate(entity.AssignPaymentMethodDate),
                WarnedDate = Util.GetFrenchFormatDate(entity.WarnedDate),
                UserId = entity.UserId,
                Channel = entity.Channel,
                Warned = entity.Warned,
                WarnedCommentary = entity.WarnedCommentary,
                AssignStateId = entity.AssignStateId,
                PaymentDate = Util.GetFrenchFormatDate(entity.PaymentDate),
                Amount = entity.Amount
            };
        }
    }
}