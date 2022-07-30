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
    public class PromissoryPaymentRepository : BaseRepository
    {
        public PromissoryPayment GetPromissoryPaymentById(int promissoryPaymentId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryPayment_Get", PromissoryPaymentMapper.Mapper, promissoryPaymentId);

            return GetFirstElement(data);
        }

        public IEnumerable<PromissoryPaymentSummary> GetPromissoryPaymentsSummary(string SelectedClient, string SelectedUser, string DateFromSystem, string DateToSystem, string DateFromPayment, string DateToPayment, string AmountFrom, string AmountTo)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryPaymentSummary_GetList", PromissoryPaymentSummaryMapper.Mapper, SelectedClient, SelectedUser, DateFromSystem, DateToSystem, DateFromPayment, DateToPayment, AmountFrom, AmountTo);

            return data.ToList();
        }

        public IEnumerable<PromissorySummary> GetPromissoryPaymentPromissories(string SelectedClient, string SelectedUser, string DateFromSystem, string DateToSystem, string DateFromPayment, string DateToPayment, string AmountFrom, string AmountTo)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryPaymentPromissories_GetList", PromissorySummaryMapper.Mapper, SelectedClient, SelectedUser, DateFromSystem, DateToSystem, DateFromPayment, DateToPayment, AmountFrom, AmountTo);

            return data.ToList();
        }

        public IEnumerable<PromissoryPayment> GetPromissoryPayments()
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryPayment_GetList", PromissoryPaymentMapper.Mapper);

            return data.ToList();
        }


        public PromissoryPaymentMethod GetPromissoryPaymentMethod(int promissoryPaymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryPaymentMethod_Get", PromissoryPaymentMethodMapper.Mapper, promissoryPaymentMethodId);

            return GetFirstElement(data);
        }

        public IEnumerable<PromissoryPaymentMethod> GetPromissoryPaymentMethodByPayment(int promissoryPaymentId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryPaymentMethodByPayment_GetList", PromissoryPaymentMethodMapper.Mapper, promissoryPaymentId);

            return data.ToList();
        }


        public IEnumerable<PromissoryPaymentMethod> GetPromissoryPaymentMethods()
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryPaymentMethod_GetList", PromissoryPaymentMethodMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<PromissoryPaymentMethod> GetPromissoryPaymentMethodsByCashCycle(int cashCycleId, int cashierId, int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryPaymentMethodByCashCycle_GetList", PromissoryPaymentMethodMapper.Mapper, cashCycleId, cashierId, paymentMethodId);

            return data.ToList();
        }

        public IEnumerable<PromissoryPaymentMethod> GetPromissoryPaymentMethodsBalance(int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryPaymentMethodBalance_GetList", PromissoryPaymentMethodMapper.Mapper, paymentMethodId);

            return data.ToList();
        }

        public PromissoryPaymentPromissory GetPromissoryPaymentPromissory(int promissoryPaymentPromissory)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryPaymentPromissory_Get", PromissoryPaymentPromissoryMapper.Mapper, promissoryPaymentPromissory);

            return GetFirstElement(data);
        }

        public IEnumerable<PromissoryPaymentPromissory> GetPromissoryPaymentPromissoryByPayment(int promissoryPaymentId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryPaymentPromissoryByPayment_GetList", PromissoryPaymentPromissoryMapper.Mapper, promissoryPaymentId);

            return data.ToList();
        }


        public IEnumerable<PromissoryPaymentPromissory> GetPromissoryPaymentromissorys()
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryPaymentPromissory_GetList", PromissoryPaymentPromissoryMapper.Mapper);

            return data.ToList();
        }



        public void DeletePromissoryPayment(int promissoryPaymentId)
        {
            var command = Db.GetStoredProcCommand("pub_PromissoryPayment_Delete");
            Db.AddInParameter(command, "@PromissoryPaymentId", DbType.Int32, promissoryPaymentId);
            Db.ExecuteScalar(command);
        }

        public void DeletePromissoryPaymentMethod(int promissoryPaymentId)
        {
            var command = Db.GetStoredProcCommand("pub_PromissoryPaymentMethod_Delete");
            Db.AddInParameter(command, "@PromissoryPaymentId", DbType.Int32, promissoryPaymentId);
            Db.ExecuteScalar(command);
        }

        public void DeletePromissoryPaymentPromissory(int promissoryPaymentId)
        {
            var command = Db.GetStoredProcCommand("pub_PromissoryPaymentPromissory_Delete");
            Db.AddInParameter(command, "@promissoryPaymentId", DbType.Int32, promissoryPaymentId);
            Db.ExecuteScalar(command);
        }

        public int AddPromissoryPayment(PromissoryPayment PromissoryPayment)
        {
            var command = Db.GetStoredProcCommand("pub_PromissoryPayment_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, PromissoryPayment.Amount);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, PromissoryPayment.SystemDate);
            Db.AddInParameter(command, "@ClientId", DbType.Int32, PromissoryPayment.ClientId);
            Db.AddInParameter(command, "@PaymentDate", DbType.DateTime, PromissoryPayment.PaymentDate);
            Db.AddInParameter(command, "@UserId", DbType.Int32, PromissoryPayment.UserId);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdatePromissoryPayment(PromissoryPayment PromissoryPayment)
        {
            var command = Db.GetStoredProcCommand("pub_PromissoryPayment_Update");

            Db.AddInParameter(command, "@PromissoryPaymentId", DbType.Int32, PromissoryPayment.PromissoryPaymentId);
            Db.AddInParameter(command, "@Amount", DbType.Double, PromissoryPayment.Amount);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, PromissoryPayment.SystemDate);
            Db.AddInParameter(command, "@ClientId", DbType.Int32, PromissoryPayment.ClientId);
            Db.AddInParameter(command, "@PaymentDate", DbType.DateTime, PromissoryPayment.PaymentDate);
            Db.AddInParameter(command, "@UserId", DbType.Int32, PromissoryPayment.UserId);
            Db.ExecuteScalar(command);
        }


        public void AddPromissoryPaymentMethod(PromissoryPaymentMethod PromissoryPaymentMethod)
        {
            var command = Db.GetStoredProcCommand("pub_PromissoryPaymentMethod_Insert");

            Db.AddInParameter(command, "@PromissoryPaymentId", DbType.Int32, PromissoryPaymentMethod.PromissoryPaymentId);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, PromissoryPaymentMethod.PaymentMethodId);
            Db.AddInParameter(command, "@Amount", DbType.Int32, PromissoryPaymentMethod.Amount);
            Db.AddInParameter(command, "@Saving", DbType.Int32, PromissoryPaymentMethod.Saving);
            Db.AddInParameter(command, "@Cycle", DbType.Double, PromissoryPaymentMethod.Cycle);
            Db.ExecuteScalar(command);
        }


        public void UpdatePromissoryPaymentMethod(PromissoryPaymentMethod PromissoryPaymentMethod)
        {
            var command = Db.GetStoredProcCommand("pub_PromissoryPaymentMethod_Update");

            Db.AddInParameter(command, "@PromissoryPaymentMethodId", DbType.Int32, PromissoryPaymentMethod.PromissoryPaymentMethodId);
            Db.AddInParameter(command, "@PromissoryPaymentId", DbType.Int32, PromissoryPaymentMethod.PromissoryPaymentId);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, PromissoryPaymentMethod.PaymentMethodId);
            Db.AddInParameter(command, "@Saving", DbType.Int32, PromissoryPaymentMethod.Saving);
            Db.AddInParameter(command, "@Cycle", DbType.Double, PromissoryPaymentMethod.Cycle);
            Db.ExecuteScalar(command);
        }

        public void AddPromissoryPaymentPromissory(PromissoryPaymentPromissory PromissoryPaymentPromissory)
        {
            var command = Db.GetStoredProcCommand("pub_PromissoryPaymentPromissory_Insert");

            Db.AddInParameter(command, "@PromissoryPaymentId", DbType.Int32, PromissoryPaymentPromissory.PromissoryPaymentId);
            Db.AddInParameter(command, "@PromissoryId", DbType.Int32, PromissoryPaymentPromissory.PromissoryId);

            Db.ExecuteScalar(command);
        }

        public void UpdatePromissoryPaymentPromissory(PromissoryPaymentPromissory PromissoryPaymentPromissory)
        {
            var command = Db.GetStoredProcCommand("pub_PromissoryPaymentPromissory_Update");

            Db.AddInParameter(command, "@PromissoryPaymentPromissoryId", DbType.Int32, PromissoryPaymentPromissory.PromissoryPaymentPromissoryId);
            Db.AddInParameter(command, "@PromissoryPaymentId", DbType.Int32, PromissoryPaymentPromissory.PromissoryPaymentId);
            Db.AddInParameter(command, "@PromissoryId", DbType.Int32, PromissoryPaymentPromissory.PromissoryId);
            Db.ExecuteScalar(command);
        }
    }
}
