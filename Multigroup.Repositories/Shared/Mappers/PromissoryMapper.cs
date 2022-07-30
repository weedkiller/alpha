using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class PromissorySummaryMapper
    {
        public static IRowMapper<PromissorySummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<PromissorySummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }


    public class PromissoryMapper
    {
        public static IRowMapper<Promissory> Mapper
        {
            get
            {
                var mapper = MapBuilder<Promissory>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class PromissoryPartnerMapper
    {
        public static IRowMapper<PromissoryPartner> Mapper
        {
            get
            {
                var mapper = MapBuilder<PromissoryPartner>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class PromissoryPaymentMapper
    {
        public static IRowMapper<PromissoryPayment> Mapper
        {
            get
            {
                var mapper = MapBuilder<PromissoryPayment>.MapAllProperties()
                    .DoNotMap(m => m.PaymentMethods)
                    .DoNotMap(m => m.Promissories)
                    .DoNotMap(m => m.PromissoriesAll)
                    .Build();
                return mapper;
            }
        }
    }

    public class PromissoryPaymentSummaryMapper
    {
        public static IRowMapper<PromissoryPaymentSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<PromissoryPaymentSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class PromissoryPaymentMethodMapper
    {
        public static IRowMapper<PromissoryPaymentMethod> Mapper
        {
            get
            {
                var mapper = MapBuilder<PromissoryPaymentMethod>.MapAllProperties()
                    .DoNotMap(m => m.PaymentMethod)
                    .Build();
                return mapper;
            }
        }
    }

    public class PromissoryPaymentPromissoryMapper
    {
        public static IRowMapper<PromissoryPaymentPromissory> Mapper
        {
            get
            {
                var mapper = MapBuilder<PromissoryPaymentPromissory>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class PromissorySurrenderPartnerMapper
    {
        public static IRowMapper<PromissorySurrenderPartner> Mapper
        {
            get
            {
                var mapper = MapBuilder<PromissorySurrenderPartner>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class PromissorySurrenderPartnerSummaryMapper
    {
        public static IRowMapper<PromissorySurrenderPartnerSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<PromissorySurrenderPartnerSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class PromissorySurrenderMapper
    {
        public static IRowMapper<PromissorySurrender> Mapper
        {
            get
            {
                var mapper = MapBuilder<PromissorySurrender>.MapAllProperties()
                    .DoNotMap(m => m.PaymentMethods)
                    .DoNotMap(m => m.rendereds)
                    .DoNotMap(m => m.PromissoriesAll)
                    .Build();
                return mapper;
            }
        }
    }

    public class PromissorySurrenderMethodMapper
    {
        public static IRowMapper<PromissorySurrenderMethod> Mapper
        {
            get
            {
                var mapper = MapBuilder<PromissorySurrenderMethod>.MapAllProperties()
                    .DoNotMap(m => m.PaymentMethod)
                    .Build();
                return mapper;
            }
        }
    }

    public class PromissoryRenderedMapper
    {
        public static IRowMapper<PromissoryRendered> Mapper
        {
            get
            {
                var mapper = MapBuilder<PromissoryRendered>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
