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
using Multigroup.DomainModel.Shared;

namespace Multigroup.Portal.Controllers.Maintenance
{
    public class ArticleController : Controller
    {
        private ArticleService _articleService;
        private IAuditLogHelper _auditLogHelper;

        public ArticleController()
        {
            _articleService = new ArticleService();
            _auditLogHelper = new AuditLogHelper();
        }

        // GET: /Article/
        [LogActionFilter]
        [Description("Articles")]
        public ActionResult Index()
        {
            ArticleFilterVm model = new ArticleFilterVm();
            var collections = new CollectionHelper();
            model.ListHeading = new SelectList(_articleService.GetHeadings(), "HeadingId", "Name");
            return PartialView("~/Views/Maintenance/Article/Article.cshtml", model);
        }
        public ActionResult GetTable(int? SelectedHeadings)
        {
            var model = ArticleTableVm.ToViewModel(_articleService.GetArticlesSummary(SelectedHeadings));
            return PartialView("~/Views/Maintenance/Article/ArticleTable.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Articles")]
        public ActionResult Create()
        {
            var model = new ArticleVm();
            var collections = new CollectionHelper();
            model.ddlHeading = collections.GetHeadingsSingleSelectVm();
            return PartialView("~/Views/Maintenance/Article/ArticleCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(ArticleVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _articleService.AddArticle(model.ToModelObject());
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
        [LogActionFilter]
        [Description("Editar Articles")]
        public ActionResult Edit(int id)
        {
            var entity = _articleService.GetArticlesById(id);
            var model = ArticleVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlHeading= collections.GetHeadingsSingleSelectVm();
            model.ddlHeading.SelectedItem = entity.HeadingId.ToString();
            return PartialView("~/Views/Maintenance/Article/ArticleEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(ArticleVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _articleService.UpdateArticle(model.ToModelObject());

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