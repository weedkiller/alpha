using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Menu
{
    public class PageTableVm
    {
        public int Id { get; set; }
        public int MenuComponentId { get; set; }
        public string Name { get; set; }
    }
}