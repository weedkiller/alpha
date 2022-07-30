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

namespace Multigroup.Portal.Models.Administration
{
    public class PaymentMethodReconciledVm : BaseVm
    {
        public int PaymentMethodReconciledId { get; set; }
        public int PaymentMethodId { get; set; }
        public int UserId { get; set; }
        public string ReconciledDate { get; set; }

        public string Time { get; set; }
        public string SystemDate { get; set; }
        public double Amount { get; set; }
        public string Commentary { get; set; }
        public string PaymentMethod { get; set; }
        public string SystemTime { get; set; }

        public PaymentMethodReconciled ToModelObject()
        {
            return new PaymentMethodReconciled
            {
                ReconciledDate = Convert.ToDateTime(SystemDate),
                PaymentMethodId = PaymentMethodId,
                Amount = Amount,
                Commentary = Commentary,
            };
        }

        public static PaymentMethodReconciledVm FromDomainModel(PaymentMethodReconciledSummary entity)
        {
            return new PaymentMethodReconciledVm
            {
                PaymentMethod = entity.PaymentMethod,
                Commentary = entity.Commentary,
                Time = entity.Time,
                ReconciledDate = entity.ReconciledDate.ToShortDateString(),
                SystemDate = DateTime.Now.ToShortDateString(),
                SystemTime = DateTime.Now.ToString("HH:mm"),
        };
        }
    }
}