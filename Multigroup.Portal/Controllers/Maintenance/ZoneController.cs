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
    public class ZoneController : Controller
    {
        private ZoneService _zoneService;
        private CityService _cityService;
        private IAuditLogHelper _auditLogHelper;

        public ZoneController()
        {
            _zoneService = new ZoneService();
            _cityService = new CityService();
            _auditLogHelper = new AuditLogHelper();
        }
        //
        // GET: /Zone/
        public ActionResult Index()
        {
            var model = new ZoneVm();
            model.CityList = CityListVm.ToViewModelList(_cityService.GetCitysWithOutZone());
            var zonas = ZoneListVm.ToViewModelList(_zoneService.GetZonesWithCities());
            var list = new List<ZoneListVm>();
            foreach(ZoneListVm zona in zonas)
            {
                zona.CityList = CityListVm.ToViewModelList(_cityService.GetCitiesByZone(zona.ZoneId));
                list.Add(zona);
            }
            model.ZoneList = list;
            return PartialView("~/Views/Maintenance/Zone/Zone.cshtml", model);
        }

        [HttpPost]
        public ActionResult Save(ZoneVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }
                _cityService.UpdateZones();
                foreach (ZoneListVm zone in model.ZoneList)
                {
                    if (zone.CityList != null)
                    {
                        foreach (CityListVm city in zone.CityList)
                        {
                            _cityService.SaveZone(city.CityId, zone.ZoneId);
                        }
                    }

                }
                return Index();
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = model,
                    IntegrationType = AuditLogType.CustomerService
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
        public ActionResult Edit(int id)
        {
            Zone zone = _zoneService.GetZoneById(id);
            ZoneListVm model = new ZoneListVm();
            var zona = model.FromDomainModel(zone);
            return PartialView("~/Views/Maintenance/Zone/ZoneEdit.cshtml", zona);
        }
        
        public JsonResult GetPaymentDatesByZone(int id)
        {
            try
            {
                var response = _zoneService.GetPaymentZones(id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.StatusDescription = "Error al recuperar fechas.";
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Edit(ZoneListVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }
                if (model.ZoneId < 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Faltan campos requeridos");
                }
                if (!model.IsPaymentDate && ((string.IsNullOrEmpty(model.PaymentDay1.ToString()) || string.IsNullOrEmpty(model.PaymentDay1.ToString())) || (model.PaymentDay1 < 1 || model.PaymentDay1 > 31 || model.PaymentDay1 < 2 || model.PaymentDay2 > 31)))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Debe ingresar fechas de pago válidas");
                }


                _zoneService.UpdateZones(model.ToModelObject());

                return Index();
            }
            catch (Exception ex)
            {
                var log = new AuditLog()
                {
                    LogLevel = AuditLogLevel.Error,
                    Description = ex.Message,
                    InputParam = model,
                    IntegrationType = AuditLogType.CustomerService
                };
                _auditLogHelper.LogAuditFail(log);

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar");
            }
        }
    }
}