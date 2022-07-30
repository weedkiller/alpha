using Multigroup.DomainModel;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Widgets
{
    public class MPWidgetTableVm
    {
        public bool IncludedAll { get; set; }
        public IEnumerable<MPWidgetListVm> MPWidgetList { get; set; }

        public static MPWidgetTableVm ToViewModel(IEnumerable<MPSellerAccountSummary> entities)
        {
            return new MPWidgetTableVm
            {
                MPWidgetList = MPWidgetListVm.ToViewModelList(entities),
            };
        }
    }

    public class MPWidgetListVm
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string AccountNumber { get; set; }
        public double Amount { get; set; }
        public string Color { get; set; }
        public string Token { get; set; }
        public static IEnumerable<MPWidgetListVm> ToViewModelList(IEnumerable<MPSellerAccountSummary> entities)
        {
            return entities.Select(x => new MPWidgetListVm
            {
                Id = x.MPSellerAccountId,
                UserId = x.UserName,
                AccountNumber = x.MPAccountId,
                Amount = x.Amount,
                Color = x.Color,
                Token = x.Token
            });
        }
    }
}