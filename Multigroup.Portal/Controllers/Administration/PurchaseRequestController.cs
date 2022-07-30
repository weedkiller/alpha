using Multigroup.Common.Logging;
using Multigroup.Portal.Filters;
using Multigroup.Portal.Models.Administration;
using Multigroup.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multigroup.Portal.Helpers;
using Multigroup.DomainModel.Shared;
using Multigroup.Portal.Models.Maintenance;
using Multigroup.Portal.Utilities;
using Multigroup.Portal.Providers;

namespace Multigroup.Portal.Controllers.Administration
{
    public class PurchaseRequestController : Controller
    {
        private PurchaseRequestService _purchaseRequestService;
        private IAuditLogHelper _auditLogHelper;
        private UserTypeService _userTypeService;
        private UserService _userService;
        private ProviderService _providerService;
        private ArticleService _articleService;
        private ProfileProvider _profileProvider;


        public PurchaseRequestController()
        {
            _purchaseRequestService = new PurchaseRequestService();
            _auditLogHelper = new AuditLogHelper();
            _userTypeService = new UserTypeService();
            _userService = new UserService();
            _providerService = new ProviderService();
            _articleService = new ArticleService();
            _profileProvider = new ProfileProvider();
        }

        // GET: /PurchaseRequest/
        [LogActionFilter]
        [Description("PurchaseRequests")]
        public ActionResult Index()
        {
            PurchaseRequestFilterVm model = new PurchaseRequestFilterVm();
            var collections = new CollectionHelper();
            model.Active = true;
            model.ListProvider = new SelectList(_providerService.GetProvidersWithId(), "ProviderId", "Name");
            model.ListUser = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUserAuthorized = new SelectList(_userService.GetUsersComplete(), "UserId", "Names");
            model.ListUrgency = new SelectList(_purchaseRequestService.GetUrgencys(), "urgencyId", "Description");
            model.UserId = _profileProvider.CurrentUserId;
            return PartialView("~/Views/PurchaseRequest/PurchaseRequest.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedUrgency, int? SelectedProvider, int? SelectedUser, int? SelectedUserAuthorized, string DateFromCreate, string DateToCreate, string DateFromNeed, string DateToNeed, string AmountFrom, string AmountTo, string Active, string Processed)
        {
            var model = PurchaseRequestTableVm.ToViewModel(_purchaseRequestService.GetPurchaseRequestsSummary(SelectedUrgency, SelectedProvider, SelectedUser, SelectedUserAuthorized, DateFromCreate, DateToCreate, DateFromNeed, DateToNeed, AmountFrom, AmountTo, Active, Processed));
            return PartialView("~/Views/PurchaseRequest/PurchaseRequestTable.cshtml", model);
        }

        [LogActionFilter]
        [Description("PurchaseRequestsManaged")]
        public ActionResult IndexManaged()
        {
            var user = _profileProvider.CurrentUserId;
            var model = PurchaseRequestTableVm.ToViewModel(_purchaseRequestService.GetPurchaseRequestsSummaryManaged(user));
            return PartialView("~/Views/PurchaseRequest/PurchaseRequestManaged.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear PurchaseRequests")]
        public ActionResult Create()
        {
            var model = new PurchaseRequestVm();
            var collections = new CollectionHelper();
            List<PurchaseRequestDetail> lists = new List<PurchaseRequestDetail>();
            model.Details = lists;
            model.CreateDate = DateTime.Now.ToShortDateString();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUser.SelectedItem = _profileProvider.CurrentUserId.ToString();
            model.ddlUserAuthorized = collections.GetUsersAuthorizedSingleSelectVm(0);
            model.ddlUrgency = collections.GetUrgencySingleSelectVm();
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.ddlUrgency.SelectedItem = "2";
            Session["SessionPurchaseRequest"] = model;
            return PartialView("~/Views/PurchaseRequest/PurchaseRequestCreate.cshtml", model);
        }

        public ActionResult IndexArticles()
        {
            PurchaseRequestVm model = (PurchaseRequestVm)Session["SessionPurchaseRequest"];
            ViewBag.ErrorMessageTrabajo = "false";
            return PartialView("~/Views/PurchaseRequest/ArticlesTable.cshtml", model);
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
            PurchaseRequestVm principal = (PurchaseRequestVm)Session["SessionPurchaseRequest"];
            if (ArticleId.HasValue)
            {
                PurchaseRequestDetail detail = new PurchaseRequestDetail();
                detail.ArticleId = ArticleId.Value;
                detail.Quantity = QuantityArticle;
                detail.Price = PriceArticle;
                detail.Total = QuantityArticle * PriceArticle;
                detail.Description = DescriptionArticle;
                detail.Article = _articleService.GetArticlesById(ArticleId.Value).Name;

                List<PurchaseRequestDetail> list = (List<PurchaseRequestDetail>)principal.Details;
                Int32 id = ArticleId.Value;
                var item = list.FirstOrDefault(x => x.ArticleId == id);
                if (item != null)
                    existe = true;
                else
                    list.Add(detail);
                principal.Details = list;


                double total = 0;

                foreach (PurchaseRequestDetail det in list)
                {
                    total = total + det.Total;
                }
                ViewBag.Total = total.ToString();
                
                Session["Total"] = total.ToString();

                Session["SessionPurchaseRequest"] = principal;
                Session["esNuevo"] = false;

                var collections = new CollectionHelper();
                principal.ddlUserAuthorized = collections.GetUsersAuthorizedSingleSelectVm(total);
                if (existe == false)
                {
                    ViewBag.ErrorMessageTrabajo = "false";
                    return PartialView("~/Views/PurchaseRequest/ArticlesTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessageTrabajo = "true";
                    return PartialView("~/Views/PurchaseRequest/ArticlesTable.cshtml", principal);
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
            PurchaseRequestVm principal = (PurchaseRequestVm)Session["SessionPurchaseRequest"];
            if (ArticleId.HasValue)
            {
                PurchaseRequestDetail detail = new PurchaseRequestDetail();
                detail.ArticleId = ArticleId.Value;
                detail.Quantity = QuantityArticle;
                detail.Price = PriceArticle;
                detail.Total = QuantityArticle * PriceArticle;
                detail.Article = _articleService.GetArticlesById(ArticleId.Value).Name;
                detail.Description = DescriptionArticle;

                List<PurchaseRequestDetail> list = (List<PurchaseRequestDetail>)principal.Details;
                Int32 id = ArticleId.Value;
                var item = list.FirstOrDefault(x => x.ArticleId == id);
                list.Remove(item);
                list.Add(detail);
                principal.Details = list;


                double total = 0;

                foreach (PurchaseRequestDetail det in list)
                {
                    total = total + det.Total;
                }
                ViewBag.Total = total.ToString();

                Session["Total"] = total.ToString();

                Session["SessionPurchaseRequest"] = principal;
                Session["esNuevo"] = false;

                if (existe == false)
                {
                    ViewBag.ErrorMessageTrabajo = "false";
                    return PartialView("~/Views/PurchaseRequest/ArticlesTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessageTrabajo = "true";
                    return PartialView("~/Views/PurchaseRequest/ArticlesTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un articulo");
            }
        }

        public ActionResult RemoveArticles(int Id)
        {
            PurchaseRequestVm principal = (PurchaseRequestVm)Session["SessionPurchaseRequest"];
            List<PurchaseRequestDetail> list = (List<PurchaseRequestDetail>)principal.Details;
            var item = list.FirstOrDefault(x => x.ArticleId == Id);
            list.Remove(item);

            principal.Details = list;
            double total = 0;

            foreach (PurchaseRequestDetail det in list)
            {
                total = total + det.Total;
            }
            ViewBag.Total = total.ToString();

            Session["Total"] = total.ToString();

            Session["SessionPurchaseRequest"] = principal;

            Session["esNuevo"] = false;

            return PartialView("~/Views/PurchaseRequest/ArticlesTable.cshtml", principal);
        }


        [HttpPost]
        public ActionResult Create(PurchaseRequestVm model)
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
                PurchaseRequestVm principal = (PurchaseRequestVm)Session["SessionPurchaseRequest"];
                model.Details = principal.Details;
                if (model.Details.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Concepto.");
                }

                var purchase = model.ToModelObject();

                if (purchase.CreateDate > purchase.NeedDate)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Necesidad no puede ser anterior a la fecha de Creación.");

                User userAuthorized = _userService.GetUser(purchase.UserAuthorizedId);

                if (model.Amount > userAuthorized.Limit)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El limite del Autorizador / Comprador: " + userAuthorized.Limit + " es menor al monto de la solicitud");
                }

                _purchaseRequestService.AddPurchaseRequest(purchase);

                return Index();
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Se produjo un error al intentar guardar los datos.");
            }
        }

        public ActionResult View(int id)
        {
            var entity = _purchaseRequestService.GetPurchaseRequestsById(id);
            var model = PurchaseRequestVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUserAuthorized = collections.GetUsersSingleSelectVm();
            model.ddlUrgency = collections.GetUrgencySingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlUserAuthorized.SelectedItem = entity.UserAuthorizedId.ToString();
            model.ddlUrgency.SelectedItem = entity.UrgencyId.ToString();

            return PartialView("~/Views/PurchaseRequest/PurchaseRequestView.cshtml", model);
        }

        public ActionResult ViewManaged(int id)
        {
            var entity = _purchaseRequestService.GetPurchaseRequestsById(id);
            var model = PurchaseRequestVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUrgency = collections.GetUrgencySingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlUrgency.SelectedItem = entity.UrgencyId.ToString();

            return PartialView("~/Views/PurchaseRequest/PurchaseRequestViewManaged.cshtml", model);
        }

        public ActionResult Edit(int id)
        {
            var entity = _purchaseRequestService.GetPurchaseRequestsById(id);
            var model = PurchaseRequestVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUserAuthorized = collections.GetUsersSingleSelectVm();
            model.ddlUrgency = collections.GetUrgencySingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlUrgency.SelectedItem = entity.UrgencyId.ToString();
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.ddlUserAuthorized.SelectedItem = entity.UserAuthorizedId.ToString();
            Session["SessionPurchaseRequest"] = model;

            return PartialView("~/Views/PurchaseRequest/PurchaseRequestEdit.cshtml", model);
        }

        public ActionResult EditManaged(int id)
        {
            var entity = _purchaseRequestService.GetPurchaseRequestsById(id);
            var model = PurchaseRequestVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlProvider = collections.GetProviderIdsSingleSelectVm();
            model.ddlUser = collections.GetUsersSingleSelectVm();
            model.ddlUserAuthorized = collections.GetUsersSingleSelectVm();
            model.ddlUrgency = collections.GetUrgencySingleSelectVm();
            model.ddlProvider.SelectedItem = entity.ProveedorId.ToString();
            model.ddlUser.SelectedItem = entity.UserId.ToString();
            model.ddlUrgency.SelectedItem = entity.UrgencyId.ToString();
            model.ddlArticles = collections.GetArticleSingleSelectVm();
            model.ddlHeadings = collections.GetHeadingsSingleSelectVm();
            model.ddlUserAuthorized.SelectedItem = entity.UserAuthorizedId.ToString();
            Session["SessionPurchaseRequest"] = model;

            return PartialView("~/Views/PurchaseRequest/PurchaseRequestEditManaged.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(PurchaseRequestVm model)
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
                PurchaseRequestVm principal = (PurchaseRequestVm)Session["SessionPurchaseRequest"];
                model.Details = principal.Details;
                if (model.Details.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Concepto.");
                }              

                PurchaseRequest purchase = model.ToModelObject();

                User userAuthorized = _userService.GetUser(purchase.UserAuthorizedId);

                if (model.Amount > userAuthorized.Limit)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El limite del Autorizador / Comprador: " + userAuthorized.Limit + " es menor al monto de la solicitud");
                }

                purchase.CreateDate = Convert.ToDateTime(principal.CreateDate);
                purchase.Processed = principal.Processed;
                purchase.Managed = principal.Managed;

                if (purchase.CreateDate > purchase.NeedDate)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Necesidad no puede ser anterior a la fecha de Creación.");

                _purchaseRequestService.UpdatePurchaseRequest(purchase);

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
        public ActionResult EditManaged(PurchaseRequestVm model)
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
                PurchaseRequestVm principal = (PurchaseRequestVm)Session["SessionPurchaseRequest"];
                model.Details = principal.Details;
                if (model.Details.Count() == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe cargar al menos un Concepto.");
                }

                PurchaseRequest purchase = model.ToModelObject();

                User userAuthorized = _userService.GetUser(purchase.UserAuthorizedId);

                if (model.Amount > userAuthorized.Limit)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "El limite del Autorizador / Comprador: " + userAuthorized.Limit + " es menor al monto de la solicitud");
                }

                purchase.CreateDate = Convert.ToDateTime(principal.CreateDate);
                purchase.Processed = principal.Processed;
                if (purchase.CreateDate > purchase.NeedDate)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "La fecha de Necesidad no puede ser anterior a la fecha de Creación.");

                purchase.Managed = true;
                _purchaseRequestService.UpdatePurchaseRequest(purchase);

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

        public JsonResult GetUsersAuthorized(double limit)
        {
            try
            {
                var response = _userService.GetUsersAuthorized(limit);

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("Error al recuperar usuarios con límite", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                _purchaseRequestService.DeletePurchaseRequest(id);
                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

    }
}