using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class ContactCallMapper
    {
        public static IRowMapper<ContactCall> Mapper
        {
            get
            {
                var mapper = MapBuilder<ContactCall>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class ContactCallStateMapper
    {
        public static IRowMapper<ContactCallState> Mapper
        {
            get
            {
                var mapper = MapBuilder<ContactCallState>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class ContactCallSummaryMapper
    {
        public static IRowMapper<ContactCallSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<ContactCallSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
