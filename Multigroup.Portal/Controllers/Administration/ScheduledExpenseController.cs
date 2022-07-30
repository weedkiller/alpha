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
    public class ScheduledExpenseController : Controller
    {
        private ScheduledExpenseService _scheduledExpenseService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProviderService _providerService;
        private ArticleService _articleService;
        private PurchaseRequestService _purchaseRequestService;
        private ProfileProvider _profileProvider;

        public ScheduledExpenseController()
        {
            _scheduledExpenseService = new ScheduledExpenseService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _providerService = new ProviderService();
            _articleService = new ArticleService();
            _purchaseRequestService = new PurchaseRequestService();
            _profileProvider = new ProfileProvider();
        }

        // GET: /ScheduledExpense/
        [LogActionFilter]
        [Description("ScheduledExpenses")]
        public ActionResult Index()
        {
            ScheduledExpenseFilterVm model = new ScheduledExpenseFilterVm();
            var collections = new CollectionHelper();
            model.ListProvider = new SelectList(_providerService.GetProvidersWithId(), "ProviderId", "Name");
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListAuthorized = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.Authorized = true;
            model.UserCreateId = _profileProvider.CurrentUserId;
            return PartialView("~/Views/ScheduledExpense/ScheduledExpense.cshtml", model);
        }

        [LogActionFilter]
        [Description("ScheduledExpensesManaged")]
        public ActionResult IndexManaged()
        {
            var user = _profileProvider.CurrentUserId;
            var model = ScheduledExpenseTableVm.ToViewModel(_scheduledExpenseService.GetScheduledExpenseSummaryManaged(user));
            return PartialView("~/Views/ScheduledExpense/ScheduledExpenseManaged.cshtml", model);
        }

        [LogActionFilter]
        [Description("ScheduledExpenses")]
        public ActionResult IndexFromAuth()
        {
            ScheduledExpenseFilterVm model = new ScheduledExpenseFilterVm();
            var collections = new CollectionHelper();
            model.ListProvider = new SelectList(_providerService.GetProvidersWithId(), "ProviderId", "Name");
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserAuth = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.Authorized = true;
            return PartialView("~/Views/ScheduledExpense/ScheduledExpenseFromAuth.cshtml", model);
        }

        [LogActionFilter]
        [Description("ScheduledExpenses")]
        public ActionResult IndexAuth()
        {
            ScheduledExpenseFilterVm model = new ScheduledExpenseFilterVm();
            var collections = new CollectionHelper();
            model.ListProvider = new SelectList(_providerService.GetProvidersWithId(), "ProviderId", "Name");
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListAuthorized= new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.Authorized = true;
            return PartialView("~/Views/ScheduledExpense/ScheduledExpenseAuth.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedProvider, int? SelectedUser, string DateFromCreate, string DateToCreate, string DateFromPurchase, string DateToPurchase, string AmountFrom, string AmountTo, string PurchaseRequestId, string isAuthorized, string Processed, int? SelectedAuthorized)
        {
            if (SelectedAuthorized == null)
                SelectedAuthorized = 0;
            var model = ScheduledExpenseTableVm.ToViewModel(_scheduledExpenseService.GetScheduledExpensesSummary(SelectedProvider, SelectedUser, DateFromCreate, DateToCreate, DateFromPurchase, DateToPurchase, AmountFrom,  AmountTo, PurchaseRequestId, isAuthorized, Processed, SelectedAuthorized.ToString(), "1"));
            return PartialView("~/Views/ScheduledExpense/ScheduledExpenseTable.cshtml", model);
        }

        public ActionResult GetTableAuth(int? SelectedProvider, int? SelectedUser, string DateFromCreate, string DateToCreate, string DateFromPurchase, string DateToPurchase, string AmountFrom, string AmountTo, string PurchaseRequestId, string isAuthorized, string Processed)
        {
            var user = _profileProvider.CurrentUserId;
            var model = ScheduledExpenseTableVm.ToViewModel(_scheduledExpenseService.GetScheduledExpensesSummary(SelectedProvider, SelectedUser, DateFromCreate, DateToCreate, DateFromPurchase, DateToPurchase, AmountFrom, AmountTo, PurchaseRequestId, isAuthorized, Processed, user.ToString(), "1"));
            return PartialView("~/Views/ScheduledExpense/ScheduledExpenseAuthTable.cshtml", model);
        }

        public ActionResult GetTableFromAuth(int? SelectedProvider, int? SelectedUser, string DateFromCreate, string DateToCreate, string DateFromPurchase, string DateToPurchase, string AmountFrom, string AmountTo, string PurchaseRequestId, string isAuthorized, string Processed, string userAuth)
        {
            if (userAuth == null || userAuth == "")
                userAuth = "0";
            var model = ScheduledExpenseTableVm.ToViewModel(_scheduledExpenseService.GetScheduledExpensesSummary(SelectedProvider, SelectedUser, DateFromCreate, DateToCreate, DateFromPurchase, DateToPurchase, AmountFrom, AmountTo, PurchaseRequestId, isAuthorized, Processed, userAuth, "1"));
            return PartialView("~/Views/ScheduledExpense/ScheduledExpenseAuthTable.cshtml", model);
        }

        [HttpGet]
        public ActionResult GetTablePurchase(int? SelectedProvider, int? SelectedUser, string DateFromCreate, string DateToCreate, string DateFromNeed, string DateToNeed, string AmountFrom, string AmountTo, string Active)
        {
            var model = PurchaseRequestTableVm.ToViewModel(_purchaseRequestService.GetPurchaseRequestsSummary(0, SelectedProvider, SelectedUser, _profileProvider.CurrentUserId, DateFromCreate, DateToCreate, DateFromNeed, DateToNeed, AmountFrom, AmountTo, Active, "0"));
            return PartialView("~/Views/ScheduledExpense/PurchaseRequestTable.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear ScheduledExpenses")]
        public ActionResult Create()
        {
            var model = new ScheduledExpenseVm();
            var collections = new CollectionHelper();
            List<ScheduledExpenseDetail> lists = new List<ScheduledExpenseDetail>();
            model.Details = lists;
            model.MassCreation = false;
            model.MonthlyPeriod = true;
            model.IsAuthorized = true;
            model.CreateDate = DateTime.Now.ToShortDateString();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUser.SelectedItem = _profileProvider.CurrentUserId.ToString();
            model.ddlUserAuthorized = collections.GetUsersSingleSelectVm();
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            Session["SessionScheduledExpense"] = model;
            return PartialView("~/Views/ScheduledExpense/ScheduledExpenseCreate.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear ScheduledExpenses")]
        public ActionResult CreateFromPurchaseRequest(int purchaseRequestId)
        {
            PurchaseRequest purchase = _purchaseRequestService.GetPurchaseRequestsById(purchaseRequestId);


            var model = new ScheduledExpenseVm();
            var collections = new CollectionHelper();
            List<ScheduledExpenseDetail> lists = new List<ScheduledExpenseDetail>();
            foreach(PurchaseRequestDetail detail in purchase.Details)
            {
                ScheduledExpenseDetail detalle = new ScheduledExpenseDetail();
                detalle.Article = detail.Article;
                detalle.ArticleId = detail.ArticleId;
                detalle.Price = detail.Price;
                detalle.Quantity = detail.Quantity;
                detalle.Total = detail.Total;
                detalle.Description = detail.Description;
                lists.Add(detalle);
            }
            model.Details = lists;
            model.MassCreation = false;
            model.MonthlyPeriod = true;
            model.IsAuthorized = true;
            model.CreateDate = DateTime.Now.ToShortDateString();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUser.SelectedItem = _profileProvider.CurrentUserId.ToString();
            model.ddlUserAuthorized = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = purchase.ProveedorId.ToString();
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.PurchaseRequestId = purchaseRequestId;
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.Amount = purchase.Amount;
            Session["SessionScheduledExpense"] = model;
            return PartialView("~/Views/ScheduledExpense/ScheduledExpenseCreate.cshtml", model);
        }

        public ActionResult IndexArticles()
        {
            ScheduledExpenseVm model = (ScheduledExpenseVm)Session["SessionScheduledExpense"];
            ViewBag.ErrorMessageTrabajo = "false";
            return PartialView("~/Views/ScheduledExpense/ArticlesTable.cshtml", model);
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
            ScheduledExpenseVm principal = (ScheduledExpenseVm)Session["SessionScheduledExpense"];
            if (ArticleId.HasValue)
            {
                ScheduledExpenseDetail detail = new ScheduledExpenseDetail();
                detail.ArticleId = ArticleId.Value;
                detail.Quantity = QuantityArticle;
                detail.Price = PriceArticle;
                detail.Total = QuantityArticle * PriceArticle;
                detail.Article = _articleService.GetArticlesById(ArticleId.Value).Name;
                detail.Description = DescriptionArticle;

                List<ScheduledExpenseDetail> list = (List<ScheduledExpenseDetail>)principal.Details;
                Int32 id = ArticleId.Value;
                var item = list.FirstOrDefault(x => x.ArticleId == id);
                if (item != null)
                    existe = true;
                else
                    list.Add(detail);
                principal.Details = list;


                double total = 0;

                foreach (ScheduledExpenseDetail det in list)
                {
                    total = total + det.Total;
                }
                ViewBag.Total = total.ToString();

                Session["Total"] = total.ToString();

                Session["SessionScheduledExpense"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessageTrabajo = "false";
                    return PartialView("~/Views/ScheduledExpense/ArticlesTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessageTrabajo = "true";
                    return PartialView("~/Views/ScheduledExpense/ArticlesTable.cshtml", principal);
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
            ScheduledExpenseVm principal = (ScheduledExpenseVm)Session["SessionScheduledExpense"];
            if (ArticleId.HasValue)
            {
                ScheduledExpenseDetail detail = new ScheduledExpenseDetail();
                detail.ArticleId = ArticleId.Value;
                detail.Quantity = QuantityArticle;
                detail.Price = PriceArticle;
                detail.Total = QuantityArticle * PriceArticle;
                detail.Article = _articleService.GetArticlesById(ArticleId.Value).Name;
                detail.Description = DescriptionArticle;

                List<ScheduledExpenseDetail> list = (List<ScheduledExpenseDetail>)principal.Details;
                Int32 id = ArticleId.Value;
                var item = list.FirstOrDefault(x => x.ArticleId == id);
                list.Remove(item);
                list.Add(detail);
                principal.Details = list;


                double total = 0;

                foreach (ScheduledExpenseDetail det in list)
                {
                    total = total + det.Total;
                }
                ViewBag.Total = total.ToString();

                Session["Total"] = total.ToString();

                Session["SessionScheduledExpense"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessageTrabajo = "false";
                    return PartialView("~/Views/ScheduledExpense/ArticlesTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessageTrabajo = "true";
                    return PartialView("~/Views/ScheduledExpense/ArticlesTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un articulo");
            }
        }

        public ActionResult RemoveArticles(int Id)
        {
            ScheduledExpenseVm principal = (ScheduledExpenseVm)Session["SessionScheduledExpense"];
            List<ScheduledExpenseDetail> list = (List<ScheduledExpenseDetail>)principal.Details;
            var item = list.FirstOrDefault(x => x.ArticleId == Id);
            list.Remove(item);

            principal.Details = list;
            double total = 0;

            foreach (ScheduledExpenseDetail det in list)
            {
                total = total + det.Total;
            }
            ViewBag.Total = total.ToString();

            Session["Total"] = total.ToString();

            Session["SessionScheduledExpense"] = principal;

            Session["esNuevo"] = false;

            return PartialView("~/Views/ScheduledExpense/ArticlesTable.cshtml", principal);
        }

        public ActionResult ConfirmScheduled(int id)
        {
            try
            {
                ScheduledExpense sche = _scheduledExpenseService.GetScheduledExpensesById(id);
                sche.IsAuthorized = true;
                _scheduledExpenseService.UpdateScheduledExpense(sche);
                return IndexAuth();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al Confirmar");
            }
        }

        public ActionResult DesconfirmScheduled(int id)
        {
            try
            {
                ScheduledExpense sche = _scheduledExpenseService.GetScheduledExpensesById(id);
                sche.IsAuthorized = false;
                _scheduledExpenseService.UpdateScheduledExpense(sche);
                return IndexAuth();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al Confirmar");
            }
        }

        [HttpPost]
        public ActionResult Create(ScheduledExpenseVm model)
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
                ScheduledExpenseVm principal = (ScheduledExpenseVm)Session["SessionScheduledExpense"];
                model.Details = principal.Details;
                if (model.Details.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Concepto.");
                }
                var scheduledExpense = model.ToModelObject();

                if (scheduledExpense.PurchaseDate.Value.Date < DateTime.Now.Date)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de compra debe ser mayor o igual a la actual");

                if (scheduledExpense.UserAuthorizedId != scheduledExpense.UserId)
                    scheduledExpense.IsAuthorized = false;

                User userAuthorized = _userService.GetUser(scheduledExpense.UserAuthorizedId);

                if (model.Amount > userAuthorized.Limit)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El limite del Autorizador: " + userAuthorized.Limit + " es menor al monto del gasto programado");
                }

                if (model.MassCreation)
                {
                    int result = 0;
                    if (!int.TryParse(model.QuantityPeriod, out result))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar la cantidad de periodos en Creación Masiva.");
                    }
                    if (model.MonthlyPeriod)
                    {
                        _scheduledExpenseService.AddScheduledExpense(scheduledExpense);
                        for (int i = 1; i < int.Parse(model.QuantityPeriod); i++)
                        {
                            ScheduledExpense scheduled = scheduledExpense;
                            scheduled.PurchaseDate = scheduledExpense.PurchaseDate.Value.AddMonths(1);
                            _scheduledExpenseService.AddScheduledExpense(scheduled);
                        }
                    }
                    else
                    {
                        int periodo = 0;
                        if (!int.TryParse(model.PeriodInterval, out periodo))
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe seleccionar Periodo de días para creación masiva no mensual.");
                        }
                        _scheduledExpenseService.AddScheduledExpense(scheduledExpense);
                        for (int i = 1; i < int.Parse(model.QuantityPeriod); i++)
                        {
                            ScheduledExpense scheduled = scheduledExpense;
                            scheduled.PurchaseDate = scheduledExpense.PurchaseDate.Value.AddDays(periodo);
                            _scheduledExpenseService.AddScheduledExpense(scheduled);
                        }
                    }
                }
                else
                    _scheduledExpenseService.AddScheduledExpense(scheduledExpense);

                return Index();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        public ActionResult View(int id)
        {
            var entity = _scheduledExpenseService.GetScheduledExpensesById(id);
            var model = ScheduledExpenseVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUserAuthorized = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlUserAuthorized.SelectedItem = entity.UserAuthorizedId.ToString();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            return PartialView("~/Views/ScheduledExpense/ScheduledExpenseView.cshtml", model);
        }

        public ActionResult ViewManaged(int id)
        {
            var entity = _scheduledExpenseService.GetScheduledExpensesById(id);
            var model = ScheduledExpenseVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUserAuthorized = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlUserAuthorized.SelectedItem = entity.UserAuthorizedId.ToString();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            return PartialView("~/Views/ScheduledExpense/ScheduledExpenseViewManaged.cshtml", model);
        }

        public ActionResult AuthView(int id)
        {
            var entity = _scheduledExpenseService.GetScheduledExpensesById(id);
            var model = ScheduledExpenseVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUserAuthorized = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlUserAuthorized.SelectedItem = entity.UserAuthorizedId.ToString();

            return PartialView("~/Views/ScheduledExpense/ScheduledExpenseAuthView.cshtml", model);
        }

        public ActionResult Edit(int id)
        {
            var entity = _scheduledExpenseService.GetScheduledExpensesById(id);
            var model = ScheduledExpenseVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUserAuthorized = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlUserAuthorized.SelectedItem = entity.UserAuthorizedId.ToString();
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            Session["SessionScheduledExpense"] = model;

            return PartialView("~/Views/ScheduledExpense/ScheduledExpenseEdit.cshtml", model);
        }

        public ActionResult EditManaged(int id)
        {
            var entity = _scheduledExpenseService.GetScheduledExpensesById(id);
            var model = ScheduledExpenseVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUserAuthorized = collections.GetUsersSingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlUserAuthorized.SelectedItem = entity.UserAuthorizedId.ToString();
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            Session["SessionScheduledExpense"] = model;

            return PartialView("~/Views/ScheduledExpense/ScheduledExpenseEditManaged.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(ScheduledExpenseVm model)
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
                ScheduledExpenseVm principal = (ScheduledExpenseVm)Session["SessionScheduledExpense"];
                model.Details = principal.Details;
                if (model.Details.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Concepto.");
                }
                ScheduledExpense scheduled = model.ToModelObject();



                scheduled.CreateDate = Convert.ToDateTime(principal.CreateDate);
                scheduled.Processed = principal.Processed;
                scheduled.Managed = principal.Managed;
                scheduled.Active = principal.Active;
                if (scheduled.CreateDate > scheduled.PurchaseDate)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Compra no puede ser anterior a la fecha de Creación.");

                _scheduledExpenseService.UpdateScheduledExpense(scheduled);

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

        [HttpPost]
        public ActionResult EditManaged(ScheduledExpenseVm model)
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
                ScheduledExpenseVm principal = (ScheduledExpenseVm)Session["SessionScheduledExpense"];
                model.Details = principal.Details;
                if (model.Details.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Concepto.");
                }
                ScheduledExpense scheduled = model.ToModelObject();



                scheduled.CreateDate = Convert.ToDateTime(principal.CreateDate);

                if (scheduled.CreateDate > scheduled.PurchaseDate)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Compra no puede ser anterior a la fecha de Creación.");

                scheduled.Managed = true;
                scheduled.Processed = principal.Processed;
                scheduled.Active = principal.Active;
                _scheduledExpenseService.UpdateScheduledExpense(scheduled);

                return IndexManaged();
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

        public ActionResult Delete(int id)
        {
            try
            {
                _scheduledExpenseService.DeleteScheduledExpense(id);
                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

    }
}