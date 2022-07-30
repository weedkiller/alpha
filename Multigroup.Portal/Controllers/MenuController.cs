using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Constants;
using Multigroup.Portal.Models.Menu;
using Multigroup.Portal.Providers;
using Multigroup.Services.Menu;
using Multigroup.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Controllers
{
    public class MenuController : Controller
    {
        private UserLoginProvider _userLoginProvider;
        private ProfileProvider _profileProvider;
        private MenuService _menuService;

        public MenuController()
        {
            _userLoginProvider = new UserLoginProvider();
            _profileProvider = new ProfileProvider();
            _menuService = new MenuService();
        }

        // GET: Menu
        public ActionResult Index()
        {
            var items = _menuService.GetCurrentMenu(_profileProvider.CurrentUserId, MenuTypeCon.Left);
            IEnumerable<MenuComponentVm> menu = MenuComponentVm.ToViewModel(items);
            return PartialView("MenuAdmin", menu);
        }
    }
}
