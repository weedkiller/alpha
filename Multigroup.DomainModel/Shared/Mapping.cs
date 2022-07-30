using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class Mapping
    {
        public int Id { get; set; }
        public int MapperTypeId { get; set; }
        public string CodeArgos { get; set; }
        public string CodeExternal { get; set; }
        public MapperType MapperType { get; set; }
    }
}
