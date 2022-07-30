using Multigroup.Common.Logging;
using Multigroup.Portal.Filters;
using Multigroup.Portal.Models.Shared;
using Multigroup.Portal.Models.Widgets;
using Multigroup.Services.Shared;
using Resources;
using System;
using System.Net;
using System.Web.Mvc;

namespace Multigroup.Portal.Controllers.Shared
{
    public class WidgetController : Controller
    {
        private MPSellerAccountService _mpSellerAccountService;
        private IAuditLogHelper _auditLogHelper;

        public WidgetController()
        {
            _mpSellerAccountService = new MPSellerAccountService();
            _auditLogHelper = new AuditLogHelper();
        }

        [LogActionFilter]
        [Description("MP Widget")]
        public ActionResult MercadoPago()
        {
            var model = MPWidgetTableVm.ToViewModel(_mpSellerAccountService.GetWidget());
            return PartialView("~/Views/Widgets/MercadoPago.cshtml", model);
        }

        public ActionResult Mapa()
        {
            return PartialView("~/Views/Widgets/Mapa.cshtml");
        }
    }
}
