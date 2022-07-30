using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class ContractMapper
    {
        public static IRowMapper<Contract> Mapper
        {
            get
            {
                var mapper = MapBuilder<Contract>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class ContractSummaryMapper
    {
        public static IRowMapper<ContractSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<ContractSummary>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class ContractCuotasMapper
    {
        public static IRowMapper<ContractCuotas> Mapper
        {
            get
            {
                var mapper = MapBuilder<ContractCuotas>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class ContractSummaryTotalMapper
    {
        public static IRowMapper<ContractSummaryTotal> Mapper
        {
            get
            {
                var mapper = MapBuilder<ContractSummaryTotal>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class PaymentDateMapper
    {
        public static IRowMapper<PaymentDate> Mapper
        {
            get
            {
                var mapper = MapBuilder<PaymentDate>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class StatusContractMapper
    {
        public static IRowMapper<StatusContract> Mapper
        {
            get
            {
                var mapper = MapBuilder<StatusContract>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class ContractChartMapper
    {
        public static IRowMapper<ContractChart> Mapper
        {
            get
            {
                var mapper = MapBuilder<ContractChart>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class ContractFileMapper
    {
        public static IRowMapper<ContractFile> Mapper
        {
            get
            {
                var mapper = MapBuilder<ContractFile>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class SpendingFileMapper
    {
        public static IRowMapper<SpendingFile> Mapper
        {
            get
            {
                var mapper = MapBuilder<SpendingFile>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

}
