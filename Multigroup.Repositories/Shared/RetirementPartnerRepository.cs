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
    public class RetirementPartnerRepository : BaseRepository
    {
        public RetirementPartner GetRetirementPartnerById(int retirementPartnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_RetirementPartner_Get", RetirementPartnerMapper.Mapper, retirementPartnerId);

            return GetFirstElement(data);
        }

        public IEnumerable<RetirementPartnerSummary> GetRetirementPartnersSummary(string SelectedPartner, string SelectedUser, string DateFromSystem, string DateToSystem, string DateFrom, string DateTo, string AmountFrom, string AmountTo)
        {
            var data = Db.ExecuteSprocAccessor("pub_RetirementPartnerSummary_GetList", RetirementPartnerSummaryMapper.Mapper, SelectedPartner, SelectedUser, DateFromSystem, DateToSystem, DateFrom, DateTo, AmountFrom, AmountTo);

            return data.ToList();
        }

        public IEnumerable<RetirementPartner> GetRetirementPartners()
        {
            var data = Db.ExecuteSprocAccessor("pub_RetirementPartner_GetList", RetirementPartnerMapper.Mapper);

            return data.ToList();
        }


        public RetirementPartnerPaymentMethod GetRetirementPartnerPaymentMethod(int retirementPartnerPaymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_RetirementPartnerPaymentMethod_Get", RetirementPartnerPaymentMethodMapper.Mapper, retirementPartnerPaymentMethodId);

            return GetFirstElement(data);
        }

        public IEnumerable<RetirementPartnerPaymentMethod> GetRetirementPartnerPaymentMethodByRet(int retirementPartnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_RetirementPartnerPaymentMethodByRet_GetList", RetirementPartnerPaymentMethodMapper.Mapper, retirementPartnerId);

            return data.ToList();
        }

        public IEnumerable<RetirementPartnerPaymentMethod> GetRetirementPartnerPaymentMethodsByCashCycle(int cashCycleId, int cashierId, int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_RetirementPartnerPaymentMethodByCashCycle_GetList", RetirementPartnerPaymentMethodMapper.Mapper, cashCycleId, cashierId, paymentMethodId);

            return data.ToList();
        }

        public IEnumerable<RetirementPartnerPaymentMethod> GetRetirementPartnerPaymentMethodsBalance(int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_RetirementPartnerPaymentMethodBalance_GetList", RetirementPartnerPaymentMethodMapper.Mapper, paymentMethodId);

            return data.ToList();
        }


        public IEnumerable<RetirementPartnerPaymentMethod> GetRetirementPartnerPaymentMethods()
        {
            var data = Db.ExecuteSprocAccessor("pub_RetirementPartnerPaymentMethod_GetList", RetirementPartnerPaymentMethodMapper.Mapper);

            return data.ToList();
        }

        public RetirementPartnerAllocation GetRetirementPartnerAllocation(int retirementPartnerAllocation)
        {
            var data = Db.ExecuteSprocAccessor("pub_RetirementPartnerEarningsAllocation_Get", RetirementPartnerAllocationMapper.Mapper, retirementPartnerAllocation);

            return GetFirstElement(data);
        }

        public IEnumerable<RetirementPartnerAllocation> GetRetirementPartnerAllocationByRet(int retirementPartnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_RetirementPartnerAllocationByRet_GetList", RetirementPartnerAllocationMapper.Mapper, retirementPartnerId);

            return data.ToList();
        }


        public IEnumerable<RetirementPartnerAllocation> GetRetirementPartnerAllocations()
        {
            var data = Db.ExecuteSprocAccessor("pub_RetirementPartnerAllocation_GetList", RetirementPartnerAllocationMapper.Mapper);

            return data.ToList();
        }



        public void DeleteRetirementPartner(int retirementPartnerId)
        {
            var command = Db.GetStoredProcCommand("pub_RetirementPartner_Delete");
            Db.AddInParameter(command, "@RetirementPartnerId", DbType.Int32, retirementPartnerId);
            Db.ExecuteScalar(command);
        }

        public void DeleteRetirementPartnerPaymentMethod(int retirementPartnerId)
        {
            var command = Db.GetStoredProcCommand("pub_RetirementPartnerPaymentMethod_Delete");
            Db.AddInParameter(command, "@RetirementPartnerId", DbType.Int32, retirementPartnerId);
            Db.ExecuteScalar(command);
        }

        public void DeleteRetirementPartnerAllocation(int retirementPartnerId)
        {
            var command = Db.GetStoredProcCommand("pub_RetirementPartnerAllocation_Delete");
            Db.AddInParameter(command, "@retirementPartnerId", DbType.Int32, retirementPartnerId);
            Db.ExecuteScalar(command);
        }

        public int AddRetirementPartner(RetirementPartner RetirementPartner)
        {
            var command = Db.GetStoredProcCommand("pub_RetirementPartner_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, RetirementPartner.Amount);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, RetirementPartner.SystemDate);
            Db.AddInParameter(command, "@PartnerId", DbType.Int32, RetirementPartner.PartnerId);
            Db.AddInParameter(command, "@Date", DbType.DateTime, RetirementPartner.Date);
            Db.AddInParameter(command, "@UserId", DbType.Int32, RetirementPartner.UserId);
            Db.AddInParameter(command, "@Commentary", DbType.String, RetirementPartner.Commentary);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdateRetirementPartner(RetirementPartner RetirementPartner)
        {
            var command = Db.GetStoredProcCommand("pub_RetirementPartner_Update");

            Db.AddInParameter(command, "@RetirementPartnerId", DbType.Int32, RetirementPartner.RetirementPartnerId);
            Db.AddInParameter(command, "@Amount", DbType.Double, RetirementPartner.Amount);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, RetirementPartner.SystemDate);
            Db.AddInParameter(command, "@PartnerId", DbType.Int32, RetirementPartner.PartnerId);
            Db.AddInParameter(command, "@Date", DbType.DateTime, RetirementPartner.Date);
            Db.AddInParameter(command, "@UserId", DbType.Int32, RetirementPartner.UserId);
            Db.AddInParameter(command, "@Commentary", DbType.String, RetirementPartner.Commentary);
            Db.ExecuteScalar(command);
        }


        public void AddRetirementPartnerPaymentMethod(RetirementPartnerPaymentMethod RetirementPartnerPaymentMethod)
        {
            var command = Db.GetStoredProcCommand("pub_RetirementPartnerPaymentMethod_Insert");

            Db.AddInParameter(command, "@RetirementPartnerId", DbType.Int32, RetirementPartnerPaymentMethod.RetirementPartnerId);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, RetirementPartnerPaymentMethod.PaymentMethodId);
            Db.AddInParameter(command, "@Amount", DbType.Int32, RetirementPartnerPaymentMethod.Amount);
            Db.AddInParameter(command, "@Saving", DbType.Int32, RetirementPartnerPaymentMethod.Saving);
            Db.AddInParameter(command, "@Cycle", DbType.String, RetirementPartnerPaymentMethod.Cycle);
            Db.ExecuteScalar(command);
        }


        public void UpdateRetirementPartnerPaymentMethod(RetirementPartnerPaymentMethod RetirementPartnerPaymentMethod)
        {
            var command = Db.GetStoredProcCommand("pub_RetirementPartnerPaymentMethod_Update");

            Db.AddInParameter(command, "@RetirementPartnerPaymentMethodId", DbType.Int32, RetirementPartnerPaymentMethod.RetirementPartnerPaymentMethodId);
            Db.AddInParameter(command, "@RetirementPartnerId", DbType.Int32, RetirementPartnerPaymentMethod.RetirementPartnerId);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, RetirementPartnerPaymentMethod.PaymentMethodId);
            Db.AddInParameter(command, "@Amount", DbType.Int32, RetirementPartnerPaymentMethod.Amount);
            Db.AddInParameter(command, "@Saving", DbType.Int32, RetirementPartnerPaymentMethod.Saving);
            Db.AddInParameter(command, "@Cycle", DbType.String, RetirementPartnerPaymentMethod.Cycle);
            Db.ExecuteScalar(command);
        }

        public void AddRetirementPartnerAllocation(RetirementPartnerAllocation RetirementPartnerAllocation)
        {
            var command = Db.GetStoredProcCommand("pub_RetirementPartnerAllocation_Insert");

            Db.AddInParameter(command, "@RetirementPartnerId", DbType.Int32, RetirementPartnerAllocation.RetirementPartnerId);
            Db.AddInParameter(command, "@EarningsAllocationPartnerId", DbType.Int32, RetirementPartnerAllocation.EarningsAllocationPartnerId);
            Db.AddInParameter(command, "@Amount", DbType.Int32, RetirementPartnerAllocation.Amount);

            Db.ExecuteScalar(command);
        }

        public void UpdateRetirementPartnerPromissory(RetirementPartnerAllocation RetirementPartnerAllocation)
        {
            var command = Db.GetStoredProcCommand("pub_RetirementPartnerAllocation_Update");

            Db.AddInParameter(command, "@RetirementPartnerAllocationId", DbType.Int32, RetirementPartnerAllocation.RetirementPartnerAllocationId);
            Db.AddInParameter(command, "@RetirementPartnerId", DbType.Int32, RetirementPartnerAllocation.RetirementPartnerId);
            Db.AddInParameter(command, "@EarningsAllocationId", DbType.Int32, RetirementPartnerAllocation.EarningsAllocationPartnerId);
            Db.AddInParameter(command, "@Amount", DbType.Int32, RetirementPartnerAllocation.Amount);
            Db.ExecuteScalar(command);
        }
    }
}
