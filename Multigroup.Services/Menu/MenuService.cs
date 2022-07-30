using Multigroup.Common.Logging;
using Multigroup.DomainModel.CustomException;
using Multigroup.DomainModel.Menu;
using Multigroup.DomainModel.Shared.Constants;
using Multigroup.DomainModel.Shared.Enums;
using Multigroup.Repositories.Menu;
using Multigroup.Services.Providers;
using Multigroup.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Multigroup.Services.Menu
{
    public class MenuService
    {
        private MenuRepository _menuRepository;
        private UserService _userService;
        private RoleService _roleService;
        private IAuditLogHelper _auditLogHelper;

        public MenuService()
        {
            _menuRepository = new MenuRepository();
            _userService = new UserService();
            _roleService = new RoleService();
            _auditLogHelper = new AuditLogHelper();
        }

        public int AddMenuComponent(MenuComponent entity)
        {
            if (entity.ParentId == 0)
            {
                entity.ParentId = null;
            }
            if(entity.ParentId.HasValue)
            {
                var parent = _menuRepository.GetMenuComponent(entity.ParentId.Value);
                entity.Description = parent.Description + " - " + entity.Name; 
            }
            else
            {
                entity.Description = entity.Name;
            }
            return _menuRepository.AddMenuComponent(entity);
        }

        public void UpdateMenuComponent(MenuComponent entity)
        {
            if(entity.ParentId == 0)
            {
                entity.ParentId = null;
            }

            if (entity.ParentId.HasValue)
            {
                var parent = _menuRepository.GetMenuComponent(entity.ParentId.Value);
                entity.Description = parent.Description + " - " + entity.Name;
            }
            else
            {
                entity.Description = entity.Name;
            }
            var currenMenu = _menuRepository.GetMenuComponent(entity.MenuComponentId);
            entity.ButtonId = currenMenu.ButtonId;
            entity.PageStatic = currenMenu.PageStatic;
            _menuRepository.UpdateMenuComponent(entity);
        }

        public void DeleteMenuComponent(MenuComponentRole menu)
        {
            var entities = GetMenuComponents(MenuTypeCon.Left);
            
            if(entities.Any(x => x.ParentId == menu.MenuId))
            {
                throw new BusinessException("No se puede eliminar la entrada de menu por que posee páginas dependientes");
            }
            //if(entities.Any(x => x.MenuComponentId == menu.MenuId && x.ParentId != null))
            //{
            //    throw new BusinessException("No se puede eliminar la entrada de menu, solo los agrupadores de páginas pueden ser eliminados");
            //}
            if(entities.Any(x => x.MenuComponentId == menu.MenuId && x.PageStatic == (int)PageStaticEnum.Static && x.ButtonId != null))
            {
                throw new BusinessException("No posee permisos para eliminar esta entrada de menu");
            }

            var pages = GetMenuComponentPages().Where(x => x.MenuComponentId == menu.MenuId);
            
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    if (pages.Any())
                    {
                        DeleteMenuComponentPage(pages.FirstOrDefault());
                    }

                    menu.RoleId = _roleService.GetRolesByUser(menu.UserId).FirstOrDefault().RoleId;
                    _menuRepository.DeleteMenuComponentRole(menu);
                    _menuRepository.DeleteMenuComponent(menu.Menu);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    var log = new AuditLog()
                    {
                        LogLevel = AuditLogLevel.Error,
                        Description = ex.Message + " " + ex.StackTrace,
                        InputParam = menu,
                        IntegrationType = AuditLogType.MenuService
                    };
                    _auditLogHelper.LogAuditFail(log);
                    throw ex;
                }
            }
        }

        public void AddMenuComponentRole(IEnumerable<MenuComponentRole> menus)
        {
            foreach(var menu in menus)
            {
                _menuRepository.AddMenuComponentRole(menu);
            }
        }

        public void DeleteMenuComponentRole(IEnumerable<MenuComponentRole> menus)
        {
            foreach (var menu in menus) 
            {
                _menuRepository.DeleteMenuComponentRole(menu);
            }
        }

        public IEnumerable<MenuComponent> GetMenuComponents(string menuType, int? roleId = null)
        {
            return _menuRepository.GetMenuComponents(menuType, roleId).ToList();
        }

        public IEnumerable<MenuComponent> GetCurrentMenu(int userId, string menuType)
        {
            var user = _userService.GetUser(userId);
            var roles = user.Roles;
            
            if(roles.Any())
            {
                var menus = new List<MenuComponent>();
                var items = GetMenuComponents(menuType, roles.FirstOrDefault().RoleId);
                foreach(var item in items)
                {
                    if (item.Enabled == true)
                        menus.Add(item);
                }
                return menus;
            }

            return new List<MenuComponent>();
        }

        public IEnumerable<MenuComponentPage> GetMenuComponentPages()
        {
            return _menuRepository.GetMenuComponentPages();
        }

        public MenuComponentPage GetMenuComponentPage(int menuComponentPageId)
        {
            return _menuRepository.GetMenuComponentPage(menuComponentPageId);
        }

        public void AddMenuComponentPage(MenuComponentPage entity)
        {
            var menus = GetCurrentMenu(entity.UserId, MenuTypeCon.Left);
            var parent = menus.Where(x => x.ParentId == null && x.Name == ConfigurationProvider.CustomPageName);
            
            if(!parent.Any())
            {
                throw new Exception("No existe una opcion de menu padre para agregar la pagina");
            }

            var menu = parent.FirstOrDefault();
            var lastItem = menus.Where(x => x.ParentId == menu.MenuComponentId).OrderByDescending(y => y.SortOrder);
            
            entity.MenuComponent = new MenuComponent { 
                Name = entity.Name, 
                Description = menu.Name + " - " + entity.Name,
                ParentId = menu.MenuComponentId,
                MenuType = MenuTypeCon.Left,
                PageStatic = (int)PageStaticEnum.Dinamic,
                ButtonId = "btn" + entity.Name.Replace(" ", string.Empty),
                SortOrder = lastItem.Any()? lastItem.FirstOrDefault().SortOrder : 1
            };

            var itemId = _menuRepository.AddMenuComponent(entity.MenuComponent);
            entity.MenuComponentId = itemId;
            _menuRepository.AddMenuComponentPage(entity);
        }

        public void UpdateMenuComponentPage(MenuComponentPage entity)
        {
            var oldEntity = _menuRepository.GetMenuComponentPage(entity.MenuComponentPageId);
            var menu = _menuRepository.GetMenuComponent(oldEntity.MenuComponentId);
            menu.Name = entity.Name;
            menu.Description = menu.Description.Split('-').FirstOrDefault() + "- " + entity.Name;
            menu.PageStatic = (int)PageStaticEnum.Dinamic;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _menuRepository.UpdateMenuComponent(menu);
                    entity.MenuComponentId = oldEntity.MenuComponentId;
                    _menuRepository.UpdateMenuComponentPage(entity);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    var log = new AuditLog()
                    {
                        LogLevel = AuditLogLevel.Error,
                        Description = ex.Message + " " + ex.StackTrace,
                        InputParam = entity,
                        IntegrationType = AuditLogType.MenuService
                    };
                    _auditLogHelper.LogAuditFail(log);
                    throw ex;
                }
            }
        }

        public void DeleteMenuComponentPage(MenuComponentPage entity)
        {
            var item = _menuRepository.GetMenuComponentPage(entity.MenuComponentPageId);
            entity.MenuComponentId = item.MenuComponentId;
            _menuRepository.DeleteMenuComponentPage(entity);
        }

        public IEnumerable<MenuType> GetMenuTypes()
        {
            var types = new List<MenuType>();
            types.Add(new MenuType { MenuTypeId = MenuTypeCon.Left, Name = "Izquierda" });
            types.Add(new MenuType { MenuTypeId = MenuTypeCon.Top, Name = "Superior" });
            types.Add(new MenuType { MenuTypeId = MenuTypeCon.Right, Name = "Derecho" });
            types.Add(new MenuType { MenuTypeId = MenuTypeCon.Mobile, Name = "Mobile" });

            return types;
        }
    }
}
