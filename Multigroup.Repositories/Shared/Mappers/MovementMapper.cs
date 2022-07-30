using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class MovementTypeMapper
    {
        public static IRowMapper<MovementType> Mapper
        {
            get
            {
                var mapper = MapBuilder<MovementType>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class MovementResumeMapper
    {
        public static IRowMapper<MovementResume> Mapper
        {
            get
            {
                var mapper = MapBuilder<MovementResume>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class MovementBalanceMapper
    {
        public static IRowMapper<MovementBalance> Mapper
        {
            get
            {
                var mapper = MapBuilder<MovementBalance>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class VerifiedMapper
    {
        public static IRowMapper<Verified> Mapper
        {
            get
            {
                var mapper = MapBuilder<Verified>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
