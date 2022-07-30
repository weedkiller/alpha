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
    public class PaymentMethodsRepository : BaseRepository
    {
        public PaymentMethod GetPaymentMethodsById(int PaymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentMethod_Get", GenericMapper<PaymentMethod>.Mapper, PaymentMethodId);

            return GetFirstElement(data);
        }


        public IEnumerable<PaymentMethod> GetPaymentMethodss()
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentMethod_GetList", GenericMapper<PaymentMethod>.Mapper);

            return data.ToList();
        }

        public IEnumerable<PaymentMethodSummary> GetPaymentMethodsSummary()
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentMethodSummary_GetList", GenericMapper<PaymentMethodSummary>.Mapper);

            return data.ToList();
        }

        public IEnumerable<PaymentMethodReconciled> GetPaymentMethodReconciled()
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentMethodReconciled_GetList", GenericMapper<PaymentMethodReconciled>.Mapper);

            return data.ToList();
        }

        public PaymentMethodReconciled GetPaymentByReceipNumber(int paymentMethodReconciledId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentMethodReconciled_Get", GenericMapper<PaymentMethodReconciled>.Mapper, paymentMethodReconciledId);

            return GetFirstElement(data);
        }

        public IEnumerable<PaymentMethodReconciledSummary> GetPaymentMethodReconciledSummary(int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentMethodReconciledSummary_GetList", GenericMapper<PaymentMethodReconciledSummary>.Mapper, paymentMethodId);

            return data.ToList();
        }

        public PaymentMethodVerification GetPaymentMethodVerificationBySpending(int MovementId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentMethodVerificationBySpending_GetList", GenericMapper<PaymentMethodVerification>.Mapper, MovementId);

            return GetFirstElement(data);
        }

        public PaymentMethodVerification GetPaymentMethodVerificationBySpendingPayment(int MovementId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentMethodVerificationBySpendingPayment_GetList", GenericMapper<PaymentMethodVerification>.Mapper, MovementId);

            return GetFirstElement(data);
        }

        public PaymentMethodVerification GetPaymentMethodVerificationBySallaryPayment(int MovementId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentMethodVerificationBySallaryPayment_GetList", GenericMapper<PaymentMethodVerification>.Mapper, MovementId);

            return GetFirstElement(data);
        }

        public PaymentMethodReconciledSummary GetPaymentMethodReconciledSummaryLast(int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentMethodReconciledSummary_Get", GenericMapper<PaymentMethodReconciledSummary>.Mapper, paymentMethodId);

            return GetFirstElement(data);
        }

        public IEnumerable<PaymentMethodReconciledLast> GetPaymentMethodReconciledLast()
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentMethodReconciledLast_GetList", GenericMapper<PaymentMethodReconciledLast>.Mapper);

            return data.ToList();
        }

        public IEnumerable<PaymentPreferencePaymentMethods> GetPaymentMethodByPaymentPreference(int paymentPreferenceId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentPreferencePM_GetList", GenericMapper<PaymentPreferencePaymentMethods>.Mapper, paymentPreferenceId);

            return data.ToList();
        }

        public void Delete(int PaymentMethodId)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentMethod_Delete");
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, PaymentMethodId);
            Db.ExecuteScalar(command);
        }

        public void DeletePaymentMethodByPaymentPreference(int PaymentPreferenceId)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentPreferencePM_Delete");
            Db.AddInParameter(command, "@PaymentPreferenceId", DbType.Int32, PaymentPreferenceId);
            Db.ExecuteScalar(command);
        }

        public void AddPaymentMethods(PaymentMethod paymentMethods)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentMethod_Insert");

            Db.AddInParameter(command, "@Name", DbType.String, paymentMethods.Name);
            Db.AddInParameter(command, "@Active", DbType.Boolean, paymentMethods.Active);
            Db.AddInParameter(command, "@Percentage", DbType.Double, paymentMethods.Percentage);
            Db.AddInParameter(command, "@Consolidated", DbType.Boolean, paymentMethods.Consolidated);
            Db.AddInParameter(command, "@Verifiable", DbType.Boolean, paymentMethods.Verifiable);
            Db.AddInParameter(command, "@Automatic", DbType.Boolean, paymentMethods.Automatic);
            Db.AddInParameter(command, "@Asignation", DbType.Boolean, paymentMethods.Asignation);
            Db.AddInParameter(command, "@PaymentPreferenceId", DbType.Int32, paymentMethods.PaymentPreferenceId);
            Db.AddInParameter(command, "@Channel", DbType.String, paymentMethods.Channel);

            Db.ExecuteScalar(command);
        }

        public void Update(PaymentMethod paymentMethods)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentMethod_Update");

            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, paymentMethods.PaymentMethodId);
            Db.AddInParameter(command, "@Name", DbType.String, paymentMethods.Name);
            Db.AddInParameter(command, "@Active", DbType.Boolean, paymentMethods.Active);
            Db.AddInParameter(command, "@Percentage", DbType.Double, paymentMethods.Percentage);
            Db.AddInParameter(command, "@Consolidated", DbType.Boolean, paymentMethods.Consolidated);
            Db.AddInParameter(command, "@Verifiable", DbType.Boolean, paymentMethods.Verifiable);
            Db.AddInParameter(command, "@Automatic", DbType.Boolean, paymentMethods.Automatic);
            Db.AddInParameter(command, "@Asignation", DbType.Boolean, paymentMethods.Asignation);
            Db.AddInParameter(command, "@PaymentPreferenceId", DbType.Int32, paymentMethods.PaymentPreferenceId);
            Db.AddInParameter(command, "@Channel", DbType.String, paymentMethods.Channel);

            Db.ExecuteScalar(command);
        }

        public void AddPaymentMethodVerification(PaymentMethodVerification paymentMethods)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentMethodVerification_Insert");

            Db.AddInParameter(command, "@Time", DbType.String, paymentMethods.Time);
            Db.AddInParameter(command, "@MovementType", DbType.String, paymentMethods.MovementType);
            Db.AddInParameter(command, "@MovementID", DbType.Int32, paymentMethods.MovementID);
            Db.AddInParameter(command, "@VerificationDate", DbType.DateTime, paymentMethods.VerificationDate);

            Db.ExecuteScalar(command);
        }

        public int AddPaymentMethodReconciled(PaymentMethodReconciled payment)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentMethodReconciled_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, payment.Amount);
            Db.AddInParameter(command, "@Commentary", DbType.String, payment.Commentary);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, payment.PaymentMethodId);
            Db.AddInParameter(command, "@ReconciledDate", DbType.DateTime, payment.ReconciledDate);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, payment.SystemDate);
            Db.AddInParameter(command, "@Time", DbType.String, payment.Time);
            Db.AddInParameter(command, "@UserId", DbType.Int32, payment.UserId);



            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdatePaymentMethodReconciled(PaymentMethodReconciled payment)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentMethodReconciled_Update");

            Db.AddInParameter(command, "@PaymentMethodReconciledId", DbType.Int32, payment.PaymentMethodReconciledId);
            Db.AddInParameter(command, "@Amount", DbType.Double, payment.Amount);
            Db.AddInParameter(command, "@Commentary", DbType.String, payment.Commentary);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, payment.PaymentMethodId);
            Db.AddInParameter(command, "@ReconciledDate", DbType.DateTime, payment.ReconciledDate);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, payment.SystemDate);
            Db.AddInParameter(command, "@Time", DbType.String, payment.Time);
            Db.AddInParameter(command, "@UserId", DbType.Int32, payment.UserId);

            Db.ExecuteScalar(command);
        }

        public int AddPaymentPreference(PaymentPreference paymentPreference)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentPreference_Insert");

            Db.AddInParameter(command, "@Description", DbType.String, paymentPreference.Description);
            Db.AddInParameter(command, "@Date", DbType.DateTime, DateTime.Now);
            Db.AddInParameter(command, "@Active", DbType.Boolean, paymentPreference.Active);
            Db.AddInParameter(command, "@Assing", DbType.Boolean, paymentPreference.Assing);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void AddPaymentPreferencePM(PaymentPreferencePaymentMethods paymentPreferencePaymentMethods)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentPreferencePM_Insert");

            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, paymentPreferencePaymentMethods.PaymentMethodId);
            Db.AddInParameter(command, "@PaymentPreferenceId", DbType.Int32, paymentPreferencePaymentMethods.PaymentPreferenceId);

            Db.ExecuteScalar(command);
        }

        public void UpdatePaymentPreference(PaymentPreference paymentPreference)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentPreference_Update");

            Db.AddInParameter(command, "@PaymentPreferenceId", DbType.Int32, paymentPreference.PaymentPreferenceId);
            Db.AddInParameter(command, "@Description", DbType.String, paymentPreference.Description);
            Db.AddInParameter(command, "@Date", DbType.DateTime, DateTime.Now);
            Db.AddInParameter(command, "@Active", DbType.Boolean, paymentPreference.Active);
            Db.AddInParameter(command, "@Assing", DbType.Boolean, paymentPreference.Assing);

            Db.ExecuteScalar(command);
        }
    }
}
