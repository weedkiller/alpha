using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class CustomerMapper
    {
        public static IRowMapper<Customer> Mapper
        {
            get
            {
                var mapper = MapBuilder<Customer>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class CustomerPortfolioTotalMapper
    {
        public static IRowMapper<CustomerPortfolioTotal> Mapper
        {
            get
            {
                var mapper = MapBuilder<CustomerPortfolioTotal>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class CustomerPortfolioMapper
    {
        public static IRowMapper<CustomerPortfolio> Mapper
        {
            get
            {
                var mapper = MapBuilder<CustomerPortfolio>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class InstallmentReportMapper
    {
        public static IRowMapper<InstallmentReport> Mapper
        {
            get
            {
                var mapper = MapBuilder<InstallmentReport>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class IdentificationTypeMapper
    {
        public static IRowMapper<IdentificationType> Mapper
        {
            get
            {
                var mapper = MapBuilder<IdentificationType>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class MaritalStatusMapper
    {
        public static IRowMapper<MaritalStatus> Mapper
        {
            get
            {
                var mapper = MapBuilder<MaritalStatus>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class StatusClientMapper
    {
        public static IRowMapper<StatusClient> Mapper
        {
            get
            {
                var mapper = MapBuilder<StatusClient>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

}
