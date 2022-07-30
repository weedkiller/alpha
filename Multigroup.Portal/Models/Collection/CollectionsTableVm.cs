using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Multigroup.Portal.Utilities;

namespace Multigroup.Portal.Models.Collection
{
    public class CollectionsTableVm
    {
        public IEnumerable<CollectionsListVm> CollectionsList { get; set; }

        public static CollectionsTableVm ToViewModel(IEnumerable<PaymentSummary> entities)
        {
            return new CollectionsTableVm
            {
                CollectionsList = CollectionsListVm.ToViewModelList(entities),
            };
        }
    }

    public class CollectionsListVm
    {
        public int PaymentId { get; set; }
        public string PaymentDate { get; set; }
        public string type { get; set; }
        public string PaymentMethod { get; set; }
        public string Observations { get; set; }
        public string ReceipNumber { get; set; }
        public double Amount { get; set; }
        public int ContractNumber { get; set; }
        public int? MPSellerAccountId { get; set; }
        public string Customer { get; set; }
        public string DNI { get; set; }
        public string User { get; set; }


        public static IEnumerable<CollectionsListVm> ToViewModelList(IEnumerable<PaymentSummary> entities)
        {
            return entities.Select(x => new CollectionsListVm
            {
                PaymentId = x.PaymentId,
                PaymentDate = Util.GetFrenchFormatDate(x.PaymentDate),
                type = x.type,
                PaymentMethod = x.PaymentMethod,
                Observations = x.Observations,
                ReceipNumber = x.ReceipNumber,
                Amount = x.Amount,
                ContractNumber = x.ContractNumber,
            Customer = x.Customer,
            DNI = x.DNI,
            User = x.User
            });
        }
    }
}