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
    public class AgencyRepository : BaseRepository
    {
        public Agency GetAgencyById(int agencyId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Agency_Get", AgencyMapper.Mapper, agencyId);

            return GetFirstElement(data);
        }


        public IEnumerable<Agency> GetAgencys()
        {
            var data = Db.ExecuteSprocAccessor("pub_Agency_GetList", AgencyMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<AgencyType> GetAgencyTypes()
        {
            var data = Db.ExecuteSprocAccessor("pub_AgencyType_GetList", AgencyTypeMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<Cartera> GetCartera()
        {
            var data = Db.ExecuteSprocAccessor("pub_Cartera_GetList", CarteraMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<Contract> getVistaContratos(string vista)
        {
            var data = Db.ExecuteSprocAccessor("pub_VistaContratos_GetList", ContractMapper.Mapper, vista);

            return data.ToList();
        }

        public void Delete(int agencyId)
        {
            var command = Db.GetStoredProcCommand("pub_Agency_Delete");
            Db.AddInParameter(command, "@AgencyId", DbType.Int32, agencyId);
            Db.ExecuteScalar(command);
        }

        public void AddAgency(Agency agency)
        {
            var command = Db.GetStoredProcCommand("pub_Agency_Insert");

            Db.AddInParameter(command, "@Description", DbType.String, agency.Description);
            Db.AddInParameter(command, "@Adress", DbType.String, agency.Adress);
            Db.AddInParameter(command, "@Phone", DbType.String, agency.Phone);
            Db.AddInParameter(command, "@ManagerId", DbType.Int32, agency.ManagerId);
            Db.AddInParameter(command, "@Type", DbType.Int32, agency.Type);



            Db.ExecuteScalar(command);
        }

        public void Update(Agency agency)
        {
            var command = Db.GetStoredProcCommand("pub_Agency_Update");

            Db.AddInParameter(command, "@AgencyId", DbType.Int32, agency.AgencyId);
            Db.AddInParameter(command, "@Description", DbType.String, agency.Description);
            Db.AddInParameter(command, "@Adress", DbType.String, agency.Adress);
            Db.AddInParameter(command, "@Phone", DbType.String, agency.Phone);
            Db.AddInParameter(command, "@ManagerId", DbType.Int32, agency.ManagerId);
            Db.AddInParameter(command, "@Type", DbType.Int32, agency.Type); 
            
            Db.ExecuteScalar(command);
        }
    }
}
