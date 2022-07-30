using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Multigroup.DomainModel.Shared
{
    public class NavigationLog
    {
        public int UserId { get; set; }
        public string ControllerName     { get; set; }
        public string ControllerFullName { get; set; }
        public string ControllerNameSpace { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public string Parameters { get; set; }
        public string IP { get; set; }
          
        public DateTime? HttpContextTimestamp { get; set; }
        public string SessionId { get; set; }

    }
}