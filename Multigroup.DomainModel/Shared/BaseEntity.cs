using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class BaseEntityText
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
