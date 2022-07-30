using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Maintenance
{
    public class PartnerTableVm
    {
        public IEnumerable<PartnerListVm> PartnerList { get; set; }

        public static PartnerTableVm ToViewModel(IEnumerable<Partner> entities)
        {
            return new PartnerTableVm
            {
                PartnerList = PartnerListVm.ToViewModelList(entities),
            };
        }
    }

    public class PartnerListVm
    {
        public int PartnerId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public double Percentage { get; set; }
        public static IEnumerable<PartnerListVm> ToViewModelList(IEnumerable<Partner> entities)
        {
            return entities.Select(x => new PartnerListVm
            {
                PartnerId = x.PartnerId,
                Name = x.Name,
                Active = x.Active,
                Percentage = x.Percentage,
            });
        }
    }

    public class PercentageDefinitionTableVm
    {
        public IEnumerable<PercentageDefinitionListVm> PercentageDefinitionList { get; set; }

        public static PercentageDefinitionTableVm ToViewModel(IEnumerable<PercentageDefinition> entities)
        {
            return new PercentageDefinitionTableVm
            {
                PercentageDefinitionList = PercentageDefinitionListVm.ToViewModelList(entities),
            };
        }
    }

    public class PercentageDefinitionListVm
    {
        public int PercentageDefinitionId { get; set; }
        public string Name { get; set; }
        public static IEnumerable<PercentageDefinitionListVm> ToViewModelList(IEnumerable<PercentageDefinition> entities)
        {
            return entities.Select(x => new PercentageDefinitionListVm
            {
                PercentageDefinitionId = x.PercentageDefinitionId,
                Name = x.Name,
            });
        }
    }
}