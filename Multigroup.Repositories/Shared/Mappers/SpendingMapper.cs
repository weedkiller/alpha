using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class SpendingSummaryMapper
    {
        public static IRowMapper<SpendingSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<SpendingSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class SpendingDetailSummaryMapper
    {
        public static IRowMapper<SpendingDetailSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<SpendingDetailSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class SpendingMapper
    {
        public static IRowMapper<Spending> Mapper
        {
            get
            {
                var mapper = MapBuilder<Spending>.MapAllProperties()
                    .DoNotMap(m => m.Details)
                    .DoNotMap(m => m.PaymentMethods)
                    .Build();
                return mapper;
            }
        }
    }

    public class SpendingDetailMapper
    {
        public static IRowMapper<SpendingDetail> Mapper
        {
            get
            {
                var mapper = MapBuilder<SpendingDetail>.MapAllProperties()
                    .DoNotMap(m => m.Spending)
                    .DoNotMap(m => m.Article)
                    .DoNotMap(m => m.Origin)
                    .Build();
                return mapper;
            }
        }
    }

    public class SpendingPaymentMethodMapper
    {
        public static IRowMapper<SpendingPaymentMethod> Mapper
        {
            get
            {
                var mapper = MapBuilder<SpendingPaymentMethod>.MapAllProperties()
                    .DoNotMap(m => m.Spending)
                    .DoNotMap(m => m.PaymentMethod)
                    .Build();
                return mapper;
            }
        }
    }

    public class PaymentPaymentMethodMapper
    {
        public static IRowMapper<PaymentPaymentMethod> Mapper
        {
            get
            {
                var mapper = MapBuilder<PaymentPaymentMethod>.MapAllProperties()
                    .DoNotMap(m => m.Payment)
                    .DoNotMap(m => m.PaymentMethod)
                    .Build();
                return mapper;
            }
        }
    }

    public class InvoiceMapper
    {
        public static IRowMapper<Invoice> Mapper
        {
            get
            {
                var mapper = MapBuilder<Invoice>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
