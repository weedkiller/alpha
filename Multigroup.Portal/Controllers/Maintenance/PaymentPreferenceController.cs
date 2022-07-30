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

namespace Multigroup.Portal.Controllers.PaymentPreference
{
    public class PaymentPreferenceController : Controller
    {
        private PaymentMethodsService _paymentMethodsService;
        private IAuditLogHelper _auditLogHelper;
        private MPSellerAccountService _mpSellerAccountService;

        public PaymentPreferenceController()
        {
            _paymentMethodsService = new PaymentMethodsService();
            _auditLogHelper = new AuditLogHelper();
            _mpSellerAccountService = new MPSellerAccountService();
        }

        // GET: /PaymentMethods/
        [LogActionFilter]
        [Description("PaymentPreference")]
        public ActionResult Index()
        {
            var model = PaymentPreferenceTableVm.ToViewModel(_paymentMethodsService.GetPaymentPreference());
            return PartialView("~/Views/Maintenance/PaymentPreference/PaymentPreference.cshtml", model);
        }

        [LogActionFilter]
        [Description("Crear PaymentPreference")]
        public ActionResult Create()
        {
            var model = new PaymentPreferenceVm();
            var collections = new CollectionHelper();
            List<PaymentPreferencePaymentMethods> lists = new List<PaymentPreferencePaymentMethods>();
            model.Details = lists;
            model.ddlPaymentMethod = collections.GetPaymentMethodSingleSelectVm();
            Session["SessionPaymentPreference"] = model;
            return PartialView("~/Views/Maintenance/PaymentPreference/PaymentPreferenceCreate.cshtml", model);
        }

        public ActionResult IndexPaymentMethod()
        {
            var model = (PaymentPreferenceVm)Session["SessionPaymentPreference"];
            ViewBag.ErrorMessageTrabajo = "false";
            return PartialView("~/Views/Maintenance/PaymentPreference/PaymentMethodTable.cshtml", model);
        }

        [HttpPost]
        public ActionResult Create(PaymentPreferenceVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }

                PaymentPreferenceVm principal = (PaymentPreferenceVm)Session["SessionPaymentPreference"];

                _paymentMethodsService.AddPaymentPreference(model.ToModelObject(), principal.Details);
                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
        [LogActionFilter]
        [Description("Editar PaymentPreference")]
        public ActionResult Edit(int id)
        {
            var entity = _paymentMethodsService.GetPaymentPreferenceById(id);
            var model = PaymentPreferenceVm.FromDomainModel(entity);
            model.Details = _paymentMethodsService.GetPaymentMethodsByPaymentPreference(id);
            var collections = new CollectionHelper();
            model.ddlPaymentMethod = collections.GetPaymentMethodSingleSelectVm();
            Session["SessionPaymentPreference"] = model;
            return PartialView("~/Views/Maintenance/PaymentPreference/PaymentPreferenceEdit.cshtml", model);
        }

        [HttpPost]
        public ActionResult Edit(PaymentPreferenceVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }
                PaymentPreferenceVm principal = (PaymentPreferenceVm)Session["SessionPaymentPreference"];
                _paymentMethodsService.UpdatePaymentPreference(model.ToModelObject(), principal.Details);

                return Index();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }

        public ActionResult PaymentMethod(int? PaymentMethodId)
        {
            bool existe = false;
            PaymentPreferenceVm principal = (PaymentPreferenceVm)Session["SessionPaymentPreference"];
            if (PaymentMethodId.HasValue)
            {
                PaymentPreferencePaymentMethods detail = new PaymentPreferencePaymentMethods();
                detail.PaymentMethodId = PaymentMethodId.Value;
                detail.PaymentPreference = _paymentMethodsService.GetPaymentMethods(PaymentMethodId.Value).Name;

                List<PaymentPreferencePaymentMethods> list = (List<PaymentPreferencePaymentMethods>)principal.Details;
                Int32 id = PaymentMethodId.Value;
                var item = list.FirstOrDefault(x => x.PaymentMethodId == id);
                if (item != null)
                    existe = true;
                else
                    list.Add(detail);
                principal.Details = list;


                Session["SessionPaymentPreference"] = principal;

                var collections = new CollectionHelper();
                if (existe == false)
                {
                    ViewBag.ErrorMessageTrabajo = "false";
                    return PartialView("~/Views/Maintenance/PaymentPreference/PaymentMethodTable.cshtml", principal);
                }
                else
                {
                    ViewBag.ErrorMessageTrabajo = "true";
                    return PartialView("~/Views/Maintenance/PaymentPreference/PaymentMethodTable.cshtml", principal);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Seleccione un articulo");
            }
        }

        public ActionResult RemovePaymentMethod(int Id)
        {
            PaymentPreferenceVm principal = (PaymentPreferenceVm)Session["SessionPaymentPreference"];
            List<PaymentPreferencePaymentMethods> list = (List<PaymentPreferencePaymentMethods>)principal.Details;
            var item = list.FirstOrDefault(x => x.PaymentMethodId == Id);
            list.Remove(item);

            principal.Details = list;

            Session["SessionPaymentPreference"] = principal;

            ViewBag.ErrorMessageTrabajo = "false";

            return PartialView("~/Views/Maintenance/PaymentPreference/PaymentMethodTable.cshtml", principal);
        }
    }
}