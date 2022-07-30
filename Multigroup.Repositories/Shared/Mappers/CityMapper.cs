using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class CityMapper
    {
        public static IRowMapper<City> Mapper
        {
            get
            {
                var mapper = MapBuilder<City>.MapAllProperties()
                    .DoNotMap(m => m.Zone)
                    .Build();
                return mapper;
            }
        }
    }
}
