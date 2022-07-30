using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Maintenance
{
    public class ZoneVm
    {
        public IEnumerable<ZoneListVm> ZoneList { get; set; }

        public IEnumerable<CityListVm> CityList { get; set; }


        public static ZoneVm ToViewModel(IEnumerable<Zone> entities)
        {
            return new ZoneVm
            {
                ZoneList = ZoneListVm.ToViewModelList(entities),
            };
        }
    }

    public class CityListVm
    {
        public int CityId { get; set; }
        public string Name { get; set; }

        public static IEnumerable<CityListVm> ToViewModelList(IEnumerable<City> entities)
        {
            return entities.Select(x => new CityListVm
            {
                CityId = x.CityId,
                Name = x.Name,
            });
        }
    }
    public class ZoneListVm
    {
        public int ZoneId { get; set; }
        public string Name { get; set; }
        public double CommissionRate { get; set; }
        public int? PaymentDay1 { get; set; }
        public int? PaymentDay2 { get; set; }
        public bool IsPaymentDate { get; set; }

        public IEnumerable<CityListVm> CityList { get; set; }


        public static IEnumerable<ZoneListVm> ToViewModelList(IEnumerable<Zone> entities)
        {
            return entities.Select(x => new ZoneListVm
            {
                ZoneId = x.ZoneId,
                Name = x.Name,
            });
        }

        public ZoneListVm FromDomainModel(Zone entity)
        {
            return new ZoneListVm
            {
                Name = entity.Name,
                ZoneId = entity.ZoneId,
                CommissionRate = entity.CommissionRate,
                PaymentDay1 = entity.PaymentDay1,
                PaymentDay2 = entity.PaymentDay2,
                IsPaymentDate = (entity.PaymentDay1 == null && entity.PaymentDay2 == null) ? true : false
            };
        }

        public Zone ToModelObject()
        {
            var zone = new Zone()
            {
                Name = Name,
                ZoneId = ZoneId,
                CommissionRate = CommissionRate,
                PaymentDay1 = PaymentDay1,
                PaymentDay2 = PaymentDay2

            };

            return zone;
        }
    }


}