using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Filters;
using Multigroup.Portal.Models.Shared;
using Multigroup.Portal;
using Multigroup.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Multigroup.Portal.Providers;
using System.Web.Script.Serialization;
using Multigroup.Common.Logging;
using Multigroup.Portal.Helpers;

namespace Multigroup.Portal.Controllers.Shared
{
    public class UserController : Controller
    {
        private UserService _userService;
        private ZoneService _zoneService;
        private RoleService _roleService;
        private UserLoginProvider _userLoginProvider;
        private ProfileProvider _profileProvider;
        private EntityLogService _entityLogService;
        private AuditLogHelper _auditLogHelper;
        private CashierService _cashierService;

        public UserController()
        {
            _userService = new UserService();
            _zoneService = new ZoneService();
            _roleService = new RoleService();
            _userLoginProvider = new UserLoginProvider();
            _profileProvider = new ProfileProvider();
            _entityLogService = new EntityLogService();
            _auditLogHelper = new AuditLogHelper();
            _cashierService = new CashierService();
        }
        [LogActionFilter]
        [Description("Usuarios")]
        public ActionResult Index()
        {
            IEnumerable<UserSummary> users = _userService.GetUserSummary();
            var user = _profileProvider.CurrentUserId;
            var rol = _userService.GetUser(user).Roles;
            ViewBag.Rol = rol.FirstOrDefault().RoleId;
            return PartialView("~/Views/Security/User/UserIndex.cshtml", UserListVm.FromDomainModel(users));
        }

        public ActionResult UpdateLimit(int id, double Limit)
        {
            _userService.UpdateLimit(id, Limit);
            IEnumerable<UserSummary> users = _userService.GetUserSummary();
            var user = _profileProvider.CurrentUserId;
            var rol = _userService.GetUser(user).Roles;
            ViewBag.Rol = rol.FirstOrDefault().RoleId;
            return PartialView("~/Views/Security/User/UserIndex.cshtml", UserListVm.FromDomainModel(users));
        }

        [LogActionFilter]
        [Description("Detalle de Usuarios")]
        public ActionResult Details(int id)
        {
            var model = new UserVm();
            var user = _userService.GetUser(id);
            var collections = new CollectionHelper();

            var modelDetail = model.FromModelObject(user);
            modelDetail.ListRoles = new SelectList(_roleService.GetRoles(), "RoleId", "Description", modelDetail.SelectedRole);

            modelDetail.ddlUserType = collections.GetUserTypeSingleSelectVm();
            modelDetail.ddlUserType.SelectedItem = user.UserType.ToString();

            var userCashier = _userService.GetUserCashierByUserId(id);
            if (userCashier == null)
                userCashier = new UserCashier();

            modelDetail.ddlCashier = collections.GetCashierSingleSelectVm();
            modelDetail.ddlCashier.SelectedItem = userCashier.CashierId.ToString();


            return PartialView("~/Views/Security/User/UserDetails.cshtml", modelDetail);
        }
        [LogActionFilter]
        [Description("Crear Usuarios")]
        public ActionResult Create()
        {
            var user = _profileProvider.CurrentUserId;
            var rol = _userService.GetUser(user).Roles;
            var model = new UserVm();
            var collections = new CollectionHelper();
            model.ListRoles = new SelectList(_roleService.GetRolesByRol(rol.FirstOrDefault().RoleId), "RoleId", "Description");
            model.ddlUserType = collections.GetUserTypeSingleSelectVm();
            model.ListZones = new SelectList(_zoneService.GetZones(), "ZoneId", "Name");
            model.ddlGerente = collections.GetUsersByTypeSingleSelectVm(3);
            model.ddlSupervisor = collections.GetUsersByTypeSingleSelectVm(4);
            model.ddlCashier = collections.GetCashierSingleSelectVm();
            return PartialView("~/Views/Security/User/UserCreate.cshtml", model);
        }
        public JsonResult GetSellers()
        {
            try
            {
                var response = _userService.GetSellers();
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "Error al recuperar vendedores.";
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Create(UserVm userVm)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, message);
                }

                var error = ValidateUser(userVm, false);

                if (!string.IsNullOrEmpty(error))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, error);
                }

                User user = userVm.ToModelObject();
                Cashier cashier = _cashierService.GetCashiersById(int.Parse(userVm.ddlCashier.SelectedItem));

                IEnumerable<UserCashier> userCashier = _userService.GetUserCashiers();
                foreach (UserCashier userC in userCashier)
                {
                    if (int.Parse(userVm.ddlCashier.SelectedItem) == userC.CashierId && cashier.Individual == true)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La Caja es individual y ya esta asignada a: " + _userService.GetUser(userC.UserId).UserName);
                }

                _userService.AddUser(user, int.Parse(userVm.ddlCashier.SelectedItem));
                _userLoginProvider.CreateAccount(new User { UserName = userVm.UserName, Password = userVm.Password });

                string action = "Create";
                string EntityNew = new JavaScriptSerializer().Serialize(userVm);
                string EntityOld = "";
                InsertLogUser(action, EntityNew, EntityOld);

                return Index();
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = userVm,
                    IntegrationType = AuditLogType.RoleService
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar.");
            }
        }
        public void InsertLogUser(string action, string EntityNew, String EntityOld)
        {
            DateTime _date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            int UserId = _profileProvider.CurrentUserId;

            string AdditionalData = HttpContext.Session.SessionID.ToString();
            AdditionalData = AdditionalData + "," + HttpContext.Server.MachineName.ToString();
            AdditionalData = AdditionalData + "," + HttpContext.User.ToString();

            _entityLogService.Insert(_date, action, UserId, EntityNew, EntityOld, AdditionalData);
        }
        [LogActionFilter]
        [Description("Editar Usuarios")]
        public ActionResult Edit(int id)
        {

            var rol = _userService.GetUser(id).Roles;

            var model = new UserVm();
            var user = _userService.GetUser(id);
            var collections = new CollectionHelper();
            var userCashier = _userService.GetUserCashierByUserId(id);
            if (userCashier == null)
                userCashier = new UserCashier();

            var modelUser = model.FromModelObject(user);

            modelUser.ListRoles = new SelectList(_roleService.GetRolesByRol(rol.FirstOrDefault().RoleId), "RoleId", "Description", modelUser.SelectedRole);

            modelUser.ddlUserType = collections.GetUserTypeSingleSelectVm();
            modelUser.ddlUserType.SelectedItem = user.UserType.ToString();

            modelUser.ddlGerente = collections.GetUsersByTypeSingleSelectVm(3);
            modelUser.ddlGerente.SelectedItem = user.IdGerente.ToString();

            modelUser.ddlSupervisor = collections.GetUsersByTypeSingleSelectVm(4);
            modelUser.ddlSupervisor.SelectedItem = user.IdSupervisor.ToString();

            modelUser.ddlCashier = collections.GetCashierSingleSelectVm();
            modelUser.ddlCashier.SelectedItem = userCashier.CashierId.ToString();


            modelUser.ListZones = new SelectList(_zoneService.GetZones(), "ZoneId", "Name", modelUser.SelectedZone);

            return PartialView("~/Views/Security/User/UserEdit.cshtml", modelUser);
        }

        [HttpPost]
        public ActionResult Edit(UserVm userVm)
        {
            try
            {
                var error = ValidateUser(userVm, true);
                bool cambioType = false;
                if (!string.IsNullOrEmpty(error))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, error);
                }
                var user = userVm.ToModelObject();
                var userEdit = _userService.GetUser(userVm.UserId);
                var typeOld = userEdit.UserType;
                if (userEdit.UserType != user.UserType)
                    cambioType = true;

                Cashier cashier = _cashierService.GetCashiersById(int.Parse(userVm.ddlCashier.SelectedItem));
                IEnumerable<UserCashier> userCashier = _userService.GetUserCashiers();
                foreach (UserCashier userC in userCashier)
                {
                    if (int.Parse(userVm.ddlCashier.SelectedItem) == userC.CashierId && cashier.Individual == true && userC.UserId != userVm.UserId)
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La Caja es individual y ya esta asignada a: " + _userService.GetUser(userC.UserId).UserName);
                }

                _userService.UpdatetUser(user, int.Parse(userVm.ddlCashier.SelectedItem));
                if (cambioType)
                    _userService.AddUserTypeHistory(user.UserType.Value, typeOld.Value, userVm.Observations, user.UserId);
                

                if (userVm.ChangePassword && string.IsNullOrEmpty(userVm.Password))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe ingresar una contraseña");
                }
                if (userVm.ChangePassword && userVm.Password == userVm.ConfirmPassword && !string.IsNullOrEmpty(userVm.Password))
                {
                    _userLoginProvider.ResetPassword(user);
                }

                return Index();
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = userVm,
                    IntegrationType = AuditLogType.CustomerService
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        public ActionResult UserTypeHistory(int id)
        {
            var model = UserTypeHistoryTableVm.ToViewModel(_userService.GetUserTypeHistoryByUserId(id));
            return PartialView("~/Views/Security/User/UserTypeHistoryTable.cshtml", model);
        }

        private string ValidateUser(UserVm userVm, bool isEdition)
        {
            string errorMsg = string.Empty;

            if (!ModelState.IsValid)
            {
                errorMsg = "Hubo un error al crear tu cuenta. Por favor, compruebe si los datos introducidos son incorrectos.";
            }

            if (_userService.existeUsuario(userVm.IdentificationNumber, userVm.UserName))
            {
                errorMsg = "Ya existe un Usuario con ese Documento.";
            }

            if (WebSecurity.UserExists(userVm.UserName) && !isEdition)
            {
                errorMsg = "Ya existe una cuenta para este nombre de usuario. Por favor, compruebe que los datos introducidos son correctos.";
            }
            if (userVm.Password != userVm.ConfirmPassword)
            {
                errorMsg = "Las contraseñas no coinciden";
            }
            if ((string.IsNullOrEmpty(userVm.Password) | string.IsNullOrEmpty(userVm.ConfirmPassword)) && !isEdition)
            {
                errorMsg = "Debe ingresar una contraseña";
            }

            return errorMsg;
        }
    }
}
