using MCC.Portal.Providers;
using MCC.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCC.Portal.Models.Shared
{
    public class BaseVm
    {
        public int? GetNullableValue(SingleSelectVm ddl)
        {
            return (ddl != null) ? Util.ParseIntNullable(ddl.SelectedItem) : null;
        }

        public int? GetValue(SingleSelectVm ddl)
        {
            if (ddl != null)
            {
                return Util.ParseIntNullable(ddl.SelectedItem);
            }
            else
                return 0;
        }
    }
}