using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class CashCycleMapper
    {
        public static IRowMapper<CashCycle> Mapper
        {
            get
            {
                var mapper = MapBuilder<CashCycle>.MapAllProperties()
                .DoNotMap(m => m.PaymentMethods)
                .Build();
                return mapper;
            }
        }
    }

    public class CashCyclePaymentMethodMapper
    {
        public static IRowMapper<CashCyclePaymentMethod> Mapper
        {
            get
            {
                var mapper = MapBuilder<CashCyclePaymentMethod>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class CashCyclePaymentMethodSummaryMapper
    {
        public static IRowMapper<CashCyclePaymentMethodSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<CashCyclePaymentMethodSummary>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class CashCycleSummaryMapper
    {
        public static IRowMapper<CashCycleSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<CashCycleSummary>.MapAllProperties()
                .DoNotMap(m => m.PaymentMethods)
                .Build();
                return mapper;
            }


        }
    }

    public class CashCyclePaymentMapper
    {
        public static IRowMapper<CashCyclePayment> Mapper
        {
            get
            {
                var mapper = MapBuilder<CashCyclePayment>.MapAllProperties()
                .Build();
                return mapper;
            }


        }
    }

    public class CycleMapper
    {
        public static IRowMapper<Cycle> Mapper
        {
            get
            {
                var mapper = MapBuilder<Cycle>.MapAllProperties()
                .Build();
                return mapper;
            }


        }
    }
}

