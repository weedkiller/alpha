using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Shared.Charts
{
    public interface IPieChartVm
    {
        PieChartVm BuildChart(object sources);
    }
}