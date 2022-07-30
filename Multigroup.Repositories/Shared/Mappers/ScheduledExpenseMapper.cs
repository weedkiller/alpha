using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class ScheduledExpenseSummaryMapper
    {
        public static IRowMapper<ScheduledExpenseSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<ScheduledExpenseSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class ScheduledExpenseMapper
    {
        public static IRowMapper<ScheduledExpense> Mapper
        {
            get
            {
                var mapper = MapBuilder<ScheduledExpense>.MapAllProperties()
                    .DoNotMap(m => m.Details)
                    .Build();
                return mapper;
            }
        }
    }

    public class ScheduledExpenseDetailMapper
    {
        public static IRowMapper<ScheduledExpenseDetail> Mapper
        {
            get
            {
                var mapper = MapBuilder<ScheduledExpenseDetail>.MapAllProperties()
                    .DoNotMap(m => m.ScheduledExpense)
                    .DoNotMap(m => m.Article)
                    .Build();
                return mapper;
            }
        }
    }
}
