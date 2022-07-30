using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class AgencyMapper
    {
        public static IRowMapper<Agency> Mapper
        {
            get
            {
                var mapper = MapBuilder<Agency>.MapAllProperties()
                    .DoNotMap(m => m.Manager)
                    .Build();
                return mapper;
            }
        }
    }

    public class AgencyTypeMapper
    {
        public static IRowMapper<AgencyType> Mapper
        {
            get
            {
                var mapper = MapBuilder<AgencyType>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class CarteraMapper
    {
        public static IRowMapper<Cartera> Mapper
        {
            get
            {
                var mapper = MapBuilder<Cartera>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
