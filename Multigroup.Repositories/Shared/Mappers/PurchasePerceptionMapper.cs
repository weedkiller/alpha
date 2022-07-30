using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class PurchasePerceptionMapper
    {
        public static IRowMapper<PurchasePerception> Mapper
        {
            get
            {
                var mapper = MapBuilder<PurchasePerception>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class PurchaseDocumentMapper
    {
        public static IRowMapper<PurchaseDocument> Mapper
        {
            get
            {
                var mapper = MapBuilder<PurchaseDocument>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
