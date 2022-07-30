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
    public class ContributionAllocationRepository : BaseRepository
    {
        public ContributionAllocation GetContributionAllocationById(int contributionAllocationId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContributionAllocation_Get", ContributionAllocationMapper.Mapper, contributionAllocationId);

            return GetFirstElement(data);
        }

        public IEnumerable<ContributionAllocationSummary> GetContributionAllocationsSummary(string SelectedUser, string DateFrom, string DateTo, string AmountFrom, string AmountTo)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContributionAllocationSummary_GetList", ContributionAllocationSummaryMapper.Mapper, SelectedUser, DateFrom, DateTo, AmountFrom, AmountTo);

            return data.ToList();
        }

        public IEnumerable<ContributionAllocation> GetContributionAllocations()
        {
            var data = Db.ExecuteSprocAccessor("pub_ContributionAllocation_GetList", ContributionAllocationMapper.Mapper);

            return data.ToList();
        }


        public ContributionAllocationPartner GetContributionAllocationPartner(int contributionAllocationPartnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContributionAllocationPartner_Get", ContributionAllocationPartnerMapper.Mapper, contributionAllocationPartnerId);

            return GetFirstElement(data);
        }

        public IEnumerable<ContributionAllocationPartner> GetContributionAllocationPartnersByPartnerId(int partnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContributionAllocationPartnerByPartnerId_GetList", ContributionAllocationPartnerMapper.Mapper, partnerId);

            return data.ToList();
        }


        public IEnumerable<ContributionAllocationPartner> GetContributionAllocationPartners(int contributionAllocationId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContributionAllocationPartner_GetList", ContributionAllocationPartnerMapper.Mapper, contributionAllocationId);

            return data.ToList();
        }

        public void DeleteContributionAllocation(int contributionAllocationId)
        {
            var command = Db.GetStoredProcCommand("pub_ContributionAllocation_Delete");
            Db.AddInParameter(command, "@ContributionAllocationId", DbType.Int32, contributionAllocationId);
            Db.ExecuteScalar(command);
        }

        public void DeleteContributionAllocationPartner(int contributionAllocationPartnerId)
        {
            var command = Db.GetStoredProcCommand("pub_ContributionAllocationPartner_Delete");
            Db.AddInParameter(command, "@ContributionAllocationPartnerId", DbType.Int32, contributionAllocationPartnerId);
            Db.ExecuteScalar(command);
        }

        public void DeleteContributionAllocationPartnerByContributionAllocationId(int contributionAllocationId)
        {
            var command = Db.GetStoredProcCommand("pub_ContributionAllocationPartnerByContributionAllocationId_Delete");
            Db.AddInParameter(command, "@contributionAllocationId", DbType.Int32, contributionAllocationId);
            Db.ExecuteScalar(command);
        }


        public int AddContributionAllocation(ContributionAllocation ContributionAllocation)
        {
            var command = Db.GetStoredProcCommand("pub_ContributionAllocation_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, ContributionAllocation.Amount);
            Db.AddInParameter(command, "@UserId", DbType.Int32, ContributionAllocation.UserId);
            Db.AddInParameter(command, "@Concept", DbType.String, ContributionAllocation.Concept);
            Db.AddInParameter(command, "@Date", DbType.DateTime, ContributionAllocation.Date);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, ContributionAllocation.SystemDate);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }


        public void AddContributionAllocationPartner(ContributionAllocationPartner ContributionAllocationPartner)
        {
            var command = Db.GetStoredProcCommand("pub_ContributionAllocationPartner_Insert");

            Db.AddInParameter(command, "@ContributionAllocationId", DbType.Int32, ContributionAllocationPartner.ContributionAllocationId);
            Db.AddInParameter(command, "@Balance", DbType.Double, ContributionAllocationPartner.Balance);
            Db.AddInParameter(command, "@Amount", DbType.Double, ContributionAllocationPartner.Amount);
            Db.AddInParameter(command, "@PartnerId", DbType.Int32, ContributionAllocationPartner.PartnerId);


            Db.ExecuteScalar(command);
        }

        public void UpdateContributionAllocation(ContributionAllocation ContributionAllocation)
        {
            var command = Db.GetStoredProcCommand("pub_ContributionAllocation_Update");

            Db.AddInParameter(command, "@ContributionAllocationId", DbType.Int32, ContributionAllocation.ContributionAllocationId);
            Db.AddInParameter(command, "@Amount", DbType.Double, ContributionAllocation.Amount);
            Db.AddInParameter(command, "@UserId", DbType.Int32, ContributionAllocation.UserId);
            Db.AddInParameter(command, "@Concept", DbType.String, ContributionAllocation.Concept);
            Db.AddInParameter(command, "@Date", DbType.DateTime, ContributionAllocation.Date);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, ContributionAllocation.SystemDate);
            Db.ExecuteScalar(command);
        }


        public void UpdateContributionAllocationPartner(ContributionAllocationPartner ContributionAllocationPartner)
        {
            var command = Db.GetStoredProcCommand("pub_ContributionAllocationPartner_Update");

            Db.AddInParameter(command, "@ContributionAllocationPartnerId", DbType.Int32, ContributionAllocationPartner.ContributionAllocationPartnerId);
            Db.AddInParameter(command, "@ContributionAllocationId", DbType.Int32, ContributionAllocationPartner.ContributionAllocationId);
            Db.AddInParameter(command, "@Balance", DbType.Double, ContributionAllocationPartner.Balance);
            Db.AddInParameter(command, "@Amount", DbType.Double, ContributionAllocationPartner.Amount);
            Db.AddInParameter(command, "@PartnerId", DbType.Int32, ContributionAllocationPartner.PartnerId);
            Db.ExecuteScalar(command);
        }

    }
}
