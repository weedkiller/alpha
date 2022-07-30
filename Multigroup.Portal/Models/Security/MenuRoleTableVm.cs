using Multigroup.DomainModel.Menu;
using Multigroup.Portal.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Security
{
    public class MenuRoleVm
    {
        [Required(ErrorMessage = "El campo es roles es requerido")]
        public SingleSelectVm ddlRoles { get; set; }

        [Required(ErrorMessage = "El campo menu es requerido")]
        public SingleSelectVm ddlMenuTypes { get; set; }
        public int RoleId { get; set; }
        public string MenuType { get; set; }
    }

    public class MenuVm
    {
        public int RoleId { get; set; }
        public IEnumerable<int> Items { get; set; }
        public string MenuType { get; set; }
        public IEnumerable<MenuComponentRole> ToModelObject()
        {
            return Items.Select(x => new MenuComponentRole { MenuId = x, RoleId = RoleId });
        }
    }

    public class MenuItemVm
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public bool Enabled { get; set; }
        public string Status { get; set; }

        public static IEnumerable<MenuItemVm> ToViewModel(IEnumerable<MenuComponent> entities)
        {
            return entities.Select(x => new MenuItemVm
            {
                MenuName = x.Description,
                MenuId = x.MenuComponentId,
                Status = x.Enabled ? "Habilitado" : "Deshabilitado"
            });
        }
    }
}