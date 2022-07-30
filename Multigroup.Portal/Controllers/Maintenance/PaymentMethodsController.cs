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

namespace Multigroup.Portal.Controllers.PaymentMethods
{
    public class PaymentMethodsController : Controller
    {
        private PaymentMethodsService _paymentMethodsService;
        private IAuditLogHelper _auditLogHelper;
        private MPSellerAccountService _mpSellerAccountService;

        public PaymentMethodsController()
        {
            _paymentMethodsService = new PaymentMethodsService();
            _auditLogHelper = new AuditLogHelper();
            _mpSellerAccountService = new MPSellerAccountService();
        }

        // GET: /PaymentMethods/
        [LogActionFilter]
        [Description("PaymentMethodss")]
        public ActionResult Index()
        {
            var model = PaymentMethodsTableVm.ToViewModel(_paymentMethodsService.GetPaymentMethodsSummary());
            return PartialView("~/Views/Maintenance/PaymentMethods/PaymentMethods.cshtml", model);
        }
        public ActionResult GetTable()
        {
            var model = MercadoPagoTableVm.ToViewModel(_mpSellerAccountService.GetMPSellerAccounts());
            return PartialView("~/Views/Maintenance/PaymentMethods/MPTable.cshtml", model);
        }

        public ActionResult GenerateMPAccount(string MPAccountId, int? UserId, string Pass, double? MinAmount, double? MaxAmount, string Token)
        {
            try
            {
                if (MaxAmount == null || MinAmount == null || MPAccountId == null || UserId == null)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe completar todos los campos");
                else
                {
                    MPSellerAccount account = new MPSellerAccount();
                    account.MaxAmount = MaxAmount.Value;
                    account.MinAmount = MinAmount.Value;
                    account.MPAccountId = MPAccountId;
                    account.Pass = Pass;
                    account.Token = Token;
                    account.UserId = UserId.Value;
                    _mpSellerAccountService.AddMPSellerAccount(account);

                    return RedirectToAction("GetTable");
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al generar Cuenta.");
            }
        }

        [LogActionFilter]
        [Description("Crear PaymentMethodss")]
        public ActionResult Create()
        {
            var model = new PaymentMethodsVm();
            var collections = new CollectionHelper();
            model.ddlPaymentPreference = collections.GetPaymentPreferenceSingleSelectVm();
            return PartialView("~/Views/Maintenance/PaymentMethods/PaymentMethodsCreate.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(PaymentMethodsVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _paymentMethodsService.AddPaymentMethods(model.ToModelObject());
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
        [Description("Editar PaymentMethodss")]
        public ActionResult Edit(int id)
        {
            var entity = _paymentMethodsService.GetPaymentMethods(id);
            var model = PaymentMethodsVm.FromDomainModel(entity);
            var collections = new CollectionHelper();
            model.ddlPaymentPreference = collections.GetPaymentPreferenceSingleSelectVm();
            model.ddlPaymentPreference.SelectedItem = entity.PaymentPreferenceId.ToString();
            return PartialView("~/Views/Maintenance/PaymentMethods/PaymentMethodsEdit.cshtml", model);
        }
        public JsonResult EditMP(int id)
        {
            try
            {
                var entity = _mpSellerAccountService.GetMPSellerAccount(id);
                var model = MercadoPagoVm.FromDomainModel(entity);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "Error al recuperar cuenta de mercado pago.";
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public void DeleteMP(int id)
        {
            _mpSellerAccountService.DeleteMPSellerAccounts(id);
        }

        public ActionResult EditMercadoPago(int MPSellerAccountId, string MPAccountId, int? UserId, string Pass, double? MinAmount, double? MaxAmount, string Token)
        {
            try
            {
                if (MPSellerAccountId == null || MaxAmount == null || MinAmount == null || MPAccountId == null || UserId == null)
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe completar todos los campos");
                else
                {
                    MPSellerAccount account = new MPSellerAccount();
                    account.MPSellerAccountId = MPSellerAccountId;
                    account.MaxAmount = MaxAmount.Value;
                    account.MinAmount = MinAmount.Value;
                    account.MPAccountId = MPAccountId;
                    account.Pass = Pass;
                    account.Token = Token;
                    account.UserId = UserId.Value;
                    _mpSellerAccountService.UpdateMPSellerAccounts(account);

                    return RedirectToAction("GetTable");
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al generar Cuenta.");
            }
        }
        [HttpPost]
        public ActionResult Edit(PaymentMethodsVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                _paymentMethodsService.UpdatePaymentMethodss(model.ToModelObject());

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