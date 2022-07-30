using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared.Mappers;
using Multigroup.Repositories.Shared.Resources;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;


namespace Multigroup.Repositories.Shared
{
    public class SpendingRepository : BaseRepository
    {
        public Spending GetSpendingById(int spendingId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Spending_Get", SpendingMapper.Mapper, spendingId);

            return GetFirstElement(data);
        }

        public IEnumerable<SpendingSummary> GetSpendingsSummary(string SelectedProvider, string SelectedUser, string DateFromExecution, string DateToExecution, string DateFromExpiration, string DateToExpiration, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance, string PositiveAll, string Provider)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingSummary_GetList", SpendingSummaryMapper.Mapper, SelectedProvider, SelectedUser, DateFromExecution, DateToExecution, DateFromExpiration, DateToExpiration, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance, PositiveAll, Provider);

            return data.ToList();
        }

        public IEnumerable<SpendingSummary> GetSpendingsSallarySummary(string SelectedProvider, string SelectedUser, string DateFromExecution, string DateToExecution, string DateFromExpiration, string DateToExpiration, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance, string PositiveAll, string Provider)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingSallarySummary_GetList", SpendingSummaryMapper.Mapper, SelectedProvider, SelectedUser, DateFromExecution, DateToExecution, DateFromExpiration, DateToExpiration, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance, PositiveAll, Provider);

            return data.ToList();
        }

        public IEnumerable<SpendingDetailSummary> GetSpendingsDetailSummary(string SelectedProvider, string SelectedUser, string DateFromExecution, string DateToExecution, string DateFromExpiration, string DateToExpiration, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance, string PositiveAll, string Provider)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingDetailSummary_GetList", SpendingDetailSummaryMapper.Mapper, SelectedProvider, SelectedUser, DateFromExecution, DateToExecution, DateFromExpiration, DateToExpiration, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance, PositiveAll, Provider);

            return data.ToList();
        }

        public IEnumerable<Spending> GetSpendings()
        {
            var data = Db.ExecuteSprocAccessor("pub_Spending_GetList", SpendingMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<Spending> GetSpendingsByProvider(int providerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_GetSpendingByProvider_GetList", SpendingMapper.Mapper, providerId);

            return data.ToList();
        }

        public IEnumerable<Spending> GetSpendingsByEmployee(int providerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_GetSpendingByEmployee_GetList", SpendingMapper.Mapper, providerId);

            return data.ToList();
        }

        public SpendingDetail GetSpendingDetail(int spendingDetailId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingDetail_Get", SpendingDetailMapper.Mapper, spendingDetailId);

            return GetFirstElement(data);
        }


        public IEnumerable<SpendingDetail> GetSpendingDetails(int spendingId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingDetail_GetList", SpendingDetailMapper.Mapper, spendingId);

            return data.ToList();
        }

        public SpendingPaymentMethod GetSpendingPaymentMethod(int spendingPaymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentMethod_Get", SpendingPaymentMethodMapper.Mapper, spendingPaymentMethodId);

            return GetFirstElement(data);
        }


        public IEnumerable<SpendingPaymentMethod> GetSpendingPaymentMethods(int spendingId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentMethod_GetList", SpendingPaymentMethodMapper.Mapper, spendingId);

            return data.ToList();
        }

        public IEnumerable<SpendingPaymentMethod> GetSpendingsPaymentMethodsByCashCycle(int cashCycleId, int cashierId, int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentMethodByCashCycle_GetList", SpendingPaymentMethodMapper.Mapper, cashCycleId, cashierId, paymentMethodId);

            return data.ToList();
        }

        public IEnumerable<SpendingPaymentMethod> GetSpendingsPaymentMethodsBalance(int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentMethodBalance_GetList", SpendingPaymentMethodMapper.Mapper, paymentMethodId);

            return data.ToList();
        }

        public IEnumerable<SpendingPaymentMethod> GetSpendingPaymentMethodsByPM(int paymentMethodId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SpendingPaymentMethodByPM_GetList", SpendingPaymentMethodMapper.Mapper, paymentMethodId);

            return data.ToList();
        }

        public Invoice GetInvoice(int invoiceId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Invoice_Get", InvoiceMapper.Mapper, invoiceId);

            return GetFirstElement(data);
        }

        public Invoice GetInvoiceBySpendingId(int spendingId)
        {
            var data = Db.ExecuteSprocAccessor("pub_InvoiceBySpendingId_Get", InvoiceMapper.Mapper, spendingId);

            return GetFirstElement(data);
        }

        public Invoice GetInvoiceByFilter(string CUIT, int PurchaseDocumentId, string Letter, int Prefix, int Number, int spendingId)
        {
            var data = Db.ExecuteSprocAccessor("pub_InvoiceByFilter_Get", InvoiceMapper.Mapper, CUIT, PurchaseDocumentId, Letter, Prefix, Number, spendingId);

            return GetFirstElement(data);
        }

        public IEnumerable<Invoice> GetInvoices()
        {
            var data = Db.ExecuteSprocAccessor("pub_Invoice_GetList", InvoiceMapper.Mapper);

            return data.ToList();
        }


        public void DeleteSpending(int spendingId)
        {
            var command = Db.GetStoredProcCommand("pub_Spending_Delete");
            Db.AddInParameter(command, "@SpendingId", DbType.Int32, spendingId);
            Db.ExecuteScalar(command);
        }

        public void DeleteSpendingDetail(int spendingDetailId)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingDetail_Delete");
            Db.AddInParameter(command, "@SpendingDetailId", DbType.Int32, spendingDetailId);
            Db.ExecuteScalar(command);
        }

        public void DeleteSpendingDetailBySpendingId(int spendingId)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingDetailBySpendingId_Delete");
            Db.AddInParameter(command, "@spendingId", DbType.Int32, spendingId);
            Db.ExecuteScalar(command);
        }

        public void DeleteSpendingPMBySpendingId(int spendingId)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPMBySpendingId_Delete");
            Db.AddInParameter(command, "@spendingId", DbType.Int32, spendingId);
            Db.ExecuteScalar(command);
        }

        public void DeleteSpendingPaymentMethod(int spendingPaymentMethodId)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPaymentMethod_Delete");
            Db.AddInParameter(command, "@SpendingPaymentMethodId", DbType.Int32, spendingPaymentMethodId);
            Db.ExecuteScalar(command);
        }

        public int AddSpending(Spending Spending)
        {
            var command = Db.GetStoredProcCommand("pub_Spending_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, Spending.Amount);
            Db.AddInParameter(command, "@Balance", DbType.Double, Spending.Balance);
            Db.AddInParameter(command, "@Receipt", DbType.String, Spending.Receipt);
            Db.AddInParameter(command, "@Description", DbType.String, Spending.Description);
            Db.AddInParameter(command, "@Commentary", DbType.String, Spending.Commentary);
            Db.AddInParameter(command, "@ExecutionDate", DbType.DateTime, Spending.ExecutionDate);
            Db.AddInParameter(command, "@ProveedorId", DbType.Int32, Spending.ProveedorId);
            Db.AddInParameter(command, "@ExpirationDate", DbType.DateTime, Spending.ExpirationDate);
            Db.AddInParameter(command, "@SpendingTypeId", DbType.Int32, Spending.SpendingTypeId);
            Db.AddInParameter(command, "@UserId", DbType.Int32, Spending.UserId);
            Db.AddInParameter(command, "@Period", DbType.String, Spending.Period);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }


        public void AddSpendingDetail(SpendingDetail SpendingDetail)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingDetail_Insert");

            Db.AddInParameter(command, "@ArticleId", DbType.Int32, SpendingDetail.ArticleId);
            Db.AddInParameter(command, "@Price", DbType.Double, SpendingDetail.Price);
            Db.AddInParameter(command, "@Total", DbType.Double, SpendingDetail.Total);
            Db.AddInParameter(command, "@Quantity", DbType.Int32, SpendingDetail.Quantity);
            Db.AddInParameter(command, "@SpendingId", DbType.Int32, SpendingDetail.SpendingId);
            Db.AddInParameter(command, "@PurchaseRequestId", DbType.Int32, SpendingDetail.PurchaseRequestId);
            Db.AddInParameter(command, "@ScheduledExpenseId", DbType.Int32, SpendingDetail.ScheduledExpenseId);
            Db.AddInParameter(command, "@ExpenseAuthorizationId", DbType.Int32, SpendingDetail.ExpenseAuthorizationId);
            Db.AddInParameter(command, "@Description", DbType.String, SpendingDetail.Description);
            Db.AddInParameter(command, "@SalaryImputationId", DbType.String, SpendingDetail.SalaryImputationId);

            Db.ExecuteScalar(command);
        }

        public void AddSpendingPaymentMethod(SpendingPaymentMethod SpendingPaymentMethod)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPaymentMethod_Insert");

            Db.AddInParameter(command, "@SpendingId", DbType.Int32, SpendingPaymentMethod.SpendingId);
            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, SpendingPaymentMethod.PaymentMethodId);
            Db.AddInParameter(command, "@Amount", DbType.Double, SpendingPaymentMethod.Amount);
            Db.AddInParameter(command, "@Saving", DbType.Int32, SpendingPaymentMethod.Saving);
            Db.AddInParameter(command, "@Cycle", DbType.Double, SpendingPaymentMethod.Cycle);

            Db.ExecuteScalar(command);
        }

        public void UpdateSpending(Spending Spending)
        {
            var command = Db.GetStoredProcCommand("pub_Spending_Update");

            Db.AddInParameter(command, "@SpendingId", DbType.Int32, Spending.SpendingId);
            Db.AddInParameter(command, "@Amount", DbType.Double, Spending.Amount);
            Db.AddInParameter(command, "@Balance", DbType.Double, Spending.Balance);
            Db.AddInParameter(command, "@Receipt", DbType.String, Spending.Receipt);
            Db.AddInParameter(command, "@Commentary", DbType.String, Spending.Commentary);
            Db.AddInParameter(command, "@ExecutionDate", DbType.DateTime, Spending.ExecutionDate);
            Db.AddInParameter(command, "@ProveedorId", DbType.Int32, Spending.ProveedorId);
            Db.AddInParameter(command, "@ExpirationDate", DbType.DateTime, Spending.ExpirationDate);
            Db.AddInParameter(command, "@Description", DbType.String, Spending.Description);
            Db.AddInParameter(command, "@SpendingTypeId", DbType.Int32, (int)SpendingType.Spending);
            Db.AddInParameter(command, "@UserId", DbType.Int32, Spending.UserId);
            Db.AddInParameter(command, "@Period", DbType.String, Spending.Period);
            Db.ExecuteScalar(command);
        }

        public void UpdateSpendingShort(Spending Spending)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingShort_Update");

            Db.AddInParameter(command, "@SpendingId", DbType.Int32, Spending.SpendingId);

            Db.AddInParameter(command, "@Commentary", DbType.String, Spending.Commentary);
            Db.AddInParameter(command, "@ExpirationDate", DbType.DateTime, Spending.ExpirationDate);
            Db.AddInParameter(command, "@Description", DbType.String, Spending.Description);
            Db.ExecuteScalar(command);
        }


        public void UpdateSpendingDetail(SpendingDetail SpendingDetail)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingDetail_Update");

            Db.AddInParameter(command, "@SpendingDetailId", DbType.Int32, SpendingDetail.SpendingDetailId);
            Db.AddInParameter(command, "@ArticleId", DbType.Int32, SpendingDetail.ArticleId);
            Db.AddInParameter(command, "@Price", DbType.Double, SpendingDetail.Price);
            Db.AddInParameter(command, "@Total", DbType.Double, SpendingDetail.Total);
            Db.AddInParameter(command, "@Quantity", DbType.Int32, SpendingDetail.Quantity);
            Db.AddInParameter(command, "@SpendingId", DbType.Int32, SpendingDetail.SpendingId);
            Db.AddInParameter(command, "@PurchaseRequestId", DbType.Int32, SpendingDetail.PurchaseRequestId);
            Db.AddInParameter(command, "@ScheduledExpenseId", DbType.Int32, SpendingDetail.ScheduledExpenseId);
            Db.AddInParameter(command, "@ExpenseAuthorizationId", DbType.Int32, SpendingDetail.ExpenseAuthorizationId);
            Db.ExecuteScalar(command);
        }

        public void UpdateSpendingPaymentMethod(SpendingPaymentMethod SpendingPaymentMethod)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingPaymentMethod_Update");

            Db.AddInParameter(command, "@SpendingPaymentMethodId", DbType.Int32, SpendingPaymentMethod.SpendingPaymentMethodId);
            Db.AddInParameter(command, "@SpendingId", DbType.Int32, SpendingPaymentMethod.SpendingId);
            Db.AddInParameter(command, "@SpendingPaymentMethodId", DbType.Int32, SpendingPaymentMethod.SpendingPaymentMethodId);
            Db.AddInParameter(command, "@Amount", DbType.Double, SpendingPaymentMethod.Amount);
            Db.AddInParameter(command, "@Saving", DbType.Int32, SpendingPaymentMethod.Saving);
            Db.AddInParameter(command, "@Cycle", DbType.Double, SpendingPaymentMethod.Cycle);
            Db.ExecuteScalar(command);
        }

        public int AddInvoice(Invoice invoice)
        {
            var command = Db.GetStoredProcCommand("pub_Invoice_Insert");

            Db.AddInParameter(command, "@SpendingId", DbType.Int32, invoice.SpendingId);
            Db.AddInParameter(command, "@CUIT", DbType.String, invoice.CUIT);
            Db.AddInParameter(command, "@BusinessName", DbType.String, invoice.BusinessName);
            Db.AddInParameter(command, "@IvaPositionId", DbType.Int32, invoice.IvaPositionId);
            Db.AddInParameter(command, "@PurchaseDocumentId", DbType.Int32, invoice.PurchaseDocumentId);
            Db.AddInParameter(command, "@PurchasePerceptionId", DbType.Int32, invoice.PurchasePerceptionId);
            Db.AddInParameter(command, "@PerceptionAmount", DbType.Double, invoice.PerceptionAmount);
            Db.AddInParameter(command, "@Letter", DbType.String, invoice.Letter);
            Db.AddInParameter(command, "@Prefix", DbType.Int32, invoice.Prefix);
            Db.AddInParameter(command, "@Number", DbType.Int32, invoice.Number);
            Db.AddInParameter(command, "@ImputDate", DbType.DateTime, invoice.ImputDate);
            Db.AddInParameter(command, "@Net", DbType.Double, invoice.Net);
            Db.AddInParameter(command, "@Exempt", DbType.Double, invoice.Exempt);
            Db.AddInParameter(command, "@IVA21", DbType.Double, invoice.IVA21);
            Db.AddInParameter(command, "@IVA105", DbType.Double, invoice.IVA105);
            Db.AddInParameter(command, "@IVA22", DbType.Double, invoice.IVA22);
            Db.AddInParameter(command, "@IVA5", DbType.Double, invoice.IVA5);
            Db.AddInParameter(command, "@IVA25", DbType.Double, invoice.IVA25);
            Db.AddInParameter(command, "@OtherTaxes", DbType.Double, invoice.OtherTaxes);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdateInvoice(Invoice invoice)
        {
            var command = Db.GetStoredProcCommand("pub_Invoice_Update");

            Db.AddInParameter(command, "@InvoiceId", DbType.Int32, invoice.InvoiceId);
            Db.AddInParameter(command, "@SpendingId", DbType.Int32, invoice.SpendingId);
            Db.AddInParameter(command, "@CUIT", DbType.String, invoice.CUIT);
            Db.AddInParameter(command, "@BusinessName", DbType.String, invoice.BusinessName);
            Db.AddInParameter(command, "@IvaPositionId", DbType.Int32, invoice.IvaPositionId);
            Db.AddInParameter(command, "@PurchaseDocumentId", DbType.Int32, invoice.PurchaseDocumentId);
            Db.AddInParameter(command, "@PurchasePerceptionId", DbType.Int32, invoice.PurchasePerceptionId);
            Db.AddInParameter(command, "@PerceptionAmount", DbType.Double, invoice.PerceptionAmount);
            Db.AddInParameter(command, "@Letter", DbType.String, invoice.Letter);
            Db.AddInParameter(command, "@Prefix", DbType.Int32, invoice.Prefix);
            Db.AddInParameter(command, "@Number", DbType.Int32, invoice.Number);
            Db.AddInParameter(command, "@ImputDate", DbType.DateTime, invoice.ImputDate);
            Db.AddInParameter(command, "@Net", DbType.Double, invoice.Net);
            Db.AddInParameter(command, "@Exempt", DbType.Double, invoice.Exempt);
            Db.AddInParameter(command, "@IVA21", DbType.Double, invoice.IVA21);
            Db.AddInParameter(command, "@IVA105", DbType.Double, invoice.IVA105);
            Db.AddInParameter(command, "@IVA22", DbType.Double, invoice.IVA22);
            Db.AddInParameter(command, "@IVA5", DbType.Double, invoice.IVA5);
            Db.AddInParameter(command, "@IVA25", DbType.Double, invoice.IVA25);
            Db.AddInParameter(command, "@OtherTaxes", DbType.Double, invoice.OtherTaxes);
            Db.ExecuteScalar(command);
        }

        public void InsertDocument(string path, HttpPostedFileBase file, int spendingId)
        {
            try
            {
                var command = Db.GetStoredProcCommand("pub_SpendingFile_Insert");

                Db.AddInParameter(command, "@SpendingId", DbType.Int32, spendingId);
                Db.AddInParameter(command, "@Path", DbType.String, path);
                Db.AddInParameter(command, "@Name", DbType.String, file.FileName);
                Db.AddInParameter(command, "@ContentLength", DbType.Int32, file.ContentLength);

                Db.ExecuteScalar(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<SpendingFile> GetSpendingDocuments(int spendingId)
        {
            return Db.ExecuteSprocAccessor("pub_SpendingFile_GetList", SpendingFileMapper.Mapper, spendingId).ToList();
        }

        public void DeleteSpendingFile(int spendingId)
        {
            var command = Db.GetStoredProcCommand("pub_SpendingFile_Delete");
            Db.AddInParameter(command, "@SpendingId", DbType.Int32, spendingId);
            Db.ExecuteScalar(command);
        }

        public void DeleteInvoiceBySpendingId(int spendingId)
        {
            var command = Db.GetStoredProcCommand("pub_Invoice_Delete");
            Db.AddInParameter(command, "@spendingId", DbType.Int32, spendingId);
            Db.ExecuteScalar(command);
        }
    }
}
