using Multigroup.Portal.Models;
using Multigroup.Portal.Models.Account;
using Multigroup.Portal.Providers;
using Multigroup.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Controllers
{
    public class HomeController : Controller
    {
        private ProfileProvider _profileProvider;
        private UserService _userService;

        public HomeController()
        {
            _profileProvider = new ProfileProvider();
            _userService = new UserService();
        }
        public ActionResult Index()
        {
            ViewBag.roleId = _userService.GetUser(_profileProvider.CurrentUserId).Roles.Select(x => x.RoleId.ToString()).FirstOrDefault();
            ViewBag.UserId = _profileProvider.CurrentUserId.ToString();
            var model = new HomeVm();
            var profile = new UserProfileVm();
            profile.UserName = _profileProvider.CurrentUserName;
            model.UserProfile = profile;
            return View(model);
        }

        public ActionResult Dashboard()
        {
            ViewBag.roleId = _userService.GetUser(_profileProvider.CurrentUserId).Roles.Select(x => x.RoleId.ToString()).FirstOrDefault();
            return PartialView("~/Views/Dashboard/Dashboard.cshtml");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}