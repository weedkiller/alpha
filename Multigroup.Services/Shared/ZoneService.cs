using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Services.Shared
{
    public class ZoneService
    {
        private ZoneRepository _zoneRepository;
        private CityRepository _cityRepository;


        public ZoneService()
        {
            _zoneRepository = new ZoneRepository();
            _cityRepository = new CityRepository();
        }

        public Zone GetZone(int zoneId)
        {
            return _zoneRepository.GetZoneById(zoneId);
        }
        
        public Zone GetZoneById(int zoneId)
        {
            return _zoneRepository.GetZoneById(zoneId);
        }
        public string GetZoneByCollectorId(int collector)
        {
            string zonas = "";
            foreach (Zone zona in _zoneRepository.GetZoneByCollectorId(collector))
            {
                if (zonas == "")
                    zonas += zona.ZoneId.ToString();
                else
                    zonas += "," + zona.ZoneId.ToString();
            }

            return (zonas == "") ? "0" : zonas;
        }
        public IEnumerable<Zone> GetZonesByCollectorId(int collector)
        {

            return _zoneRepository.GetZoneByCollectorId(collector);
        }
        public IEnumerable<Zone> GetZones()
        {
            return _zoneRepository.GetZones();
        }

        public IEnumerable<BaseEntity> GetPaymentZones(int ZoneId)
        {
            return _zoneRepository.GetPaymentZones(ZoneId);
        }

        public IEnumerable<Zone> GetZonesWithCities()
        {
            IEnumerable<Zone> Zones = _zoneRepository.GetZones();
            foreach (Zone zone in Zones)
            {
                zone.Cities = _cityRepository.GetCitiesByZone(zone.ZoneId);
            }
            return Zones;
        }

        public void Addzone(Zone zone)
        {
            _zoneRepository.AddZone(zone);
        }

        public void DeleteZones(int zoneId)
        {
            _zoneRepository.Delete(zoneId);
        }

        public void UpdateZones(Zone zone)
        {
            _zoneRepository.Update(zone);
        }
    }
}
