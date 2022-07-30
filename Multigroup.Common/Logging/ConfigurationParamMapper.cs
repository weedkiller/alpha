using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Common.Logging
{
    public class ConfigurationParamMapper
    {
        public static IRowMapper<ConfigurationParam> Mapper
        {
            get
            {
                var mapper = MapBuilder<ConfigurationParam>.MapAllProperties().Build();
                return mapper;
            }
        }
    }
}
