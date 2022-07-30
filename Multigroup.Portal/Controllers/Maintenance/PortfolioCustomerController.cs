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
using Multigroup.Portal.Utilities;

namespace Multigroup.Portal.Controllers.Maintenance
{
    public class PortfolioCustomerController : Controller
    {
        private CustomerService _customerService;
        private ZoneService _zoneService;
        private IAuditLogHelper _auditLogHelper;
        private PaymentService _paymentService;
        private UserService _userService;

        public PortfolioCustomerController()
        {
            _customerService = new CustomerService();
            _auditLogHelper = new AuditLogHelper();
            _zoneService = new ZoneService();
            _paymentService = new PaymentService();
            _userService = new UserService();
        }
        
        [LogActionFilter]
        [Description("PortfolioCustomers")]
        public ActionResult Index()
        {
            PortfolioCustomerFilterVm model = new PortfolioCustomerFilterVm();
            var collections = new CollectionHelper();
            model.ddlPaymentDate = collections.GetPaymentDateSingleSelectVm();
            model.ddlPaymentMethod= collections.GetPaymentMethodSingleSelectVm();
            model.ListStatus = new SelectList(_customerService.GetStatusClient(), "StatusClientId", "Description");
            model.ListZones = new SelectList(_zoneService.GetZones(), "ZoneId", "Name");
            model.ListUsers = new SelectList(_userService.GetUsersComplete().Where(x => x.UserType == 7), "UserId", "Names");
            model.ListUsersAsign = new SelectList(_userService.GetUsersComplete().Where(x => x.UserType == 7), "UserId", "Names");
            model.ListMonths = new SelectList(Util.GetMonths(), "Id", "Name");
            model.SelectedMonth = DateTime.Now.Month + 1;
            model.Year = Int32.Parse(DateTime.Now.Year.ToString());
            var list = _customerService.GetPortfolioCustomerResume();
            var listNext = _customerService.GetPortfolioCustomerResumeNext();
            model.Resume = ResumeVm.ToViewModelList(list);
            model.ResumeNext = ResumeVm.ToViewModelList(listNext);
            return PartialView("~/Views/Collections/PortfolioCustomers/PortfolioCustomer.cshtml", model);
        }
        public ActionResult GetTable(List<string> SelectedStatus, List<string> SelectedZones, List<string> SelectedPaymentDate, List<string> SelectedPaymentMethod, List<string> UserId, int Year, int Month)
        {
            var model = PortfolioCustomerTableVm.ToViewModel(_customerService.GetPortfolioCustomer(SelectedStatus, SelectedZones, SelectedPaymentDate, UserId, Year, Month));

            var total = _customerService.GetPortfolioCustomerTotal(SelectedStatus, SelectedZones, SelectedPaymentDate, UserId, Year, Month);
            ViewBag.Cuota1 = total.Amount1.ToString();
            ViewBag.Cuota2 = total.Amount2.ToString();

            return PartialView("~/Views/Collections/PortfolioCustomers/PortfolioCustomerTable.cshtml", model);
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
        public JsonResult AsociateCustomer(int UserId, List<string> Customers, int Month, int Year)
        {
            try
            {
                foreach(var customer in Customers)
                {
                    _customerService.AsociateCustomer(UserId, Int32.Parse(customer), Month, Year);
                }
                
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "Error al asociar clientes.";
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
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

	}
}