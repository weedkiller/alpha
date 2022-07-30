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
    public class ZoneRepository : BaseRepository
    {
        public Zone GetZoneById(int zoneId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Zone_Get", ZoneMapper.Mapper, zoneId);

            return GetFirstElement(data);
        }
        public IEnumerable<Zone> GetZoneByCollectorId(int collector)
        {
            var data = Db.ExecuteSprocAccessor("pub_Zone_GetByUserId", ZoneMapper.Mapper, collector);

            return data.ToList();
        }
        
        public IEnumerable<Zone> GetZones()
        {
            var data = Db.ExecuteSprocAccessor("pub_Zone_GetList", GenericMapper<Zone>.Mapper);

            return data.ToList();
        }

        public IEnumerable<BaseEntity> GetPaymentZones(int ZoneId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ZonePaymentDayByZoneId_GetList", GenericMapper<BaseEntity>.Mapper, ZoneId);

            return data.ToList();
        }

        public void Delete(int zoneId)
        {
            var command = Db.GetStoredProcCommand("pub_Zone_Delete");
            Db.AddInParameter(command, "@ZoneId", DbType.Int32, zoneId);
            Db.ExecuteScalar(command);
        }

        public void AddZone(Zone Zone)
        {
            var command = Db.GetStoredProcCommand("pub_Zone_Insert");

            Db.AddInParameter(command, "@name", DbType.String, Zone.Name);
            Db.AddInParameter(command, "@ZoneId", DbType.Int32, Zone.ZoneId);


            Db.ExecuteScalar(command);
        }

        public void Update(Zone Zone)
        {
            var command = Db.GetStoredProcCommand("pub_Zone_Update");

            Db.AddInParameter(command, "@ZoneId", DbType.Int32, Zone.ZoneId);
            Db.AddInParameter(command, "@Name", DbType.String, Zone.Name);
            Db.AddInParameter(command, "@CommissionRate", DbType.Int32, Zone.CommissionRate);
            Db.AddInParameter(command, "@PaymentDay1", DbType.Int32, Zone.PaymentDay1);
            Db.AddInParameter(command, "@PaymentDay2", DbType.Int32, Zone.PaymentDay2);
            Db.ExecuteScalar(command);
        }


    }
}
