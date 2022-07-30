using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class EarningsAllocationSummaryMapper
    {
        public static IRowMapper<EarningsAllocationSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<EarningsAllocationSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }


    public class EarningsAllocationMapper
    {
        public static IRowMapper<EarningsAllocation> Mapper
        {
            get
            {
                var mapper = MapBuilder<EarningsAllocation>.MapAllProperties()
                    .DoNotMap(m => m.Partners)
                    .Build();
                return mapper;
            }
        }
    }

    public class EarningsAllocationPartnerMapper
    {
        public static IRowMapper<EarningsAllocationPartner> Mapper
        {
            get
            {
                var mapper = MapBuilder<EarningsAllocationPartner>.MapAllProperties()
                    .DoNotMap(m => m.Partner)
                    .DoNotMap(m => m.Percentage)
                    .DoNotMap(m => m.Concept)
                    .Build();
                return mapper;
            }
        }
    }
}
