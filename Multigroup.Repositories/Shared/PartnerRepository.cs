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
    public class PartnerRepository : BaseRepository
    {
        public Partner GetPartnerById(int partnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Partner_Get", PartnerMapper.Mapper, partnerId);

            return GetFirstElement(data);
        }


        public IEnumerable<Partner> GetPartners()
        {
            var data = Db.ExecuteSprocAccessor("pub_Partner_GetList", PartnerMapper.Mapper);

            return data.ToList();
        }

        public PercentageDefinition GetPercentageDefinitionById(int percentageDefinitionId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PercentageDefinition_Get", PercentageDefinitionMapper.Mapper, percentageDefinitionId);

            return GetFirstElement(data);
        }


        public IEnumerable<PercentageDefinition> GetPercentageDefinitions()
        {
            var data = Db.ExecuteSprocAccessor("pub_PercentageDefinition_GetList", PercentageDefinitionMapper.Mapper);

            return data.ToList();
        }

        public PercentageDefinitionPartner GetPercentageDefinitionPartner(int percentageDefinitionPartnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PercentageDefinitionPartner_Get", PercentageDefinitionPartnerMapper.Mapper, percentageDefinitionPartnerId);

            return GetFirstElement(data);
        }


        public IEnumerable<PercentageDefinitionPartner> GetPercentageDefinitionPartners(int percentageDefinitionId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PercentageDefinitionPartner_GetList", PercentageDefinitionPartnerMapper.Mapper, percentageDefinitionId);

            return data.ToList();
        }


        public IEnumerable<BalancePartner> GetBalancePartner(string PartnerId, string asignation)
        {
            var data = Db.ExecuteSprocAccessor("pub_BalancePartner_GetList", BalancePartnerMapper.Mapper, PartnerId, asignation);

            return data.ToList();
        }


        public void Delete(int partnerId)
        {
            var command = Db.GetStoredProcCommand("pub_Partner_Delete");
            Db.AddInParameter(command, "@PartnerId", DbType.Int32, partnerId);
            Db.ExecuteScalar(command);
        }

        public void DeletePercentageDefinition(int percentageDefinitionId)
        {
            var command = Db.GetStoredProcCommand("pub_PercentageDefinition_Delete");
            Db.AddInParameter(command, "@PercentageDefinitionId", DbType.Int32, percentageDefinitionId);
            Db.ExecuteScalar(command);
        }

        public void DeletePercentageDefinitionPartners(int percentageDefinitionId)
        {
            var command = Db.GetStoredProcCommand("pub_PercentageDefinitionPartner_Delete");
            Db.AddInParameter(command, "@PercentageDefinitionId", DbType.Int32, percentageDefinitionId);
            Db.ExecuteScalar(command);
        }

        public void AddPartner(Partner partner)
        {
            var command = Db.GetStoredProcCommand("pub_Partner_Insert");

            Db.AddInParameter(command, "@Name", DbType.String, partner.Name);
            Db.AddInParameter(command, "@Active", DbType.Boolean, partner.Active);
            Db.AddInParameter(command, "@Percentage", DbType.Double, partner.Percentage);

            Db.ExecuteScalar(command);
        }

        public int AddPercentageDefinition(PercentageDefinition percentageDefinition)
        {
            var command = Db.GetStoredProcCommand("pub_PercentageDefinition_Insert");

            Db.AddInParameter(command, "@Name", DbType.String, percentageDefinition.Name);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void AddPercentageDefinitionPartner(PercentageDefinitionPartner percentageDefinitionPartner)
        {
            var command = Db.GetStoredProcCommand("pub_PercentageDefinitionPartner_Insert");

            Db.AddInParameter(command, "@Percentage", DbType.Double, percentageDefinitionPartner.Percentage);
            Db.AddInParameter(command, "@PartnerId", DbType.String, percentageDefinitionPartner.PartnerId);
            Db.AddInParameter(command, "@PercentageDefinitionId", DbType.String, percentageDefinitionPartner.PercentageDefinitionId);

            Db.ExecuteScalar(command);
        }

        public void Update(Partner partner)
        {
            var command = Db.GetStoredProcCommand("pub_Partner_Update");

            Db.AddInParameter(command, "@PartnerId", DbType.Int32, partner.PartnerId);
            Db.AddInParameter(command, "@Name", DbType.String, partner.Name);
            Db.AddInParameter(command, "@Active", DbType.Boolean, partner.Active);
            Db.AddInParameter(command, "@Percentage", DbType.Double, partner.Percentage);

            Db.ExecuteScalar(command);
        }

        public void UpdatePercentageDefinition(PercentageDefinition percentageDefinition)
        {
            var command = Db.GetStoredProcCommand("pub_PercentageDefinition_Update");

            Db.AddInParameter(command, "@PercentageDefinitionId", DbType.Int32, percentageDefinition.PercentageDefinitionId);
            Db.AddInParameter(command, "@Name", DbType.String, percentageDefinition.Name);

            Db.ExecuteScalar(command);
        }
    }
}
