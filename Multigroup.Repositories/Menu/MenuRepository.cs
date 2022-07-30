using Multigroup.DomainModel.Menu;
using Multigroup.Repositories.Menu.Resources;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace Multigroup.Repositories.Menu
{
    public class MenuRepository : BaseRepository
    {
        public MenuComponent GetMenuComponent(int menuComponentId)
        {
            var data = Db.ExecuteSprocAccessor(StoredProcedures.pub_MenuComponent_Get, MenuMapper.Mapper, menuComponentId);
            return GetFirstElement(data);
        }

        public IEnumerable<MenuComponent> GetMenuComponents(string menuType, int? roleId = null)
        {
            var data = Db.ExecuteSprocAccessor(StoredProcedures.pub_MenuComponent_GetList, MenuMapper.Mapper, roleId, menuType);
            return data;
        }

        public int AddMenuComponent(MenuComponent entity)
        {
            var command = Db.GetStoredProcCommand(StoredProcedures.pub_MenuComponent_Insert);

            //Db.AddInParameter(command, "@MenuComponentId", DbType.Int32, entity.MenuComponentId);
            Db.AddInParameter(command, "@Name", DbType.String, entity.Name);
            Db.AddInParameter(command, "@ParentId", DbType.Int32, entity.ParentId);
            Db.AddInParameter(command, "@SortOrder", DbType.Int32, entity.SortOrder);
            Db.AddInParameter(command, "@ButtonId", DbType.String, entity.ButtonId);
            Db.AddInParameter(command, "@Description", DbType.String, entity.Description);
            Db.AddInParameter(command, "@MenuType", DbType.String, entity.MenuType);
            Db.AddInParameter(command, "@PageStatic", DbType.Int32, entity.PageStatic);
            Db.AddInParameter(command, "@Icon", DbType.String, entity.Icon);
            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdateMenuComponent(MenuComponent entity)
        {
            var command = Db.GetStoredProcCommand(StoredProcedures.pub_MenuComponent_Update);

            Db.AddInParameter(command, "@MenuComponentId", DbType.Int32, entity.MenuComponentId);
            Db.AddInParameter(command, "@Name", DbType.String, entity.Name);
            Db.AddInParameter(command, "@ParentId", DbType.Int32, entity.ParentId);
            Db.AddInParameter(command, "@SortOrder", DbType.Int32, entity.SortOrder);
            Db.AddInParameter(command, "@ButtonId", DbType.String, entity.ButtonId);
            Db.AddInParameter(command, "@Description", DbType.String, entity.Description);
            Db.AddInParameter(command, "@MenuType", DbType.String, entity.MenuType);
            Db.AddInParameter(command, "@PageStatic", DbType.Int32, entity.PageStatic);
            Db.AddInParameter(command, "@Icon", DbType.String, entity.Icon);

            var id = Db.ExecuteScalar(command);
        }

        public void DeleteMenuComponent(MenuComponent entity)
        {
            var command = Db.GetStoredProcCommand(StoredProcedures.pub_MenuComponent_Delete);

            Db.AddInParameter(command, "@MenuComponentId", DbType.Int32, entity.MenuComponentId);
            Db.ExecuteScalar(command);
        }

        public int AddMenuComponentRole(MenuComponentRole entity)
        {
            var command = Db.GetStoredProcCommand(StoredProcedures.pub_MenuComponentRole_Insert);

            Db.AddInParameter(command, "@RoleId", DbType.Int32, entity.RoleId);
            Db.AddInParameter(command, "@MenuComponentId", DbType.Int32, entity.MenuId);
            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void DeleteMenuComponentRole(MenuComponentRole entity)
        {
            var command = Db.GetStoredProcCommand(StoredProcedures.pub_MenuComponentRole_Delete);

            Db.AddInParameter(command, "@RoleId", DbType.Int32, entity.RoleId);
            Db.AddInParameter(command, "@MenuComponentId", DbType.Int32, entity.MenuId);
            Db.ExecuteScalar(command);
        }

        public IEnumerable<MenuComponentPage> GetMenuComponentPages()
        {
            var data = Db.ExecuteSprocAccessor(StoredProcedures.pub_MenuComponentPage_GetList, MenuComponentPageMapper.Mapper);
            return data;
        }

        public MenuComponentPage GetMenuComponentPage(int menuComponentPageId)
        {
            var data = Db.ExecuteSprocAccessor(StoredProcedures.pub_MenuComponentPage_Get, MenuComponentPageMapper.Mapper, menuComponentPageId);
            return GetFirstElement(data);
        }

        public int AddMenuComponentPage(MenuComponentPage entity)
        {
            var command = Db.GetStoredProcCommand(StoredProcedures.pub_MenuComponentPage_Insert);

            Db.AddInParameter(command, "@Name", DbType.String, entity.Name);
            Db.AddInParameter(command, "@HtmlContent", DbType.String, entity.HtmlContent);
            Db.AddInParameter(command, "@UserId", DbType.Int32, entity.UserId);
            Db.AddInParameter(command, "@MenuComponentId", DbType.Int32, entity.MenuComponentId);
            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdateMenuComponentPage(MenuComponentPage entity)
        {
            var command = Db.GetStoredProcCommand(StoredProcedures.pub_MenuComponentPage_Update);

            Db.AddInParameter(command, "@MenuComponentPageId", DbType.Int32, entity.MenuComponentPageId);
            Db.AddInParameter(command, "@Name", DbType.String, entity.Name);
            Db.AddInParameter(command, "@HtmlContent", DbType.String, entity.HtmlContent);
            Db.AddInParameter(command, "@UserId", DbType.Int32, entity.UserId);
            Db.AddInParameter(command, "@MenuComponentId", DbType.Int32, entity.MenuComponentId);
            Db.ExecuteNonQuery(command);
        }

        public void DeleteMenuComponentPage(MenuComponentPage entity)
        {
            var command = Db.GetStoredProcCommand(StoredProcedures.pub_MenuComponentPage_Delete);

            Db.AddInParameter(command, "@MenuComponentPageId", DbType.Int32, entity.MenuComponentPageId);
            Db.AddInParameter(command, "@MenuComponentId", DbType.Int32, entity.MenuComponentId);
            Db.ExecuteNonQuery(command);
        }
    }
}
