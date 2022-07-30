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
    public class ProviderController : Controller
    {
        private ProviderService _providerService;
        private IAuditLogHelper _auditLogHelper;
        private SpendingService _spendingService;
        private SpendingPaymentService _spendingPaymentService;
        private UserService _userService;
        private ProfileProvider _profileProvider;
        private CashierService _cashierService;

        public ProviderController()
        {
            _providerService = new ProviderService();
            _auditLogHelper = new AuditLogHelper();
            _spendingPaymentService = new SpendingPaymentService();
            _spendingService = new SpendingService();
            _userService = new UserService();
            _profileProvider = new ProfileProvider();
            _cashierService = new CashierService();
        }

        // GET: /Provider/
        [LogActionFilter]
        [Description("Providers")]
        public ActionResult Index()
        {
            ProviderFilterVm model = new ProviderFilterVm();
            var collections = new CollectionHelper();
            model.Active = true;
            model.ListProviderType = new SelectList(_providerService.GetProviderTypes(), "ProviderTypeId", "Name");
            return PartialView("~/Views/Provider/Provider.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedProviderType, string DateFrom, string DateTo, string BalanceFrom, string BalanceTo, string isActive)
        {
            var model = ProviderTableVm.ToViewModel(_providerService.GetProvidersSummary(SelectedProviderType, DateFrom, DateTo, BalanceFrom, BalanceTo, isActive));
            return PartialView("~/Views/Provider/ProviderTable.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Providers")]
        public ActionResult Create()
        {
            var model = new ProviderVm();
            var collections = new CollectionHelper();
            model.ddlProviderType = collections.GetProviderTypesSingleSelectVm();
            model.ddlIVAPosition = collections.GetIVAPositionsSingleSelectVm();
            return PartialView("~/Views/Provider/ProviderCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(ProviderVm model)
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

                if (model.Document != null && !model.Document.Equals("") && _providerService.ExisteProvider(model.Document))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Ya existe un proveedor con ese DNI");
                }

                if (model.ddlIVAPosition.SelectedItem != "4" && (model.CUIT == null || model.BusinessName == null || model.CUIT.Equals("") || model.BusinessName.Equals("")))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe ingresar CUIT y Razón Social");
                }

                if (model.ddlIVAPosition.SelectedItem != "4" && !ValidateCUIT(model.CUIT))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Ingrese un CUIT Válido");
                }

                if (model.ddlIVAPosition.SelectedItem == "4")
                {
                    model.CUIT = "";
                    model.BusinessName = "";
                }

                _providerService.AddProvider(model.ToModelObject());
                return Index();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
        [LogActionFilter]
        [Description("Editar Providers")]
        public ActionResult Edit(int id)
        {
            var entity = _providerService.GetProvidersById(id);
            var model = ProviderVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProviderType = collections.GetProviderTypesSingleSelectVm();
            model.ddlProviderType.SelectedItem = entity.ProviderTypeId.ToString();
            model.ddlIVAPosition = collections.GetIVAPositionsSingleSelectVm();
            model.ddlIVAPosition.SelectedItem = entity.IVAPositionId.ToString();
            return PartialView("~/Views/Provider/ProviderEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(ProviderVm model)
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

                if (model.ddlIVAPosition.SelectedItem != "4" && (model.CUIT.Equals("") || model.BusinessName.Equals("")))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe ingresar CUIT y Razón Social");
                }

                if (model.ddlIVAPosition.SelectedItem != "4" && !ValidateCUIT(model.CUIT))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Ingrese un CUIT Válido");
                }

                if (model.ddlIVAPosition.SelectedItem == "4")
                {
                    model.CUIT = "";
                    model.BusinessName = "";
                }

                _providerService.UpdateProvider(model.ToModelObject());

                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        [Description("CurrentAcount")]
        public ActionResult IndexCurrentAcount()
        {
            ProviderCAFilterVm model = new ProviderCAFilterVm();
            var collections = new CollectionHelper();
            model.ListProvider = new SelectList(_providerService.GetProvidersWithId(), "ProviderId", "Name");
            return PartialView("~/Views/Provider/CurrentAccount.cshtml", model);
        }

        public ActionResult GetTableCurrentAccount(List<string> SelectedProvider, string BalanceFrom, string BalanceTo, string NotImput)
        {
            if (BalanceFrom == "")
                BalanceFrom = "-10000000";
            if (BalanceTo == "")
                BalanceTo = "10000000";
            string _selectedProvider = (SelectedProvider[0] != "" && SelectedProvider.Count() > 0) ? String.Join(",", SelectedProvider) : "0";
            var model = ProviderCATableVm.ToViewModel(_providerService.GetProviderCurrentAcount(_selectedProvider, BalanceFrom, BalanceTo, NotImput));

            return PartialView("~/Views/Provider/CurrentAccountTable.cshtml", model);
        }

        [Description("ProviderCurrentAcount")]
        public ActionResult IndexProviderCurrentAccount(int id)
        {
            var model = ProviderCADetailTableVm.ToViewModel(_providerService.GetProviderCurrentAcountDetail(id));
            Provider proveedor = _providerService.GetProvidersById(id);
            model.balance = proveedor.Balance;
            model.name = proveedor.Name;
            return PartialView("~/Views/Provider/ProviderCurrentAccountTable.cshtml", model);
        }

        public ActionResult SpendingView(int id)
        {
            var entity = _spendingService.GetSpendingsById(id);
            var model = SpendingVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();

            ViewBag.provider = entity.ProveedorId.ToString();

            return PartialView("~/Views/Provider/SpendingView.cshtml", model);
        }

        public ActionResult SpendingPaymentView(int id)
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

            return PartialView("~/Views/Provider/SpendingPaymentView.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear SpendingPayments")]
        public ActionResult CreateSpengingPayment(int id)
        {
            var entity = _spendingService.GetSpendingsById(id);

            var model = new SpendingPaymentVm();
            var collections = new CollectionHelper();
            List<SpendingPaymentDetail> lists = new List<SpendingPaymentDetail>();
            model.Details = lists;
            List<SpendingPaymentPaymentMethod> payment = new List<SpendingPaymentPaymentMethod>();
            model.PaymentMethods = payment;
            model.ExecutionDate = DateTime.Now.ToShortDateString();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            Provider proveedor = _providerService.GetProvidersById(entity.ProveedorId.Value);
            model.Provider = proveedor.Name;
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUser.SelectedItem = _profileProvider.CurrentUserId.ToString();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            model.ddlSpendings = collections.GetSpendingsByProviderSingleSelectVm(0);
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.Saving = _cashierService.GetCashiersById(_userService.GetUserCashierByUserId(_profileProvider.CurrentUserId).CashierId).Name;


            SpendingPaymentDetail detail = new SpendingPaymentDetail();
            detail.SpendingId = id;
            detail.Total = entity.Balance;
            detail.Spending = _spendingService.GetSpendingsById(id).Description;
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
            return PartialView("~/Views/SpendingPayment/SpendingPaymentCreate.cshtml", model);
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

            return PartialView("~/Views/SpendingPayment/Spending.cshtml", model);
        }

        public static bool ValidateCUIT(string cuit)
        {
            if (string.IsNullOrEmpty(cuit)) throw new ArgumentNullException(nameof(cuit));
            if (cuit.Length != 11) return false;
            bool rv = false;
            int verificador;
            int resultado = 0;
            string cuit_nro = cuit.Replace("-", string.Empty);
            string codes = "6789456789";
            long cuit_long = 0;
            if (long.TryParse(cuit_nro, out cuit_long))
            {
                verificador = int.Parse(cuit_nro[cuit_nro.Length - 1].ToString());
                int x = 0;
                while (x < 10)
                {
                    int digitoValidador = int.Parse(codes.Substring((x), 1));
                    int digito = int.Parse(cuit_nro.Substring((x), 1));
                    int digitoValidacion = digitoValidador * digito;
                    resultado += digitoValidacion;
                    x++;
                }
                resultado = resultado % 11;
                rv = (resultado == verificador);
            }
            return rv;
        }

    }
}