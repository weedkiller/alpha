using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Maintenance
{
    public class MercadoPagoTableVm
    {
        public IEnumerable<MercadoPagoListVm> PaymentMethodsList { get; set; }

        public static MercadoPagoTableVm ToViewModel(IEnumerable<MPSellerAccountSummary> entities)
        {
            return new MercadoPagoTableVm
            {
                PaymentMethodsList = MercadoPagoListVm.ToViewModelList(entities),
            };
        }
    }

    public class MercadoPagoListVm
    {
        public int MPSellerAccountId { get; set; }
        public string MPAccountId { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Pass { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public string Token { get; set; }
        public static IEnumerable<MercadoPagoListVm> ToViewModelList(IEnumerable<MPSellerAccountSummary> entities)
        {
            return entities.Select(x => new MercadoPagoListVm
            {
                MPSellerAccountId = x.MPSellerAccountId,
                MPAccountId = x.MPAccountId,
                UserName = x.UserName,
                MaxAmount = x.MaxAmount,
                MinAmount = x.MinAmount,
                Token = x.Token,
                UserId = x.UserId,
            });
        }
    }
}