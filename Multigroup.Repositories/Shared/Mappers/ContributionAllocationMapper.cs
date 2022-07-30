using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class ContributionAllocationSummaryMapper
    {
        public static IRowMapper<ContributionAllocationSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<ContributionAllocationSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }


    public class ContributionAllocationMapper
    {
        public static IRowMapper<ContributionAllocation> Mapper
        {
            get
            {
                var mapper = MapBuilder<ContributionAllocation>.MapAllProperties()
                    .DoNotMap(m => m.Partners)
                    .Build();
                return mapper;
            }
        }
    }

    public class ContributionAllocationPartnerMapper
    {
        public static IRowMapper<ContributionAllocationPartner> Mapper
        {
            get
            {
                var mapper = MapBuilder<ContributionAllocationPartner>.MapAllProperties()
                    .DoNotMap(m => m.Partner)
                    .DoNotMap(m => m.Percentage)
                    .DoNotMap(m => m.Concept)
                    .Build();
                return mapper;
            }
        }
    }
}
