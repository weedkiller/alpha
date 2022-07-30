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
    public class SpendingPaymentRepository : BaseRepository
    {
        public SpendingPayment GetSpendingPaymentById(int spendingPaymentId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPayment_Get", SpendingPaymentMapper.Mapper, spendingPaymentId);

            return GetFirstElement(data);
        }

        public IEnumerable<SpendingPaymentSummary> GetSpendingPaymentsSummary(string SelectedProvider, string SelectedUser, string DateFromExecution, string DateToExecution, string DateFromSystem, string DateToSystem, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentSummary_GetList", SpendingPaymentSummaryMapper.Mapper, SelectedProvider, SelectedUser, DateFromExecution, DateToExecution, DateFromSystem, DateToSystem, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance);

            return data.ToList();
        }

        public IEnumerable<SpendingPaymentSummary> GetSallaryPaymentsSummary(string SelectedProvider, string SelectedUser, string DateFromExecution, string DateToExecution, string DateFromSystem, string DateToSystem, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance)
        {
            var data = Db.ExecuteSprocAccessor("pub_SallaryPaymentSummary_GetList", SpendingPaymentSummaryMapper.Mapper, SelectedProvider, SelectedUser, DateFromExecution, DateToExecution, DateFromSystem, DateToSystem, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance);

            return data.ToList();
        }

        public IEnumerable<SpendingPaymentDetailSummary> GetSpendingPaymentsDetailSummary(string SelectedProvider, string SelectedUser, string DateFromExecution, string DateToExecution, string DateFromSystem, string DateToSystem, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentDetailSummary_GetList", SpendingPaymentDetailSummaryMapper.Mapper, SelectedProvider, SelectedUser, DateFromExecution, DateToExecution, DateFromSystem, DateToSystem, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance);

            return data.ToList();
        }

        public IEnumerable<SpendingPaymentDetailSummary> GetSallaryPaymentsDetailSummary(string SelectedProvider, string SelectedUser, string DateFromExecution, string DateToExecution, string DateFromSystem, string DateToSystem, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance)
        {
            var data = Db.ExecuteSprocAccessor("pub_SallaryPaymentDetailSummary_GetList", SpendingPaymentDetailSummaryMapper.Mapper, SelectedProvider, SelectedUser, DateFromExecution, DateToExecution, DateFromSystem, DateToSystem, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance);

            return data.ToList();
        }


        public IEnumerable<SpendingPayment> GetSpendingPayments()
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPayment_GetList", SpendingPaymentMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<SpendingPayment> GetSallaryPayments()
        {
            var data = Db.ExecuteSprocAccessor("pub_SallaryPayment_GetList", SpendingPaymentMapper.Mapper);

            return data.ToList();
        }

        public SpendingPaymentDetail GetSpendingPaymentDetail(int spendingPaymentDetailId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentDetail_Get", SpendingPaymentDetailMapper.Mapper, spendingPaymentDetailId);

            return GetFirstElement(data);
        }

        public SpendingPaymentDetail GetSpendingPaymentDetailsBySpendingId(int spendingId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentDetailBySpendingId_Get", SpendingPaymentDetailMapper.Mapper, spendingId);

            return GetFirstElement(data);
        }


        public IEnumerable<SpendingPaymentDetail> GetSpendingPaymentDetails(int spendingPaymentId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentDetail_GetList", SpendingPaymentDetailMapper.Mapper, spendingPaymentId);

            return data.ToList();
        }



        public SpendingPaymentPaymentMethod GetSpendingPaymentPaymentMethod(int spendingPaymentPaymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentPaymentMethod_Get", SpendingPaymentPaymentMethodMapper.Mapper, spendingPaymentPaymentMethodId);

            return GetFirstElement(data);
        }


        public IEnumerable<SpendingPaymentPaymentMethod> GetSpendingPaymentPaymentMethods(int spendingPaymentId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentPaymentMethod_GetList", SpendingPaymentPaymentMethodMapper.Mapper, spendingPaymentId);

            return data.ToList();
        }

        public IEnumerable<SpendingPaymentPaymentMethod> GetSpendingsPaymentPMsByCashCycle(int cashCycleId, int cashierId, int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentPMByCashCycle_GetList", SpendingPaymentPaymentMethodMapper.Mapper, cashCycleId, cashierId, paymentMethodId);

            return data.ToList();
        }


        public IEnumerable<SpendingPaymentPaymentMethod> GetSpendingsPaymentPMsBalance(int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentPMBalance_GetList", SpendingPaymentPaymentMethodMapper.Mapper, paymentMethodId);

            return data.ToList();
        }

        public IEnumerable<SpendingPaymentPaymentMethod> GetSpendingPaymentPaymentMethodsByPM(int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentPaymentMethodByPM_GetList", SpendingPaymentPaymentMethodMapper.Mapper, paymentMethodId);

            return data.ToList();
        }



        public void DeleteSpendingPayment(int spendingPaymentId)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPayment_Delete");
            Db.AddInParameter(command, "@SpendingPaymentId", DbType.Int32, spendingPaymentId);
            Db.ExecuteScalar(command);
        }

        public void DeleteSpendingPaymentDetail(int spendingPaymentDetailId)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPaymentDetail_Delete");
            Db.AddInParameter(command, "@SpendingPaymentDetailId", DbType.Int32, spendingPaymentDetailId);
            Db.ExecuteScalar(command);
        }

        public void DeleteSpendingPaymentDetailBySpendingPaymentId(int spendingPaymentId)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPaymentDetailBySpendingPaymentId_Delete");
            Db.AddInParameter(command, "@spendingPaymentId", DbType.Int32, spendingPaymentId);
            Db.ExecuteScalar(command);
        }

        public void DeleteSpendingPaymentPMBySpendingPaymentId(int spendingPaymentId)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPaymentPMBySpendingPaymentId_Delete");
            Db.AddInParameter(command, "@spendingPaymentId", DbType.Int32, spendingPaymentId);
            Db.ExecuteScalar(command);
        }

        public void DeleteSpendingPaymentPaymentMethod(int spendingPaymentPaymentMethodId)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPaymentPaymentMethod_Delete");
            Db.AddInParameter(command, "@SpendingPaymentPaymentMethodId", DbType.Int32, spendingPaymentPaymentMethodId);
            Db.ExecuteScalar(command);
        }

        public int AddSpendingPayment(SpendingPayment SpendingPayment)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPayment_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, SpendingPayment.Amount);
            Db.AddInParameter(command, "@Balance", DbType.Double, SpendingPayment.Balance);
            Db.AddInParameter(command, "@Receipt", DbType.String, SpendingPayment.Receipt);
            Db.AddInParameter(command, "@Commentary", DbType.String, SpendingPayment.Commentary);
            Db.AddInParameter(command, "@ExecutionDate", DbType.DateTime, SpendingPayment.ExecutionDate);
            Db.AddInParameter(command, "@ProveedorId", DbType.Int32, SpendingPayment.ProveedorId);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, DateTime.Now);
            Db.AddInParameter(command, "@SpendingPaymentTypeId", DbType.Int32, SpendingPayment.SpendingPaymentTypeId);
            Db.AddInParameter(command, "@UserId", DbType.Int32, SpendingPayment.UserId);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }


        public void AddSpendingPaymentDetail(SpendingPaymentDetail SpendingPaymentDetail)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPaymentDetail_Insert");

            Db.AddInParameter(command, "@SpendingId", DbType.Int32, SpendingPaymentDetail.SpendingId);
            Db.AddInParameter(command, "@SpendingPaymentId", DbType.Int32, SpendingPaymentDetail.SpendingPaymentId);
            Db.AddInParameter(command, "@Total", DbType.Double, SpendingPaymentDetail.Total);

            Db.ExecuteScalar(command);
        }

        public void AddSpendingPaymentPaymentMethod(SpendingPaymentPaymentMethod SpendingPaymentPaymentMethod)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPaymentPaymentMethod_Insert");

            Db.AddInParameter(command, "@SpendingPaymentId", DbType.Int32, SpendingPaymentPaymentMethod.SpendingPaymentId);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, SpendingPaymentPaymentMethod.PaymentMethodId);
            Db.AddInParameter(command, "@Amount", DbType.Double, SpendingPaymentPaymentMethod.Amount);
            Db.AddInParameter(command, "@Saving", DbType.Int32, SpendingPaymentPaymentMethod.Saving);
            Db.AddInParameter(command, "@Cycle", DbType.Double, SpendingPaymentPaymentMethod.Cycle);

            Db.ExecuteScalar(command);
        }

        public void UpdateSpendingPayment(SpendingPayment SpendingPayment)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPayment_Update");

            Db.AddInParameter(command, "@SpendingPaymentId", DbType.Int32, SpendingPayment.SpendingPaymentId);
            Db.AddInParameter(command, "@Amount", DbType.Double, SpendingPayment.Amount);
            Db.AddInParameter(command, "@Balance", DbType.Double, SpendingPayment.Balance);
            Db.AddInParameter(command, "@Receipt", DbType.String, SpendingPayment.Receipt);
            Db.AddInParameter(command, "@Commentary", DbType.String, SpendingPayment.Commentary);
            Db.AddInParameter(command, "@ExecutionDate", DbType.DateTime, SpendingPayment.ExecutionDate);
            Db.AddInParameter(command, "@ProveedorId", DbType.Int32, SpendingPayment.ProveedorId);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, SpendingPayment.SystemDate);
            Db.AddInParameter(command, "@SpendingPaymentTypeId", DbType.Int32, SpendingPayment.SpendingPaymentTypeId);
            Db.AddInParameter(command, "@UserId", DbType.Int32, SpendingPayment.UserId);
            Db.ExecuteScalar(command);
        }


        public void UpdateSpendingPaymentDetail(SpendingPaymentDetail SpendingPaymentDetail)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPaymentDetail_Update");

            Db.AddInParameter(command, "@SpendingPaymentDetailId", DbType.Int32, SpendingPaymentDetail.SpendingPaymentDetailId);
            Db.AddInParameter(command, "@SpendingId", DbType.Int32, SpendingPaymentDetail.SpendingId);
            Db.AddInParameter(command, "@SpendingPaymentId", DbType.Int32, SpendingPaymentDetail.SpendingPaymentId);
            Db.AddInParameter(command, "@Amount", DbType.Double, SpendingPaymentDetail.Total);
            Db.ExecuteScalar(command);
        }

        public void UpdateSpendingPaymentPaymentMethod(SpendingPaymentPaymentMethod SpendingPaymentPaymentMethod)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPaymentPaymentMethod_Update");

            Db.AddInParameter(command, "@SpendingPaymentPaymentMethodId", DbType.Int32, SpendingPaymentPaymentMethod.SpendingPaymentPaymentMethodId);
            Db.AddInParameter(command, "@SpendingPaymentId", DbType.Int32, SpendingPaymentPaymentMethod.SpendingPaymentId);
            Db.AddInParameter(command, "@SpendingPaymentPaymentMethodId", DbType.Int32, SpendingPaymentPaymentMethod.SpendingPaymentPaymentMethodId);
            Db.AddInParameter(command, "@Amount", DbType.Double, SpendingPaymentPaymentMethod.Amount);
            Db.AddInParameter(command, "@Saving", DbType.Int32, SpendingPaymentPaymentMethod.Saving);
            Db.AddInParameter(command, "@Cycle", DbType.Double, SpendingPaymentPaymentMethod.Cycle);
            Db.ExecuteScalar(command);
        }
    }
}
