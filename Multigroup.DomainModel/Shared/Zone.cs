using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
   public class Zone
    {
       public int ZoneId { get; set; }
       public string Name { get; set; }
       public double CommissionRate { get; set; }
       public int? PaymentDay1 { get; set; }
       public int? PaymentDay2 { get; set; }
       public IEnumerable<City> Cities { get; set; }

   }
}
