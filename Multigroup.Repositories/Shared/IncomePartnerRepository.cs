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
    public class IncomePartnerRepository : BaseRepository
    {
        public IncomePartner GetIncomePartnerById(int incomePartnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_IncomePartner_Get", IncomePartnerMapper.Mapper, incomePartnerId);

            return GetFirstElement(data);
        }

        public IEnumerable<IncomePartnerSummary> GetIncomePartnersSummary(string SelectedPartner, string SelectedUser, string DateFromSystem, string DateToSystem, string DateFrom, string DateTo, string AmountFrom, string AmountTo)
        {
            var data = Db.ExecuteSprocAccessor("pub_IncomePartnerSummary_GetList", IncomePartnerSummaryMapper.Mapper, SelectedPartner, SelectedUser, DateFromSystem, DateToSystem, DateFrom, DateTo, AmountFrom, AmountTo);

            return data.ToList();
        }

        public IEnumerable<IncomePartner> GetIncomePartners()
        {
            var data = Db.ExecuteSprocAccessor("pub_IncomePartner_GetList", IncomePartnerMapper.Mapper);

            return data.ToList();
        }


        public IncomePartnerPaymentMethod GetIncomePartnerPaymentMethod(int incomePartnerPaymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_IncomePartnerPaymentMethod_Get", IncomePartnerPaymentMethodMapper.Mapper, incomePartnerPaymentMethodId);

            return GetFirstElement(data);
        }

        public IEnumerable<IncomePartnerPaymentMethod> GetIncomePartnerPaymentMethodByRet(int incomePartnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_IncomePartnerPaymentMethodByRet_GetList", IncomePartnerPaymentMethodMapper.Mapper, incomePartnerId);

            return data.ToList();
        }

        public IEnumerable<IncomePartnerPaymentMethod> GetIncomePartnerPaymentMethodsByCashCycle(int cashCycleId, int cashierId, int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_IncomePartnerPaymentMethodByCashCycle_GetList", IncomePartnerPaymentMethodMapper.Mapper, cashCycleId, cashierId, paymentMethodId);

            return data.ToList();
        }

        public IEnumerable<IncomePartnerPaymentMethod> GetIncomePartnerPaymentMethodsByBalance(int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_IncomePartnerPaymentMethodBalance_GetList", IncomePartnerPaymentMethodMapper.Mapper, paymentMethodId);

            return data.ToList();
        }


        public IEnumerable<IncomePartnerPaymentMethod> GetIncomePartnerPaymentMethods()
        {
            var data = Db.ExecuteSprocAccessor("pub_IncomePartnerPaymentMethod_GetList", IncomePartnerPaymentMethodMapper.Mapper);

            return data.ToList();
        }

        public IncomePartnerAllocation GetIncomePartnerAllocation(int incomePartnerAllocation)
        {
            var data = Db.ExecuteSprocAccessor("pub_IncomePartnerContributionAllocation_Get", IncomePartnerAllocationMapper.Mapper, incomePartnerAllocation);

            return GetFirstElement(data);
        }

        public IEnumerable<IncomePartnerAllocation> GetIncomePartnerAllocationByRet(int incomePartnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_IncomePartnerAllocationByRet_GetList", IncomePartnerAllocationMapper.Mapper, incomePartnerId);

            return data.ToList();
        }


        public IEnumerable<IncomePartnerAllocation> GetIncomePartnerAllocations()
        {
            var data = Db.ExecuteSprocAccessor("pub_IncomePartnerAllocation_GetList", IncomePartnerAllocationMapper.Mapper);

            return data.ToList();
        }



        public void DeleteIncomePartner(int incomePartnerId)
        {
            var command = Db.GetStoredProcCommand("pub_IncomePartner_Delete");
            Db.AddInParameter(command, "@IncomePartnerId", DbType.Int32, incomePartnerId);
            Db.ExecuteScalar(command);
        }

        public void DeleteIncomePartnerPaymentMethod(int incomePartnerId)
        {
            var command = Db.GetStoredProcCommand("pub_IncomePartnerPaymentMethod_Delete");
            Db.AddInParameter(command, "@IncomePartnerId", DbType.Int32, incomePartnerId);
            Db.ExecuteScalar(command);
        }

        public void DeleteIncomePartnerAllocation(int incomePartnerId)
        {
            var command = Db.GetStoredProcCommand("pub_IncomePartnerAllocation_Delete");
            Db.AddInParameter(command, "@incomePartnerId", DbType.Int32, incomePartnerId);
            Db.ExecuteScalar(command);
        }

        public int AddIncomePartner(IncomePartner IncomePartner)
        {
            var command = Db.GetStoredProcCommand("pub_IncomePartner_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, IncomePartner.Amount);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, IncomePartner.SystemDate);
            Db.AddInParameter(command, "@PartnerId", DbType.Int32, IncomePartner.PartnerId);
            Db.AddInParameter(command, "@Date", DbType.DateTime, IncomePartner.Date);
            Db.AddInParameter(command, "@UserId", DbType.Int32, IncomePartner.UserId);
            Db.AddInParameter(command, "@Commentary", DbType.String, IncomePartner.Commentary);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdateIncomePartner(IncomePartner IncomePartner)
        {
            var command = Db.GetStoredProcCommand("pub_IncomePartner_Update");

            Db.AddInParameter(command, "@IncomePartnerId", DbType.Int32, IncomePartner.IncomePartnerId);
            Db.AddInParameter(command, "@Amount", DbType.Double, IncomePartner.Amount);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, IncomePartner.SystemDate);
            Db.AddInParameter(command, "@PartnerId", DbType.Int32, IncomePartner.PartnerId);
            Db.AddInParameter(command, "@Date", DbType.DateTime, IncomePartner.Date);
            Db.AddInParameter(command, "@UserId", DbType.Int32, IncomePartner.UserId);
            Db.AddInParameter(command, "@Commentary", DbType.String, IncomePartner.Commentary);
            Db.ExecuteScalar(command);
        }


        public void AddIncomePartnerPaymentMethod(IncomePartnerPaymentMethod IncomePartnerPaymentMethod)
        {
            var command = Db.GetStoredProcCommand("pub_IncomePartnerPaymentMethod_Insert");

            Db.AddInParameter(command, "@IncomePartnerId", DbType.Int32, IncomePartnerPaymentMethod.IncomePartnerId);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, IncomePartnerPaymentMethod.PaymentMethodId);
            Db.AddInParameter(command, "@Amount", DbType.Int32, IncomePartnerPaymentMethod.Amount);
            Db.AddInParameter(command, "@Saving", DbType.Int32, IncomePartnerPaymentMethod.Saving);
            Db.AddInParameter(command, "@Cycle", DbType.String, IncomePartnerPaymentMethod.Cycle);
            Db.ExecuteScalar(command);
        }


        public void UpdateIncomePartnerPaymentMethod(IncomePartnerPaymentMethod IncomePartnerPaymentMethod)
        {
            var command = Db.GetStoredProcCommand("pub_IncomePartnerPaymentMethod_Update");

            Db.AddInParameter(command, "@IncomePartnerPaymentMethodId", DbType.Int32, IncomePartnerPaymentMethod.IncomePartnerPaymentMethodId);
            Db.AddInParameter(command, "@IncomePartnerId", DbType.Int32, IncomePartnerPaymentMethod.IncomePartnerId);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, IncomePartnerPaymentMethod.PaymentMethodId);
            Db.AddInParameter(command, "@Amount", DbType.Int32, IncomePartnerPaymentMethod.Amount);
            Db.AddInParameter(command, "@Saving", DbType.Int32, IncomePartnerPaymentMethod.Saving);
            Db.AddInParameter(command, "@Cycle", DbType.String, IncomePartnerPaymentMethod.Cycle);
            Db.ExecuteScalar(command);
        }

        public void AddIncomePartnerAllocation(IncomePartnerAllocation IncomePartnerAllocation)
        {
            var command = Db.GetStoredProcCommand("pub_IncomePartnerAllocation_Insert");

            Db.AddInParameter(command, "@IncomePartnerId", DbType.Int32, IncomePartnerAllocation.IncomePartnerId);
            Db.AddInParameter(command, "@ContributionAllocationPartnerId", DbType.Int32, IncomePartnerAllocation.ContributionAllocationPartnerId);
            Db.AddInParameter(command, "@Amount", DbType.Int32, IncomePartnerAllocation.Amount);

            Db.ExecuteScalar(command);
        }

        public void UpdateIncomePartnerPromissory(IncomePartnerAllocation IncomePartnerAllocation)
        {
            var command = Db.GetStoredProcCommand("pub_IncomePartnerAllocation_Update");

            Db.AddInParameter(command, "@IncomePartnerAllocationId", DbType.Int32, IncomePartnerAllocation.IncomePartnerAllocationId);
            Db.AddInParameter(command, "@IncomePartnerId", DbType.Int32, IncomePartnerAllocation.IncomePartnerId);
            Db.AddInParameter(command, "@ContributionAllocationId", DbType.Int32, IncomePartnerAllocation.ContributionAllocationPartnerId);
            Db.AddInParameter(command, "@Amount", DbType.Int32, IncomePartnerAllocation.Amount);
            Db.ExecuteScalar(command);
        }
    }
}
