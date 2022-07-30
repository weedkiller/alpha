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
    public class HeadingController : Controller
    {
        private ArticleService _articleService;
        private IAuditLogHelper _auditLogHelper;

        public HeadingController()
        {
            _articleService = new ArticleService();
            _auditLogHelper = new AuditLogHelper();
        }

        // GET: /Heading/
        [LogActionFilter]
        [Description("Headings")]
        public ActionResult Index()
        {
            var model = HeadingTableVm.ToViewModel(_articleService.GetHeadings());
            return PartialView("~/Views/Maintenance/Heading/Heading.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear Headings")]
        public ActionResult Create()
        {
            var model = new HeadingVm();
            var collections = new CollectionHelper();
            return PartialView("~/Views/Maintenance/Heading/HeadingCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(HeadingVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _articleService.AddHeading(model.ToModelObject());
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
        [Description("Editar Headings")]
        public ActionResult Edit(int id)
        {
            var entity = _articleService.GetHeading(id);
            var model = HeadingVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            return PartialView("~/Views/Maintenance/Heading/HeadingEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(HeadingVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _articleService.UpdateHeading(model.ToModelObject());

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