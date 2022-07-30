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
    public class CashCycleRepository : BaseRepository
    {
        public CashCycle GetCashCycleById(int cashCycleId)
        {
            var data = Db.ExecuteSprocAccessor("pub_CashCycle_Get", CashCycleMapper.Mapper, cashCycleId);

            return GetFirstElement(data);
        }

        public CashCycle GetCashCycleByCashierId(int CashierId)
        {
            var data = Db.ExecuteSprocAccessor("pub_CashCycleByCashierId_Get", CashCycleMapper.Mapper, CashierId);

            return GetFirstElement(data);
        }

        public CashCycle getlastBalance(int CashierId)
        {
            var data = Db.ExecuteSprocAccessor("pub_CashCycleLastBalanceByCashierId_Get", CashCycleMapper.Mapper, CashierId);

            return GetFirstElement(data);
        }

        public IEnumerable<CashCycle> GetCashCycles()
        {
            var data = Db.ExecuteSprocAccessor("pub_CashCycle_GetList", CashCycleMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<CashCyclePayment> GetCashCyclePayments(int cashCycleId, int cashierId)
        {
            var data = Db.ExecuteSprocAccessor("pub_CashCyclePayment_GetList", CashCyclePaymentMapper.Mapper, cashCycleId, cashierId);

            return data.ToList();
        }

        public IEnumerable<Cycle> GetCycles()
        {
            var data = Db.ExecuteSprocAccessor("pub_Cycle_GetList", CycleMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<CashCycleSummary> GetCashCycleSummaryOpen()
        {
            var data = Db.ExecuteSprocAccessor("pub_CashCycleSummaryOpen_GetList", CashCycleSummaryMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<CashCycleSummary> GetCashCycleSummaryClose(int cashierId)
        {
            var data = Db.ExecuteSprocAccessor("pub_CashCycleSummaryClose_GetList", CashCycleSummaryMapper.Mapper, cashierId);

            return data.ToList();
        }


        public CashCyclePaymentMethod GetCashCyclePaymentMethod(int cashCyclePaymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_CashCyclePaymentMethod_Get", CashCyclePaymentMethodMapper.Mapper, cashCyclePaymentMethodId);

            return GetFirstElement(data);
        }

        public CashCyclePaymentMethod GetCashCyclePaymentMethodByCashCyclePM(int cashCycleId, int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_CashCyclePaymentMethodByCashCyclePM_GetList", CashCyclePaymentMethodMapper.Mapper, cashCycleId, paymentMethodId);

            return GetFirstElement(data);
        }      


        public IEnumerable<CashCyclePaymentMethod> GetCashCyclePaymentMethods(int cashCycleId)
        {
            var data = Db.ExecuteSprocAccessor("pub_CashCyclePaymentMethod_GetList", CashCyclePaymentMethodMapper.Mapper, cashCycleId);

            return data.ToList();
        }


        public void DeleteCashCycle(int cashCycleId)
        {
            var command = Db.GetStoredProcCommand("pub_CashCycle_Delete");
            Db.AddInParameter(command, "@CashCycleId", DbType.Int32, cashCycleId);
            Db.ExecuteScalar(command);
        }

        public void DeleteCashCyclePaymentMethodr(int cashCyclePaymentMethodId)
        {
            var command = Db.GetStoredProcCommand("pub_CashCyclePaymentMethod_Delete");
            Db.AddInParameter(command, "@CashCyclePaymentMethodId", DbType.Int32, cashCyclePaymentMethodId);
            Db.ExecuteScalar(command);
        }

        public void DeleteCashCyclePaymentMethodByCashCycleId(int cashCycleId)
        {
            var command = Db.GetStoredProcCommand("pub_CashCyclePaymentMethodByCashCycleId_Delete");
            Db.AddInParameter(command, "@cashCycleId", DbType.Int32, cashCycleId);
            Db.ExecuteScalar(command);
        }


        public int AddCashCycle(CashCycle CashCycle)
        {
            var command = Db.GetStoredProcCommand("pub_CashCycle_Insert");

            Db.AddInParameter(command, "@CashierId", DbType.Int32, CashCycle.CashierId);
            Db.AddInParameter(command, "@UserId", DbType.Int32, CashCycle.UserId);
            Db.AddInParameter(command, "@Closed", DbType.Boolean, false);
            Db.AddInParameter(command, "@Date", DbType.DateTime, CashCycle.Date);
            Db.AddInParameter(command, "@CycleNumber", DbType.String, CashCycle.CycleNumber);
            Db.AddInParameter(command, "@Time", DbType.String, CashCycle.Time);
            Db.AddInParameter(command, "@UserValidateId", DbType.Int32, CashCycle.UserValidateId);
            Db.AddInParameter(command, "@Validate", DbType.String, false);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }


        public void AddCashCyclePaymentMethod(CashCyclePaymentMethod CashCyclePaymentMethod)
        {
            var command = Db.GetStoredProcCommand("pub_CashCyclePaymentMethod_Insert");

            Db.AddInParameter(command, "@CashCycleId", DbType.Int32, CashCyclePaymentMethod.CashCycleId);
            Db.AddInParameter(command, "@Balance", DbType.Double, CashCyclePaymentMethod.Balance);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, CashCyclePaymentMethod.PaymentMethodId);


            Db.ExecuteScalar(command);
        }

        public void UpdateCashCycle(CashCycle CashCycle)
        {
            var command = Db.GetStoredProcCommand("pub_CashCycle_Update");

            Db.AddInParameter(command, "@CashCycleId", DbType.Int32, CashCycle.CashCycleId);
            Db.AddInParameter(command, "@CashierId", DbType.Int32, CashCycle.CashierId);
            Db.AddInParameter(command, "@UserId", DbType.Int32, CashCycle.UserId);
            Db.AddInParameter(command, "@Closed", DbType.Boolean, CashCycle.Closed);
            Db.AddInParameter(command, "@Date", DbType.DateTime, CashCycle.Date);
            Db.AddInParameter(command, "@CycleNumber", DbType.String, CashCycle.CycleNumber);
            Db.AddInParameter(command, "@Time", DbType.String, CashCycle.Time);
            Db.AddInParameter(command, "@UserValidateId", DbType.Int32, CashCycle.UserValidateId);
            Db.AddInParameter(command, "@Validate", DbType.String, CashCycle.Validate);
            Db.ExecuteScalar(command);
        }


        public void UpdateCashCyclePaymentMethod(CashCyclePaymentMethod CashCyclePaymentMethod)
        {
            var command = Db.GetStoredProcCommand("pub_CashCyclePaymentMethod_Update");

            Db.AddInParameter(command, "@CashCyclePaymentMethodId", DbType.Int32, CashCyclePaymentMethod.CashCyclePaymentMethodId);
            Db.AddInParameter(command, "@CashCycleId", DbType.Int32, CashCyclePaymentMethod.CashCycleId);
            Db.AddInParameter(command, "@Balance", DbType.Double, CashCyclePaymentMethod.Balance);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, CashCyclePaymentMethod.PaymentMethodId);
            Db.ExecuteScalar(command);
        }

    }
}
