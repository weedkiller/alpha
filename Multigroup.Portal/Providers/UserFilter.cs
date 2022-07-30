using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Providers
{
    public class UserFilter
    {
        public IEnumerable<string> SelectedSites { get; set; }
        public IEnumerable<string> SelectedEquipments { get; set; }
        public IEnumerable<string> SelectedEquipmentModels { get; set; }
        public IEnumerable<string> SelectedFleets { get; set; }
        public IEnumerable<string> SelectedStatus { get; set; }

        public string RoutineCode { get; set; }
        public int? CriticalLevelCode { get; set; }
    }
}