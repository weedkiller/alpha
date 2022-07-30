using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Maintenance
{
    public class UserTypeTableVm
    {
        public IEnumerable<UserTypeListVm> UserTypeList { get; set; }

        public static UserTypeTableVm ToViewModel(IEnumerable<UserType> entities)
        {
            return new UserTypeTableVm
            {
                UserTypeList = UserTypeListVm.ToViewModelList(entities),
            };
        }
    }

    public class UserTypeListVm
    {
        public int UserTypeId { get; set; }
        public string Description { get; set; }
        public bool IsCommission { get; set; }
        public double Commission { get; set; }
        public static IEnumerable<UserTypeListVm> ToViewModelList(IEnumerable<UserType> entities)
        {
            return entities.Select(x => new UserTypeListVm
            {
                UserTypeId = x.UserTypeId,
                Description = x.Description,
                IsCommission = x.IsCommission,
                Commission = x.Commission,
            });
        }
    }
}