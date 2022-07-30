using Multigroup.DomainModel.Shared;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class RoleMapper
    {
        public static IRowMapper<Role> Mapper
        {
            get
            {
                var mapper = MapBuilder<Role>.MapAllProperties().Build();
                return mapper;
            }
        }
    }
}
