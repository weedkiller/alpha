using Multigroup.Portal.Providers;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Shared
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

        public IEnumerable<string> GetValue(MultiSelectVm ddl)
        {
            return (ddl != null) ? ddl.SelectedItems : null;
        }
    }
}