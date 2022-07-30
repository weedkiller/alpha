using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Enums;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Multigroup.Portal.Models.Shared;
using System.Web.Mvc;
using Multigroup.Portal.Models.Shared.Charts;

namespace Multigroup.Portal.Models.ContractModel
{
    public class ContractChartVm : LineChartVm, ILineChartVm
{
    public LineChartVm BuildChart(object sources)
    {
        var samples = sources as IEnumerable<ContractChart>;

        var model = new LineChartVm();
        resultset = new List<IEnumerable<object>>();

        if (samples != null)
        {
            foreach (var data in samples)
            {
                List<object> element = new List<object>();
                element.Add(data.Valor);
                element.Add(data.Cantidad);

                resultset.Add(element);
            }
        }

        metadata = new List<Metadata>();
        Metadata data1 = new Metadata { colIndex = 0, colName = "Valor", colType = "String" };
        Metadata data2 = new Metadata { colIndex = 1, colName = "Cantidad", colType = "Numeric" };

        metadata.Add(data1);
        metadata.Add(data2);

        model.resultset = resultset;
        model.metadata = metadata;

        return model;
    }
}
}