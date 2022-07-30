using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Maintenance
{
    public class AgencyTableVm
    {
        public IEnumerable<AgencyListVm> AgencyList { get; set; }

        public static AgencyTableVm ToViewModel(IEnumerable<Agency> entities)
        {
            return new AgencyTableVm
            {
                AgencyList = AgencyListVm.ToViewModelList(entities),
            };
        }
    }

    public class AgencyListVm
    {
        public int AgencyId { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public static IEnumerable<AgencyListVm> ToViewModelList(IEnumerable<Agency> entities)
        {
            return entities.Select(x => new AgencyListVm
            {
                AgencyId = x.AgencyId,
                Name = x.Description,
                Adress = x.Adress,
                Phone = x.Phone,
            });
        }
    }
}