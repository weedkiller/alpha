using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public Zone Zone { get; set; }
        public int ZoneId { get; set; }

    }
}
