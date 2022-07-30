using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class SpendingPaymentSummaryMapper
    {
        public static IRowMapper<SpendingPaymentSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<SpendingPaymentSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }


    public class SpendingPaymentMapper
    {
        public static IRowMapper<SpendingPayment> Mapper
        {
            get
            {
                var mapper = MapBuilder<SpendingPayment>.MapAllProperties()
                    .DoNotMap(m => m.Details)
                    .DoNotMap(m => m.PaymentMethods)
                    .Build();
                return mapper;
            }
        }
    }

    public class SpendingPaymentDetailMapper
    {
        public static IRowMapper<SpendingPaymentDetail> Mapper
        {
            get
            {
                var mapper = MapBuilder<SpendingPaymentDetail>.MapAllProperties()
                    .DoNotMap(m => m.Spending)
                    .Build();
                return mapper;
            }
        }
    }

    public class SpendingPaymentDetailSummaryMapper
    {
        public static IRowMapper<SpendingPaymentDetailSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<SpendingPaymentDetailSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class SpendingPaymentPaymentMethodMapper
    {
        public static IRowMapper<SpendingPaymentPaymentMethod> Mapper
        {
            get
            {
                var mapper = MapBuilder<SpendingPaymentPaymentMethod>.MapAllProperties()
                    .DoNotMap(m => m.PaymentMethod)
                    .Build();
                return mapper;
            }
        }
    }
}
