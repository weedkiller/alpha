using Multigroup.DomainModel.Shared;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class UserMapper
    {
        public static IRowMapper<User> Mapper
        {
            get
            {
                var mapper = MapBuilder<User>.MapAllProperties()
                    //.DoNotMap(m => m.Password)
                    .Build();
                return mapper;
            }
        }
    }

    public class UserSummaryMapper
    {
        public static IRowMapper<UserSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<UserSummary>.MapAllProperties()
                    //.DoNotMap(m => m.Password)
                    .Build();
                return mapper;
            }
        }
    }

    public class UserTypeMapper
    {
        public static IRowMapper<UserType> Mapper
        {
            get
            {
                var mapper = MapBuilder<UserType>.MapAllProperties()
                    //.DoNotMap(m => m.Password)
                    .Build();
                return mapper;
            }
        }
    }

    public class UserLoginMapper
    {
        public static IRowMapper<UserLogin> Mapper
        {
            get
            {
                var mapper = MapBuilder<UserLogin>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class UserZoneMapper
    {
        public static IRowMapper<UserZone> Mapper
        {
            get
            {
                var mapper = MapBuilder<UserZone>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class UserCashierMapper
    {
        public static IRowMapper<UserCashier> Mapper
        {
            get
            {
                var mapper = MapBuilder<UserCashier>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
