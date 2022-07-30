using System;
using System.Collections.Generic;

namespace Multigroup.DomainModel.Menu
{
    public class MenuComponent
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

        public virtual void Add(MenuComponent menuComponent)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(MenuComponent menuComponent)
        {
            throw new NotImplementedException();
        }

        public virtual MenuComponent GetChild(int i)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<MenuComponent> GetMenu()
        {
            throw new NotImplementedException();
        }
    }
}
