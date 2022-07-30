using Multigroup.Common.Logging;
using Multigroup.Portal.Filters;
using Multigroup.Portal.Models.Administration;
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

namespace Multigroup.Portal.Controllers.Administration
{
    public class EmployeeController : Controller
    {
        private EmployeeService _employeeService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProviderService _providerService;
        private SpendingService _spendingService;
        private SpendingPaymentService _spendingPaymentService;
        private ProfileProvider _profileProvider;
        private CashierService _cashierService;

        public EmployeeController()
        {
            _employeeService = new EmployeeService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _providerService = new ProviderService();
            _spendingPaymentService = new SpendingPaymentService();
            _spendingService = new SpendingService();
            _profileProvider = new ProfileProvider();
            _cashierService = new CashierService();
        }

        // GET: /Provider/
        [LogActionFilter]
        [Description("Employees")]
        public ActionResult Index()
        {
            EmployeeFilterVm model = new EmployeeFilterVm();
            var collections = new CollectionHelper();
            model.Active = true;
            model.ListProviderType = new SelectList(_userTypeService.getUserTypes(), "UserTypeId", "Description");
            return PartialView("~/Views/Employee/Employee.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedProviderType, string DateFrom, string DateTo, string BalanceFrom, string BalanceTo, string isActive)
        {
            var model = EmployeeTableVm.ToViewModel(_employeeService.GetProvidersSummary(SelectedProviderType, DateFrom, DateTo, BalanceFrom,  BalanceTo, isActive));
            return PartialView("~/Views/Employee/EmployeeTable.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Employees")]
        public ActionResult Create()
        {
            var model = new EmployeeVm();
            var collections = new CollectionHelper();
            model.ddlProviderType = collections.GetUserTypeSingleSelectVm();
            return PartialView("~/Views/Employee/EmployeeCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(EmployeeVm model)
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

                if (_employeeService.ExisteProvider(model.Document))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Ya existe un Empleado con ese DNI");
                }

                if (_employeeService.NoExisteUser(model.Document, model.ddlProviderType.SelectedItem))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El tipo de empleado requiere un Usuario registrado en el sistema");
                }

                if (model.UserId == 0)
                {
                    User usuario = _userService.GetUserByDocument(model.Document);
                    if (usuario != null)
                        model.UserId = usuario.UserId;
                }

                _employeeService.AddProvider(model.ToModelObject());
                return Index();
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = model,
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
        [LogActionFilter]
        [Description("Editar Employees")]
        public ActionResult Edit(int id)
        {
            var entity = _employeeService.GetProvidersById(id);
            var model = EmployeeVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProviderType= collections.GetUserTypeSingleSelectVm();
            model.ddlProviderType.SelectedItem = entity.ProviderTypeId.ToString();
            return PartialView("~/Views/Employee/EmployeeEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeVm model)
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

                if (_employeeService.ExisteProviderUpdate(model.Document, model.ProviderId))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Ya existe un Empleado con ese DNI / CUIT");
                }

                if (_employeeService.NoExisteUser(model.Document, model.ddlProviderType.SelectedItem))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El tipo de empleado requiere un Usuario registrado en el sistema");
                }

                if (model.UserId == 0)
                {
                    User usuario = _userService.GetUserByDocument(model.Document);
                    if (usuario != null)
                        model.UserId = usuario.UserId;
                }

                _employeeService.UpdateProvider(model.ToModelObject());

                return Index();
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = model,
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        [LogActionFilter]
        [Description("EmpleadoExiste")]
        public JsonResult GetByDocumentNumber(string DocumentNumber)
        {
            var entity = _userService.GetUserByDocument(DocumentNumber);

            try
            {
                var model = new EmployeeVm();

                model.Email = entity.Email;
                model.Document = entity.IdentificationNumber;
                model.Name = entity.Names + " " + entity.Surname;
                model.UserId = entity.UserId;
               

                var collections = new CollectionHelper();

                model.ddlProviderType = collections.GetIdentificationTypeSingleSelectVm();
                model.ddlProviderType.SelectedItem = entity.UserType.ToString();

                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "El Empleado no existe, carguelo usted mismo.";
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [Description("CurrentAcount")]
        public ActionResult IndexCurrentAcount()
        {
            EmployeeCAFilterVm model = new EmployeeCAFilterVm();
            var collections = new CollectionHelper();
            model.ListProvider = new SelectList(_providerService.GetEmployeesWithId(), "ProviderId", "Name");
            return PartialView("~/Views/Employee/CurrentAccount.cshtml", model);
        }

        public ActionResult GetTableCurrentAccount(List<string> SelectedProvider,string BalanceFrom, string BalanceTo, string NotImput)
        {
            if (BalanceFrom == "")
                BalanceFrom = "-10000000";
            if (BalanceTo == "")
                BalanceTo = "10000000";
            string _selectedProvider = (SelectedProvider[0] != "" && SelectedProvider.Count() > 0) ? String.Join(",", SelectedProvider) : "0";
            var model = EmployeeCATableVm.ToViewModel(_employeeService.GetEmployeeCurrentAcount(_selectedProvider, BalanceFrom, BalanceTo, NotImput));

            return PartialView("~/Views/Employee/CurrentAccountTable.cshtml", model);
        }

        [Description("EmployeeCurrentAcount")]
        public ActionResult IndexEmployeeCurrentAccount(int id)
        {
            var model = EmployeeCADetailTableVm.ToViewModel(_employeeService.GetEmployeeCurrentAcountDetail(id));
            Provider proveedor = _providerService.GetProvidersById(id);
            model.balance = proveedor.Balance;
            model.name = proveedor.Name;
            return PartialView("~/Views/Employee/EmployeeCurrentAccountTable.cshtml", model);
        }

        public ActionResult SallaryPaymentView(int id)
        {
            var entity = _spendingPaymentService.GetSpendingPaymentsById(id);
            var model = SpendingPaymentVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            if (entity.Balance > 0)
            {
                model.AdvancementAmount = entity.Balance;
                model.Advancement = true;
            }

            ViewBag.provider = entity.ProveedorId.ToString();

            return PartialView("~/Views/Employee/SallaryPaymentView.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear SpendingPayments")]
        public ActionResult CreateSallaryPayment(int id)
        {
            var entity = _spendingService.GetSpendingsById(id);

            var model = new SpendingPaymentVm();
            var collections = new CollectionHelper();
            List<SpendingPaymentDetail> lists = new List<SpendingPaymentDetail>();
            model.Details = lists;
            List<SpendingPaymentPaymentMethod> payment = new List<SpendingPaymentPaymentMethod>();
            model.PaymentMethods = payment;
            model.ExecutionDate = DateTime.Now.ToShortDateString();
            model.ddlProvider = collections.GetEmployeesIdsSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUser.SelectedItem = _profileProvider.CurrentUserId.ToString();
            Provider proveedor = _providerService.GetProvidersById(entity.ProveedorId.Value);
            model.Provider = proveedor.Name;
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            model.ddlSpendings = collections.GetSpendingsByEmployeeSingleSelectVm(0);
            model.Saving = _cashierService.GetCashiersById(_userService.GetUserCashierByUserId(_profileProvider.CurrentUserId).CashierId).Name;
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            SpendingPaymentDetail detail = new SpendingPaymentDetail();
            detail.SpendingId = id;
            detail.Total = entity.Balance;
            detail.Spending = entity.Period;
            List<SpendingPaymentDetail> list = (List<SpendingPaymentDetail>)model.Details;
            list.Add(detail);
            model.Details = list;


            double total = 0;

            foreach (SpendingPaymentDetail det in list)
            {
                total = total + det.Total;
            }
            model.Amount = total;

            Session["Total"] = total.ToString();
            model.AmountPayment = total;
            Session["SessionSpendingPayment"] = model;
            Session["PMs"] = new List<int>();

            ViewBag.provider = entity.ProveedorId.ToString();
            ViewBag.ErrorMessageTrabajo = "false";
            return PartialView("~/Views/SallaryPayment/SallaryPaymentCreate.cshtml", model);
        }

        [Description("Imputar Pagos")]
        public ActionResult ImputPayment(int id)
        {
            var entity = _spendingPaymentService.GetSpendingPaymentsById(id);
            Session["ImputProvider"] = entity.ProveedorId.Value.ToString();
            Session["ImputSpending"] = new List<int>();
            SpendingFilterVm model = new SpendingFilterVm();
            List<SpendingPaymentDetail> lists = new List<SpendingPaymentDetail>();
            model.Details = lists;
            model.Balance = entity.Balance;
            model.Imput = 0;
            model.SpendingPaymentId = id;
            Session["SpendingImputModel"] = model;
            Session["Balance"] = entity.Balance;

            ViewBag.provider = entity.ProveedorId.ToString();

            return PartialView("~/Views/SallaryPayment/Spending.cshtml", model);
        }
    }
}