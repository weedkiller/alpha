using Multigroup.Common.Logging;
using Multigroup.Portal.Filters;
using Multigroup.Portal.Providers;
using Multigroup.Portal.Models.ContractModel;
using Multigroup.Portal.Models.Shared;
using Multigroup.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multigroup.Portal.Helpers;
using System.Globalization;
using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Utilities;


namespace Multigroup.Portal.Controllers
{
    public class ContractController : Controller
    {
        private ProfileProvider _profileProvider;
        private UserLoginProvider _userLoginProvider;
        private ContractService _contractService;
        private AgencyService _agencyService;
        private IAuditLogHelper _auditLogHelper;
        private List<HttpPostedFileBase> _filesLoad;
        private CustomerService _customerService;
        private UserService _userService;
        private PaymentService _paymentService;
        private GeneralService _generalService;
        private CityService _cityService;


        public ContractController()
        {
            _profileProvider = new ProfileProvider();
            _userLoginProvider = new UserLoginProvider();
            _contractService = new ContractService();
            _agencyService = new AgencyService();
            _auditLogHelper = new AuditLogHelper();
            _customerService = new CustomerService();
            _userService = new UserService();
            _paymentService = new PaymentService();
            _generalService = new GeneralService();
            _cityService = new CityService();
        }

        // GET: Contract
        public ActionResult Index()
        {
            ContractFilterVm model = new ContractFilterVm();
            var collections = new CollectionHelper();
            model.ListStatus = new SelectList(_contractService.GetStatusContract("Status"), "StatusContractId", "Description");
            model.ListAgency = new SelectList(_agencyService.GetAgencys(), "AgencyId", "Description");
            model.ListStatusDetail = new SelectList(_contractService.GetStatusContract("StatusDetail"), "StatusContractId", "Description");
            model.ListContractType = new SelectList(_contractService.GetContractTypes(), "ContractTypeId", "Description");
            var customer = _contractService.GetCustomer();
            var items = customer.Select(x => new Item { Id = x.CustomerId, Name = x.Surname + " " + x.Name });
            model.ListClient = new SelectList(items, "Id", "Name");
            return PartialView("~/Views/Contract/Contract.cshtml", model);
        }

        [HttpPost]
        public ActionResult GetTable(List<int> SelectedStatus, List<int> SelectedStatusDetail, List<int> SelectedClient, List<int> ContractType, int? SelectedAgency, string DateFrom, string DateTo)
        {
            var user = _profileProvider.CurrentUserId;
            var rol = _userService.GetUser(user).Roles;
            ViewBag.Rol = rol.FirstOrDefault().RoleId;
            string client = (SelectedClient.Count() > 0) ? String.Join(",", SelectedClient) : "0";
            string status = (SelectedStatus.Count() > 0) ? String.Join(",", SelectedStatus) : "0";
            string statusDetail = (SelectedStatusDetail.Count() > 0) ? String.Join(",", SelectedStatusDetail) : "0";
            string contract = (ContractType.Count() > 0) ? String.Join(",", ContractType) : "0";
            var list = _contractService.GetContractByFilter(status, statusDetail, client, contract, SelectedAgency, DateFrom, DateTo);
            var model = ContractTableVm.ToViewModel(list);
            return PartialView("~/Views/Contract/ContractTable.cshtml", model);
        }

        [HttpPost]
        public ActionResult GetCuotas(int id)
        {
            var list = _contractService.GetContractCuotas(id);
            var model = ContractTableCuotasVm.ToViewModel(list);
            return PartialView("~/Views/Contract/ContractCuotas.cshtml", model);
        }

        public ActionResult IndexTMK()
        {
            ContractFilterVm model = new ContractFilterVm();
            var collections = new CollectionHelper();
            model.ListStatus = new SelectList(_contractService.GetStatusContract("Status"), "StatusContractId", "Description");
            model.ListAgency = new SelectList(_agencyService.GetAgencys(), "AgencyId", "Description");
            model.ListStatusDetail = new SelectList(_contractService.GetStatusContract("StatusDetail"), "StatusContractId", "Description");
            model.ListContractType = new SelectList(_contractService.GetContractTypes(), "ContractTypeId", "Description");
            var customer = _contractService.GetCustomer();
            var items = customer.Select(x => new Item { Id = x.CustomerId, Name = x.Surname + " " + x.Name });
            model.ListClient = new SelectList(items, "Id", "Name");
            return PartialView("~/Views/Contract/ContractTMK.cshtml", model);
        }

        [HttpPost]
        public ActionResult GetTableTMK(List<int> SelectedStatus, List<int> SelectedStatusDetail, List<int> SelectedClient, List<int> ContractType, int? SelectedAgency, string DateFrom, string DateTo)
        {
            var user = _profileProvider.CurrentUserId;
            var rol = _userService.GetUser(user).Roles;
            ViewBag.Rol = rol.FirstOrDefault().RoleId;
            string client = (SelectedClient.Count() > 0) ? String.Join(",", SelectedClient) : "0";
            string status = (SelectedStatus.Count() > 0) ? String.Join(",", SelectedStatus) : "0";
            string statusDetail = (SelectedStatusDetail.Count() > 0) ? String.Join(",", SelectedStatusDetail) : "0";
            string contract = (ContractType.Count() > 0) ? String.Join(",", ContractType) : "0";
            var list = _contractService.GetContractByFilterTMK(status, statusDetail, client, contract, SelectedAgency, DateFrom, DateTo, _profileProvider.CurrentUserId);
            var model = ContractTableVm.ToViewModel(list);
            return PartialView("~/Views/Contract/ContractTableTMK.cshtml", model);
        }

        // GET: Contract
        public ActionResult IndexWeb()
        {
            ContractFilterVm model = new ContractFilterVm();
            var collections = new CollectionHelper();
            model.ListStatus = new SelectList(_contractService.GetStatusContract("ContractWeb"), "StatusContractId", "Description");
            var customer = _contractService.GetCustomer();
            var items = customer.Select(x => new Item { Id = x.CustomerId, Name = x.Surname + " " + x.Name });
            model.ddlGerente = collections.GetUsersByTypeSingleSelectVm(3);
            model.ddlSupervisor = collections.GetUsersByTypeSingleSelectVm(4);
            model.ListClient = new SelectList(items, "Id", "Name");
            return PartialView("~/Views/Contract/ContractWeb.cshtml", model);
        }


        [HttpPost]
        public ActionResult GetTableWeb(List<int> SelectedStatus, List<int> SelectedClient, string DateFrom, string DateTo, int? SelectedSupervisor, int? SelectedGerente)
        {
            var user = _profileProvider.CurrentUserId;
            var rol = _userService.GetUser(user).Roles;
            ViewBag.Rol = rol.FirstOrDefault().RoleId;
            string client = (SelectedClient.Count() > 0) ? String.Join(",", SelectedClient) : "0";
            string status = (SelectedStatus.Count() > 0) ? String.Join(",", SelectedStatus) : "0";
            int supervisor = (SelectedSupervisor == null) ? 0 : SelectedSupervisor.Value;
            int gerente = (SelectedGerente == null) ? 0 : SelectedGerente.Value;
            var list = _contractService.GetContractWebByFilter(status, client, DateFrom, DateTo, user, supervisor, gerente);
            var model = ContractTableVm.ToViewModel(list);

            var total = _contractService.GetContractWebByFilterTotal(status, client, DateFrom, DateTo, user, supervisor, gerente);
            ViewBag.Cuota1 = total.Amount1.ToString();
            ViewBag.Cuota2 = total.Amount2.ToString();

            return PartialView("~/Views/Contract/ContractTableWeb.cshtml", model);
        }

        // GET: Contract
        public ActionResult IndexWebDelete()
        {
            ContractFilterVm model = new ContractFilterVm();
            var collections = new CollectionHelper();
            model.ListStatus = new SelectList(_contractService.GetStatusContract("ContractWeb"), "StatusContractId", "Description");
            var customer = _contractService.GetCustomer();
            var items = customer.Select(x => new Item { Id = x.CustomerId, Name = x.Surname + " " + x.Name });
            model.ddlGerente = collections.GetUsersByTypeSingleSelectVm(3);
            model.ddlSupervisor = collections.GetUsersByTypeSingleSelectVm(4);
            model.ListClient = new SelectList(items, "Id", "Name");
            return PartialView("~/Views/Contract/ContractWebDelete.cshtml", model);
        }


        [HttpPost]
        public ActionResult GetTableWebDelete(List<int> SelectedStatus, List<int> SelectedClient, string DateFrom, string DateTo, int? SelectedSupervisor, int? SelectedGerente)
        {
            var user = _profileProvider.CurrentUserId;
            var rol = _userService.GetUser(user).Roles;
            ViewBag.Rol = rol.FirstOrDefault().RoleId;
            string client = (SelectedClient.Count() > 0) ? String.Join(",", SelectedClient) : "0";
            string status = (SelectedStatus.Count() > 0) ? String.Join(",", SelectedStatus) : "0";
            int supervisor = (SelectedSupervisor == null) ? 0 : SelectedSupervisor.Value;
            int gerente = (SelectedGerente == null) ? 0 : SelectedGerente.Value;
            var list = _contractService.GetContractWebByFilter(status, client, DateFrom, DateTo, 0, supervisor, gerente);
            var model = ContractTableVm.ToViewModel(list);

            return PartialView("~/Views/Contract/ContractTableWebDelete.cshtml", model);
        }

        // GET: Contract/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [LogActionFilter]
        [Description("Crear Customers")]
        public ActionResult Create()
        {
            var model = new ContractVm();
            var collections = new CollectionHelper();
            model.ddlIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
            model.ddlMaritalStatus = collections.GetMaritalStatusSingleSelectVm();
            model.ddlSpouseIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
            model.ddlCity = collections.GetCitySingleSelectVm();
            model.ddlSeller = collections.GetSellerSingleSelectVm();
            model.ddlManager = collections.GetUsersByTypeSingleSelectVm(3);
            model.ddlSupervisor = collections.GetUsersByTypeSingleSelectVm(4);
            model.ddlPaymentDate = collections.GetPaymentDateSingleSelectVm();
            model.ddlAgency = collections.GetAgencySingleSelectVm();
            model.ddlPaymentPreference = collections.GetPaymentPreferenceSingleSelectVm();
            model.ddlPaymentPlace= collections.GetPaymentPlaceSingleSelectVm();
            model.ddlContractType = collections.GetContractTypeSingleSelectVm();
            model.ddlState = collections.GetStateSingleSelectVm();

            return PartialView("~/Views/Contract/ContractCreate.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Contract Web")]
        public ActionResult CreateWeb()
        {
            var model = new ContractVm();
            var collections = new CollectionHelper();
            model.ddlIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
            model.ddlMaritalStatus = collections.GetMaritalStatusSingleSelectVm();
            model.ddlSpouseIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
            model.ddlCity = collections.GetCitySingleSelectVm();
            model.ddlSeller = collections.GetSellerSingleSelectVm();
            model.ddlManager = collections.GetUsersByTypeSingleSelectVm(3);
            model.ddlSupervisor = collections.GetUsersByTypeSingleSelectVm(4);
            model.ddlPaymentDate = collections.GetPaymentDateSingleSelectVm();
            model.ddlAgency = collections.GetAgencySingleSelectVm();
            model.ddlPaymentPreference = collections.GetPaymentPreferenceSingleSelectVm();
            model.ddlPaymentPlace = collections.GetPaymentPlaceSingleSelectVm();
            model.ddlContractType = collections.GetContractTypeSingleSelectVm();
            model.ddlState = collections.GetStateSingleSelectVm();

            return PartialView("~/Views/Contract/ContractCreateWeb.cshtml", model);
        }


        [LogActionFilter]
        [Description("Crear Contract House")]
        public ActionResult CreateHouse()
        {
            var model = new ContractHouseVm();
            var collections = new CollectionHelper();
            model.ddlIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
            model.ddlMaritalStatus = collections.GetMaritalStatusSingleSelectVm();
            model.ddlSpouseIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
            model.ddlCity = collections.GetCitySingleSelectVm();
            model.ddlSeller = collections.GetSellerSingleSelectVm();
            model.ddlManager = collections.GetUsersByTypeSingleSelectVm(3);
            model.ddlSupervisor = collections.GetUsersByTypeSingleSelectVm(4);
            model.ddlPaymentDate = collections.GetPaymentDateSingleSelectVm();
            model.ddlAgency = collections.GetAgencySingleSelectVm();
            model.ddlPaymentPreference = collections.GetPaymentPreferenceSingleSelectVm();
            model.ddlPaymentPlace = collections.GetPaymentPlaceSingleSelectVm();
            model.ddlContractType = collections.GetContractTypeSingleSelectVm();
            model.ddlState = collections.GetStateSingleSelectVm();

            return PartialView("~/Views/Contract/ContractHouseCreate.cshtml", model);
        }
        // POST: Contract/Create
        [HttpPost]
        public ActionResult Create(ContractVm model)
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
                if (!_contractService.ExisteContractPaper(model.Number))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El Número de contrato ingresado no existe en el sistema.");
                }
                if (_contractService.ExisteContract(model.Number))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El Número de contrato ya ha sido cargado");
                }
                if (model.ddlPaymentDate == null || model.ddlPaymentDate.SelectedItem == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar fecha de Pago");
                }
                else
                {
                    _contractService.AddContract(model.ToModelObject(), _profileProvider.CurrentUserId);
                    return Index();
                }
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = model,
                    IntegrationType = AuditLogType.CustomerService
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        [HttpPost]
        public ActionResult CreateWeb(ContractWebSummaryVm model)
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
                if (model.ddlPaymentDate == null || model.ddlPaymentDate.SelectedItem == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar fecha de Pago");
                }
                else if((!model.Quotation.HasValue || model.Quotation.Value == 0) && (!model.QuotationDolar.HasValue || model.QuotationDolar.Value == 0))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe ingresar Cotización");
                }
                else if (model.QuotationDate == null || model.QuotationDate.Equals(""))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar fecha de Cotización");
                }
                else
                {
                    QuotationContract quotation = new QuotationContract();
                    quotation.Quotation = model.Quotation;
                    quotation.QuotationDolar = model.QuotationDolar;
                    quotation.QuotationDate = Util.ParseDateTime(model.QuotationDate);
                    _contractService.AddContractWeb(model.ToModelObject(), _profileProvider.CurrentUserId, quotation);
                    return new HttpStatusCodeResult(HttpStatusCode.OK, "Contrato creado con exito");
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        // POST: Contract/CreateHouse
        [HttpPost]
        public ActionResult CreateHouse(ContractHouseVm model)
            {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }
                if (!_contractService.ExisteContractPaperHouse(model.Number))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El Número de contrato ingresado no existe en el sistema.");
                }
                if (_contractService.ExisteContractHouse(model.Number))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El Número de contrato ya ha sido cargado");
                }
                if (model.ddlPaymentDate == null || model.ddlPaymentDate.SelectedItem == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar fecha de Pago");
                }
                else
                {
                    _contractService.AddContract(model.ToModelObject(), _profileProvider.CurrentUserId);
                    return Index();
                }
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = model,
                    IntegrationType = AuditLogType.CustomerService
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        // GET: Contract/Edit/5
        public ActionResult Edit(int id)
        {
            var entity = _contractService.GetContract(id);
            var model = ContractVm.FromDomainModel(entity);        
            var collections = new CollectionHelper();
            model.ddlSeller = collections.GetSellerSingleSelectVm();
            model.ddlSeller.SelectedItem = entity.SellerId.ToString();

            model.ddlManager = collections.GetUsersByTypeSingleSelectVm(3);
            model.ddlManager.SelectedItem = entity.ManagerId.ToString();

            model.ddlSupervisor = collections.GetUsersByTypeSingleSelectVm(4);
            model.ddlSupervisor.SelectedItem = entity.SupervisorId.ToString();
            
            model.ddlPaymentDate = collections.GetPaymentDateSingleSelectVm();
            model.ddlPaymentDate.SelectedItem = entity.PaymentDateId.ToString();

            model.ddlAgency = collections.GetAgencySingleSelectVm();
            model.ddlAgency.SelectedItem = entity.AgencyId.ToString();

            model.ddlPaymentPreference = collections.GetPaymentPreferenceSingleSelectVm();
            model.ddlPaymentPreference.SelectedItem = entity.PaymentPreference.ToString();

            model.ddlPaymentPlace = collections.GetPaymentPlaceSingleSelectVm();
            model.ddlPaymentPlace.SelectedItem = entity.PaymentPlace.ToString();

            model.ddlContractType = collections.GetContractTypeSingleSelectVm();
            model.ddlContractType.SelectedItem = entity.ContractType.ToString();


            ViewBag.ContractId = id;
            return PartialView("~/Views/Contract/ContractEdit.cshtml", model);
        }

        public ActionResult View(int id)
        {
            var entity = _contractService.GetContract(id);
            var model = ContractVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlSeller = collections.GetSellerSingleSelectVm();
            model.ddlSeller.SelectedItem = entity.SellerId.ToString();

            model.ddlManager = collections.GetUsersByTypeSingleSelectVm(3);
            model.ddlManager.SelectedItem = entity.ManagerId.ToString();

            model.ddlSupervisor = collections.GetUsersByTypeSingleSelectVm(4);
            model.ddlSupervisor.SelectedItem = entity.SupervisorId.ToString();

            model.ddlPaymentDate = collections.GetPaymentDateSingleSelectVm();
            model.ddlPaymentDate.SelectedItem = entity.PaymentDateId.ToString();

            model.ddlAgency = collections.GetAgencySingleSelectVm();
            model.ddlAgency.SelectedItem = entity.AgencyId.ToString();

            model.ddlPaymentPreference = collections.GetPaymentPreferenceSingleSelectVm();
            model.ddlPaymentPreference.SelectedItem = entity.PaymentPreference.ToString();

            model.ddlPaymentPlace = collections.GetPaymentPlaceSingleSelectVm();
            model.ddlPaymentPlace.SelectedItem = entity.PaymentPlace.ToString();

            model.ddlContractType = collections.GetContractTypeSingleSelectVm();
            model.ddlContractType.SelectedItem = entity.ContractType.ToString();


            ViewBag.ContractId = id;
            return PartialView("~/Views/Contract/ContractView.cshtml", model);
        }
        // POST: Contract/Edit/5
        [HttpPost]
        public ActionResult Edit(ContractVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }
                _contractService.UpdateContracts(model.ToModelObject());
                SaveFile(model);

                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        public ActionResult DeleteContract(int id)
        {
            try
            {
                _contractService.DeleteContracts(id);
                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
        public ActionResult EditHouse(int id)
        {
            var entity = _contractService.GetContract(id);
            var model = ContractHouseVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlSeller = collections.GetSellerSingleSelectVm();
            model.ddlSeller.SelectedItem = entity.SellerId.ToString();

            model.ddlManager = collections.GetUsersByTypeSingleSelectVm(3);
            model.ddlManager.SelectedItem = entity.ManagerId.ToString();

            model.ddlSupervisor = collections.GetUsersByTypeSingleSelectVm(4);
            model.ddlSupervisor.SelectedItem = entity.SupervisorId.ToString();

            model.ddlPaymentDate = collections.GetPaymentDateSingleSelectVm();
            model.ddlPaymentDate.SelectedItem = entity.PaymentDateId.ToString();

            model.ddlAgency = collections.GetAgencySingleSelectVm();
            model.ddlAgency.SelectedItem = entity.AgencyId.ToString();

            model.ddlPaymentPreference = collections.GetPaymentPreferenceSingleSelectVm();
            model.ddlPaymentPreference.SelectedItem = entity.PaymentPreference.ToString();

            model.ddlPaymentPlace = collections.GetPaymentPlaceSingleSelectVm();
            model.ddlPaymentPlace.SelectedItem = entity.PaymentPlace.ToString();

            model.ddlContractType = collections.GetContractTypeSingleSelectVm();
            model.ddlContractType.SelectedItem = entity.ContractType.ToString();


            ViewBag.ContractId = id;
            return PartialView("~/Views/Contract/ContractHouseEdit.cshtml", model);
        }
        // POST: Contract/Edit/5
        [HttpPost]
        public ActionResult EditHouse(ContractHouseVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }
                _contractService.UpdateContracts(model.ToModelObject());

                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
        [LogActionFilter]
        [Description("Crear Customers")]
        public ActionResult ChangeStatus(int id)
        {
            var entity = _contractService.GetContract(id);
            var model = ContractVm.FromDomainModel(entity);
            var collections = new CollectionHelper();

            model.ddlStatus = collections.GetStatusSingleSelectVm();
            model.ddlStatus.SelectedItem = entity.Status.ToString();

            model.ddlStatusDetail = collections.GetStatusDetailSingleSelectVm();
            model.ddlStatusDetail.SelectedItem = entity.StatusDetail.ToString();
            return PartialView("~/Views/Contract/StatusModal.cshtml", model);
        }


        public IEnumerable<SelectListItem> Months
        {
            get
            {
                return Enumerable.Range(1, 12).Select(x => new SelectListItem
                {
                    Value = x.ToString(),
                    Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(x)
                });
            }
        }
        [HttpPost]
        public ActionResult ChangeStatus(ContractVm model)
        {
            try
            {
                // TODO: Add update logic here
                if (int.Parse(model.ddlStatus.SelectedItem) == 18 & (model.StartMonth == null || model.StartMonth == ""))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                if (Convert.ToDateTime(model.StartMonth) < DateTime.Now.Date & int.Parse(model.ddlStatus.SelectedItem) == 18)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de inicio debe ser igual o mayor a la actual");
                }
                var contrato = _contractService.GetContract(model.ContractId);
                if (int.Parse(model.ddlStatus.SelectedItem) == 2)
                {
                    IEnumerable<Payment> pagos = _paymentService.GetPaymentByClientId(contrato.CustomerId).Where(p => p.type == "Cuota");
                    if (pagos.Count() > 0)
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El contrato ya tiene pagos efectuados");
                }
                int status = contrato.Status.Value;
                if (contrato.PaymentDateId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El Contrato no tiene fecha preferida de Pago");
                }
                _contractService.UpdateContractStatus(model.ContractId, int.Parse(model.ddlStatus.SelectedItem), int.Parse(model.ddlStatusDetail.SelectedItem), 3, model.StartMonth);
                _contractService.AddStatusContractHistory(model.ContractId, _profileProvider.CurrentUserId, status, int.Parse(model.ddlStatus.SelectedItem), model.ObservationsStatus);
                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        
        // POST: Contract/Delete/5
        [HttpPost]
        public void Delete(int id)
        {
            _contractService.DeleteInterestById(id);
        }
        [HttpPost]
        public void DeleteLastPayment(int contractId)
        {
            _contractService.DeleteLastPayment(contractId);
        }
        [HttpPost]
        public void ReacomodarCuotas(int id, int InstallmentNumber)
        {
            _contractService.UpdateInstallmentDate(id, InstallmentNumber);
        }

        public ActionResult GetSavedFilesByContract(int ContractId)
        {
            var result = _contractService.GetContractDocuments(ContractId);
            var model = result.Select(x => new DataUploadFilesResultVm { Name = x.Name, Length = x.ContentLength, Path = x.Path });
            List<DataUploadFilesResultVm> resultado = new List<DataUploadFilesResultVm>();
            foreach (DataUploadFilesResultVm file in model)
            {
                resultado.Add(file);
            }
            Contract contrato = _contractService.GetContract(ContractId);
            if (contrato.Number.ToString().StartsWith("9999"))
            { 
                string folder = "Files\\Multigroup\\Contracts\\" + contrato.Number.ToString().Remove(0, 4);
                resultado.Add(new DataUploadFilesResultVm { Name = "contract.pdf", Length = 2390, Path = folder });
                try
                {
                    string folderScoring = "Files\\Multigroup\\Contracts\\" + contrato.Number.ToString();
                    resultado.Add(new DataUploadFilesResultVm { Name = "Scoring.pdf", Length = 2390, Path = folderScoring });
                }
                catch (Exception ex)
                {

                }
            }
            model = resultado;
            return PartialView("~/Views/Contract/ContractAttach.cshtml", model);
        }


        public ActionResult InterestList(int id)
        {
            var model = InterestTableVm.ToViewModel(_contractService.GetInstallmentInsterestsByContractId(id));
            return PartialView("~/Views/Contract/InterestTable.cshtml", model);
        }

        public ActionResult StatusChangeList(int id)
        {
            var model = StatusChangeTableVm.ToViewModel(_contractService.GetStatusContractHistoryByContractId(id));
            return PartialView("~/Views/Contract/StatusChangeTable.cshtml", model);
        }

        public void SaveFile(ContractVm contract)
        {
            string fName = string.Empty;

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
                            _contractService.InsertDocument(fileName, contract.ContractId, contract.Number.ToString());
                        }
                    }
                }
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

        public void SaveFileWeb(ContractWebVm contract)
        {
            string fName = string.Empty;

            try
            {
                if (SessionProvider.UpLoadFiles != null)
                {
                    _contractService.DeleteContractFileWeb(contract.ContractId);
                    foreach (HttpPostedFileBase fileName in SessionProvider.UpLoadFiles)
                    {
                        //   HttpPostedFileBase file = fileName;
                        //Save file content goes here
                        if (fileName != null && fileName.ContentLength > 0)
                        {
                            _contractService.InsertDocumentWeb(fileName, contract.ContractId, contract.ContractId.ToString());
                        }
                    }
                }
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
        public JsonResult GetContractByDNI(int DNI)
        {
            try
            {
                if (!_customerService.ExisteCustomerByDNI(DNI.ToString()))
                {
                    Response.StatusCode = 500;
                    Response.StatusDescription = "El DNI ingresado no corresponde a un cliente registrado.";
                    return Json("El DNI ingresado no corresponde a un cliente registrado.", JsonRequestBehavior.AllowGet);
                }
                else
                {                               
                    var response = _contractService.GetContractByDNI(DNI);
                    if (response.Any())
                        return Json(response, JsonRequestBehavior.AllowGet);
                    else
                    {
                        Response.StatusCode = 500;
                        Response.StatusDescription = "El DNI ingresado no posee planes en vigencia.";
                        return Json("El DNI ingresado no posee planes en vigencia.", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "El DNI ingresado no corresponde a un cliente registrado.";
                return Json("Error al recuperar cliente", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAmountByContractId(int ContractId, string PaymentDate)
        {
            try
            {
                var response = _contractService.GetAmountByContractId(ContractId, PaymentDate);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "Error al recuperar Monto.";
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GeneralStatus(int id)
        {
            try
            {
                var response = _contractService.GetContractResume(id);
                var model = ContractStatusVm.FromDomainModel(response);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "Error al cargar datos.";
                return Json("Error al cargar datos", JsonRequestBehavior.AllowGet);
            }
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

        [AllowAnonymous]
        public ActionResult SignContract(string id)
        {
            _userLoginProvider.Login("nuevoingarx", "nuevoingarx");

            var idFinal = _generalService.DesEncriptar(id);
            var entity = _contractService.GetContractWeb(int.Parse(idFinal));
            var quotation = _contractService.GetQuotationContract(int.Parse(idFinal));
            var model = ContractVm.FromDomainModel(entity);
            var customer = _customerService.GetCustomersById(entity.CustomerId);
            var collections = new CollectionHelper();
            model.Clave = _generalService.ClaveEncriptar(customer.Name, customer.Surname, customer.IdentificationNumber, entity.Number.ToString());
            model.Name = customer.Name;
            model.Surname = customer.Surname;
            model.IdentificationNumber = customer.IdentificationNumber;
            model.Birthdate = customer.Birthdate.ToString();
            model.Address = customer.Address;
            model.State = customer.State;
            model.ZipCode = customer.ZipCode;
            model.Phone = customer.Phone;
            model.CellPhone = customer.CellPhone;
            model.Email = customer.Email;
            model.Occupation = customer.Occupation;
            model.ContactHours = customer.ContactHours;
            model.SpouseIdentificationNumber = customer.SpouseIdentificationNumber;
            model.SpouseName = customer.SpouseName;
            model.SpouseSurname = customer.SpouseSurname;
            model.CUIT = customer.CUIT;
            model.AddressDetail = customer.AddressDetail;
            model.ddlIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
            model.ddlMaritalStatus = collections.GetMaritalStatusSingleSelectVm();
            model.ddlSpouseIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
            model.ddlCity = collections.GetCitySingleSelectVm();
            model.ddlSeller = collections.GetSellerSingleSelectVm();
            model.ddlManager = collections.GetUsersByTypeSingleSelectVm(3);
            model.ddlSupervisor = collections.GetUsersByTypeSingleSelectVm(4);
            model.ddlPaymentDate = collections.GetPaymentDateSingleSelectVm();
            model.ddlAgency = collections.GetAgencySingleSelectVm();
            model.ddlPaymentPreference = collections.GetPaymentPreferenceSingleSelectVm();
            model.ddlPaymentPlace = collections.GetPaymentPlaceSingleSelectVm();
            model.ddlContractType = collections.GetContractTypeSingleSelectVm();
            model.ddlState = collections.GetStateSingleSelectVm();
            model.ddlSeller = collections.GetSellerSingleSelectVm();
            model.ddlSeller.SelectedItem = entity.SellerId.ToString();

            model.ddlManager = collections.GetUsersByTypeSingleSelectVm(3);
            model.ddlManager.SelectedItem = entity.ManagerId.ToString();

            model.ddlSupervisor = collections.GetUsersByTypeSingleSelectVm(4);
            model.ddlSupervisor.SelectedItem = entity.SupervisorId.ToString();

            model.ddlPaymentDate = collections.GetPaymentDateSingleSelectVm();
            model.ddlPaymentDate.SelectedItem = entity.PaymentDateId.ToString();

            model.ddlAgency = collections.GetAgencySingleSelectVm();
            model.ddlAgency.SelectedItem = entity.AgencyId.ToString();

            model.ddlPaymentPreference = collections.GetPaymentPreferenceSingleSelectVm();
            model.ddlPaymentPreference.SelectedItem = entity.PaymentPreference.ToString();

            model.ddlPaymentPlace = collections.GetPaymentPlaceSingleSelectVm();
            model.ddlPaymentPlace.SelectedItem = entity.PaymentPlace.ToString();

            model.ddlContractType = collections.GetContractTypeSingleSelectVm();
            model.ddlContractType.SelectedItem = entity.ContractType.ToString();

            model.Seller = entity.SellerId.ToString();
            model.Supervisor = entity.SupervisorId.ToString();
            model.Manager = entity.ManagerId.ToString();
            model.IdentificationType = _customerService.GetIdentificationType(customer.IdentificationType.Value).Description;
            if (customer.MaritalStatus.HasValue)
            model.MaritalStatus = _customerService.GetMaritalStatusById(customer.MaritalStatus.Value).Description;
            model.City = _cityService.GetcitysById(customer.CityId.Value).Name;
            if (customer.SpouseIdentificationType.HasValue)
                model.SpouseIdentificationType = _customerService.GetIdentificationType(customer.SpouseIdentificationType.Value).Description;
            if (entity.PaymentDateId.HasValue)
            model.PaymentDate = _paymentService.GetPaymentDate(entity.PaymentDateId.Value).Description;

            model.QuotationString = (quotation.Quotation.HasValue) ? quotation.Quotation.Value.ToString() : " - ";
            model.QuotationDolarString = (quotation.QuotationDolar.HasValue) ? quotation.QuotationDolar.Value.ToString() : " - ";
            model.QuotationDate = quotation.QuotationDate.Value.ToShortDateString();

            ViewBag.ContractId = id;
            return PartialView("~/Views/Contract/SignContract.cshtml", model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ChargeSign(ContractWebVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }
                if (_contractService.IsContractSign(model.ContractId))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El contrato ya ha sido firmado con anterioridad");
                }
                else 
                {               
                Contract contract = _contractService.GetContractWeb(model.ContractId);
                User user = _userService.GetUser(contract.SellerId.Value);
                SaveFileWeb(model);
                _contractService.NotifySignContract(contract.ContractId, contract.CustomerId, user, model.htmlContract);
                _contractService.UpdateContractWebStatus(contract.ContractId);
                return Index();
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
            finally
            {
                _userLoginProvider.Logout();

            }
        }

        //[AllowAnonymous]
        [HttpPost]
        public ActionResult ConfirmContract(ContractWebVm model)
        {
            try
            {
                IEnumerable<Contract> contratos= _contractService.GetContractByNumber("9999" + model.ContractId.ToString());
                if (contratos.Count() > 0)
                     new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
                else
                     _contractService.ConfirmContracts(model.ContractId);
                return IndexWeb();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        [HttpPost]
        public ActionResult DeleteContractWeb(ContractWebVm model)
        {
            try
            {
                IEnumerable<Contract> contratos = _contractService.GetContractByNumber("9999" + model.ContractId.ToString());
                if (contratos.Count() > 0)
                    new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
                else
                    _contractService.DeleteContractWeb(model.ContractId);
                return IndexWebDelete();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
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

        [AllowAnonymous]
        public ActionResult ScoringContract(string id)
        {
            _userLoginProvider.Login("nuevoingarx", "nuevoingarx");

            var idFinal = _generalService.DesEncriptar(id);
            var entity = _contractService.GetContract(int.Parse(idFinal));
            var model = ContractVm.FromDomainModel(entity);

            var customer = _customerService.GetCustomersById(entity.CustomerId);
            var collections = new CollectionHelper();
            model.Clave = _generalService.ClaveEncriptar(customer.Name, customer.Surname, customer.IdentificationNumber, entity.Number.ToString());
            model.Name = customer.Name;
            model.Surname = customer.Surname;

            model.IdentificationNumber = customer.IdentificationNumber;

            ViewBag.ContractId = id;
            return PartialView("~/Views/Contract/ScoringContract.cshtml", model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ChargeScoring(ContractWebVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }
                Contract contract = _contractService.GetContract(model.ContractId);
                User user = _userService.GetUser(contract.SellerId.Value);
                _contractService.NotifyScoringContract(contract.Number, contract.CustomerId, user, model.htmlContract);
                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
            finally
            {
                _userLoginProvider.Logout();

            }
        }

    }

}

