using Multigroup.DomainModel;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Shared
{
    public class RoleTableVm
    {
        public bool IncludedAll { get; set; }
        public IEnumerable<RoleListVm> RoleList { get; set; }

        public static RoleTableVm ToViewModel(IEnumerable<Role> entities)
        {
            return new RoleTableVm
            {
                RoleList = RoleListVm.ToViewModelList(entities),
            };
        }
    }

    public class RoleListVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public static IEnumerable<RoleListVm> ToViewModelList(IEnumerable<Role> entities)
        {
            return entities.Select(x => new RoleListVm
            {
                Id = x.RoleId,
                Name = x.Name,
                Description = x.Description
            });
        }
    }
}