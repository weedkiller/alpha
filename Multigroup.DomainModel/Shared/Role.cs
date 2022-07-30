using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Multigroup.DomainModel.Shared
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Administrator { get; set; }
        public bool Active { get; set; }


        public override String ToString()
        {

            return Description;
        }
    }
}
