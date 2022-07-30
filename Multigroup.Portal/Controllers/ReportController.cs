using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Constants;
using Multigroup.Portal.Helpers;
using Multigroup.Portal.Models.ContractModel;
using Multigroup.Portal.Models.Shared;
using Multigroup.Portal.Providers;
using Multigroup.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Multigroup.Portal.Models.Maintenance;
using Multigroup.Portal.Utilities;

namespace Multigroup.Portal.Controllers
{
    public class ReportController : Controller
    {
        private UserLoginProvider _userLoginProvider;
        private ProfileProvider _profileProvider;
        private ContractService _contractService;
        private AgencyService _agencyService;
        private PaymentService _paymentService;
        private UserService _userService;
        private CustomerService _customerService;
        private CityService _cityService;

        public ReportController()
        {
            _userLoginProvider = new UserLoginProvider();
            _profileProvider = new ProfileProvider();
            _contractService = new ContractService();
            _agencyService = new AgencyService();
            _paymentService = new PaymentService();
            _userService = new UserService();
            _customerService = new CustomerService();
            _cityService = new CityService();
        }

        // GET: Menu
        public ActionResult Contract()
        {
            ContractFilterVm model = new ContractFilterVm();

            model.ListStatus = new SelectList(_contractService.GetStatusContract("Status"), "StatusContractId", "Description");
            model.ListAgency = new SelectList(_agencyService.GetAgencys(), "AgencyId", "Description");

            var customer = _contractService.GetCustomer();
            var items = customer.Select(x => new Item { Id = x.CustomerId, Name = x.Surname + " " + x.Name });
            model.ListClient = new SelectList(items, "Id", "Name");
            return PartialView("~/Views/Report/Contract.cshtml", model);
        }


        public JsonResult ContractChart(List<string> SelectedAgency)
        {
            var model = new ContractChartVm();
            string agency = (SelectedAgency.Count() > 0 && SelectedAgency[0] != "") ? String.Join(",", SelectedAgency) : "0";
            var data = _contractService.GetContractChart(agency);

            return Json(model.BuildChart(data), JsonRequestBehavior.AllowGet);
        }
        public ActionResult SupervisorSales()
        {
            ContractFilterVm model = new ContractFilterVm();
            var collections = new CollectionHelper();
            model.ListContractType = new SelectList(_contractService.GetContractTypes(), "ContractTypeId", "Description");
            return PartialView("~/Views/Report/SupervisorSales.cshtml", model);
        }
        public JsonResult SupervisorSalesChart(string DateTo, string DateFrom, string ContractType)
        {
            var model = new SupervisorSalesChartVm();
            var data = _contractService.GetSupervisorReport(DateFrom, DateTo, ContractType);

            return Json(model.BuildChart(data), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Portfolio()
        {
            PaymentHistoryFilterVm model = new PaymentHistoryFilterVm();
            var collections = new CollectionHelper();
            model.ListUsers = new SelectList(_userService.GetUsers().Where(x => x.UserType == 7), "UserId", "Names");
            return PartialView("~/Views/Report/Portfolio.cshtml", model);
        }
        public JsonResult PortfolioChart(string DatePortfolio, List<string> telemarketer)
        {
            var model = new PortFolioChartVm();
            string _selectedUser = (telemarketer.Count() > 0 && telemarketer[0] != "") ? String.Join(",", telemarketer) : "0";
            var data = _contractService.GetPortfolioReport(DatePortfolio, _selectedUser);

            return Json(model.BuildChart(data), JsonRequestBehavior.AllowGet);
        }


        public ActionResult CustomerDebt()
        {
            var model = CustomerDebtTableVm.ToViewModel(_contractService.GetCustomerDebt());
            return PartialView("~/Views/Report/CustomerDebt.cshtml", model);
        }
        public ActionResult PaymentHistory()
        {
            PaymentHistoryFilterVm model = new PaymentHistoryFilterVm();
            var collections = new CollectionHelper();
            model.ListPaymentMethods = new SelectList(_paymentService.GetPaymentMethods(), "PaymentMethodId", "Name");
            model.ListUsers = new SelectList(_userService.GetUsersSurName().Where(x => x.UserType == 7), "UserId", "Names");
            model.ListSupervisor = new SelectList(_userService.GetUsersSurName().Where(x => x.UserType == 4), "UserId", "Names");
            model.ListProvince = new SelectList(_cityService.GetStates(), "StateId", "Name");
            model.ListCities = new SelectList(_cityService.GetCitys(), "CityId", "Name");
            var customer = _contractService.GetCustomer();
            var items = customer.Select(x => new Item { Id = x.CustomerId, Name = x.Surname + " " + x.Name });
            model.ListCustomers = new SelectList(items, "Id", "Name");
            return PartialView("~/Views/Report/PaymentHistory.cshtml", model);
        }

        public ActionResult PaymentHistoryTable(List<string> SelectedCustomer, List<string> SelectedPaymentMethod, List<string> SelectedUser, string InstallmentNumber, string DateFrom, string DateTo, int? SelectedProvince, List<string> SelectedCities, string DateIFrom, string DateITo, List<string> SelectedSupervisor)
        {
            double total = 0;
            int cantidad = 0;
            if (DateFrom == "")
                DateFrom = "01/01/1900";
            if (DateTo == "")
                DateTo = "01/01/2100";
            if (DateIFrom == "")
                DateIFrom = "01/01/1900";
            if (DateITo == "")
                DateITo = "01/01/2100";
            if (SelectedProvince == null)
                SelectedProvince = 0;
            var _citys = (SelectedCities[0] != "" && SelectedCities.Count() > 0) ? String.Join(",", SelectedCities) : "0";
            string _selectedPaymentMethod = (SelectedPaymentMethod[0] != "" && SelectedPaymentMethod.Count() > 0) ? String.Join(",", SelectedPaymentMethod) : "0";
            string _selectedCustomer = (SelectedCustomer[0] != "" && SelectedCustomer.Count() > 0) ? String.Join(",", SelectedCustomer) : "0";
            string _selectedUser = (SelectedUser[0] != "" && SelectedUser.Count() > 0) ? String.Join(",", SelectedUser) : "0";
            string _selectedSupervisor = (SelectedSupervisor[0] != "" && SelectedSupervisor.Count() > 0) ? String.Join(",", SelectedSupervisor) : "0";
            IEnumerable<PaymentHistory> pagos = _paymentService.GetPaymentHistory(_selectedCustomer, _selectedPaymentMethod, _selectedUser, InstallmentNumber.ToString(), DateFrom, DateTo, SelectedProvince.ToString(), _citys, DateIFrom, DateITo, _selectedSupervisor);
            var model = PaymentHistoryTableVm.ToViewModel(pagos);
            foreach (PaymentHistory pago in pagos)
            {
                total += double.Parse(pago.Amount);
                cantidad++;
            }
            ViewBag.total = total.ToString();
            ViewBag.count = cantidad.ToString();
            return PartialView("~/Views/Report/PaymentHistoryTable.cshtml", model);
        }


        public ActionResult IndexZone()
        {
            PortfolioCustomerFilterVm model = new PortfolioCustomerFilterVm();
            var collections = new CollectionHelper();
            model.ListProvince = new SelectList(_cityService.GetStates(), "StateId", "Name");
            model.ListCities = new SelectList(_cityService.GetCitys(), "CityId", "Name");
            model.ListMonths = new SelectList(Util.GetMonths(), "Id", "Name");
            model.ListUsers = new SelectList(_userService.GetUsersComplete().Where(x => x.UserType == 7), "UserId", "Names");
            model.SelectedMonth = DateTime.Now.Month + 1;
            model.Year = Int32.Parse(DateTime.Now.Year.ToString());

            return PartialView("~/Views/Report/PortfolioCustomerZone.cshtml", model);
        }
        public ActionResult GetTableZone(int? SelectedProvince, List<string> SelectedCities, int Year, int Month, List<string> UserId)
        {
            var model = PortfolioCustomerTableVm.ToViewModel(_customerService.GetPortfolioCustomerZone(SelectedProvince, SelectedCities, Year, Month, UserId));

            var total = _customerService.GetPortfolioCustomerTotalZone(SelectedProvince, SelectedCities, Year, Month, UserId);
            ViewBag.Cuota1 = total.Amount1.ToString();
            ViewBag.Cuota2 = total.Amount2.ToString();
            ViewBag.Cuota3 = (total.Amount2 + total.Amount1).ToString();

            return PartialView("~/Views/Report/PortfolioCustomerZoneTable.cshtml", model);
        }

        public ActionResult IndexInstallment()
        {
            PortfolioCustomerFilterVm model = new PortfolioCustomerFilterVm();
            var collections = new CollectionHelper();
            model.Installment = 1;

            return PartialView("~/Views/Report/InstallmentReport.cshtml", model);
        }
        public ActionResult GetTableInstallment(int Installment)
        {
            var model = InstallmentReportTableVm.ToViewModel(_customerService.GetInstallmentReport(Installment));

            return PartialView("~/Views/Report/InstallmentReportTable.cshtml", model);
        }

        public ActionResult StatusContract()
        {
            StatusReportFilterVm model = new StatusReportFilterVm();
            var collections = new CollectionHelper();
            model.ListStatus = new SelectList(_contractService.GetStatusContract("Status"), "StatusContractId", "Description");
            return PartialView("~/Views/Report/StatusReport.cshtml", model);
        }

        public ActionResult StatusContractTable(List<string> SelectedStatus, string DateFrom, string DateTo)
        {
            if (DateFrom == "")
                DateFrom = "01/01/1900";
            if (DateTo == "")
                DateTo = "01/01/2100";
            string _selectedStatus = (SelectedStatus[0] != "" && SelectedStatus.Count() > 0) ? String.Join(",", SelectedStatus) : "0";
            IEnumerable<StatusReport> contratos = _contractService.GetStatusReport(_selectedStatus, DateFrom, DateTo);
            var model = StatusReportTableVm.ToViewModel(contratos);
            return PartialView("~/Views/Report/StatusReportTable.cshtml", model);
        }
    }
}
