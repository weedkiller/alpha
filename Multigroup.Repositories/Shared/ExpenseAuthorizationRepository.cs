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
    public class ExpenseAuthorizationRepository : BaseRepository
    {
        public ExpenseAuthorization GetExpenseAuthorizationById(int expenseAuthorizationId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ExpenseAuthorization_Get", ExpenseAuthorizationMapper.Mapper, expenseAuthorizationId);

            return GetFirstElement(data);
        }

        public IEnumerable<ExpenseAuthorizationSummary> GetExpenseAuthorizationsSummary(string SelectedProvider, string SelectedUser, string DateFromCreate, string DateToCreate, string DateFromPurchase, string DateToPurchase, string AmountFrom, string AmountTo, string PurchaseRequestId, string PurchaseOrderTypeId, string SelectedUserBuyer, string Processed)
        {
            var data = Db.ExecuteSprocAccessor("pub_ExpenseAuthorizationSummary_GetList", ExpenseAuthorizationSummaryMapper.Mapper, SelectedProvider,SelectedUser, DateFromCreate, DateToCreate, DateFromPurchase, DateToPurchase, AmountFrom, AmountTo, PurchaseRequestId, PurchaseOrderTypeId, 0, SelectedUserBuyer, Processed);

            return data.ToList();
        }

        public IEnumerable<ExpenseAuthorizationSummary> GetExpenseAuthorizationSummaryManaged(string SelectedUserAuthorized)
        {
            var data = Db.ExecuteSprocAccessor("pub_ExpenseAuthorizationSummaryManaged_GetList", ExpenseAuthorizationSummaryMapper.Mapper, SelectedUserAuthorized);

            return data.ToList();
        }

        public IEnumerable<ExpenseAuthorization> GetExpenseAuthorizations()
        {
            var data = Db.ExecuteSprocAccessor("pub_ExpenseAuthorization_GetList", ExpenseAuthorizationMapper.Mapper);

            return data.ToList();
        }

        public ExpenseAuthorizationDetail GetExpenseAuthorizationDetail(int expenseAuthorizationDetailId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ExpenseAuthorizationDetail_Get", ExpenseAuthorizationDetailMapper.Mapper, expenseAuthorizationDetailId);

            return GetFirstElement(data);
        }


        public IEnumerable<ExpenseAuthorizationDetail> GetExpenseAuthorizationDetails(int expenseAuthorizationId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ExpenseAuthorizationDetail_GetList", ExpenseAuthorizationDetailMapper.Mapper, expenseAuthorizationId);

            return data.ToList();
        }

     

        public void DeleteExpenseAuthorization(int expenseAuthorizationId)
        {
            var command = Db.GetStoredProcCommand("pub_ExpenseAuthorization_Delete");
            Db.AddInParameter(command, "@ExpenseAuthorizationId", DbType.Int32, expenseAuthorizationId);
            Db.ExecuteScalar(command);
        }

        public void DeleteExpenseAuthorizationDetail(int expenseAuthorizationDetailId)
        {
            var command = Db.GetStoredProcCommand("pub_ExpenseAuthorizationDetail_Delete");
            Db.AddInParameter(command, "@ExpenseAuthorizationDetailId", DbType.Int32, expenseAuthorizationDetailId);
            Db.ExecuteScalar(command);
        }

        public void DeleteExpenseAuthorizationDetailByExpenseId(int expenseAuthorizationId)
        {
            var command = Db.GetStoredProcCommand("pub_ExpenseAuthorizationDetailByExpenseId_Delete");
            Db.AddInParameter(command, "@expenseAuthorizationId", DbType.Int32, expenseAuthorizationId);
            Db.ExecuteScalar(command);
        }

        public int AddExpenseAuthorization(ExpenseAuthorization ExpenseAuthorization)
        {
            var command = Db.GetStoredProcCommand("pub_ExpenseAuthorization_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, ExpenseAuthorization.Amount);
            Db.AddInParameter(command, "@Commentary", DbType.String, ExpenseAuthorization.Commentary);
            Db.AddInParameter(command, "@CreateDate", DbType.DateTime, DateTime.Now);
            Db.AddInParameter(command, "@IsAuthorized", DbType.Boolean, 0);
            Db.AddInParameter(command, "@ProveedorId", DbType.Int32, ExpenseAuthorization.ProveedorId);
            Db.AddInParameter(command, "@PurchaseDate", DbType.DateTime, ExpenseAuthorization.PurchaseDate);
            Db.AddInParameter(command, "@PurchaseOrderTypeId", DbType.Int32, 1);
            Db.AddInParameter(command, "@UserBuyerId", DbType.Int32, ExpenseAuthorization.UserBuyerId);
            Db.AddInParameter(command, "@UserId", DbType.Int32, ExpenseAuthorization.UserId);
            Db.AddInParameter(command, "@PurchaseRequestId", DbType.Int32, ExpenseAuthorization.PurchaseRequestId);
            Db.AddInParameter(command, "@ScheduledExpenseId", DbType.Int32, ExpenseAuthorization.ScheduledExpenseId);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }


        public void AddExpenseAuthorizationDetail(ExpenseAuthorizationDetail ExpenseAuthorizationDetail)
        {
            var command = Db.GetStoredProcCommand("pub_ExpenseAuthorizationDetail_Insert");

            Db.AddInParameter(command, "@ArticleId", DbType.Int32, ExpenseAuthorizationDetail.ArticleId);
            Db.AddInParameter(command, "@Price", DbType.Double, ExpenseAuthorizationDetail.Price);
            Db.AddInParameter(command, "@Total", DbType.Double, ExpenseAuthorizationDetail.Total);
            Db.AddInParameter(command, "@Quantity", DbType.Int32, ExpenseAuthorizationDetail.Quantity);
            Db.AddInParameter(command, "@ExpenseAuthorizationId", DbType.Int32, ExpenseAuthorizationDetail.ExpenseAuthorizationId);
            Db.AddInParameter(command, "@Description", DbType.String, ExpenseAuthorizationDetail.Description);

            Db.ExecuteScalar(command);
        }

        public void UpdateExpenseAuthorization(ExpenseAuthorization ExpenseAuthorization)
        {
            var command = Db.GetStoredProcCommand("pub_ExpenseAuthorization_Update");

            Db.AddInParameter(command, "@ExpenseAuthorizationId", DbType.Int32, ExpenseAuthorization.ExpenseAuthorizationId);
            Db.AddInParameter(command, "@Amount", DbType.Double, ExpenseAuthorization.Amount);
            Db.AddInParameter(command, "@Commentary", DbType.String, ExpenseAuthorization.Commentary);
            Db.AddInParameter(command, "@CreateDate", DbType.DateTime, ExpenseAuthorization.CreateDate);
            Db.AddInParameter(command, "@IsAuthorized", DbType.Boolean, 0);
            Db.AddInParameter(command, "@ProveedorId", DbType.Int32, ExpenseAuthorization.ProveedorId);
            Db.AddInParameter(command, "@PurchaseDate", DbType.DateTime, ExpenseAuthorization.PurchaseDate);
            Db.AddInParameter(command, "@PurchaseOrderTypeId", DbType.Int32, 1);
            Db.AddInParameter(command, "@UserBuyerId", DbType.Int32, ExpenseAuthorization.UserBuyerId);
            Db.AddInParameter(command, "@UserId", DbType.Int32, ExpenseAuthorization.UserId);
            Db.AddInParameter(command, "@PurchaseRequestId", DbType.Int32, ExpenseAuthorization.PurchaseRequestId);
            Db.AddInParameter(command, "@ScheduledExpenseId", DbType.Int32, ExpenseAuthorization.ScheduledExpenseId);
            Db.AddInParameter(command, "@Processed", DbType.Boolean, ExpenseAuthorization.Processed);
            Db.AddInParameter(command, "@Managed", DbType.Boolean, ExpenseAuthorization.Managed);
            Db.ExecuteScalar(command);
        }


        public void UpdateExpenseAuthorizationDetail(ExpenseAuthorizationDetail ExpenseAuthorizationDetail)
        {
            var command = Db.GetStoredProcCommand("pub_ExpenseAuthorizationDetail_Update");

            Db.AddInParameter(command, "@ExpenseAuthorizationDetailId", DbType.Int32, ExpenseAuthorizationDetail.ExpenseAuthorizationDetailId);
            Db.AddInParameter(command, "@ArticleId", DbType.Int32, ExpenseAuthorizationDetail.ArticleId);
            Db.AddInParameter(command, "@Price", DbType.Double, ExpenseAuthorizationDetail.Price);
            Db.AddInParameter(command, "@Total", DbType.Double, ExpenseAuthorizationDetail.Total);
            Db.AddInParameter(command, "@Quantity", DbType.Int32, ExpenseAuthorizationDetail.Quantity);
            Db.AddInParameter(command, "@ExpenseAuthorizationId", DbType.Int32, ExpenseAuthorizationDetail.ExpenseAuthorizationId);
            Db.ExecuteScalar(command);
        }
    }
}
