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
    public class CashierRepository : BaseRepository
    {
        public Cashier GetCashierById(int CashierId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Cashier_Get", CashierMapper.Mapper, CashierId);

            return GetFirstElement(data);
        }


        public IEnumerable<Cashier> GetCashiers()
        {
            var data = Db.ExecuteSprocAccessor("pub_Cashier_GetList", CashierMapper.Mapper);

            return data.ToList();
        }

        public Cashier GetCashiersByUserId(int userId)
        {
            var data = Db.ExecuteSprocAccessor("pub_CashierByUser_GetList", CashierMapper.Mapper, userId);

            return GetFirstElement(data);
        }

        public IEnumerable<TransferCashierPaymentMethod> GetTransferCashierPaymentMethodPosByCashCycle(int cashCycleId, int cashierId, int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_TransferCashierPMPosByCashCycle_GetList", TransferCashierPaymentMethodMapper.Mapper, cashCycleId, cashierId, paymentMethodId);

            return data.ToList();
        }

        public IEnumerable<TransferCashierPaymentMethod> GetTransferCashierPaymentMethodPosBalance(int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_TransferCashierPMPosBalance_GetList", TransferCashierPaymentMethodMapper.Mapper, paymentMethodId);

            return data.ToList();
        }

        public IEnumerable<TransferCashierPaymentMethod> GetTransferCashierPaymentMethodNegByCashCycle(int cashCycleId, int cashierId, int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_TransferCashierPMNegByCashCycle_GetList", TransferCashierPaymentMethodMapper.Mapper, cashCycleId, cashierId, paymentMethodId);

            return data.ToList();
        }

        public IEnumerable<TransferCashierPaymentMethod> GetTransferCashierPaymentMethodNegBalance(int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_TransferCashierPMNegBalance_GetList", TransferCashierPaymentMethodMapper.Mapper, paymentMethodId);

            return data.ToList();
        }

        public void DeleteCashier(int CashierId)
        {
            var command = Db.GetStoredProcCommand("pub_Cashier_Delete");
            Db.AddInParameter(command, "@CashierId", DbType.Int32, CashierId);
            Db.ExecuteScalar(command);
        }

        public void DeleteHeading(int headingId)
        {
            var command = Db.GetStoredProcCommand("pub_Heading_Delete");
            Db.AddInParameter(command, "@HeadingId", DbType.Int32, headingId);
            Db.ExecuteScalar(command);
        }

        public int AddCashier(Cashier Cashier)
        {
            var command = Db.GetStoredProcCommand("pub_Cashier_Insert");

            Db.AddInParameter(command, "@Name", DbType.String, Cashier.Name);
            Db.AddInParameter(command, "@Active", DbType.Boolean, Cashier.Active);
            Db.AddInParameter(command, "@Individual", DbType.Boolean, Cashier.Individual);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int AddTransferCashierPaymentMethod(TransferCashierPaymentMethod transferCashierPaymentMethod)
        {
            var command = Db.GetStoredProcCommand("pub_TransferCashierPaymentMethod_Insert");

            Db.AddInParameter(command, "@Cycle", DbType.String, transferCashierPaymentMethod.Cycle);
            Db.AddInParameter(command, "@Entry", DbType.Boolean, transferCashierPaymentMethod.Entry);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, transferCashierPaymentMethod.PaymentMethodId);
            Db.AddInParameter(command, "@Saving", DbType.String, transferCashierPaymentMethod.Saving);
            Db.AddInParameter(command, "@TransferCashierId", DbType.Int32, transferCashierPaymentMethod.TransferCashierId);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int AddTransferCashier(TransferCashier transferCashier)
        {
            var command = Db.GetStoredProcCommand("pub_TransferCashier_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, transferCashier.Amount);
            Db.AddInParameter(command, "@Commentary", DbType.String, transferCashier.Commentary);
            Db.AddInParameter(command, "@Date", DbType.Date, transferCashier.Date);
            Db.AddInParameter(command, "@DestinyCashierId", DbType.Int32, transferCashier.DestinyCashierId);
            Db.AddInParameter(command, "@OriginCashierId", DbType.Int32, transferCashier.OriginCashierId);
            Db.AddInParameter(command, "@Time", DbType.String, transferCashier.Time);
            Db.AddInParameter(command, "@UserId", DbType.Int32, transferCashier.UserId);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdateCashier(Cashier Cashier)
        {
            var command = Db.GetStoredProcCommand("pub_Cashier_Update");

            Db.AddInParameter(command, "@CashierId", DbType.Int32, Cashier.CashierId);
            Db.AddInParameter(command, "@Name", DbType.String, Cashier.Name);
            Db.AddInParameter(command, "@Active", DbType.Boolean, Cashier.Active);
            Db.AddInParameter(command, "@Individual", DbType.Boolean, Cashier.Individual);
            Db.ExecuteScalar(command);
        }

    }
}
