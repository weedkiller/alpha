using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Multigroup.Services.Shared
{
    public class CityService
    {
        private CityRepository _cityRepository;

        public CityService()
        {
            _cityRepository = new CityRepository();
        }

        public City GetCity(int cityId)
        {
            return _cityRepository.GetCityById(cityId);
        }

        public State GetStateByName(string name)
        {
            return _cityRepository.GetStateByName(name);
        }

        public City GetcitysById(int cityId)
        {
            return _cityRepository.GetCityById(cityId);
        }

        public IEnumerable<City> GetCitys()
        {
            return _cityRepository.GetCitys();
        }

        public IEnumerable<State> GetStates()
        {
            return _cityRepository.GetStates();
        }

        public IEnumerable<City> GetCitysWithOutZone()
        {
            return _cityRepository.GetCitysWithOutZone();
        }

        public IEnumerable<City> GetCitiesByZone(int zoneId)
        {
            return _cityRepository.GetCitiesByZone(zoneId);
        }
        public void SaveZone(int cityId, int zoneId)
        {
            _cityRepository.SaveZone(cityId, zoneId);
        }

        public void Addcity(City city)
        {
            _cityRepository.AddCity(city);
        }

        public void DeleteCitys(int cityId)
        {
            _cityRepository.Delete(cityId);
        }

        public void UpdateCitys(City city)
        {
            _cityRepository.Update(city);
        }

        public void UpdateZones()
        {
            _cityRepository.UpdateZones();
        }
    }
}
