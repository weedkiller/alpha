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
    public class PaymentRepository : BaseRepository
    {
        public Payment GetPaymentById(int paymentId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Payment_Get", GenericMapper<Payment>.Mapper, paymentId);

            return GetFirstElement(data);
        }

        public PaymentDate GetPaymentDateById(int paymentDateId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentDate_Get", GenericMapper<PaymentDate>.Mapper, paymentDateId);

            return GetFirstElement(data);
        }

        public Payment GetPaymentByReceipNumber(string receipNumber, string paymentMethod)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentByReceipNumber_Get", GenericMapper<Payment>.Mapper, receipNumber, paymentMethod);

            return GetFirstElement(data);
        }
        public IEnumerable<PaymentPaymentMethod> GetPaymentPaymentMethodByCashCycle(int cashCycleId, int cashierId, int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentPMByCashCycle_GetList", PaymentPaymentMethodMapper.Mapper, cashCycleId, cashierId, paymentMethodId);

            return data.ToList();
        }

        public IEnumerable<PaymentPaymentMethod> GetPaymentPaymentMethodBalance(int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentPaymentMethodBalance_GetList", PaymentPaymentMethodMapper.Mapper, paymentMethodId);

            return data.ToList();
        }
        public IEnumerable<Payment> GetPayments()
        {
            var data = Db.ExecuteSprocAccessor("pub_Payment_GetList", GenericMapper<Payment>.Mapper);

            return data.ToList();
        }

        public IEnumerable<PaymentResume> GetPaymentResume(int contractId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentResume_GetList", GenericMapper<PaymentResume>.Mapper, contractId);

            return data.ToList();
        }

        public void DeleteInterestByInstallmentId(int InstallmentId)
        {
            var command = Db.GetStoredProcCommand("pub_InstallmentInterests_DeleteIdByInstallment");
            Db.AddInParameter(command, "@InstallmentId", DbType.Int32, InstallmentId);
            Db.ExecuteScalar(command);
        }

        public IEnumerable<PaymentAllocation> GetPaymentsAllocation()
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentAllocation_GetList", GenericMapper<PaymentAllocation>.Mapper);

            return data.ToList();
        }

        public IEnumerable<PaymentAllocation> GetPaymentsAllocationByPaymentId(int paymentId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentAllocationByPaymentId_GetList", GenericMapper<PaymentAllocation>.Mapper, paymentId);

            return data.ToList();
        }
        public IEnumerable<PaymentMethod> GetPaymentMethods()
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentMethod_GetList", GenericMapper<PaymentMethod>.Mapper);

            return data.ToList();
        }
        public IEnumerable<PaymentType> GetPaymentTypes()
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentType_GetList", GenericMapper<PaymentType>.Mapper);

            return data.ToList();
        }
        public IEnumerable<PaymentSummary> GetCollectionsByFilter(string SelectedPaymentMethod, string SelectedType, string SelectedPaymentPreference, string DateFrom, string DateTo, string UserId, string selectedCustomer)
        {
            var data = Db.ExecuteSprocAccessor("pub_CollectionsHistory_GetList", GenericMapper<PaymentSummary>.Mapper, SelectedPaymentMethod, SelectedType, SelectedPaymentPreference, DateTo, DateFrom, UserId, selectedCustomer);

            return data.ToList();
        }

        public IEnumerable<PaymentHistory> GetPaymentHistory(string _selectedCustomer, string _selectedPaymentMethod, string _selectedUser, string InstallmentNumber, string DateFrom, string DateTo, string province, string cities, string DateIFrom, string DateITo, string _selectedSupervisor)
        {
            if (InstallmentNumber.Equals(""))
                InstallmentNumber = "0";
            var data = Db.ExecuteSprocAccessor("pub_PaymentHistory_GetList", GenericMapper<PaymentHistory>.Mapper, _selectedCustomer, _selectedPaymentMethod, _selectedUser, DateTo, DateFrom, InstallmentNumber, province, cities, DateITo, DateIFrom, _selectedSupervisor);

            return data.ToList();
        }

        public IEnumerable<Payment> GetPaymentTypes(int clientId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentsByClientId_GetList", GenericMapper<Payment>.Mapper, clientId);

            return data.ToList();
        }

        public AssignPaymentPreference GetAssignPaymentPreferenceById(int AssignPaymentPreferenceId)
        {
            var data = Db.ExecuteSprocAccessor("pub_AssignPaymentPreference_Get", GenericMapper<AssignPaymentPreference>.Mapper, AssignPaymentPreferenceId);

            return GetFirstElement(data);
        }

        public IEnumerable<AssignPaymentPreference> GetAssignPaymentPreferences()
        {
            var data = Db.ExecuteSprocAccessor("pub_AssignPaymentPreference_GetList", GenericMapper<AssignPaymentPreference>.Mapper);

            return data.ToList();
        }

        public IEnumerable<AssignPaymentPreferenceSummary> GetAssignPaymentPreferenceSummary(string _selectedPaymentPreference, string _selectedUser, string _selectedState, string InstallmentNumber, string DateFrom, string DateTo, string customer)
        {
            var data = Db.ExecuteSprocAccessor("pub_AssignPaymentPreferenceSummary_GetList", GenericMapper<AssignPaymentPreferenceSummary>.Mapper, _selectedPaymentPreference, _selectedUser, _selectedState, InstallmentNumber, DateFrom, DateTo, customer);

            return data.ToList();
        }

        public IEnumerable<AssignPaymentPreferenceSummary> GetAssignPaymentPreferenceSummaryByAssignPaymentMethod(string _selectedPaymentPreference, string _selectedUser, string isProcessed, string isAdvised)
        {
            var data = Db.ExecuteSprocAccessor("pub_AssignPaymentPreferenceSummaryByAssignPM_GetList", GenericMapper<AssignPaymentPreferenceSummary>.Mapper, _selectedPaymentPreference, _selectedUser, isProcessed, isAdvised);

            return data.ToList();
        }

        public IEnumerable<AssignPaymentPreferenceSummary> GetAssignPaymentPreferenceSummaryChannelWarned(string _selectedPaymentMethod, string _selectedUser, string _SelectedCustomer, string DateFrom, string DateTo, string isWarned)
        {
            var data = Db.ExecuteSprocAccessor("pub_AssignPaymentPreferenceSummaryChannelWarned_GetList", GenericMapper<AssignPaymentPreferenceSummary>.Mapper, _selectedPaymentMethod, _selectedUser, _SelectedCustomer, DateFrom, DateTo, isWarned);

            return data.ToList();
        }

        public IEnumerable<AssignPaymentPreferenceWithVoucher> GetAssignPaymentPreferenceSummaryPaymentVoucher(string _selectedPaymentMethod, string _selectedUser, string _SelectedCustomer, string DateFrom, string DateTo, string DatePaymentFrom, string DatePaymentTo, string isPending)
        {
            var data = Db.ExecuteSprocAccessor("pub_AssignPaymentPreferenceWithVoucher_GetList", GenericMapper<AssignPaymentPreferenceWithVoucher>.Mapper, _selectedPaymentMethod, _selectedUser, _SelectedCustomer, DateFrom, DateTo, DatePaymentFrom, DatePaymentTo, isPending);

            return data.ToList();
        }

        public IEnumerable<PaymentVoucherDetail> GetPaymentVoucherDetail(int id)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentVoucherDetail_GetList", GenericMapper<PaymentVoucherDetail>.Mapper, id);

            return data.ToList();
        }

        public PaymentVoucher GetPaymentVoucher(int id)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentVoucher_Get", GenericMapper<PaymentVoucher>.Mapper, id);

            return GetFirstElement(data);
        }

        public IEnumerable<PaymentVoucherInstallment> GetPaymentVoucherInstallment(int id)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentVoucherInstallment_GetList", GenericMapper<PaymentVoucherInstallment>.Mapper, id);

            return data.ToList();
        }

        public IEnumerable<PaymentVoucherSummary> GetPaymentVoucherSummary(string _selectedPaymentMethod, string _selectedUser, string _SelectedCustomer, string DateFrom, string DateTo, string isRegister)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaymentVoucherSummary_GetList", GenericMapper<PaymentVoucherSummary>.Mapper, _selectedPaymentMethod, _selectedUser, _SelectedCustomer, DateFrom, DateTo, isRegister);

            return data.ToList();
        }

        public IEnumerable<AssignPaymentPreferenceSummary> GetAssignPaymentPreferenceSummaryByCuota(int InstallmentId)
        {
            var data = Db.ExecuteSprocAccessor("pub_AssignPaymentPreferenceSummaryByCuota_GetList", GenericMapper<AssignPaymentPreferenceSummary>.Mapper, InstallmentId);

            return data.ToList();
        }

        public IEnumerable<AssignState> GetAssignStates()
        {
            var data = Db.ExecuteSprocAccessor("pub_AssignState_GetList", GenericMapper<AssignState>.Mapper);

            return data.ToList();
        }

        public void Delete(int paymentId)
        {
            var command = Db.GetStoredProcCommand("pub_Payment_Delete");
            Db.AddInParameter(command, "@PaymentId", DbType.Int32, paymentId);
            Db.ExecuteScalar(command);
        }

        public void DeletePaymentAllocation(int paymentAllocationId)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentAllocation_Delete");
            Db.AddInParameter(command, "@PaymentAllocationId", DbType.Int32, paymentAllocationId);
            Db.ExecuteScalar(command);
        }


        public int AddPayment(Payment payment, int user)
        {
            var command = Db.GetStoredProcCommand("pub_Payment_Insert");

            Db.AddInParameter(command, "@PaymentDate", DbType.DateTime, payment.PaymentDate);
            Db.AddInParameter(command, "@type", DbType.String, payment.type);
            Db.AddInParameter(command, "@PaymentMethod", DbType.String, payment.PaymentMethod);
            Db.AddInParameter(command, "@Observations", DbType.String, payment.Observations);
            Db.AddInParameter(command, "@ReceipNumber", DbType.String, payment.ReceipNumber);
            Db.AddInParameter(command, "@MPSellerAccountId", DbType.String, payment.MPSellerAccountId);
            Db.AddInParameter(command, "@UserId", DbType.Int32, user);



            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void Update(Payment payment)
        {
            var command = Db.GetStoredProcCommand("pub_Payment_Update");

            Db.AddInParameter(command, "@PaymentId", DbType.Int32, payment.PaymentId);
            Db.AddInParameter(command, "@PaymentDate", DbType.DateTime, payment.PaymentDate);
            Db.AddInParameter(command, "@type", DbType.String, payment.type);
            Db.AddInParameter(command, "@PaymentMethod", DbType.String, payment.PaymentMethod);
            Db.AddInParameter(command, "@Observations", DbType.String, payment.Observations);
            Db.AddInParameter(command, "@ReceipNumber", DbType.String, payment.ReceipNumber);

            Db.ExecuteScalar(command);
        }

        public int AddAssignPaymentPreference(AssignPaymentPreference assignPaymentPreference)
        {
            var command = Db.GetStoredProcCommand("pub_AssignPaymentPreference_Insert");

            Db.AddInParameter(command, "@AssignPaymentMethodDate", DbType.DateTime, assignPaymentPreference.AssignPaymentMethodDate);
            Db.AddInParameter(command, "@AssignStateId", DbType.Int32, assignPaymentPreference.AssignStateId);
            Db.AddInParameter(command, "@Channel", DbType.String, assignPaymentPreference.Channel);
            Db.AddInParameter(command, "@Follow", DbType.String, assignPaymentPreference.Follow);
            Db.AddInParameter(command, "@FollowDate", DbType.DateTime, assignPaymentPreference.FollowDate);
            Db.AddInParameter(command, "@InstallmentId", DbType.Int32, assignPaymentPreference.InstallmentId);
            Db.AddInParameter(command, "@Observations", DbType.String, assignPaymentPreference.Observations);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, assignPaymentPreference.PaymentMethodId);
            Db.AddInParameter(command, "@PaymentPreferenceId", DbType.Int32, assignPaymentPreference.PaymentPreferenceId);
            Db.AddInParameter(command, "@UserId", DbType.Int32, assignPaymentPreference.UserId);
            Db.AddInParameter(command, "@Warned", DbType.Boolean, false);
            Db.AddInParameter(command, "@WarnedCommentary", DbType.String, assignPaymentPreference.WarnedCommentary);
            Db.AddInParameter(command, "@WarnedDate", DbType.DateTime, assignPaymentPreference.WarnedDate);
            Db.AddInParameter(command, "@PaymentDate", DbType.DateTime, assignPaymentPreference.PaymentDate);
            Db.AddInParameter(command, "@Amount", DbType.Double, assignPaymentPreference.Amount);


            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdateAssignPaymentPreference(AssignPaymentPreference assignPaymentPreference)
        {
            var command = Db.GetStoredProcCommand("pub_AssignPaymentPreference_Update");

            Db.AddInParameter(command, "@AssignPaymentPreferenceId", DbType.Int32, assignPaymentPreference.AssignPaymentPreferenceId);
            Db.AddInParameter(command, "@AssignPaymentMethodDate", DbType.DateTime, assignPaymentPreference.AssignPaymentMethodDate);
            Db.AddInParameter(command, "@AssignStateId", DbType.Int32, assignPaymentPreference.AssignStateId);
            Db.AddInParameter(command, "@Channel", DbType.String, assignPaymentPreference.Channel);
            Db.AddInParameter(command, "@Follow", DbType.String, assignPaymentPreference.Follow);
            Db.AddInParameter(command, "@FollowDate", DbType.DateTime, assignPaymentPreference.FollowDate);
            Db.AddInParameter(command, "@InstallmentId", DbType.Int32, assignPaymentPreference.InstallmentId);
            Db.AddInParameter(command, "@Observations", DbType.String, assignPaymentPreference.Observations);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, assignPaymentPreference.PaymentMethodId);
            Db.AddInParameter(command, "@PaymentPreferenceId", DbType.Int32, assignPaymentPreference.PaymentPreferenceId);
            Db.AddInParameter(command, "@UserId", DbType.Int32, assignPaymentPreference.UserId);
            Db.AddInParameter(command, "@Warned", DbType.Boolean, assignPaymentPreference.Warned);
            Db.AddInParameter(command, "@WarnedCommentary", DbType.String, assignPaymentPreference.WarnedCommentary);
            Db.AddInParameter(command, "@WarnedDate", DbType.DateTime, assignPaymentPreference.WarnedDate);
            Db.AddInParameter(command, "@PaymentDate", DbType.DateTime, assignPaymentPreference.PaymentDate);
            Db.AddInParameter(command, "@Amount", DbType.Double, assignPaymentPreference.Amount);

            Db.ExecuteScalar(command);
        }

        public void DeleteAssignPaymentPreference(int AssignPaymentPreferenceId)
        {
            var command = Db.GetStoredProcCommand("pub_AssignPaymentPreference_Delete");
            Db.AddInParameter(command, "@AssignPaymentPreferenceId", DbType.Int32, AssignPaymentPreferenceId);
            Db.ExecuteScalar(command);
        }


        public int AddPaymentVoucher(PaymentVoucher voucher)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentVoucher_Insert");

            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, voucher.PaymentMethodId);
            Db.AddInParameter(command, "@IsConfirmed", DbType.Boolean, voucher.IsConfirmed);
            Db.AddInParameter(command, "@Amount", DbType.Double, voucher.Amount);
            Db.AddInParameter(command, "@Code", DbType.String, voucher.Code);
            Db.AddInParameter(command, "@Commentary", DbType.String, voucher.Commentary);
            Db.AddInParameter(command, "@UserId", DbType.Int32, voucher.UserId);
            Db.AddInParameter(command, "@VoucherDate", DbType.DateTime, voucher.VoucherDate);         

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdatePaymentVoucher(PaymentVoucher paymentVoucher)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentVoucher_Update");

            Db.AddInParameter(command, "@PaymentVoucherId", DbType.Int32, paymentVoucher.PaymentVoucherId);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, paymentVoucher.PaymentMethodId);
            Db.AddInParameter(command, "@IsConfirmed", DbType.Boolean, paymentVoucher.IsConfirmed);
            Db.AddInParameter(command, "@Amount", DbType.Double, paymentVoucher.Amount);
            Db.AddInParameter(command, "@Code", DbType.String, paymentVoucher.Code);
            Db.AddInParameter(command, "@Commentary", DbType.String, paymentVoucher.Commentary);
            Db.AddInParameter(command, "@UserId", DbType.Int32, paymentVoucher.UserId);
            Db.AddInParameter(command, "@ConfirmationUserId", DbType.Int32, paymentVoucher.ConfirmationUserId);
            Db.AddInParameter(command, "@VoucherDate", DbType.DateTime, paymentVoucher.VoucherDate);
            Db.AddInParameter(command, "@ConfirmationDate", DbType.DateTime, paymentVoucher.ConfirmationDate);

            Db.ExecuteScalar(command);
        }

        public int AddPaymentVoucherInstallment(PaymentVoucherInstallment voucherInstallment)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentVoucherInstallment_Insert");

            Db.AddInParameter(command, "@AssignPaymentPreferenceId", DbType.Int32, voucherInstallment.AssignPaymentPreferenceId);
            Db.AddInParameter(command, "@InstallmentId", DbType.Int32, voucherInstallment.InstallmentId);
            Db.AddInParameter(command, "@Amount", DbType.Double, voucherInstallment.Amount);
            Db.AddInParameter(command, "@IsSamePaymentMethod", DbType.Boolean, voucherInstallment.IsSamePaymentMethod);
            Db.AddInParameter(command, "@PaymentVoucherId", DbType.Int32, voucherInstallment.PaymentVoucherId);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int AddPaymentVoucherPayment(PaymentVoucherPayment voucherPayment)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentVoucherPayment_Insert");

            Db.AddInParameter(command, "@PaymentVoucherInstallmentId", DbType.Int32, voucherPayment.PaymentVoucherInstallmentId);
            Db.AddInParameter(command, "@PaymentId", DbType.Int32, voucherPayment.PaymentId);


            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void AddPaymentPaymentMethod(PaymentPaymentMethod PaymentPaymentMethod)
        {
            var command = Db.GetStoredProcCommand("pub_PaymentPaymentMethod_Insert");

            Db.AddInParameter(command, "@PaymentId", DbType.Int32, PaymentPaymentMethod.PaymentId);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, PaymentPaymentMethod.PaymentMethodId);
            Db.AddInParameter(command, "@Amount", DbType.Double, PaymentPaymentMethod.Amount);
            Db.AddInParameter(command, "@Saving", DbType.Int32, PaymentPaymentMethod.Saving);
            Db.AddInParameter(command, "@Cycle", DbType.Double, PaymentPaymentMethod.Cycle);

            Db.ExecuteScalar(command);
        }

    }
}
