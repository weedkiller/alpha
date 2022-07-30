using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Models.Shared;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Collection
{
    public class PaymentTableVm
    {
        public IEnumerable<PaymentListVm> PaymentList { get; set; }

        public static PaymentTableVm ToViewModel(IEnumerable<Payment> entities)
        {
            return new PaymentTableVm
            {
                PaymentList = PaymentListVm.ToViewModelList(entities),
            };
        }
    }

    public class PaymentListVm
    {
        public int PaymentId { get; set; }
        public string PaymentDate { get; set; }
        public string type { get; set; }
        public string PaymentMethod { get; set; }
        public string Observations { get; set; }
        public string ReceipNumber { get; set; }
        public double Amount { get; set; }
        public int ContractNumber { get; set; }
        public static IEnumerable<PaymentListVm> ToViewModelList(IEnumerable<Payment> entities)
        {
            return entities.Select(x => new PaymentListVm
            {
                PaymentId = x.PaymentId,
                PaymentDate = Util.GetFrenchFormatDate(x.PaymentDate),
                type = x.type,
                PaymentMethod = x.PaymentMethod,
                Observations = x.Observations,
                ReceipNumber = x.ReceipNumber,
                Amount = x.Amount,
                ContractNumber = x.ContractNumber,
            });
        }
    }
}