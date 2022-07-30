using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Collection
{
    public class CollectionsResumeTableVm
    {
        public IEnumerable<CollectionsResumeListVm> CollectionsList { get; set; }

        public static CollectionsResumeTableVm ToViewModel(IEnumerable<PaymentResume> entities)
        {
            return new CollectionsResumeTableVm
            {
                CollectionsList = CollectionsResumeListVm.ToViewModelList(entities),
            };
        }
    }

    public class CollectionsResumeListVm
    {
        public string Description { get; set; }
        public double Value { get; set; }

        public static IEnumerable<CollectionsResumeListVm> ToViewModelList(IEnumerable<PaymentResume> entities)
        {
            return entities.Select(x => new CollectionsResumeListVm
            {
                Value = x.Value,
                Description = x.Description
            });
        }
    }
}