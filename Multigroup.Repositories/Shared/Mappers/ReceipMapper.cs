using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class ReceipMapper
    {
        public static IRowMapper<Receip> Mapper
        {
            get
            {
                var mapper = MapBuilder<Receip>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class ReceipSummaryMapper
    {
        public static IRowMapper<ReceipSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<ReceipSummary>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

}
