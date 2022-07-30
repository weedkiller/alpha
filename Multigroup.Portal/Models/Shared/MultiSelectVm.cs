using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Shared
{
    public class MultiSelectVm
    {
        public IEnumerable<string> SelectedItems { get; set; }
        public IEnumerable<SelectListItem> ListItems { get; set; }
        public MultiSelectVm()
        {
            ListItems = new List<SelectListItem>();
        }
    }

    public class SingleSelectVm
    {
        public string SelectedItem { get; set; }
        public IEnumerable<SelectListItem> ListItems { get; set; }
        public SingleSelectVm()
        {
            ListItems = new List<SelectListItem>();
        }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ItemCode
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}