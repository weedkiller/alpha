using Multigroup.DomainModel.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Menu
{
    public class MenuComponentVm
    {
        public int MenuComponentId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int SortOrder { get; set; }
        public string ButtonId { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public string Icon { get; set; }
        public string MenuType { get; set; }
        public int PageStatic { get; set; }

        public static IEnumerable<MenuComponentVm> ToViewModel(IEnumerable<MenuComponent> entities)
        {
            return entities.Select(x => new MenuComponentVm
            {
                MenuComponentId = x.MenuComponentId,
                Name = x.Name,
                ParentId = x.ParentId,
                SortOrder = x.SortOrder,
                ButtonId = x.ButtonId,
                Description = x.Description,
                Enabled = x.Enabled,
                Icon = x.Icon,
                MenuType = x.MenuType,
                PageStatic = x.PageStatic
            });
        }
    }
}