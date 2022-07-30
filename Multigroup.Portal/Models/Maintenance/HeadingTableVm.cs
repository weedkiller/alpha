using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Maintenance
{
    public class HeadingTableVm
    {
        public IEnumerable<HeadingListVm> HeadingList { get; set; }

        public static HeadingTableVm ToViewModel(IEnumerable<Heading> entities)
        {
            return new HeadingTableVm
            {
                HeadingList = HeadingListVm.ToViewModelList(entities),
            };
        }
    }

    public class HeadingListVm
    {
        public int HeadingId { get; set; }
        public string Name { get; set; }
        public static IEnumerable<HeadingListVm> ToViewModelList(IEnumerable<Heading> entities)
        {
            return entities.Select(x => new HeadingListVm
            {
                HeadingId = x.HeadingId,
                Name = x.Name,
            });
        }
    }
}