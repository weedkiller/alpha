using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class PartnerMapper
    {
        public static IRowMapper<Partner> Mapper
        {
            get
            {
                var mapper = MapBuilder<Partner>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class PercentageDefinitionPartnerSummaryMapper
    {
        public static IRowMapper<PercentageDefinitionPartnerSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<PercentageDefinitionPartnerSummary>.MapAllProperties()
                    .DoNotMap(m => m.Amount)
                    .Build();
                return mapper;
            }
        }
    }

    public class PercentageDefinitionPartnerMapper
    {
        public static IRowMapper<PercentageDefinitionPartner> Mapper
        {
            get
            {
                var mapper = MapBuilder<PercentageDefinitionPartner>.MapAllProperties()
                    .DoNotMap(m => m.Name)
                    .Build();
                return mapper;
            }
        }
    }

    public class PercentageDefinitionMapper
    {
        public static IRowMapper<PercentageDefinition> Mapper
        {
            get
            {
                var mapper = MapBuilder<PercentageDefinition>.MapAllProperties()
                    .DoNotMap(m => m.Partners)
                    .Build();
                return mapper;
            }
        }
    }

    public class BalancePartnerMapper
    {
        public static IRowMapper<BalancePartner> Mapper
        {
            get
            {
                var mapper = MapBuilder<BalancePartner>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
