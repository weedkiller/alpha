using Multigroup.Common.Logging;
using Multigroup.Portal.Filters;
using Multigroup.Portal.Models.Maintenance;
using Multigroup.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multigroup.Portal.Helpers;
using Multigroup.Portal.Models.Collection;
using Multigroup.DomainModel.Shared;


namespace Multigroup.Portal.Controllers.Maintenance
{
    public class CustomerController : Controller
    {
        private CustomerService _customerService;
        private CityService _cityService;
        private ZoneService _zoneService;
        private IAuditLogHelper _auditLogHelper;
        private PaymentService _paymentService;
        private VisitService _visitsService;

        public CustomerController()
        {
            _customerService = new CustomerService();
            _auditLogHelper = new AuditLogHelper();
            _zoneService = new ZoneService();
            _paymentService = new PaymentService();
            _cityService = new CityService();
            _visitsService = new VisitService();
        }
        [LogActionFilter]
        [Description("CustomerHistory")]
        public ActionResult CustomerHistory(int id)
        {
            IEnumerable<Payment> pagos = _paymentService.GetPaymentByClientId(id);
            var model = PaymentTableVm.ToViewModel(pagos);
            double total = 0;

            foreach (Payment pago in pagos)
            {
                total += pago.Amount;
            }
            ViewBag.Total = total;
            return PartialView("~/Views/Maintenance/Customer/CustomerHistory.cshtml", model);
        }
        public ActionResult CustomerObservations(int id)
        {
            IEnumerable<AgencyPaymentHistory> pagos = _visitsService.GetAgencyPaymentHistoryByCustomer(id);
            var model = AgencyPaymentHistoryTableVm.ToViewModel(pagos);
            return PartialView("~/Views/Maintenance/Customer/CustomerObservations.cshtml", model);
        }
        public ActionResult CustomerObservationsLast(int id)
        {
            IEnumerable<AgencyPaymentHistory> pagos = _visitsService.GetAgencyPaymentHistoryByCustomerLastMonth(id);
            var model = AgencyPaymentHistoryTableVm.ToViewModel(pagos);
            return PartialView("~/Views/Maintenance/Customer/CustomerObservations.cshtml", model);
        }

        // GET: /Customer/
        [LogActionFilter]
        [Description("Customers")]
        public ActionResult Index()
        {
            CustomerFilterVm model = new CustomerFilterVm();
            // ACA DEVOLVER LO QUE SE NECESITE PARA LOS DOS COMBOS
            model.ListStatus = new SelectList(_customerService.GetStatusClient(), "StatusClientId", "Description");
            model.ListZones = new SelectList(_zoneService.GetZones(), "ZoneId", "Name");
            return PartialView("~/Views/Maintenance/Customer/Customer.cshtml", model);
        }
        public ActionResult GetTable(List<int> SelectedStatus, List<int> SelectedZones)
        {
            var model = CustomerTableVm.ToViewModel(_customerService.GetCustomersByStatusAndZone(SelectedStatus, SelectedZones));
            return PartialView("~/Views/Maintenance/Customer/CustomerTable.cshtml", model);
        }

        //public ActionResult GetCustomerByUser(List<int> SelectedStatus, List<int> SelectedZones, List<int> SelectedPayment)
        //{
        //    var model = CustomerTableVm.ToViewModel(_customerService.GetCustomersByStatusAndZoneAndPayment(SelectedStatus, SelectedZones, SelectedPayment));
        //    return PartialView("~/Views/Maintenance/Customer/CustomerTable.cshtml", model);
        //}

        [LogActionFilter]
        [Description("Crear Customers")]
        public ActionResult Create()
        {
            var model = new CustomerVm();
            var collections = new CollectionHelper();
            model.ddlIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
            model.ddlMaritalStatus = collections.GetMaritalStatusSingleSelectVm();
            model.ddlSpouseIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
            model.ddlCity = collections.GetCitySingleSelectVm();
            model.ddlState = collections.GetStateSingleSelectVm();
            return PartialView("~/Views/Maintenance/Customer/CustomerCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(CustomerVm model)
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
                if (_customerService.ExisteCustomerByDNI(model.IdentificationNumber))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El DNI ingresado pertenece a un cliente registrado.");
                }
                else
                {
                    _customerService.AddCustomer(model.ToModelObject());
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
        public ActionResult CreateContract(CustomerVm model)
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
                else
                {
                    var aux = 0;
                    if (_customerService.ExisteCustomerByDNI(model.IdentificationNumber))
                        aux = _customerService.GetCustomerByDNI(model.IdentificationNumber).CustomerId;
                    else
                        aux = _customerService.AddCustomer(model.ToModelObject());
                    var customer = _customerService.GetCustomer(aux);

                    return Json(customer);
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

        [LogActionFilter]
        [Description("Editar Customers")]
        public JsonResult GetByDocumentNumber(string DocumentNumber)
        {
            var entity = _customerService.GetCustomerByDNI(DocumentNumber);

                try
                {
                    var model = CustomerVm.FromDomainModel(entity);
                    var collections = new CollectionHelper();

                    model.ddlIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
                    model.ddlIdentificationType.SelectedItem = entity.IdentificationType.ToString();

                    model.ddlMaritalStatus = collections.GetMaritalStatusSingleSelectVm();
                    model.ddlMaritalStatus.SelectedItem = entity.MaritalStatus.ToString();

                    model.ddlSpouseIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
                    model.ddlSpouseIdentificationType.SelectedItem = entity.SpouseIdentificationType.ToString();

                    model.ddlCity = collections.GetCitySingleSelectVm();
                    model.ddlCity.SelectedItem = entity.CityId.ToString();

                    model.ddlState = collections.GetStateSingleSelectVm();
                    model.ddlState.SelectedItem = _cityService.GetStateByName(entity.State).StateId.ToString();


                    return Json(model, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    Response.StatusDescription = "El DNI ingresado no corresponde a un cliente registrado.";
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
                }

        }
        public JsonResult MapList()
        {
            try
            {
                var entity = _customerService.GetCustomersSummary();
                return Json(entity, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "Error al recuperar clientes.";
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        
        [LogActionFilter]
        [Description("Ver Customers")]
        public ActionResult Details(int id)
        {
            var entity = _customerService.GetCustomer(id);
            var model = CustomerVm.FromDomainModel(entity);
            var collections = new CollectionHelper();

            model.ddlIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
            model.ddlIdentificationType.SelectedItem = entity.IdentificationType.ToString();

            model.ddlMaritalStatus = collections.GetMaritalStatusSingleSelectVm();
            model.ddlMaritalStatus.SelectedItem = entity.MaritalStatus.ToString();

            model.ddlSpouseIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
            model.ddlSpouseIdentificationType.SelectedItem = entity.SpouseIdentificationType.ToString();

            model.ddlStatusClient = collections.GetStatusClientSingleSelectVm();
            model.ddlStatusClient.SelectedItem = entity.StatusClient.ToString();

            model.ddlCity = collections.GetCitySingleSelectVm();
            model.ddlCity.SelectedItem = entity.CityId.ToString();

            model.ddlState = collections.GetStateSingleSelectVm();
            model.ddlState.SelectedItem = _cityService.GetStateByName(entity.State).StateId.ToString();

            return PartialView("~/Views/Maintenance/Customer/CustomerDetails.cshtml", model);
        }


        [LogActionFilter]
        [Description("Crear Customers")]
        public ActionResult ChangeStatus(int id)
        {
            var entity = _customerService.GetCustomer(id);
            var model = CustomerVm.FromDomainModel(entity);
            var collections = new CollectionHelper();

            model.ddlStatusClient = collections.GetStatusClientSingleSelectVm();
            model.ddlStatusClient.SelectedItem = entity.StatusClient.ToString();


            return PartialView("~/Views/Maintenance/Customer/StatusModal.cshtml", model);
        }

        [HttpPost]
        public ActionResult ChangeStatus(CustomerVm model)
        {
            try
            {
                // TODO: Add update logic here
                _customerService.UpdateCustomerStatus(model.CustomerId, int.Parse(model.ddlStatusClient.SelectedItem));

                return Index();
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        [LogActionFilter]
        [Description("Editar Customers")]
        public ActionResult Edit(int id)
        {
            var entity = _customerService.GetCustomer(id);
            var model = CustomerVm.FromDomainModel(entity);
            var collections = new CollectionHelper();

            model.ddlIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
            model.ddlIdentificationType.SelectedItem = entity.IdentificationType.ToString();

            model.ddlMaritalStatus = collections.GetMaritalStatusSingleSelectVm();
            model.ddlMaritalStatus.SelectedItem = entity.MaritalStatus.ToString();

            model.ddlSpouseIdentificationType = collections.GetIdentificationTypeSingleSelectVm();
            model.ddlSpouseIdentificationType.SelectedItem = entity.SpouseIdentificationType.ToString();

            model.ddlStatusClient = collections.GetStatusClientSingleSelectVm();
            model.ddlStatusClient.SelectedItem = entity.StatusClient.ToString();

            model.ddlCity = collections.GetCitySingleSelectVm();
            model.ddlCity.SelectedItem = entity.CityId.ToString();

            model.ddlState = collections.GetStateSingleSelectVm();
            model.ddlState.SelectedItem = _cityService.GetStateByName(entity.State).StateId.ToString();


            return PartialView("~/Views/Maintenance/Customer/CustomerEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(CustomerVm model)
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
                if (model.CustomerId < 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar, cliente no existe");
                }

                _customerService.UpdateCustomers(model.ToModelObject());

                return Index();
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
	}
}