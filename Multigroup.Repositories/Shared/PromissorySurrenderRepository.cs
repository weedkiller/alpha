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
    public class PromissorySurrenderRepository : BaseRepository
    {
        public PromissorySurrender GetPromissorySurrenderById(int promissorySurrenderId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissorySurrender_Get", PromissorySurrenderMapper.Mapper, promissorySurrenderId);

            return GetFirstElement(data);
        }

        public IEnumerable<PromissorySurrender> GetPromissorySurrenders()
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissorySurrender_GetList", PromissorySurrenderMapper.Mapper);

            return data.ToList();
        }

        public PromissorySurrender GetPromissorySurrenderBySurrenderPartnerId(int promissorySurrenderPartnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissorySurrenderBySurrenderPartner_Get", PromissorySurrenderMapper.Mapper, promissorySurrenderPartnerId);

            return GetFirstElement(data);
        }

        public IEnumerable<PromissorySurrenderPartnerSummary> GetPromissorySurrendersPartnerSummary(string SelectedPartner, string SelectedUser, string DateFromPayment, string DateToPayment, string AmountFrom, string AmountTo, string Paid)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissorySurrenderPartnerSummary_GetList", PromissorySurrenderPartnerSummaryMapper.Mapper, SelectedPartner, SelectedUser , DateFromPayment, DateToPayment, AmountFrom, AmountTo, Paid);

            return data.ToList();
        }

        public PromissorySurrenderPartner GetPromissorySurrenderPartnerById(int promissorySurrenderPartnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissorySurrenderPartner_Get", PromissorySurrenderPartnerMapper.Mapper, promissorySurrenderPartnerId);

            return GetFirstElement(data);
        }

        public PromissorySurrenderPartner GetPromissorySurrenderPartnerByPromissory(int promissoryId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissorySurrenderPartnerByPromissory_Get", PromissorySurrenderPartnerMapper.Mapper, promissoryId);

            return GetFirstElement(data);
        }

        public PromissorySurrenderPartner GetPromissorySurrenderPartnerByPromissoryPartner(int promissoryId, int partnerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissorySurrenderPartnerByPromissoryPartner_Get", PromissorySurrenderPartnerMapper.Mapper, promissoryId, partnerId);

            return GetFirstElement(data);
        }

        public IEnumerable<PromissorySurrenderPartner> GetPromissorySurrenderPartners()
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissorySurrenderPartner_GetList", PromissorySurrenderPartnerMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<PromissorySurrenderPartner> GetPromissorySurrenderPartnerPaidByPayment(int promissoryPayment)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissorySurrenderPartnerPaidByPayment_GetList", PromissorySurrenderPartnerMapper.Mapper, promissoryPayment);

            return data.ToList();
        }

        public PromissorySurrenderMethod GetPromissorySurrenderMethodById(int promissorySurrenderMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissorySurrenderMethod_Get", PromissorySurrenderMethodMapper.Mapper, promissorySurrenderMethodId);

            return GetFirstElement(data);
        }

        public IEnumerable<PromissorySurrenderMethod> GetPromissorySurrenderMethodsBySurrender(int promissorySurrenderId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissorySurrenderMethod_GetList", PromissorySurrenderMethodMapper.Mapper, promissorySurrenderId);

            return data.ToList();
        }

        public IEnumerable<PromissorySurrenderMethod> GetPromissorySurrendersMethodsByCashCycle(int cashCycleId, int cashierId, int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissorySurrenderMethodByCashCycle_GetList", PromissorySurrenderMethodMapper.Mapper, cashCycleId, cashierId, paymentMethodId);

            return data.ToList();
        }

        public IEnumerable<PromissorySurrenderMethod> GetPromissorySurrendersMethodsBalance(int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissorySurrenderMethodBalance_GetList", PromissorySurrenderMethodMapper.Mapper, paymentMethodId);

            return data.ToList();
        }


        public PromissoryRendered GetPromissoryRenderedById(int promissoryRenderedId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryRendered_Get", PromissoryRenderedMapper.Mapper, promissoryRenderedId);

            return GetFirstElement(data);
        }

        public IEnumerable<PromissoryRendered> GetPromissoryRenderedBySurrender(int promissorySurrenderId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PromissoryRendered_GetList", PromissoryRenderedMapper.Mapper, promissorySurrenderId);

            return data.ToList();
        }

        public void DeletePromissorySurrender(int promissorySurrenderId)
        {
            var command = Db.GetStoredProcCommand("pub_PromissorySurrender_Delete");
            Db.AddInParameter(command, "@PromissorySurrenderId", DbType.Int32, promissorySurrenderId);
            Db.ExecuteScalar(command);
        }

        public void DeletePromissorySurrenderPartner(int promissoryPaymentId)
        {
            var command = Db.GetStoredProcCommand("pub_PromissorySurrenderPartner_Delete");
            Db.AddInParameter(command, "@PromissoryPaymentId", DbType.Int32, promissoryPaymentId);
            Db.ExecuteScalar(command);
        }

        public void DeletePromissorySurrenderMethodBySurrenderId(int promissorySurrenderId)
        {
            var command = Db.GetStoredProcCommand("pub_PromissorySurrenderMethod_Delete");
            Db.AddInParameter(command, "@promissorySurrenderId", DbType.Int32, promissorySurrenderId);
            Db.ExecuteScalar(command);
        }

        public void DeletePromissoryRenderedBySurrenderId(int promissorySurrenderId)
        {
            var command = Db.GetStoredProcCommand("pub_PromissoryRendered_Delete");
            Db.AddInParameter(command, "@promissorySurrenderId", DbType.Int32, promissorySurrenderId);
            Db.ExecuteScalar(command);
        }

        public int AddPromissorySurrender(PromissorySurrender PromissorySurrender)
        {
            var command = Db.GetStoredProcCommand("pub_PromissorySurrender_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, PromissorySurrender.Amount);
            Db.AddInParameter(command, "@PaymentDate", DbType.DateTime, PromissorySurrender.PaymentDate);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, DateTime.Now);
            Db.AddInParameter(command, "@UserId", DbType.Int32, PromissorySurrender.UserId);
            Db.AddInParameter(command, "@PartnerId", DbType.Int32, PromissorySurrender.PartnerId);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdatePromissorySurrender(PromissorySurrender PromissorySurrender)
        {
            var command = Db.GetStoredProcCommand("pub_PromissorySurrender_Update");

            Db.AddInParameter(command, "@PromissorySurrenderId", DbType.Int32, PromissorySurrender.PromissorySurrenderId);
            Db.AddInParameter(command, "@Amount", DbType.Double, PromissorySurrender.Amount);
            Db.AddInParameter(command, "@PaymentDate", DbType.DateTime, PromissorySurrender.PaymentDate);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, DateTime.Now);
            Db.AddInParameter(command, "@UserId", DbType.Int32, PromissorySurrender.UserId);
            Db.AddInParameter(command, "@PartnerId", DbType.Int32, PromissorySurrender.PartnerId);
            Db.ExecuteScalar(command);
        }

        public void AddPromissorySurrenderPartner(PromissorySurrenderPartner PromissorySurrenderPartner)
        {
            var command = Db.GetStoredProcCommand("pub_PromissorySurrenderPartner_Insert");

            Db.AddInParameter(command, "@PromissoryPaymentId", DbType.Int32, PromissorySurrenderPartner.PromissoryPaymentId);
            Db.AddInParameter(command, "@Amount", DbType.Double, PromissorySurrenderPartner.Amount);
            Db.AddInParameter(command, "@IsPaid", DbType.Boolean, PromissorySurrenderPartner.IsPaid);
            Db.AddInParameter(command, "@PromissoryId", DbType.Int32, PromissorySurrenderPartner.PromissoryId);
            Db.AddInParameter(command, "@PartnerId", DbType.Int32, PromissorySurrenderPartner.PartnerId);

            Db.ExecuteScalar(command);
        }


        public void UpdatePromissorySurrenderPartner(PromissorySurrenderPartner PromissorySurrenderPartner)
        {
            var command = Db.GetStoredProcCommand("pub_PromissorySurrenderPartner_Update");

            Db.AddInParameter(command, "@PromissorySurrenderPartnerId", DbType.Int32, PromissorySurrenderPartner.PromissorySurrenderPartnerId);
            Db.AddInParameter(command, "@PromissoryPaymentId", DbType.Int32, PromissorySurrenderPartner.PromissoryPaymentId);
            Db.AddInParameter(command, "@Amount", DbType.Double, PromissorySurrenderPartner.Amount);
            Db.AddInParameter(command, "@IsPaid", DbType.Boolean, PromissorySurrenderPartner.IsPaid);
            Db.AddInParameter(command, "@PromissoryId", DbType.Int32, PromissorySurrenderPartner.PromissoryId);
            Db.AddInParameter(command, "@PartnerId", DbType.Int32, PromissorySurrenderPartner.PartnerId);
            Db.ExecuteScalar(command);
        }

        public void AddPromissorySurrenderMethod(PromissorySurrenderMethod PromissorySurrenderMethod)
        {
            var command = Db.GetStoredProcCommand("pub_PromissorySurrenderMethod_Insert");

            Db.AddInParameter(command, "@PromissorySurrenderId", DbType.Int32, PromissorySurrenderMethod.PromissorySurrenderId);
            Db.AddInParameter(command, "@Amount", DbType.Double, PromissorySurrenderMethod.Amount);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, PromissorySurrenderMethod.PaymentMethodId);
            Db.AddInParameter(command, "@Saving", DbType.Int32, PromissorySurrenderMethod.Saving);
            Db.AddInParameter(command, "@Cycle", DbType.Double, PromissorySurrenderMethod.Cycle);

            Db.ExecuteScalar(command);
        }

        public void AddPromissoryRendered(PromissoryRendered PromissoryRendered)
        {
            var command = Db.GetStoredProcCommand("pub_PromissoryRendered_Insert");

            Db.AddInParameter(command, "@PromissorySurrenderId", DbType.Int32, PromissoryRendered.PromissorySurrenderId);
            Db.AddInParameter(command, "@PromissoryId", DbType.Int32, PromissoryRendered.PromissoryId);

            Db.ExecuteScalar(command);
        }
    }
}
