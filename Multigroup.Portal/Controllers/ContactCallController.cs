using Multigroup.Common.Logging;
using Multigroup.Portal.Filters;
using Multigroup.Portal.Models.Maintenance;
using Multigroup.Portal.Models.ContactCallModel;
using Multigroup.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multigroup.Portal.Helpers;
using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Providers;
using Multigroup.Portal.Utilities;
using System.Text.RegularExpressions;

namespace Multigroup.Portal.Controllers.ContactCall
{
    public class ContactCallController : Controller
    {
        private ContactCallService _ContactCallService;
        private IAuditLogHelper _auditLogHelper;
        private UserService _userService;
        private ProfileProvider _profileProvider;
        private List<HttpPostedFileBase> _filesLoad;
        public ContactCallController()
        {
            _ContactCallService = new ContactCallService();
            _auditLogHelper = new AuditLogHelper();
            _userService = new UserService();
            _profileProvider = new ProfileProvider();
        }

        // GET: /ContactCall/
        [LogActionFilter]
        [Description("ContactCalls")]
        public ActionResult Index()
        {
            ContactCallFilterVm model = new ContactCallFilterVm();
            var collections = new CollectionHelper();
            var user = _profileProvider.CurrentUserId;
            var usuario = _userService.GetUser(user);
            var rol = _userService.GetUser(user).Roles;
            model.ListStatus = new SelectList(_ContactCallService.GetContactCallStates(), "ContactCallStateId", "Description");
            model.ListManager = new SelectList(_userService.GetUsersByType(3), "Id", "Name");
            model.ListSupervisor = new SelectList(_userService.GetUsersByType(4), "Id", "Name");
            model.ListSeller = new SelectList(_userService.GetUsersByType(1), "Id", "Name");
            model.ddlManager = collections.GetUsersByTypeSingleSelectVm(3);
            model.ddlSupervisor = collections.GetUsersByTypeSingleSelectVm(4);
            model.ddlSeller = collections.GetUsersByTypeSingleSelectVm(1);
            if (rol.FirstOrDefault().RoleId == 13)
            {
                return PartialView("~/Views/ContactCall/ContactCall.cshtml", model);
            }
            else if (usuario.UserType == 3)
            {
                return PartialView("~/Views/ContactCall/ContactCallManager.cshtml", model);
            }
            else if (usuario.UserType == 4)
            {
                return PartialView("~/Views/ContactCall/ContactCallSupervisor.cshtml", model);
            }
            else if (usuario.UserType == 1)
            {
                return PartialView("~/Views/ContactCall/ContactCallSeller.cshtml", model);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "No tiene permitida esta accíon");

        }
        public ActionResult GetTable(List<int> SelectedManager, List<int> SelectedSupervisor, List<int> SelectedSeller, List<int> ContactCallStatus, string DateFrom, string DateTo)
        {
            var user = _profileProvider.CurrentUserId;
            var rol = _userService.GetUser(user).Roles;
            ViewBag.Rol = rol.FirstOrDefault().RoleId;
            var usuario = _userService.GetUser(user);
            string Manager = (SelectedManager.Count() > 0) ? String.Join(",", SelectedManager) : "0";
            string Supervisor = (SelectedSupervisor.Count() > 0) ? String.Join(",", SelectedSupervisor) : "0";
            string Seller = (SelectedSeller.Count() > 0) ? String.Join(",", SelectedSeller) : "0";
            string Status = (ContactCallStatus.Count() > 0) ? String.Join(",", ContactCallStatus) : "0";
            if (DateFrom == "")
                DateFrom = "01/01/1900";
            if (DateTo == "")
                DateTo = "01/01/2100";
            if (usuario.UserType == 3)
            {
                Manager = user.ToString();
            }
            else if (usuario.UserType == 4)
            {
                Supervisor = user.ToString();
            }
            else if (usuario.UserType == 1)
            {
                Seller = user.ToString();
            }
            var list = _ContactCallService.GetContactCallSummaryByFilter(Status, DateFrom, DateTo, Manager, Supervisor, Seller);
            var model = ContactCallTableVm.ToViewModel(list);
            return PartialView("~/Views/ContactCall/ContactCallTable.cshtml", model);
        }

        public ActionResult GetTableSeller(List<int> SelectedManager, List<int> SelectedSupervisor, List<int> SelectedSeller, List<int> ContactCallStatus, string DateFrom, string DateTo)
        {
            var user = _profileProvider.CurrentUserId;
            var rol = _userService.GetUser(user).Roles;
            ViewBag.Rol = rol.FirstOrDefault().RoleId;
            var usuario = _userService.GetUser(user);
            string Manager = (SelectedManager.Count() > 0) ? String.Join(",", SelectedManager) : "0";
            string Supervisor = (SelectedSupervisor.Count() > 0) ? String.Join(",", SelectedSupervisor) : "0";
            string Seller = (SelectedSeller.Count() > 0) ? String.Join(",", SelectedSeller) : "0";
            string Status = (ContactCallStatus.Count() > 0) ? String.Join(",", ContactCallStatus) : "0";
            if (DateFrom == "")
                DateFrom = "01/01/1900";
            if (DateTo == "")
                DateTo = "01/01/2100";
            if (usuario.UserType == 3)
            {
                Manager = user.ToString();
            }
            else if (usuario.UserType == 4)
            {
                Supervisor = user.ToString();
            }
            else if (usuario.UserType == 1)
            {
                Seller = user.ToString();
            }
            var list = _ContactCallService.GetContactCallSummaryByFilter(Status, DateFrom, DateTo, Manager, Supervisor, Seller);
            var model = ContactCallTableVm.ToViewModel(list);
            return PartialView("~/Views/ContactCall/ContactCallTableSeller.cshtml", model);
        }

        [HttpPost]
        public JsonResult AssignManager(int Manager, List<string> Contacts)
        {
            try
            {
                foreach (var contact in Contacts)
                {
                    var contactCall = _ContactCallService.GetContactCallsById(Int32.Parse(contact));
                    contactCall.ManagerId = Manager;
                    _ContactCallService.UpdateContactCalls(contactCall);
                }

                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "Error al asisgnar gerente";
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AssignSupervisor(int Supervisor, List<string> Contacts)
        {
            try
            {
                foreach (var contact in Contacts)
                {
                    var contactCall = _ContactCallService.GetContactCallsById(Int32.Parse(contact));
                    contactCall.SupervisorId = Supervisor;
                    _ContactCallService.UpdateContactCalls(contactCall);
                }

                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "Error al asisgnar supervisor";
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AssignSeller(int Seller, List<string> Contacts)
        {
            try
            {
                foreach (var contact in Contacts)
                {
                    var contactCall = _ContactCallService.GetContactCallsById(Int32.Parse(contact));
                    contactCall.SellerId = Seller;
                    _ContactCallService.UpdateContactCalls(contactCall);
                }

                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "Error al asisgnar vendedor.";
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [LogActionFilter]
        [Description("Crear comentario de contacto")]
        public ActionResult CallStatus(int id)
        {
            var contact = _ContactCallService.GetContactCallsById(id);
            var model = new ContactCallVm();
            var collections = new CollectionHelper();
            model.ddlContactCallState = collections.GetContactCallStatusSingleSelectVm();
            model.ddlContactCallState.SelectedItem = contact.ContactCallStateId.ToString();
            model.ContactCallId = id;
            return PartialView("~/Views/ContactCall/CallModal.cshtml", model);
        }

        [HttpPost]
        public ActionResult CallStatus(ContactCallVm model)
        {
            try
            {
                if (model.ContactTime == null || model.ContactTime.Equals("") || !EsHora(model.ContactTime))
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe ingresar hora válida");

                if (model.ContactDate == null || model.ContactDate.Equals(""))
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe ingresar Fecha");

                var contactCall = _ContactCallService.GetContactCallsById(model.ContactCallId);
                contactCall.ContactCallStateId = Util.ParseIntNullable(model.ddlContactCallState.SelectedItem);
                contactCall.Commentary = model.Commentary;
                contactCall.ContactDate = Util.ParseDateTime(model.ContactDate);
                contactCall.ContactTime = model.ContactTime;
                _ContactCallService.UpdateContactCalls(contactCall);
                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        public static bool EsHora(string cadena)
        {
            string expresion;
            expresion = @"^([0-1]?[0-9]|[2][0-3]):([0-5][0-9])(:[0-5][0-9])?$";
            System.Text.RegularExpressions.Regex automata = new Regex(expresion);
            return automata.IsMatch(cadena);
        }

        public ActionResult OpenAttach()
        {
            var model = new ContactCallVm();

            return PartialView("~/Views/ContactCall/ContactCallAttach.cshtml", model);
        }

        public ActionResult LoadFile()
        {
            bool isSavedSuccessfully = true;

            if (SessionProvider.UpLoadFiles == null)
            {
                _filesLoad = new List<HttpPostedFileBase>();
            }
            else
            {
                _filesLoad = SessionProvider.UpLoadFiles;
            }

            string fName = string.Empty;

            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName + DateTime.Now.ToString();
                    if (file != null && file.ContentLength > 0)
                    {
                        _filesLoad.Add(file);
                        SessionProvider.UpLoadFiles = _filesLoad;
                    }
                }
            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error al cargar el archivo" });
            }
        }

        public ActionResult RemoveFiles(string fileDelete)
        {
            bool isSavedSuccessfully = true;
            try
            {
                if (SessionProvider.UpLoadFiles != null)
                {
                    _filesLoad = SessionProvider.UpLoadFiles;
                    HttpPostedFileBase file = null;

                    foreach (HttpPostedFileBase fileName in _filesLoad)
                    {
                        if (fileName.FileName.Equals(fileDelete))
                        {
                            file = fileName;
                        }
                    }

                    if (file != null)
                    {
                        _filesLoad.Remove(file);
                    }
                    SessionProvider.UpLoadFiles = _filesLoad;
                }
            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = "ok" });
            }
            else
            {
                return Json(new { Message = "Error al eliminar el archivo" });
            }
        }



        [HttpPost]
        public ActionResult CargarArchivos()
        {
            try
            {
                var ruta = SaveExcel();
                _ContactCallService.CargarExcel(ruta);
                if (SessionProvider.UpLoadFiles != null)
                {
                    _filesLoad = SessionProvider.UpLoadFiles;
                    HttpPostedFileBase file = null;

                    foreach (HttpPostedFileBase fileName in _filesLoad)
                    {
                        file = fileName;
                    }

                    if (file != null)
                    {
                        _filesLoad.Remove(file);
                    }
                    SessionProvider.UpLoadFiles = _filesLoad;
                }
                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        public string SaveExcel()
        {
            string fName = string.Empty;
            var ruta = "";
            try
            {
                if (SessionProvider.UpLoadFiles != null)
                {
                    foreach (HttpPostedFileBase fileName in SessionProvider.UpLoadFiles)
                    {
                        //   HttpPostedFileBase file = fileName;
                        //Save file content goes here
                        if (fileName != null && fileName.ContentLength > 0)
                        {
                             ruta = _ContactCallService.InsertExcel(fileName);
                        }
                    }
                }
                return ruta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SessionProvider.UpLoadFiles = null;
                
            }
        }


        [LogActionFilter]
        [Description("Crear ContactCalls")]
        public ActionResult Create()
        {
            var model = new ContactCallVm();
            var collections = new CollectionHelper();
            model.ddlManager = collections.GetUsersByTypeSingleSelectVm(3);
            return PartialView("~/Views/ContactCall/ContactCallCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(ContactCallVm model)
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
                var contactCall = model.ToModelObject();
                contactCall.ContactCallStateId = 1;
                contactCall.SystemDate = DateTime.Now;
                _ContactCallService.AddContactCall(contactCall);
                return Index();
            }
            catch (Exception ex)
            {             

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
        [LogActionFilter]
        [Description("Editar ContactCalls")]
        public ActionResult Edit(int id)
        {

            var entity = _ContactCallService.GetContactCallsById(id);
            var model = ContactCallVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlManager = collections.GetUsersByTypeSingleSelectVm(3);
            model.ddlManager.SelectedItem = entity.ManagerId.ToString();         
            return PartialView("~/Views/ContactCall/ContactCallEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(ContactCallVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _ContactCallService.UpdateContactCalls(model.ToModelObject());

                return Index();
            }
            catch (Exception ex)
            {
               return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
	}
}