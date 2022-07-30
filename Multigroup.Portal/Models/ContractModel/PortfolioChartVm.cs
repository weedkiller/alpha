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
    public class PortFolioChartVm : BarChartVm, IBarChartVm
    {
        public BarChartVm BuildChart(object sources, params object[] parameters)
        {
            var events = sources as IEnumerable<PortfolioReport>;
            var model = new PortFolioChartVm();
            resultset = new List<IEnumerable<object>>();

            if (events != null)
            {
                IEnumerable<Data> info = new List<Data>();

                info = events.Select(x => new Data { label = x.Telemarketer, value = (float)x.Quantity, description = x.State.ToString(), Id = x.amount });

                foreach (var data in info)
                {
                    List<object> reds = new List<object>();
                    reds.Add(data.description);
                    reds.Add(data.label);
                    reds.Add(data.value);
                    reds.Add(data.Id);
                    resultset.Add(reds);
                }
            }

            metadata = new List<Metadata>();
            Metadata data1 = new Metadata { colIndex = 0, colName = "Estado", colType = "String" };
            Metadata data2 = new Metadata { colIndex = 1, colName = "Telemarketer", colType = "String" };
            Metadata data3 = new Metadata { colIndex = 2, colName = "Cantidad", colType = "Value" };
            Metadata data4 = new Metadata { colIndex = 3, colName = "Monto", colType = "Value" };

            metadata.Add(data1);
            metadata.Add(data2);
            metadata.Add(data3);
            metadata.Add(data4);

            model.resultset = resultset;
            model.metadata = metadata;

            return model;
        }
    }
}