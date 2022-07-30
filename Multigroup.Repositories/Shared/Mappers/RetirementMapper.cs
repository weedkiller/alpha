using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{


    public class RetirementPartnerMapper
    {
        public static IRowMapper<RetirementPartner> Mapper
        {
            get
            {
                var mapper = MapBuilder<RetirementPartner>.MapAllProperties()
                    .DoNotMap(m => m.PaymentMethods)
                    .DoNotMap(m => m.Allocations)
                    .DoNotMap(m => m.AllocationsAll)
                    .Build();
                return mapper;
            }
        }
    }

    public class RetirementPartnerSummaryMapper
    {
        public static IRowMapper<RetirementPartnerSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<RetirementPartnerSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class RetirementPartnerPaymentMethodMapper
    {
        public static IRowMapper<RetirementPartnerPaymentMethod> Mapper
        {
            get
            {
                var mapper = MapBuilder<RetirementPartnerPaymentMethod>.MapAllProperties()
                    .DoNotMap(m => m.PaymentMethod)
                    .Build();
                return mapper;
            }
        }
    }

    public class RetirementPartnerAllocationMapper
    {
        public static IRowMapper<RetirementPartnerAllocation> Mapper
        {
            get
            {
                var mapper = MapBuilder<RetirementPartnerAllocation>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
