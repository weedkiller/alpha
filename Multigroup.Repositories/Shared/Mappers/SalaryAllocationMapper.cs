using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class SalaryAllocationSummaryMapper
    {
        public static IRowMapper<SalaryAllocationSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<SalaryAllocationSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class SalaryAllocationMapper
    {
        public static IRowMapper<SalaryAllocation> Mapper
        {
            get
            {
                var mapper = MapBuilder<SalaryAllocation>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
