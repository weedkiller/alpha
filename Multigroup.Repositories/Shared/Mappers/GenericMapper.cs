using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class GenericMapper<T> where T : class, new()
    {
        public static IRowMapper<T> Mapper
        {
            get
            {
                var mapper = MapBuilder<T>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
