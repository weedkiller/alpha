using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Shared.Charts
{
    public class LineChartVm
    {
        public List<IEnumerable<object>> resultset { get; set; }
        public List<Metadata> metadata { get; set; }

        public double orthoAxisFixedMin { get; set; }
    }
}