using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class PurchaseRequestSummaryMapper
    {
        public static IRowMapper<PurchaseRequestSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<PurchaseRequestSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class UrgencyMapper
    {
        public static IRowMapper<Urgency> Mapper
        {
            get
            {
                var mapper = MapBuilder<Urgency>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class PurchaseRequestMapper
    {
        public static IRowMapper<PurchaseRequest> Mapper
        {
            get
            {
                var mapper = MapBuilder<PurchaseRequest>.MapAllProperties()
                    .DoNotMap(m => m.Details)
                    .Build();
                return mapper;
            }
        }
    }

    public class PurchaseRequestDetailMapper
    {
        public static IRowMapper<PurchaseRequestDetail> Mapper
        {
            get
            {
                var mapper = MapBuilder<PurchaseRequestDetail>.MapAllProperties()
                    .DoNotMap(m => m.Article)
                    .Build();
                return mapper;
            }
        }
    }
}
