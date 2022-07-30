using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{


    public class IncomePartnerMapper
    {
        public static IRowMapper<IncomePartner> Mapper
        {
            get
            {
                var mapper = MapBuilder<IncomePartner>.MapAllProperties()
                    .DoNotMap(m => m.PaymentMethods)
                    .DoNotMap(m => m.Allocations)
                    .DoNotMap(m => m.AllocationsAll)
                    .Build();
                return mapper;
            }
        }
    }

    public class IncomePartnerSummaryMapper
    {
        public static IRowMapper<IncomePartnerSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<IncomePartnerSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class IncomePartnerPaymentMethodMapper
    {
        public static IRowMapper<IncomePartnerPaymentMethod> Mapper
        {
            get
            {
                var mapper = MapBuilder<IncomePartnerPaymentMethod>.MapAllProperties()
                    .DoNotMap(m => m.PaymentMethod)
                    .Build();
                return mapper;
            }
        }
    }

    public class IncomePartnerAllocationMapper
    {
        public static IRowMapper<IncomePartnerAllocation> Mapper
        {
            get
            {
                var mapper = MapBuilder<IncomePartnerAllocation>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
