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
using Multigroup.Portal.Models.Maintenance;
using Multigroup.Portal.Utilities;
using Multigroup.Portal.Providers;
using System.Web.UI.WebControls;
using Multigroup.Portal.Models.Shared;



namespace Multigroup.Portal.Controllers.Administration
{
    public class SpendingController : Controller
    {
        private SpendingService _spendingService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProviderService _providerService;
        private ArticleService _articleService;
        private ProfileProvider _profileProvider;
        private PurchaseRequestService _purchaseRequestService;
        private ScheduledExpenseService _scheduledExpenseService;
        private ExpenseAuthorizationService _expenseAuthorizationService;
        private PaymentMethodsService _paymentMethodsServvice;
        private CashierService _cashierService;
        private List<HttpPostedFileBase> _filesLoad;

        public SpendingController()
        {
            _spendingService = new SpendingService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _providerService = new ProviderService();
            _articleService = new ArticleService();
            _profileProvider = new ProfileProvider();
            _purchaseRequestService = new PurchaseRequestService();
            _scheduledExpenseService = new ScheduledExpenseService();
            _expenseAuthorizationService = new ExpenseAuthorizationService();
            _paymentMethodsServvice = new PaymentMethodsService();
            _cashierService = new CashierService();
        }

        // GET: /Spending/
        [LogActionFilter]
        [Description("Spendings")]
        public ActionResult Index()
        {
            SpendingFilterVm model = new SpendingFilterVm();
            var collections = new CollectionHelper();
            model.ListProvider = new SelectList(_providerService.GetProvidersWithId(), "ProviderId", "Name");
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.UserId = _profileProvider.CurrentUserId;
            return PartialView("~/Views/Spending/Spending.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedProvider, int? SelectedUser, string DateFromExecution, string DateToExecution, string DateFromExpiration, string DateToExpiration, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance, string PositiveAll, string Provider)
        {
            var model = SpendingTableVm.ToViewModel(_spendingService.GetSpendingSummary(SelectedProvider, SelectedUser, DateFromExecution, DateToExecution, DateFromExpiration, DateToExpiration, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance, PositiveAll, Provider));
            return PartialView("~/Views/Spending/SpendingTable.cshtml", model);
        }

        public ActionResult GetTableDetail(int? SelectedProvider, int? SelectedUser, string DateFromExecution, string DateToExecution, string DateFromExpiration, string DateToExpiration, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance, string PositiveAll, string Provider)
        {
            var model = SpendingDetailTableVm.ToViewModel(_spendingService.GetSpendingDetailSummary(SelectedProvider, SelectedUser, DateFromExecution, DateToExecution, DateFromExpiration, DateToExpiration, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance, PositiveAll, Provider));
            return PartialView("~/Views/Spending/SpendingTableDetail.cshtml", model);
        }

        [HttpGet]
        public ActionResult GetTablePurchase(int? SelectedProvider, int? SelectedUser, string DateFromCreate, string DateToCreate, string DateFromNeed, string DateToNeed, string AmountFrom, string AmountTo, string Active)
        {
            var model = PurchaseRequestTableVm.ToViewModel(_purchaseRequestService.GetPurchaseRequestsSummary(0, SelectedProvider, SelectedUser, 0, DateFromCreate, DateToCreate, DateFromNeed, DateToNeed, AmountFrom, AmountTo, Active, "0"));
            return PartialView("~/Views/Spending/PurchaseRequestTable.cshtml", model);
        }
        [HttpGet]
        public ActionResult GetTableScheduled(int? SelectedProvider, int? SelectedUser, string DateFromCreate, string DateToCreate, string DateFromPurchase, string DateToPurchase, string AmountFrom, string AmountTo, string PurchaseRequestId, string isAuthorized)
        {
            var model = ScheduledExpenseTableVm.ToViewModel(_scheduledExpenseService.GetScheduledExpensesSummary(SelectedProvider, SelectedUser, DateFromCreate, DateToCreate, DateFromPurchase, DateToPurchase, AmountFrom, AmountTo, PurchaseRequestId, isAuthorized, "0", "0", "1"));
            return PartialView("~/Views/Spending/ScheduledExpenseTable.cshtml", model);
        }

        [HttpGet]
        public ActionResult GetTableExpense(int? SelectedProvider, int? SelectedUser, string DateFromCreate, string DateToCreate, string DateFromPurchase, string DateToPurchase, string AmountFrom, string AmountTo, string PurchaseRequestId)
        {
            var model = ExpenseAuthorizationTableVm.ToViewModel(_expenseAuthorizationService.GetExpenseAuthorizationSummary(SelectedProvider, SelectedUser, DateFromCreate, DateToCreate, DateFromPurchase, DateToPurchase, AmountFrom, AmountTo, PurchaseRequestId, 0, "0"));
            return PartialView("~/Views/Spending/ExpenseAuthorizationTable.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Spendings")]
        public ActionResult Create()
        {
            var model = new SpendingVm();
            var collections = new CollectionHelper();
            List<SpendingDetail> lists = new List<SpendingDetail>();
            model.Details = lists;
            List<SpendingPaymentMethod> payment = new List<SpendingPaymentMethod>();
            model.PaymentMethods = payment;
            model.ExecutionDate = DateTime.Now.ToShortDateString();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlIVAPosition = collections.GetIVAPositionsSingleSelectVm();
            model.ddlPurchaseDocument = collections.GetPurchaseDocumentIdsSingleSelectVm();
            model.ddlPurchasePerception = collections.GetPurchasePerceptionIdsSingleSelectVm();
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUser.SelectedItem = _profileProvider.CurrentUserId.ToString();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserSE = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserEA = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.Saving = _cashierService.GetCashiersById(_userService.GetUserCashierByUserId(_profileProvider.CurrentUserId).CashierId).Name;
            model.chkCurrentAccount = false;
            Session["SessionSpending"] = model;
            Session["CurrentAccount"] = false;
            Session["PMs"] = new List<int>();
            return PartialView("~/Views/Spending/SpendingCreate.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Spending")]
        public ActionResult CreateFromPurchaseRequest(int purchaseRequestId)
        {
            PurchaseRequest purchase = _purchaseRequestService.GetPurchaseRequestsById(purchaseRequestId);


            var model = new SpendingVm();
            var collections = new CollectionHelper();
            List<SpendingDetail> lists = new List<SpendingDetail>();
            foreach (PurchaseRequestDetail detail in purchase.Details)
            {
                SpendingDetail detalle = new SpendingDetail();
                detalle.Article = detail.Article;
                detalle.ArticleId = detail.ArticleId;
                detalle.Price = detail.Price;
                detalle.Quantity = detail.Quantity;
                detalle.Total = detail.Total;
                detalle.Description = detail.Description;
                detalle.PurchaseRequestId = detail.PurchaseRequestId;
                detalle.Origin = "Solicitud de Compra " + detail.PurchaseRequestId;
                lists.Add(detalle);
            }
            model.Details = lists;
            List<SpendingPaymentMethod> payment = new List<SpendingPaymentMethod>();
            model.PaymentMethods = payment;
            model.ExecutionDate = DateTime.Now.ToShortDateString();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUser.SelectedItem = _profileProvider.CurrentUserId.ToString();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            model.ddlIVAPosition = collections.GetIVAPositionsSingleSelectVm();
            model.ddlPurchaseDocument = collections.GetPurchaseDocumentIdsSingleSelectVm();
            model.ddlPurchasePerception = collections.GetPurchasePerceptionIdsSingleSelectVm();
            model.Saving = _cashierService.GetCashiersById(_userService.GetUserCashierByUserId(_profileProvider.CurrentUserId).CashierId).Name;
            if (purchase.ProveedorId.HasValue)
            {
                model.ddlProvider = collections.GetProvidersByIdSingleSelectVm(purchase.ProveedorId.Value);
                model.ddlProvider.SelectedItem = purchase.ProveedorId.ToString();
            }
            else
            {
                model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            }
            model.PurchaseRequestId = purchase.PurchaseRequestId;
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();

            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserSE = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserEA = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.Amount = purchase.Amount;
            model.AmountPayment = model.Amount;
            Session["SessionSpending"] = model;

            return PartialView("~/Views/Spending/SpendingCreate.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Spending")]
        public ActionResult AddFromPurchaseRequest(int purchaseRequestId)
        {
            PurchaseRequest purchase = _purchaseRequestService.GetPurchaseRequestsById(purchaseRequestId);


            var model = (SpendingVm)Session["SessionSpending"];
            var collections = new CollectionHelper();
            List<SpendingDetail> lists = new List<SpendingDetail>();
            foreach (SpendingDetail detail in model.Details)
            {
                SpendingDetail detalle = new SpendingDetail();
                detalle.Article = detail.Article;
                detalle.ArticleId = detail.ArticleId;
                detalle.Price = detail.Price;
                detalle.Quantity = detail.Quantity;
                detalle.Total = detail.Total;
                detalle.Description = detail.Description;
                detalle.Origin = detail.Origin;
                detalle.PurchaseRequestId = detail.PurchaseRequestId;
                detalle.ScheduledExpenseId = detail.ScheduledExpenseId;
                detalle.ExpenseAuthorizationId = detail.ExpenseAuthorizationId;

                lists.Add(detalle);
            }
            foreach (PurchaseRequestDetail detail in purchase.Details)
            {
                SpendingDetail detalle = new SpendingDetail();
                detalle.Article = detail.Article;
                detalle.ArticleId = detail.ArticleId;
                detalle.Price = detail.Price;
                detalle.Quantity = detail.Quantity;
                detalle.Total = detail.Total;
                detalle.Description = detail.Description;
                detalle.Origin = "Solicitud de Compra " + detail.PurchaseRequestId;
                detalle.PurchaseRequestId = detail.PurchaseRequestId;
                lists.Add(detalle);
            }
            model.Details = lists;
            List<SpendingPaymentMethod> payment = new List<SpendingPaymentMethod>();
            model.PaymentMethods = payment;
            if (purchase.ProveedorId.HasValue)
            {
                model.ddlProvider = collections.GetProvidersByIdSingleSelectVm(purchase.ProveedorId.Value);
                model.ddlProvider.SelectedItem = purchase.ProveedorId.ToString();
            }
            else
            {
                model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            }
            model.PurchaseRequestId = purchase.PurchaseRequestId;
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserSE = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserEA = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ddlIVAPosition = collections.GetIVAPositionsSingleSelectVm();
            model.ddlPurchaseDocument = collections.GetPurchaseDocumentIdsSingleSelectVm();
            model.ddlPurchasePerception = collections.GetPurchasePerceptionIdsSingleSelectVm();
            model.Amount = purchase.Amount + model.Amount;
            model.AmountPayment = model.Amount;
            Session["SessionSpending"] = model;

            return PartialView("~/Views/Spending/SpendingCreate.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Spending")]
        public ActionResult AddFromScheduledExpense(int scheduledExpenseId)
        {
            ScheduledExpense scheduled = _scheduledExpenseService.GetScheduledExpensesById(scheduledExpenseId);


            var model = (SpendingVm)Session["SessionSpending"];
            var collections = new CollectionHelper();
            List<SpendingDetail> lists = new List<SpendingDetail>();
            foreach (SpendingDetail detail in model.Details)
            {
                SpendingDetail detalle = new SpendingDetail();
                detalle.Article = detail.Article;
                detalle.ArticleId = detail.ArticleId;
                detalle.Price = detail.Price;
                detalle.Quantity = detail.Quantity;
                detalle.Total = detail.Total;
                detalle.Description = detail.Description;
                detalle.Origin = detail.Origin;
                detalle.PurchaseRequestId = detail.PurchaseRequestId;
                detalle.ScheduledExpenseId = detail.ScheduledExpenseId;
                detalle.ExpenseAuthorizationId = detail.ExpenseAuthorizationId;
                lists.Add(detalle);
            }

            foreach (ScheduledExpenseDetail detail in scheduled.Details)
            {
                SpendingDetail detalle = new SpendingDetail();
                detalle.Article = detail.Article;
                detalle.ArticleId = detail.ArticleId;
                detalle.Price = detail.Price;
                detalle.Quantity = detail.Quantity;
                detalle.Total = detail.Total;
                detalle.Description = detail.Description;
                detalle.Origin = "Gasto Programado " + detail.ScheduledExpenseId;
                detalle.ScheduledExpenseId = detail.ScheduledExpenseId;
                lists.Add(detalle);
            }
            model.Details = lists;
            List<SpendingPaymentMethod> payment = new List<SpendingPaymentMethod>();
            model.PaymentMethods = payment;

            model.ddlProvider = collections.GetProvidersByIdSingleSelectVm(scheduled.ProveedorId);
            model.ddlProvider.SelectedItem = scheduled.ProveedorId.ToString();
            model.ScheduledExpenseId = scheduled.ScheduledExpenseId;
            model.ddlIVAPosition = collections.GetIVAPositionsSingleSelectVm();
            model.ddlPurchaseDocument = collections.GetPurchaseDocumentIdsSingleSelectVm();
            model.ddlPurchasePerception = collections.GetPurchasePerceptionIdsSingleSelectVm();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserSE = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserEA = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.Amount = scheduled.Amount + model.Amount;
            model.AmountPayment = model.Amount;
            Session["SessionSpending"] = model;

            return PartialView("~/Views/Spending/SpendingCreate.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Spending")]
        public ActionResult CreateFromScheduledExpense(int scheduledExpenseId)
        {
            ScheduledExpense scheduled = _scheduledExpenseService.GetScheduledExpensesById(scheduledExpenseId);


            var model = new SpendingVm();
            var collections = new CollectionHelper();
            List<SpendingDetail> lists = new List<SpendingDetail>();
            foreach (ScheduledExpenseDetail detail in scheduled.Details)
            {
                SpendingDetail detalle = new SpendingDetail();
                detalle.Article = detail.Article;
                detalle.ArticleId = detail.ArticleId;
                detalle.Price = detail.Price;
                detalle.Quantity = detail.Quantity;
                detalle.Total = detail.Total;
                detalle.Description = detail.Description;
                detalle.Origin = "Gasto Programado " + detail.ScheduledExpenseId;
                detalle.ScheduledExpenseId = detail.ScheduledExpenseId;
                lists.Add(detalle);
            }
            model.Details = lists;
            List<SpendingPaymentMethod> payment = new List<SpendingPaymentMethod>();
            model.PaymentMethods = payment;
            model.ExecutionDate = DateTime.Now.ToShortDateString();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUser.SelectedItem = _profileProvider.CurrentUserId.ToString();
            model.ddlProvider = collections.GetProvidersByIdSingleSelectVm(scheduled.ProveedorId);
            model.ddlIVAPosition = collections.GetIVAPositionsSingleSelectVm();
            model.ddlPurchaseDocument = collections.GetPurchaseDocumentIdsSingleSelectVm();
            model.ddlPurchasePerception = collections.GetPurchasePerceptionIdsSingleSelectVm();
            model.ddlProvider.SelectedItem = scheduled.ProveedorId.ToString();
            model.ScheduledExpenseId = scheduled.ScheduledExpenseId;
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            model.Saving = _cashierService.GetCashiersById(_userService.GetUserCashierByUserId(_profileProvider.CurrentUserId).CashierId).Name;
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserSE = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserEA = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.Amount = scheduled.Amount;
            model.AmountPayment = model.Amount;
            Session["SessionSpending"] = model;

            return PartialView("~/Views/Spending/SpendingCreate.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Spending")]
        public ActionResult CreateFromExpenseAuthorization(int expenseAuthorizationId)
        {
            ExpenseAuthorization expense = _expenseAuthorizationService.GetExpenseAuthorizationsById(expenseAuthorizationId);


            var model = new SpendingVm();
            var collections = new CollectionHelper();
            List<SpendingDetail> lists = new List<SpendingDetail>();
            foreach (ExpenseAuthorizationDetail detail in expense.Details)
            {
                SpendingDetail detalle = new SpendingDetail();
                detalle.Article = detail.Article;
                detalle.ArticleId = detail.ArticleId;
                detalle.Price = detail.Price;
                detalle.Quantity = detail.Quantity;
                detalle.Total = detail.Total;
                detalle.Description = detail.Description;
                detalle.Origin = "Autorización de Gasto " + detail.ExpenseAuthorizationId;
                detalle.ExpenseAuthorizationId = detail.ExpenseAuthorizationId;
                lists.Add(detalle);
            }
            model.Details = lists;
            List<SpendingPaymentMethod> payment = new List<SpendingPaymentMethod>();
            model.PaymentMethods = payment;
            model.ExecutionDate = DateTime.Now.ToShortDateString();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUser.SelectedItem = _profileProvider.CurrentUserId.ToString();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlIVAPosition = collections.GetIVAPositionsSingleSelectVm();
            model.ddlPurchaseDocument = collections.GetPurchaseDocumentIdsSingleSelectVm();
            model.ddlPurchasePerception = collections.GetPurchasePerceptionIdsSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            model.Saving = _cashierService.GetCashiersById(_userService.GetUserCashierByUserId(_profileProvider.CurrentUserId).CashierId).Name;
            if (expense.ProveedorId.HasValue)
            {
                model.ddlProvider = collections.GetProvidersByIdSingleSelectVm(expense.ProveedorId.Value);
                model.ddlProvider.SelectedItem = expense.ProveedorId.ToString();
            }
            else
            {
                model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            }
            model.ExpenseAuthorizationId = expense.ExpenseAuthorizationId;
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();

            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserSE = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserEA = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.Amount = expense.Amount;
            model.AmountPayment = model.Amount;
            Session["SessionSpending"] = model;

            return PartialView("~/Views/Spending/SpendingCreate.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Spending")]
        public ActionResult AddFromExpenseAuthorization(int expenseAuthorizationId)
        {
            ExpenseAuthorization expense = _expenseAuthorizationService.GetExpenseAuthorizationsById(expenseAuthorizationId);


            var model = (SpendingVm)Session["SessionSpending"];
            var collections = new CollectionHelper();
            List<SpendingDetail> lists = new List<SpendingDetail>();
            foreach (SpendingDetail detail in model.Details)
            {
                SpendingDetail detalle = new SpendingDetail();
                detalle.Article = detail.Article;
                detalle.ArticleId = detail.ArticleId;
                detalle.Price = detail.Price;
                detalle.Quantity = detail.Quantity;
                detalle.Total = detail.Total;
                detalle.Description = detail.Description;
                detalle.Origin = detail.Origin;
                detalle.PurchaseRequestId = detail.PurchaseRequestId;
                detalle.ScheduledExpenseId = detail.ScheduledExpenseId;
                detalle.ExpenseAuthorizationId = detail.ExpenseAuthorizationId;
                lists.Add(detalle);
            }

            foreach (ExpenseAuthorizationDetail detail in expense.Details)
            {
                SpendingDetail detalle = new SpendingDetail();
                detalle.Article = detail.Article;
                detalle.ArticleId = detail.ArticleId;
                detalle.Price = detail.Price;
                detalle.Quantity = detail.Quantity;
                detalle.Total = detail.Total;
                detalle.Description = detail.Description;
                detalle.Origin = "Autorización de Gasto " + detail.ExpenseAuthorizationId;
                detalle.ExpenseAuthorizationId = detail.ExpenseAuthorizationId;
                lists.Add(detalle);
            }
            model.Details = lists;
            List<SpendingPaymentMethod> payment = new List<SpendingPaymentMethod>();
            model.PaymentMethods = payment;
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            if (expense.ProveedorId.HasValue)
            {
                model.ddlProvider = collections.GetProvidersByIdSingleSelectVm(expense.ProveedorId.Value);
                model.ddlProvider.SelectedItem = expense.ProveedorId.ToString();
            }
            else
            {
                model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            }
            model.ddlIVAPosition = collections.GetIVAPositionsSingleSelectVm();
            model.ddlPurchaseDocument = collections.GetPurchaseDocumentIdsSingleSelectVm();
            model.ddlPurchasePerception = collections.GetPurchasePerceptionIdsSingleSelectVm();
            model.ExpenseAuthorizationId = expense.ExpenseAuthorizationId;
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserSE = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserEA = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.Amount = expense.Amount + model.Amount;
            model.AmountPayment = model.Amount;
            Session["SessionSpending"] = model;

            return PartialView("~/Views/Spending/SpendingCreate.cshtml", model);
        }

        public ActionResult IndexArticles()
        {
            SpendingVm model = (SpendingVm)Session["SessionSpending"];
            ViewBag.ErrorMessageTrabajo = "false";
            return PartialView("~/Views/Spending/ArticlesTable.cshtml", model);
        }

        public ActionResult IndexPaymentMethod()
        {
            SpendingVm model = (SpendingVm)Session["SessionSpending"];
            ViewBag.ErrorMessagePayment = "false";
            return PartialView("~/Views/Spending/PaymentMethodTable.cshtml", model);
        }

        public JsonResult GetArticlesByHeading(int HeadingId)
        {
            var collections = new CollectionHelper();
            var response = _articleService.GetArticlesSummary(HeadingId);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Articles(int QuantityArticle, int? ArticleId, double PriceArticle, string DescriptionArticle)
        {
            bool existe = false;
            SpendingVm principal = (SpendingVm)Session["SessionSpending"];
            if (ArticleId.HasValue)
            {
                SpendingDetail detail = new SpendingDetail();
                detail.ArticleId = ArticleId.Value;
                detail.Quantity = QuantityArticle;
                detail.Price = PriceArticle;
                detail.Total = QuantityArticle * PriceArticle;
                detail.Article = _articleService.GetArticlesById(ArticleId.Value).Name;
                detail.Origin = "Independiente";
                detail.Description = DescriptionArticle;

                List<SpendingDetail> list = (List<SpendingDetail>)principal.Details;
                Int32 id = ArticleId.Value;
                var item = list.FirstOrDefault(x => x.ArticleId == id);
                if (item != null)
                    existe = true;
                else
                    list.Add(detail);
                principal.Details = list;


                double total = 0;

                foreach (SpendingDetail det in list)
                {
                    total = total + det.Total;
                }
                ViewBag.Total = total.ToString();

                Session["Total"] = total.ToString();
                principal.AmountPayment = total;
                Session["SessionSpending"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessageTrabajo = "false";
                    return PartialView("~/Views/Spending/ArticlesTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessageTrabajo = "true";
                    return PartialView("~/Views/Spending/ArticlesTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un articulo");
            }
        }

        public ActionResult PaymentMethod(int? PaymentMethod, double Amount)
        {
            bool existe = false;
            SpendingVm principal = (SpendingVm)Session["SessionSpending"];
            if (PaymentMethod.HasValue)
            {
                SpendingPaymentMethod payment = new SpendingPaymentMethod();
                payment.PaymentMethodId = PaymentMethod.Value;
                payment.PaymentMethod = _paymentMethodsServvice.GetPaymentMethodssById(PaymentMethod.Value).Name;
                payment.Amount = Amount;

                List<int> listPM = (List<int>)Session["PMs"];
                listPM.Add(PaymentMethod.Value);
                Session["PMs"] = listPM;

                List<SpendingPaymentMethod> list = (List<SpendingPaymentMethod>)principal.PaymentMethods;
                Int32 id = PaymentMethod.Value;
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                if (item != null)
                    existe = true;
                else
                    list.Add(payment);
                principal.PaymentMethods = list;

                Session["SessionSpending"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessagePayment = "false";
                    return PartialView("~/Views/Spending/PaymentMethodTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessagePayment = "true";
                    return PartialView("~/Views/Spending/PaymentMethodTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un articulo");
            }
        }

        public ActionResult RemovePaymentMethod(int Id)
        {
            SpendingVm principal = (SpendingVm)Session["SessionSpending"];
            List<SpendingPaymentMethod> list = (List<SpendingPaymentMethod>)principal.PaymentMethods;
            var item = list.FirstOrDefault(x => x.PaymentMethodId == Id);
            list.Remove(item);

            principal.PaymentMethods = list;

            Session["SessionSpending"] = principal;

            Session["esNuevo"] = false;
            ViewBag.ErrorMessagePayment = "false";
            return PartialView("~/Views/Spending/PaymentMethodTable.cshtml", principal);
        }

        public ActionResult PaymentMethods(string CurrentAccount, string ExpirationDate)
        {
            SpendingVm principal = (SpendingVm)Session["SessionSpending"];
            if (CurrentAccount.Equals("1"))
            {
                principal.ExpirationDate = ExpirationDate;
                Session["SessionSpending"] = principal;
                Session["CurrentAccount"] = true;
            }
            else
            {
                principal.ExpirationDate = null;
                Session["SessionSpending"] = principal;
                Session["CurrentAccount"] = false;
            }

            Session["PMs"] = new List<int>();
            return PartialView("~/Views/Spending/ArticlesTable.cshtml", principal);
        }

        public ActionResult CancelPaymentMethods()
        {
            SpendingVm principal = (SpendingVm)Session["SessionSpending"];


            List<SpendingPaymentMethod> list = (List<SpendingPaymentMethod>)principal.PaymentMethods;
            foreach (int id in (List<int>)Session["PMs"])
            {
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                list.Remove(item);
            }

            principal.PaymentMethods = list;
            Session["PMs"] = new List<int>();
            Session["SessionSpending"] = principal;

            ViewBag.ErrorMessagePayment = "false";

            return PartialView("~/Views/Spending/PaymentMethodTable.cshtml", principal);
        }

        public ActionResult ArticlesUpdate(int QuantityArticle, int? ArticleId, int? PurchaseId, int? ScheduledId, int? ExpenseId, double PriceArticle, string Origin, string DescriptionArticle)
        {
            bool existe = false;
            SpendingVm principal = (SpendingVm)Session["SessionSpending"];
            if (ArticleId.HasValue)
            {
                SpendingDetail detail = new SpendingDetail();
                detail.ArticleId = ArticleId.Value;
                detail.Quantity = QuantityArticle;
                detail.Price = PriceArticle;
                detail.Total = QuantityArticle * PriceArticle;
                detail.Article = _articleService.GetArticlesById(ArticleId.Value).Name;
                detail.Origin = Origin;
                detail.Description = DescriptionArticle;
                detail.PurchaseRequestId = PurchaseId;
                detail.ScheduledExpenseId = ScheduledId;
                detail.ExpenseAuthorizationId = ExpenseId;

                List<SpendingDetail> list = (List<SpendingDetail>)principal.Details;
                Int32 id = ArticleId.Value;
                if (PurchaseId.HasValue)
                {
                    var item = list.FirstOrDefault(x => x.ArticleId == id && x.PurchaseRequestId == PurchaseId.Value);
                    list.Remove(item);
                }
                else if (ExpenseId.HasValue)
                {
                    var item = list.FirstOrDefault(x => x.ArticleId == id && x.ExpenseAuthorizationId == ExpenseId.Value);
                    list.Remove(item);
                }
                else if (PurchaseId.HasValue)
                {
                    var item = list.FirstOrDefault(x => x.ArticleId == id && x.PurchaseRequestId == PurchaseId.Value);
                    list.Remove(item);
                }
                else
                {
                    var item = list.FirstOrDefault(x => x.ArticleId == id && x.Origin == "Independiente");
                    list.Remove(item);
                }

                list.Add(detail);
                principal.Details = list;


                double total = 0;

                foreach (SpendingDetail det in list)
                {
                    total = total + det.Total;
                }
                ViewBag.Total = total.ToString();

                Session["Total"] = total.ToString();
                principal.AmountPayment = total;
                Session["SessionSpending"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessageTrabajo = "false";
                    return PartialView("~/Views/Spending/ArticlesTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessageTrabajo = "true";
                    return PartialView("~/Views/Spending/ArticlesTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un articulo");
            }
        }

        public ActionResult RemoveArticles(int Id)
        {
            SpendingVm principal = (SpendingVm)Session["SessionSpending"];
            List<SpendingDetail> list = (List<SpendingDetail>)principal.Details;
            var item = list.FirstOrDefault(x => x.ArticleId == Id);
            list.Remove(item);

            principal.Details = list;
            double total = 0;

            foreach (SpendingDetail det in list)
            {
                total = total + det.Total;
            }
            ViewBag.Total = total.ToString();

            Session["Total"] = total.ToString();

            Session["SessionSpending"] = principal;

            Session["esNuevo"] = false;

            return PartialView("~/Views/Spending/ArticlesTable.cshtml", principal);
        }


        [HttpPost]
        public ActionResult Create(SpendingVm model)
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
                SpendingVm principal = (SpendingVm)Session["SessionSpending"];
                model.Details = principal.Details;
                model.PaymentMethods = principal.PaymentMethods;
                bool currentAccount = (bool)Session["CurrentAccount"];

                if (model.Details.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Concepto.");
                }

                if (model.PaymentMethods.Count() == 0 && model.ddlPaymentMethods == null && !currentAccount)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar al menos un medio de Pago.");
                }
                else if (model.PaymentMethods.Count() == 0 && !currentAccount)
                {
                    SpendingPaymentMethod pm = new SpendingPaymentMethod();
                    pm.Amount = model.Amount;
                    pm.PaymentMethodId = int.Parse(model.ddlPaymentMethods.SelectedItem);
                    List<SpendingPaymentMethod> listPm = (List<SpendingPaymentMethod>)model.PaymentMethods;
                    listPm.Add(pm);
                    model.PaymentMethods = listPm;
                }
                else if (currentAccount)
                {
                    model.PaymentMethods = new List<SpendingPaymentMethod>();
                    model.ExpirationDate = principal.ExpirationDate;
                    model.Balance = model.Amount;
                }

                var spending = model.ToModelObject();
                double totalpago = 0;
                foreach (SpendingPaymentMethod pay in model.PaymentMethods)
                {
                    totalpago = totalpago + pay.Amount;
                }

                if (!currentAccount && totalpago != model.Amount)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La suma de los totales de los medios de pago debe ser igual al total del Gasto.");

                spending.UserId = _profileProvider.CurrentUserId;

                User userAuthorized = _userService.GetUser(spending.UserId);

                bool otraForma = false;

                foreach (SpendingDetail detalle in spending.Details)
                {
                    if (detalle.ExpenseAuthorizationId != null || detalle.ScheduledExpenseId != null)
                        otraForma = true;
                }

                if ((model.Amount > userAuthorized.Limit) && !otraForma)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El limite del usuario: " + userAuthorized.Limit + " es menor al monto del gasto programado");
                }


                if (spending.ExpirationDate != null && spending.ExecutionDate > spending.ExpirationDate)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Vencimiento no puede ser anterior a la fecha de Ejecución.");

                _spendingService.AddSpending(spending, principal.invoice);

                return Index();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        public ActionResult View(int id)
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
            model.ddlIVAPosition = collections.GetIVAPositionsSingleSelectVm();
            model.ddlPurchaseDocument = collections.GetPurchaseDocumentIdsSingleSelectVm();
            model.ddlPurchasePerception = collections.GetPurchasePerceptionIdsSingleSelectVm();
            ViewBag.SpendingId = id;

            return PartialView("~/Views/Spending/SpendingView.cshtml", model);
        }

        public ActionResult Edit(int id)
        {
            var entity = _spendingService.GetSpendingsById(id);
            var model = SpendingVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            var detail = _spendingService.GetSpendingDetails(id);

            Session["PMs"] = new List<int>();
            foreach (SpendingDetail detalle in detail)
            {

                if (!model.ExpenseAuthorizationId.HasValue)
                    model.ExpenseAuthorizationId = detalle.ExpenseAuthorizationId;
                if (!model.ScheduledExpenseId.HasValue)
                    model.ScheduledExpenseId = detalle.ScheduledExpenseId;
                if (!model.PurchaseRequestId.HasValue)
                    model.PurchaseRequestId = detalle.PurchaseRequestId;
            }
            bool todos = true;
            if (model.ScheduledExpenseId.HasValue)
            {
                model.ddlProvider = collections.GetProvidersByIdSingleSelectVm(entity.ProveedorId.Value);
                todos = false;
            }
            else if (model.PurchaseRequestId.HasValue)
            {
                PurchaseRequest purchase = _purchaseRequestService.GetPurchaseRequestsById(model.PurchaseRequestId.Value);
                if (purchase.ProveedorId.HasValue)
                {
                    model.ddlProvider = collections.GetProvidersByIdSingleSelectVm(purchase.ProveedorId.Value);
                    todos = false;
                }
            }
            else if (model.ExpenseAuthorizationId.HasValue)
            {
                ExpenseAuthorization expense = _expenseAuthorizationService.GetExpenseAuthorizationsById(model.ExpenseAuthorizationId.Value);
                if (expense.ProveedorId.HasValue)
                {
                    model.ddlProvider = collections.GetProvidersByIdSingleSelectVm(expense.ProveedorId.Value);
                    todos = false;
                }
            }

            if (todos == true)
                model.ddlProvider = collections.GetProviderIdsSingleSelectVm();

            Invoice invoice = _spendingService.GetInvoiceBySpendingId(id);

            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.ddlPaymentMethods = collections.GetPaymentMethodSingleSelectVm();
            model.ddlPaymentMethodPM = collections.GetPaymentMethodSingleSelectVm();
            model.ddlIVAPosition = collections.GetIVAPositionsSingleSelectVm();
            model.ddlPurchaseDocument = collections.GetPurchaseDocumentIdsSingleSelectVm();
            model.ddlPurchasePerception = collections.GetPurchasePerceptionIdsSingleSelectVm();
            ViewBag.SpendingId = id;
            if (invoice != null)
            {
                model.ddlIVAPosition.SelectedItem = invoice.IvaPositionId.ToString();
                model.ddlPurchaseDocument.SelectedItem = invoice.PurchaseDocumentId.ToString();
                model.ddlPurchasePerception.SelectedItem = invoice.PurchasePerceptionId.ToString();
                model.IvaPositionInt = invoice.IvaPositionId;
                model.IBusinessName = invoice.BusinessName;
                model.ICUIT = invoice.CUIT;
                model.IExempt = invoice.Exempt;
                model.IImputDate = invoice.ImputDate.ToShortDateString();
                model.IIVA105 = invoice.IVA105;
                model.IIVA21 = invoice.IVA21;
                model.IIVA22 = invoice.IVA22;
                model.IIVA25 = invoice.IVA25;
                model.IIVA5 = invoice.IVA5;
                model.ILetter = invoice.Letter;
                model.INet = invoice.Net;
                model.INumber = invoice.Number;
                model.IOtherTaxes = invoice.OtherTaxes;
                model.IPerceptionAmount = invoice.PerceptionAmount;
                model.IPrefix = invoice.Prefix;
                model.invoice = invoice;
            }

            Session["SessionSpending"] = model;

            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserSE = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserEA = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.AmountPayment = model.Amount;
            model.Saving = _cashierService.GetCashiersById(_userService.GetUserCashierByUserId(_profileProvider.CurrentUserId).CashierId).Name;
            if (model.ExpirationDate != null && model.ExpirationDate != "")
            {
                model.chkCurrentAccount = true;
                Session["CurrentAccount"] = true;
            }
            else
            {
                model.chkCurrentAccount = false;
                Session["CurrentAccount"] = false;
            }


            model.chkImputed = _spendingService.IsImputed(id);

            if (_spendingService.IsEditable(id))
                return PartialView("~/Views/Spending/SpendingEdit.cshtml", model);
            else
                return PartialView("~/Views/Spending/SpendingEditShort.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(SpendingVm model)
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
                SpendingVm principal = (SpendingVm)Session["SessionSpending"];
                model.Details = principal.Details;
                model.PaymentMethods = principal.PaymentMethods;
                bool currentAccount = (bool)Session["CurrentAccount"];

                if (model.Details.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Concepto.");
                }

                model.ExpirationDate = null;
                model.Balance = 0;

                if (model.PaymentMethods.Count() == 0 && model.ddlPaymentMethods == null && !currentAccount)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar al menos un medio de Pago.");
                }
                else if (model.PaymentMethods.Count() == 0 && !currentAccount)
                {
                    SpendingPaymentMethod pm = new SpendingPaymentMethod();
                    pm.Amount = model.Amount;
                    pm.PaymentMethodId = int.Parse(model.ddlPaymentMethods.SelectedItem);
                    List<SpendingPaymentMethod> listPm = (List<SpendingPaymentMethod>)model.PaymentMethods;
                    listPm.Add(pm);
                    model.PaymentMethods = listPm;
                }
                else if (currentAccount)
                {
                    model.PaymentMethods = new List<SpendingPaymentMethod>();
                    model.ExpirationDate = principal.ExpirationDate;
                    model.Balance = model.Amount;
                }

                Spending spending = model.ToModelObject();
                spending.UserId = _profileProvider.CurrentUserId;

                double totalpago = 0;
                foreach (SpendingPaymentMethod pay in model.PaymentMethods)
                {
                    totalpago = totalpago + pay.Amount;
                }

                if (!currentAccount && totalpago != model.Amount)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La suma de los totales de los medios de pago debe ser igual al total del Gasto.");

                if (spending.ExpirationDate != null && spending.ExecutionDate > spending.ExpirationDate)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Vencimiento no puede ser anterior a la fecha de Ejecución.");

                _spendingService.UpdateSpending(spending, principal.invoice);
                SaveFile(model);
                return Index();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                if (_spendingService.IsEditable(id))
                {
                    _spendingService.DeleteSpending(id);
                    return Index();
                }
                else
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "No se puede eliminar el gasto");

            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al eliminar");
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

        public ActionResult GetSavedFilesBySpending(int SpendingId)
        {
            var result = _spendingService.GetSpendingDocuments(SpendingId);
            var model = result.Select(x => new DataUploadFilesResultVm { Name = x.Name, Length = x.ContentLength, Path = x.Path });
            List<DataUploadFilesResultVm> resultado = new List<DataUploadFilesResultVm>();
            foreach (DataUploadFilesResultVm file in model)
            {
                resultado.Add(file);
            }

            model = resultado;
            return PartialView("~/Views/Spending/SpendingAttach.cshtml", model);
        }

        public ActionResult GetSavedFilesBySpendingView(int SpendingId)
        {
            var result = _spendingService.GetSpendingDocuments(SpendingId);
            var model = result.Select(x => new DataUploadFilesResultVm { Name = x.Name, Length = x.ContentLength, Path = x.Path });
            List<DataUploadFilesResultVm> resultado = new List<DataUploadFilesResultVm>();
            foreach (DataUploadFilesResultVm file in model)
            {
                resultado.Add(file);
            }

            model = resultado;
            return PartialView("~/Views/Spending/SpendingAttachView.cshtml", model);
        }

        public void SaveFile(SpendingVm spending)
        {
            string fName = string.Empty;

            try
            {
                if (SessionProvider.UpLoadFiles != null)
                {
                    _spendingService.DeleteSpendingFile(spending.SpendingId);
                    foreach (HttpPostedFileBase fileName in SessionProvider.UpLoadFiles)
                    {
                        //   HttpPostedFileBase file = fileName;
                        //Save file content goes here
                        if (fileName != null && fileName.ContentLength > 0)
                        {
                            _spendingService.InsertDocument(fileName, spending.SpendingId, spending.SpendingId.ToString());
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

        public ActionResult Invoice(int IvaPositionId, int PurchaseDocumentId, int PurchasePerceptionId, string BusinessName, string CUIT, double? Exempt, double? IVA105,
                double? IVA21, double? IVA22, double? IVA25, double? IVA5, double? OtherTaxes, string Letter, double? Net, int Number, double? PerceptionAmount, int Prefix)
        {
            SpendingVm principal = (SpendingVm)Session["SessionSpending"];

            Invoice invoice = new Invoice();

            invoice.IvaPositionId = IvaPositionId;
            invoice.PurchaseDocumentId = PurchaseDocumentId;
            invoice.PurchasePerceptionId = PurchasePerceptionId;
            invoice.BusinessName = BusinessName;
            invoice.CUIT = CUIT;
            if (Exempt.HasValue)
                invoice.Exempt = Exempt.Value;
            if (IVA105.HasValue)
                invoice.IVA105 = IVA105.Value;
            if (IVA21.HasValue)
                invoice.IVA21 = IVA21.Value;
            if (IVA22.HasValue)
                invoice.IVA22 = IVA22.Value;
            if (IVA25.HasValue)
                invoice.IVA25 = IVA25.Value;
            if (IVA5.HasValue)
                invoice.IVA5 = IVA5.Value;
            invoice.Letter = Letter;
            if (Net.HasValue)
                invoice.Net = Net.Value;
            invoice.Number = Number;
            if (OtherTaxes.HasValue)
                invoice.OtherTaxes = OtherTaxes.Value;
            if (PerceptionAmount.HasValue)
                invoice.PerceptionAmount = PerceptionAmount.Value;
            invoice.Prefix = Prefix;


            Invoice invoiceExiste = _spendingService.GetInvoiceByFilter(invoice.CUIT, invoice.PurchaseDocumentId, Letter, Prefix, Number, principal.SpendingId);

            if (invoiceExiste == null)
            {
                principal.invoice = invoice;
                principal.Receipt = invoice.Letter + "-" + invoice.Prefix + "-" + invoice.Number;
                Session["SessionSpending"] = principal;
                ViewBag.ErrorMessageInvoice = "true";
                return PartialView("~/Views/Spending/ArticlesTable.cshtml", principal);
            }

            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La Factura ingresada ya se ha registrado en otro pago");
            }
        }

        public ActionResult infoByProvider(int providerId)
        {
            SpendingVm model = (SpendingVm)Session["SessionSpending"];
            Provider provider = _providerService.GetProvidersById(providerId);
            model.ddlIVAPosition.SelectedItem = provider.IVAPositionId.ToString();
            model.IBusinessName = provider.BusinessName;
            model.ICUIT = provider.CUIT;
            model.IvaPositionInt = provider.IVAPositionId;
            Session["SessionSpending"] = model;
            ViewBag.ErrorMessageInvoice = "false";
            ViewBag.ErrorMessageTrabajo = "false";
            return PartialView("~/Views/Spending/ArticlesTable.cshtml", model);
        }
    }
}