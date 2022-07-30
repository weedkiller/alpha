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
    public class ExpenseAuthorizationController : Controller
    {
        private ExpenseAuthorizationService _expenseAuthorizationService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProviderService _providerService;
        private ArticleService _articleService;
        private PurchaseRequestService _purchaseRequestService;
        private ScheduledExpenseService _scheduledExpenseService;
        private ProfileProvider _profileProvider;

        public ExpenseAuthorizationController()
        {
            _expenseAuthorizationService = new ExpenseAuthorizationService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _providerService = new ProviderService();
            _articleService = new ArticleService();
            _purchaseRequestService = new PurchaseRequestService();
            _scheduledExpenseService = new ScheduledExpenseService();
            _profileProvider = new ProfileProvider();
        }

        // GET: /ExpenseAuthorization/
        [LogActionFilter]
        [Description("ExpenseAuthorization")]
        public ActionResult Index()
        {
            ExpenseAuthorizationFilterVm model = new ExpenseAuthorizationFilterVm();
            var collections = new CollectionHelper();
            model.ListProvider = new SelectList(_providerService.GetProvidersAndEmployeesWithId(), "ProviderId", "Name");
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserBuyer = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.UserCreateId = _profileProvider.CurrentUserId;
            return PartialView("~/Views/ExpenseAuthorization/ExpenseAuthorization.cshtml", model);
        }

        [LogActionFilter]
        [Description("ExpenseAuthorizationManaged")]
        public ActionResult IndexManaged()
        {
            var user = _profileProvider.CurrentUserId;
            var model = ExpenseAuthorizationTableVm.ToViewModel(_expenseAuthorizationService.GetExpenseAuthorizationSummaryManaged(user));
            return PartialView("~/Views/ExpenseAuthorization/ExpenseAuthorizationManaged.cshtml", model);
        }

        [HttpGet]
        public ActionResult GetTable(int? SelectedProvider, int? SelectedUser, string DateFromCreate, string DateToCreate, string DateFromPurchase, string DateToPurchase, string AmountFrom, string AmountTo, string PurchaseRequestId, int? SelectedUserBuyer, string Processed)
        {
            var model = ExpenseAuthorizationTableVm.ToViewModel(_expenseAuthorizationService.GetExpenseAuthorizationSummary(SelectedProvider, SelectedUser, DateFromCreate, DateToCreate, DateFromPurchase, DateToPurchase, AmountFrom, AmountTo, PurchaseRequestId, SelectedUserBuyer, Processed));
            return PartialView("~/Views/ExpenseAuthorization/ExpenseAuthorizationTable.cshtml", model);
        }

        [HttpGet]
        public ActionResult GetTablePurchase(int? SelectedProvider, int? SelectedUser, string DateFromCreate, string DateToCreate, string DateFromNeed, string DateToNeed, string AmountFrom, string AmountTo, string Active)
        {
            var model = PurchaseRequestTableVm.ToViewModel(_purchaseRequestService.GetPurchaseRequestsSummary(0, SelectedProvider, SelectedUser, 0, DateFromCreate, DateToCreate, DateFromNeed, DateToNeed, AmountFrom, AmountTo, Active, "0"));
            return PartialView("~/Views/ExpenseAuthorization/PurchaseRequestTable.cshtml", model);
        }
        [HttpGet]
        public ActionResult GetTableScheduled(int? SelectedProvider, int? SelectedUser, string DateFromCreate, string DateToCreate, string DateFromPurchase, string DateToPurchase, string AmountFrom, string AmountTo, string PurchaseRequestId, string isAuthorized)
        {
            var model = ScheduledExpenseTableVm.ToViewModel(_scheduledExpenseService.GetScheduledExpensesSummary(SelectedProvider, SelectedUser, DateFromCreate, DateToCreate, DateFromPurchase, DateToPurchase, AmountFrom, AmountTo, PurchaseRequestId, isAuthorized, "0", "0", "1"));
            return PartialView("~/Views/ExpenseAuthorization/ScheduledExpenseTable.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear ExpensesAuthorization")]
        public ActionResult Create()
        {
            var model = new ExpenseAuthorizationVm();
            var collections = new CollectionHelper();
            ViewBag.UsuarioIgualAutorizado = "1";
            List<ExpenseAuthorizationDetail> lists = new List<ExpenseAuthorizationDetail>();
            model.Details = lists;
            model.CreateDate = DateTime.Now.ToShortDateString();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUser.SelectedItem = _profileProvider.CurrentUserId.ToString();
            model.ddlUserBuyer = collections.GetUsersSingleSelectVm();
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserSE = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            Session["SessionExpenseAuthorization"] = model;
            return PartialView("~/Views/ExpenseAuthorization/ExpenseAuthorizationCreate.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear ExpensesAuthorization")]
        public ActionResult CreateFromPurchaseRequest(int purchaseRequestId)
        {
            ViewBag.UsuarioIgualAutorizado = "1";
            PurchaseRequest purchase = _purchaseRequestService.GetPurchaseRequestsById(purchaseRequestId);

            var model = new ExpenseAuthorizationVm();
            var collections = new CollectionHelper();
            List<ExpenseAuthorizationDetail> lists = new List<ExpenseAuthorizationDetail>();
            foreach (PurchaseRequestDetail detail in purchase.Details)
            {
                ExpenseAuthorizationDetail detalle = new ExpenseAuthorizationDetail();
                detalle.Article = detail.Article;
                detalle.ArticleId = detail.ArticleId;
                detalle.Price = detail.Price;
                detalle.Quantity = detail.Quantity;
                detalle.Total = detail.Total;
                detalle.Description = detail.Description;
                lists.Add(detalle);
            }
            model.Details = lists;
            model.CreateDate = DateTime.Now.ToShortDateString();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUser.SelectedItem = _profileProvider.CurrentUserId.ToString();
            model.ddlUserBuyer = collections.GetUsersSingleSelectVm();
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.ddlProvider.SelectedItem = purchase.ProveedorId.ToString();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserSE = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.PurchaseRequestId = purchaseRequestId;
            model.Amount = purchase.Amount;
            Session["SessionExpenseAuthorization"] = model;
            return PartialView("~/Views/ExpenseAuthorization/ExpenseAuthorizationCreate.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear ExpensesAuthorization")]
        public ActionResult CreateFromScheduledExpense(int scheduledExpenseId)
        {

            ScheduledExpense scheduled = _scheduledExpenseService.GetScheduledExpensesById(scheduledExpenseId);

            var model = new ExpenseAuthorizationVm();
            var collections = new CollectionHelper();
            List<ExpenseAuthorizationDetail> lists = new List<ExpenseAuthorizationDetail>();
            foreach (ScheduledExpenseDetail detail in scheduled.Details)
            {
                ExpenseAuthorizationDetail detalle = new ExpenseAuthorizationDetail();
                detalle.Article = detail.Article;
                detalle.ArticleId = detail.ArticleId;
                detalle.Price = detail.Price;
                detalle.Quantity = detail.Quantity;
                detalle.Total = detail.Total;
                detalle.Description = detail.Description;
                lists.Add(detalle);
            }
            if (scheduled.UserAuthorizedId != scheduled.UserId)
            {
                ViewBag.UsuarioIgualAutorizado = "0";
                model.ddlProvider = collections.GetProvidersByIdSingleSelectVm(scheduled.ProveedorId);
                model.ddlProvider.SelectedItem = scheduled.ProveedorId.ToString();
            }

            else
            {
                ViewBag.UsuarioIgualAutorizado = "1";
                model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
                model.ddlProvider.SelectedItem = scheduled.ProveedorId.ToString();
            }

            model.Details = lists;
            model.CreateDate = DateTime.Now.ToShortDateString();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUser.SelectedItem = _profileProvider.CurrentUserId.ToString();
            model.ddlUserBuyer = collections.GetUsersSingleSelectVm();
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.ScheduledExpenseId = scheduledExpenseId;
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserSE = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.Amount = scheduled.Amount;
            Session["SessionExpenseAuthorization"] = model;
            return PartialView("~/Views/ExpenseAuthorization/ExpenseAuthorizationCreate.cshtml", model);
        }

        public ActionResult IndexArticles()
        {
            ExpenseAuthorizationVm model = (ExpenseAuthorizationVm)Session["SessionExpenseAuthorization"];
            ViewBag.ErrorMessageTrabajo = "false";
            return PartialView("~/Views/ExpenseAuthorization/ArticlesTable.cshtml", model);
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
            ExpenseAuthorizationVm principal = (ExpenseAuthorizationVm)Session["SessionExpenseAuthorization"];
            if (ArticleId.HasValue)
            {
                ExpenseAuthorizationDetail detail = new ExpenseAuthorizationDetail();
                detail.ArticleId = ArticleId.Value;
                detail.Quantity = QuantityArticle;
                detail.Price = PriceArticle;
                detail.Total = QuantityArticle * PriceArticle;
                detail.Article = _articleService.GetArticlesById(ArticleId.Value).Name;
                detail.Description = DescriptionArticle;

                List<ExpenseAuthorizationDetail> list = (List<ExpenseAuthorizationDetail>)principal.Details;
                Int32 id = ArticleId.Value;
                var item = list.FirstOrDefault(x => x.ArticleId == id);
                if (item != null)
                    existe = true;
                else
                    list.Add(detail);
                principal.Details = list;


                double total = 0;

                foreach (ExpenseAuthorizationDetail det in list)
                {
                    total = total + det.Total;
                }
                ViewBag.Total = total.ToString();

                Session["Total"] = total.ToString();

                Session["SessionExpenseAuthorization"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessageTrabajo = "false";
                    return PartialView("~/Views/ExpenseAuthorization/ArticlesTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessageTrabajo = "true";
                    return PartialView("~/Views/ExpenseAuthorization/ArticlesTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un articulo");
            }
        }

        public ActionResult ArticlesUpdate(int QuantityArticle, int? ArticleId, double PriceArticle, string DescriptionArticle)
        {
            bool existe = false;
            ExpenseAuthorizationVm principal = (ExpenseAuthorizationVm)Session["SessionExpenseAuthorization"];
            if (ArticleId.HasValue)
            {
                ExpenseAuthorizationDetail detail = new ExpenseAuthorizationDetail();
                detail.ArticleId = ArticleId.Value;
                detail.Quantity = QuantityArticle;
                detail.Price = PriceArticle;
                detail.Total = QuantityArticle * PriceArticle;
                detail.Article = _articleService.GetArticlesById(ArticleId.Value).Name;
                detail.Description = DescriptionArticle;

                List<ExpenseAuthorizationDetail> list = (List<ExpenseAuthorizationDetail>)principal.Details;
                Int32 id = ArticleId.Value;
                var item = list.FirstOrDefault(x => x.ArticleId == id);
                list.Remove(item);
                list.Add(detail);
                principal.Details = list;


                double total = 0;

                foreach (ExpenseAuthorizationDetail det in list)
                {
                    total = total + det.Total;
                }
                ViewBag.Total = total.ToString();

                Session["Total"] = total.ToString();

                Session["SessionExpenseAuthorization"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessageTrabajo = "false";
                    return PartialView("~/Views/ExpenseAuthorization/ArticlesTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessageTrabajo = "true";
                    return PartialView("~/Views/ExpenseAuthorization/ArticlesTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un articulo");
            }
        }

        public ActionResult RemoveArticles(int Id)
        {
            ExpenseAuthorizationVm principal = (ExpenseAuthorizationVm)Session["SessionExpenseAuthorization"];
            List<ExpenseAuthorizationDetail> list = (List<ExpenseAuthorizationDetail>)principal.Details;
            var item = list.FirstOrDefault(x => x.ArticleId == Id);
            list.Remove(item);

            principal.Details = list;
            double total = 0;

            foreach (ExpenseAuthorizationDetail det in list)
            {
                total = total + det.Total;
            }
            ViewBag.Total = total.ToString();

            Session["Total"] = total.ToString();

            Session["SessionExpenseAuthorization"] = principal;

            Session["esNuevo"] = false;

            return PartialView("~/Views/ExpenseAuthorization/ArticlesTable.cshtml", principal);
        }


        [HttpPost]
        public ActionResult Create(ExpenseAuthorizationVm model)
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
                ExpenseAuthorizationVm principal = (ExpenseAuthorizationVm)Session["SessionExpenseAuthorization"];
                model.Details = principal.Details;
                if (model.Details.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Concepto.");
                }
                model.PurchaseDate = model.CreateDate;
                var purchase = model.ToModelObject();

                User userAuthorized = _userService.GetUser(purchase.UserId);

                if (model.Amount > userAuthorized.Limit)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El limite del Autorizador / Comprador: " + userAuthorized.Limit + " es menor al monto de la solicitud");
                }
                _expenseAuthorizationService.AddExpenseAuthorization(purchase);

                return Index();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        public ActionResult View(int id)
        {
            var entity = _expenseAuthorizationService.GetExpenseAuthorizationsById(id);
            var model = ExpenseAuthorizationVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUserBuyer = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlUserBuyer.SelectedItem = entity.UserBuyerId.ToString();

            return PartialView("~/Views/ExpenseAuthorization/ExpenseAuthorizationView.cshtml", model);
        }

        public ActionResult ViewManaged(int id)
        {
            var entity = _expenseAuthorizationService.GetExpenseAuthorizationsById(id);
            var model = ExpenseAuthorizationVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUserBuyer = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlUserBuyer.SelectedItem = entity.UserBuyerId.ToString();

            return PartialView("~/Views/ExpenseAuthorization/ExpenseAuthorizationViewManaged.cshtml", model);
        }

        public ActionResult Edit(int id)
        {
            var entity = _expenseAuthorizationService.GetExpenseAuthorizationsById(id);
            var model = ExpenseAuthorizationVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            if (entity.ScheduledExpenseId.HasValue)
                model.ddlProvider = collections.GetProvidersByIdSingleSelectVm(entity.ScheduledExpenseId.Value);
            else
                model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUserBuyer = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlUserBuyer.SelectedItem = entity.UserBuyerId.ToString();
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserSE = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            Session["SessionExpenseAuthorization"] = model;

            return PartialView("~/Views/ExpenseAuthorization/ExpenseAuthorizationEdit.cshtml", model);
        }

        public ActionResult EditManaged(int id)
        {
            var entity = _expenseAuthorizationService.GetExpenseAuthorizationsById(id);
            var model = ExpenseAuthorizationVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            if (entity.ScheduledExpenseId.HasValue)
                model.ddlProvider = collections.GetProvidersByIdSingleSelectVm(entity.ProveedorId.Value);
            else if (entity.ScheduledExpenseId.HasValue)
                model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUserBuyer = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlUserBuyer.SelectedItem = entity.UserBuyerId.ToString();
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserSE = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            Session["SessionExpenseAuthorization"] = model;

            return PartialView("~/Views/ExpenseAuthorization/ExpenseAuthorizationEditManaged.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(ExpenseAuthorizationVm model)
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
                ExpenseAuthorizationVm principal = (ExpenseAuthorizationVm)Session["SessionExpenseAuthorization"];
                model.Details = principal.Details;
                model.PurchaseDate = model.CreateDate;
                if (model.Details.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Concepto.");
                }


                ExpenseAuthorization purchase = model.ToModelObject();

                User userAuthorized = _userService.GetUser(purchase.UserId);

                if (model.Amount > userAuthorized.Limit)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El limite del Autorizador / Comprador: " + userAuthorized.Limit + " es menor al monto de la solicitud");
                }

                purchase.Active = principal.Active;
                purchase.Managed = principal.Managed;
                purchase.Processed = principal.Processed;
                purchase.CreateDate = Convert.ToDateTime(principal.CreateDate);
                _expenseAuthorizationService.UpdateExpenseAuthorization(purchase);

                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        [HttpPost]
        public ActionResult EditManaged(ExpenseAuthorizationVm model)
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
                ExpenseAuthorizationVm principal = (ExpenseAuthorizationVm)Session["SessionExpenseAuthorization"];
                model.Details = principal.Details;
                model.PurchaseDate = model.CreateDate;
                if (model.Details.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Concepto.");
                }


                ExpenseAuthorization purchase = model.ToModelObject();

                User userAuthorized = _userService.GetUser(purchase.UserId);

                if (model.Amount > userAuthorized.Limit)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El limite del Autorizador / Comprador: " + userAuthorized.Limit + " es menor al monto de la solicitud");
                }

                purchase.Active = principal.Active;
                purchase.Managed = true;
                purchase.Processed = principal.Processed;
                purchase.CreateDate = Convert.ToDateTime(principal.CreateDate);
                _expenseAuthorizationService.UpdateExpenseAuthorization(purchase);

                return IndexManaged();
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
                _expenseAuthorizationService.DeleteExpenseAuthorization(id);
                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

    }
}