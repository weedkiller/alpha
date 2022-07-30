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
    public class SalaryAllocationController : Controller
    {
        private SalaryAllocationService _salaryAllocationService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProviderService _providerService;
        private PurchaseRequestService _purchaseRequestService;
        private ProfileProvider _profileProvider;

        public SalaryAllocationController()
        {
            _salaryAllocationService = new SalaryAllocationService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _providerService = new ProviderService();
            _purchaseRequestService = new PurchaseRequestService();
            _profileProvider = new ProfileProvider();
        }

        // GET: /SalaryAllocation/
        [LogActionFilter]
        [Description("SalaryAllocations")]
        public ActionResult Index()
        {
            SalaryAllocationFilterVm model = new SalaryAllocationFilterVm();
            var collections = new CollectionHelper();
            model.ListProvider = new SelectList(_providerService.GetEmployeesWithId(), "ProviderId", "Name");
            model.ListUserAuth = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.Authorized = true;
            return PartialView("~/Views/SalaryAllocation/SalaryAllocation.cshtml", model);
        }

        [LogActionFilter]
        [Description("SalarySettlement")]
        public ActionResult IndexSettlement()
        {
            SalaryAllocationFilterVm model = new SalaryAllocationFilterVm();
            var collections = new CollectionHelper();
            model.ListProvider = new SelectList(_providerService.GetEmployeesWithId(), "ProviderId", "Name");
            return PartialView("~/Views/SalaryAllocation/SalarySettlement.cshtml", model);
        }

        [LogActionFilter]
        [Description("SalarySettlement")]
        public ActionResult IndexGenerateSettlement()
        {
            SalaryAllocationFilterVm model = new SalaryAllocationFilterVm();
            var collections = new CollectionHelper();
            model.ListProvider = new SelectList(_providerService.GetEmployeesWithId(), "ProviderId", "Name");
            return PartialView("~/Views/SalaryAllocation/SalarySettlementCreate.cshtml", model);
        }

        public ActionResult GetTable(int? SelectedProvider, string DateFromStart, string DateToStart, string DateFromEnd, string DateToEnd, string AmountFrom, string AmountTo,  string isAuthorized, int? SelectedAuthorized)
        {
            var model = SalaryAllocationTableVm.ToViewModel(_salaryAllocationService.GetSalaryAllocationsSummary(SelectedProvider, DateFromStart, DateToStart, DateFromEnd, DateToEnd, AmountFrom,  AmountTo, isAuthorized, SelectedAuthorized));
            return PartialView("~/Views/SalaryAllocation/SalaryAllocationTable.cshtml", model);
        }

        public ActionResult GetTableSettlement(int? SelectedProvider, string Period)
        {
            var model = SalaryAllocationTableVm.ToViewModel(_salaryAllocationService.GetSalarySettlementSummary(SelectedProvider, Period));
            return PartialView("~/Views/SalaryAllocation/SalarySettlementTable.cshtml", model);
        }

        public ActionResult GetTableGenerateSettlement(string Period)
        {
            if (Period != "")
            { 
            var model = SalaryAllocationTableVm.ToViewModel(_salaryAllocationService.GetGenerateSalarySettlementSummary(Period));
            Session["SessionGenerateSettlement"] = model;
            Session["Period"] = Period;
            return PartialView("~/Views/SalaryAllocation/SalaryGenerateSettlementTable.cshtml", model);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe ingresar Periodo");
        }

        public ActionResult RemoveEmployee(int salaryAllocationId)
        {
            SalaryAllocationTableVm principal = (SalaryAllocationTableVm)Session["SessionGenerateSettlement"];
            IEnumerable<SalaryAllocationListVm> lista = (IEnumerable<SalaryAllocationListVm>)principal.SalaryAllocationList;
            List<SalaryAllocationListVm> list = lista.ToList();
            var item = list.FirstOrDefault(x => x.SalaryAllocationId == salaryAllocationId);
            list.Remove(item);

            principal.SalaryAllocationList = list;

            Session["SessionGenerateSettlement"] = principal;
            return PartialView("~/Views/SalaryAllocation/SalaryGenerateSettlementTable.cshtml", principal);
        }

        [LogActionFilter]
        [Description("Crear SalaryAllocations")]
        public ActionResult Create()
        {
            var model = new SalaryAllocationVm();
            var collections = new CollectionHelper();
            model.IsAuthorized = true;
            model.ddlProvider = collections.GetEmployeesIdsSingleSelectVm();
            model.ddlUserAuthorized = collections.GetUsersSingleSelectVm();
            return PartialView("~/Views/SalaryAllocation/SalaryAllocationCreate.cshtml", model);
        }

        public ActionResult ConfirmSalary(int id)
        {
            try
            {
                SalaryAllocation salary = _salaryAllocationService.GetSalaryAllocationsById(id);
                if (salary.UserAuthorizedId != _profileProvider.CurrentUserId)
                    ViewBag.ErrorMessageAuth = "true";
                else
                {
                    salary.IsAuthorized = true;
                    _salaryAllocationService.UpdateSalaryAllocation(salary);
                    ViewBag.ErrorMessageAuth = "false";                   
                }
                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al Confirmar");
            }
        }

        [HttpPost]
        public ActionResult Create(SalaryAllocationVm model)
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
                var salaryAllocation = model.ToModelObject();

                if (salaryAllocation.EndDate.Date < salaryAllocation.StartDate.Date)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Inicio debe ser mayor a la fecha de Fin");

                    salaryAllocation.IsAuthorized = false;
                    _salaryAllocationService.AddSalaryAllocation(salaryAllocation);

                return Index();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        public ActionResult GenerateSettlement(SalaryAllocationFilterVm filtro)
        {
            try
            {
                SalaryAllocationTableVm principal = (SalaryAllocationTableVm)Session["SessionGenerateSettlement"];
                IEnumerable<SalaryAllocationListVm> lista = (IEnumerable<SalaryAllocationListVm>)principal.SalaryAllocationList;
                List<Int32> ListaIds = new List<Int32>();
                foreach (SalaryAllocationListVm salary in lista)
                    ListaIds.Add(salary.SalaryAllocationId);
                    
                _salaryAllocationService.GenerateSettlement(ListaIds,(string)Session["Period"], _profileProvider.CurrentUserId, filtro.Description);
                lista = new List<SalaryAllocationListVm>();
                Session["SessionGenerateSettlement"] = lista;
                return IndexSettlement();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar liquidar los sueldos.");
            }
        }

        public ActionResult View(int id)
        {
            var entity = _salaryAllocationService.GetSalaryAllocationsById(id);
            var model = SalaryAllocationVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetEmployeesIdsSingleSelectVm();
            model.ddlUserAuthorized = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUserAuthorized.SelectedItem = entity.UserAuthorizedId.ToString();
            return PartialView("~/Views/SalaryAllocation/SalaryAllocationView.cshtml", model);
        }


        public ActionResult Edit(int id)
        {
            var entity = _salaryAllocationService.GetSalaryAllocationsById(id);
            var model = SalaryAllocationVm.FromDomainModel(entity);
            var collections = new CollectionHelper();          
            model.ddlUserAuthorized = collections.GetUsersSingleSelectVm();
            if (model.IsAuthorized)
                model.ddlProvider = collections.GetEmployeesByIdSingleSelectVm(entity.ProveedorId);
            else
                model.ddlProvider = collections.GetEmployeesIdsSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUserAuthorized.SelectedItem = entity.UserAuthorizedId.ToString();

            return PartialView("~/Views/SalaryAllocation/SalaryAllocationEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(SalaryAllocationVm model)
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
                SalaryAllocation salaryOld = _salaryAllocationService.GetSalaryAllocationsById(model.SalaryAllocationId);
                SalaryAllocation salary = model.ToModelObject();

                if (salary.EndDate.Date < salary.StartDate.Date)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Inicio debe ser mayor a la fecha de Fin");
                if (salaryOld.IsAuthorized)
                {
                    salary.IsAuthorized = true;
                    if (salaryOld.EndDate < salary.EndDate)
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Fin no puede aumentarse");

                    if (salaryOld.LastMonth != null && salaryOld.LastMonth != "")
                    {
                        String Month = salary.EndDate.Month.ToString();
                        String Year = salary.EndDate.Year.ToString();

                        string[] aux = salaryOld.LastMonth.Split('-');

                        string MonthLast = aux[0];
                        string YearLast = aux[1];

                        if (int.Parse(Year) < int.Parse("20" + YearLast))
                            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Fin no puede ser menor al ultimo mes imputado");
                        else if (int.Parse(Year) == int.Parse("20" + YearLast))
                        {
                            if (int.Parse(Month) < int.Parse(MonthLast))
                                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Fin no puede ser menor al ultimo mes imputado");
                        }
                    }
                }

                _salaryAllocationService.UpdateSalaryAllocation(salary);

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

    }
}