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
    public class EarningsAllocationRepository : BaseRepository
    {
        public EarningsAllocation GetEarningsAllocationById(int earningsAllocationId)
        {
            var data = Db.ExecuteSprocAccessor("pub_EarningsAllocation_Get", EarningsAllocationMapper.Mapper, earningsAllocationId);

            return GetFirstElement(data);
        }

        public IEnumerable<EarningsAllocationSummary> GetEarningsAllocationsSummary(string SelectedUser, string DateFrom, string DateTo, string AmountFrom, string AmountTo)
        {
            var data = Db.ExecuteSprocAccessor("pub_EarningsAllocationSummary_GetList", EarningsAllocationSummaryMapper.Mapper, SelectedUser, DateFrom, DateTo, AmountFrom, AmountTo);

            return data.ToList();
        }

        public IEnumerable<EarningsAllocation> GetEarningsAllocations()
        {
            var data = Db.ExecuteSprocAccessor("pub_EarningsAllocation_GetList", EarningsAllocationMapper.Mapper);

            return data.ToList();
        }


        public EarningsAllocationPartner GetEarningsAllocationPartner(int earningsAllocationPartnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_EarningsAllocationPartner_Get", EarningsAllocationPartnerMapper.Mapper, earningsAllocationPartnerId);

            return GetFirstElement(data);
        }


        public IEnumerable<EarningsAllocationPartner> GetEarningsAllocationPartners(int earningsAllocationId)
        {
            var data = Db.ExecuteSprocAccessor("pub_EarningsAllocationPartner_GetList", EarningsAllocationPartnerMapper.Mapper, earningsAllocationId);

            return data.ToList();
        }

        public IEnumerable<EarningsAllocationPartner> GetEarningsAllocationPartnersByPartnerId(int partnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_EarningsAllocationPartnerByPartnerId_GetList", EarningsAllocationPartnerMapper.Mapper, partnerId);

            return data.ToList();
        }

        public void DeleteEarningsAllocation(int earningsAllocationId)
        {
            var command = Db.GetStoredProcCommand("pub_EarningsAllocation_Delete");
            Db.AddInParameter(command, "@EarningsAllocationId", DbType.Int32, earningsAllocationId);
            Db.ExecuteScalar(command);
        }

        public void DeleteEarningsAllocationPartner(int earningsAllocationPartnerId)
        {
            var command = Db.GetStoredProcCommand("pub_EarningsAllocationPartner_Delete");
            Db.AddInParameter(command, "@EarningsAllocationPartnerId", DbType.Int32, earningsAllocationPartnerId);
            Db.ExecuteScalar(command);
        }

        public void DeleteEarningsAllocationPartnerByEarningsAllocationId(int earningsAllocationId)
        {
            var command = Db.GetStoredProcCommand("pub_EarningsAllocationPartnerByEarningsAllocationId_Delete");
            Db.AddInParameter(command, "@earningsAllocationId", DbType.Int32, earningsAllocationId);
            Db.ExecuteScalar(command);
        }


        public int AddEarningsAllocation(EarningsAllocation EarningsAllocation)
        {
            var command = Db.GetStoredProcCommand("pub_EarningsAllocation_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, EarningsAllocation.Amount);
            Db.AddInParameter(command, "@UserId", DbType.Int32, EarningsAllocation.UserId);
            Db.AddInParameter(command, "@Concept", DbType.String, EarningsAllocation.Concept);
            Db.AddInParameter(command, "@Date", DbType.DateTime, EarningsAllocation.Date);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, EarningsAllocation.SystemDate);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }


        public void AddEarningsAllocationPartner(EarningsAllocationPartner EarningsAllocationPartner)
        {
            var command = Db.GetStoredProcCommand("pub_EarningsAllocationPartner_Insert");

            Db.AddInParameter(command, "@EarningsAllocationId", DbType.Int32, EarningsAllocationPartner.EarningsAllocationId);
            Db.AddInParameter(command, "@Balance", DbType.Double, EarningsAllocationPartner.Balance);
            Db.AddInParameter(command, "@Amount", DbType.Double, EarningsAllocationPartner.Amount);
            Db.AddInParameter(command, "@PartnerId", DbType.Int32, EarningsAllocationPartner.PartnerId);


            Db.ExecuteScalar(command);
        }

        public void UpdateEarningsAllocation(EarningsAllocation EarningsAllocation)
        {
            var command = Db.GetStoredProcCommand("pub_EarningsAllocation_Update");

            Db.AddInParameter(command, "@EarningsAllocationId", DbType.Int32, EarningsAllocation.EarningsAllocationId);
            Db.AddInParameter(command, "@Amount", DbType.Double, EarningsAllocation.Amount);
            Db.AddInParameter(command, "@UserId", DbType.Int32, EarningsAllocation.UserId);
            Db.AddInParameter(command, "@Concept", DbType.String, EarningsAllocation.Concept);
            Db.AddInParameter(command, "@Date", DbType.DateTime, EarningsAllocation.Date);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, EarningsAllocation.SystemDate);
            Db.ExecuteScalar(command);
        }


        public void UpdateEarningsAllocationPartner(EarningsAllocationPartner EarningsAllocationPartner)
        {
            var command = Db.GetStoredProcCommand("pub_EarningsAllocationPartner_Update");

            Db.AddInParameter(command, "@EarningsAllocationPartnerId", DbType.Int32, EarningsAllocationPartner.EarningsAllocationPartnerId);
            Db.AddInParameter(command, "@EarningsAllocationId", DbType.Int32, EarningsAllocationPartner.EarningsAllocationId);
            Db.AddInParameter(command, "@Balance", DbType.Double, EarningsAllocationPartner.Balance);
            Db.AddInParameter(command, "@Amount", DbType.Double, EarningsAllocationPartner.Amount);
            Db.AddInParameter(command, "@PartnerId", DbType.Int32, EarningsAllocationPartner.PartnerId);
            Db.ExecuteScalar(command);
        }

    }
}
