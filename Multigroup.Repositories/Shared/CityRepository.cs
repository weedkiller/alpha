using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared.Mappers;
using Multigroup.Repositories.Shared.Resources;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Multigroup.Repositories.Shared
{
    public class CityRepository : BaseRepository
    {
        public City GetCityById(int cityId)
        {
            var data = Db.ExecuteSprocAccessor("pub_City_Get", CityMapper.Mapper, cityId);

            return GetFirstElement(data);
        }

        public State GetStateByName(string name)
        {
            var data = Db.ExecuteSprocAccessor("pub_StateByName_Get", GenericMapper<State>.Mapper, name);

            return GetFirstElement(data);
        }

        public State GetState(int stateId)
        {
            var data = Db.ExecuteSprocAccessor("pub_State_Get", GenericMapper<State>.Mapper, stateId);

            return GetFirstElement(data);
        }


        public IEnumerable<City> GetCitys()
        {
            var data = Db.ExecuteSprocAccessor("pub_City_GetList", CityMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<State> GetStates()
        {
            var data = Db.ExecuteSprocAccessor("pub_State_GetList", GenericMapper<State>.Mapper);

            return data.ToList();
        }

        public IEnumerable<City> GetCitysWithOutZone()
        {
            var data = Db.ExecuteSprocAccessor("pub_City_GetCitysWithOutZone", CityMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<City> GetCitiesByZone(int zoneId)
        {
            var data = Db.ExecuteSprocAccessor("pub_City_GetCitiesByZone", CityMapper.Mapper, zoneId);

            return data.ToList();
        }

        public void Delete(int cityId)
        {
            var command = Db.GetStoredProcCommand("pub_City_Delete");
            Db.AddInParameter(command, "@CityId", DbType.Int32, cityId);
            Db.ExecuteScalar(command);
        }

        public void AddCity(City City)
        {
            var command = Db.GetStoredProcCommand("pub_City_Insert");

            Db.AddInParameter(command, "@name", DbType.String, City.Name);
            Db.AddInParameter(command, "@ZoneId", DbType.Int32, City.ZoneId);


            Db.ExecuteScalar(command);
        }

        public void Update(City City)
        {
            var command = Db.GetStoredProcCommand("pub_City_Update");

            Db.AddInParameter(command, "@CityId", DbType.Int32, City.CityId);
            Db.AddInParameter(command, "@name", DbType.String, City.Name);
            Db.AddInParameter(command, "@ZoneId", DbType.Int32, City.ZoneId);
            Db.ExecuteScalar(command);
        }
        public void UpdateZones()
        {
            var command = Db.GetStoredProcCommand("pub_City_UpdateZones");
            Db.ExecuteScalar(command);
        }

        public void SaveZone(int cityId, int zoneId)
        {
            var command = Db.GetStoredProcCommand("pub_City_UpdateZonesCity");

            Db.AddInParameter(command, "@cityId", DbType.Int32, cityId);
            Db.AddInParameter(command, "@zoneId", DbType.Int32, zoneId);
            Db.ExecuteScalar(command);
        }
    }
}
