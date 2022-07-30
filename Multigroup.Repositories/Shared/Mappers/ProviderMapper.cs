using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class ProviderMapper
    {
        public static IRowMapper<Provider> Mapper
        {
            get
            {
                var mapper = MapBuilder<Provider>.MapAllProperties()
                    .DoNotMap(m => m.ProviderType)
                    .DoNotMap(m => m.User)
                    .Build();
                return mapper;
            }
        }
    }

    public class ProviderSummaryMapper
    {
        public static IRowMapper<ProviderSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<ProviderSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class ProviderTypeMapper
    {
        public static IRowMapper<ProviderType> Mapper
        {
            get
            {
                var mapper = MapBuilder<ProviderType>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class EmployeeCurrentAcountMapper
    {
        public static IRowMapper<EmployeeCurrentAcount> Mapper
        {
            get
            {
                var mapper = MapBuilder<EmployeeCurrentAcount>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class EmployeeCurrentAcountDetailMapper
    {
        public static IRowMapper<EmployeeCurrentAcountDetail> Mapper
        {
            get
            {
                var mapper = MapBuilder<EmployeeCurrentAcountDetail>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class ProviderCurrentAcountMapper
    {
        public static IRowMapper<ProviderCurrentAcount> Mapper
        {
            get
            {
                var mapper = MapBuilder<ProviderCurrentAcount>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }      
    }

    public class ProviderCurrentAcountDetailMapper
    {
        public static IRowMapper<ProviderCurrentAcountDetail> Mapper
        {
            get
            {
                var mapper = MapBuilder<ProviderCurrentAcountDetail>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class IVAPositionMapper
    {
        public static IRowMapper<IVAPosition> Mapper
        {
            get
            {
                var mapper = MapBuilder<IVAPosition>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
