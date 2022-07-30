using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class ZoneMapper
    {
        public static IRowMapper<Zone> Mapper
        {
            get
            {
                var mapper = MapBuilder<Zone>.MapAllProperties()
                     .DoNotMap(m => m.Cities)
                    .Build();
                return mapper;
            }
        }
    }
}
