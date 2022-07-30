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
    public class PromissoryRepository : BaseRepository
    {
        public Promissory GetPromissoryById(int promissoryId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Promissory_Get", PromissoryMapper.Mapper, promissoryId);

            return GetFirstElement(data);
        }

        public Promissory GetPromissoryWithPartnerAmount(int promissoryId, int promissorySurrenderId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryWithPartnerAmount_Get", PromissoryMapper.Mapper, promissoryId, promissorySurrenderId);

            return GetFirstElement(data);
        }

        public IEnumerable<PromissorySummary> GetPromissorysSummary(string SelectedClient, string SelectedUser, string DateFromBroadcast, string DateToBroadcast, string DateFromCollection, string DateToCollection, string AmountFrom, string AmountTo, string Paid, string Number)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissorySummary_GetList", PromissorySummaryMapper.Mapper, SelectedClient, SelectedUser, DateFromBroadcast, DateToBroadcast, DateFromCollection, DateToCollection, AmountFrom, AmountTo, Paid, Number);

            return data.ToList();
        }

        public IEnumerable<Promissory> GetPromissorys()
        {
            var data = Db.ExecuteSprocAccessor("pub_Promissory_GetList", PromissoryMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<Promissory> GetPromissoriesByPartnerNotPaid(int partnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryByPartnerNotPaid_GetList", PromissoryMapper.Mapper, partnerId);

            return data.ToList();
        }


        public IEnumerable<PromissoryPartner> GetPromissoryPartners(int promissoryId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryPartner_GetList", PromissoryPartnerMapper.Mapper, promissoryId);

            return data.ToList();
        }



        public PromissoryPartner GetPromissoryPartner(int promissoryPartnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryPartner_Get", PromissoryPartnerMapper.Mapper, promissoryPartnerId);

            return GetFirstElement(data);
        }



        public void DeletePromissory(int promissoryId)
        {
            var command = Db.GetStoredProcCommand("pub_Promissory_Delete");
            Db.AddInParameter(command, "@PromissoryId", DbType.Int32, promissoryId);
            Db.ExecuteScalar(command);
        }

        public void DeletePromissoryPartner(int promissoryId)
        {
            var command = Db.GetStoredProcCommand("pub_PromissoryPartner_Delete");
            Db.AddInParameter(command, "@PromissoryId", DbType.Int32, promissoryId);
            Db.ExecuteScalar(command);
        }


        public int AddPromissory(Promissory Promissory)
        {
            var command = Db.GetStoredProcCommand("pub_Promissory_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, Promissory.Amount);
            Db.AddInParameter(command, "@Commentary", DbType.String, Promissory.Commentary);
            Db.AddInParameter(command, "@ClientId", DbType.Int32, Promissory.ClientId);
            Db.AddInParameter(command, "@BroadcastDate", DbType.DateTime, Promissory.BroadcastDate);
            Db.AddInParameter(command, "@UserId", DbType.Int32, Promissory.UserId);
            Db.AddInParameter(command, "@CollectionDate", DbType.DateTime, Promissory.CollectionDate);
            Db.AddInParameter(command, "@Description", DbType.String, Promissory.Description);
            Db.AddInParameter(command, "@isPaid", DbType.Boolean, Promissory.isPaid);
            Db.AddInParameter(command, "@Number", DbType.Int32, Promissory.Number);
            Db.AddInParameter(command, "@PercentageDefinitionId", DbType.Int32, Promissory.PercentageDefinitionId);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }


        public void AddPromissoryPartner(PromissoryPartner PromissoryPartner)
        {
            var command = Db.GetStoredProcCommand("pub_PromissoryPartner_Insert");

            Db.AddInParameter(command, "@PartnerId", DbType.Int32, PromissoryPartner.PartnerId);
            Db.AddInParameter(command, "@PromissoryId", DbType.Int32, PromissoryPartner.PromissoryId);
            Db.AddInParameter(command, "@Amount", DbType.Double, PromissoryPartner.Amount);

            Db.ExecuteScalar(command);
        }

        public void UpdatePromissory(Promissory Promissory)
        {
            var command = Db.GetStoredProcCommand("pub_Promissory_Update");

            Db.AddInParameter(command, "@PromissoryId", DbType.Int32, Promissory.PromissoryId);
            Db.AddInParameter(command, "@Amount", DbType.Double, Promissory.Amount);
            Db.AddInParameter(command, "@Commentary", DbType.String, Promissory.Commentary);
            Db.AddInParameter(command, "@ClientId", DbType.Int32, Promissory.ClientId);
            Db.AddInParameter(command, "@BroadcastDate", DbType.DateTime, Promissory.BroadcastDate);
            Db.AddInParameter(command, "@UserId", DbType.Int32, Promissory.UserId);
            Db.AddInParameter(command, "@CollectionDate", DbType.DateTime, Promissory.CollectionDate);
            Db.AddInParameter(command, "@Description", DbType.String, Promissory.Description);
            Db.AddInParameter(command, "@isPaid", DbType.Boolean, Promissory.isPaid);
            Db.AddInParameter(command, "@Number", DbType.Int32, Promissory.Number);
            Db.AddInParameter(command, "@PercentageDefinitionId", DbType.Int32, Promissory.PercentageDefinitionId);
            Db.ExecuteScalar(command);
        }

        public void UpdatePromissoryPartner(PromissoryPartner PromissoryPartner)
        {
            var command = Db.GetStoredProcCommand("pub_PromissoryDetail_Update");

            Db.AddInParameter(command, "@PromissoryPartnerId", DbType.Int32, PromissoryPartner.PromissoryPartnerId);
            Db.AddInParameter(command, "@PartnerId", DbType.Int32, PromissoryPartner.PartnerId);
            Db.AddInParameter(command, "@PromissoryId", DbType.Int32, PromissoryPartner.PromissoryId);
            Db.AddInParameter(command, "@Amount", DbType.Double, PromissoryPartner.Amount);
            Db.ExecuteScalar(command);
        }
    }
}
