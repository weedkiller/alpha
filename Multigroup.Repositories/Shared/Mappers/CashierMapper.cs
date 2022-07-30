using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class CashierMapper
    {
        public static IRowMapper<Cashier> Mapper
        {
            get
            {
                var mapper = MapBuilder<Cashier>.MapAllProperties().Build();
                return mapper;
            }
        }
    }

    public class TransferCashierMapper
    {
        public static IRowMapper<TransferCashier> Mapper
        {
            get
            {
                var mapper = MapBuilder<TransferCashier>.MapAllProperties().Build();
                return mapper;
            }
        }
    }    
    
    public class TransferCashierPaymentMethodMapper
    {
        public static IRowMapper<TransferCashierPaymentMethod> Mapper
        {
            get
            {
                var mapper = MapBuilder<TransferCashierPaymentMethod>.MapAllProperties().Build();
                return mapper;
            }
        }
    }
}
