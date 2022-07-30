using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class ExpenseAuthorizationSummaryMapper
    {
        public static IRowMapper<ExpenseAuthorizationSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<ExpenseAuthorizationSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class ExpenseAuthorizationMapper
    {
        public static IRowMapper<ExpenseAuthorization> Mapper
        {
            get
            {
                var mapper = MapBuilder<ExpenseAuthorization>.MapAllProperties()
                    .DoNotMap(m => m.Details)
                    .Build();
                return mapper;
            }
        }
    }

    public class ExpenseAuthorizationDetailMapper
    {
        public static IRowMapper<ExpenseAuthorizationDetail> Mapper
        {
            get
            {
                var mapper = MapBuilder<ExpenseAuthorizationDetail>.MapAllProperties()
                    .DoNotMap(m => m.ExpenseAuthorization)
                    .DoNotMap(m => m.Article)
                    .Build();
                return mapper;
            }
        }
    }
}
