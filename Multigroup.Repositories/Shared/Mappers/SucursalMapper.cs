using Multigroup.DomainModel.Shared;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class SucursalMapper
    {
        public static IRowMapper<Sucursal> Mapper
        {
            get
            {
                var mapper = MapBuilder<Sucursal>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
