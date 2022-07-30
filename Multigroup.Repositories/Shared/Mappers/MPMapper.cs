using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class MPMapper
    {
        public static IRowMapper<MPSellerAccountSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<MPSellerAccountSummary>.MapAllProperties()
                     .DoNotMap(m => m.Amount)
                     .DoNotMap(m => m.Color)
                    .Build();
                return mapper;
            }
        }
    }
}
