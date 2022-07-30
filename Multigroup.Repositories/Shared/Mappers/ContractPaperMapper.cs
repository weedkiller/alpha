using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class ContractPaperMapper
    {
        public static IRowMapper<ContractPaper> Mapper
        {
            get
            {
                var mapper = MapBuilder<ContractPaper>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class ContractPaperSummaryMapper
    {
        public static IRowMapper<ContractPaperSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<ContractPaperSummary>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

}
