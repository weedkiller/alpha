using System.Collections.Generic;

namespace Multigroup.DomainModel.Menu
{
    public class Menu : MenuComponent
    {
        private List<MenuComponent> _items;

        public Menu()
        {
            _items = new List<MenuComponent>();
        }

        public override void Add(MenuComponent menuComponent)
        {
            _items.Add(menuComponent);
        }

        public override void Delete(MenuComponent menuComponent)
        {
            _items.Remove(menuComponent);
        }

        //public override MenuComponent GetChild(int i)
        //{
        //    return (MenuComponent)menuComponents[i];
        //}

        public override IEnumerable<MenuComponent> GetMenu()
        {
            return _items;
        }
    }
}
