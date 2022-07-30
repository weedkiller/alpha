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
    public class ScheduledExpenseRepository : BaseRepository
    {
        public ScheduledExpense GetScheduledExpenseById(int scheduledExpenseId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ScheduledExpense_Get", ScheduledExpenseMapper.Mapper, scheduledExpenseId);

            return GetFirstElement(data);
        }

        public IEnumerable<ScheduledExpenseSummary> GetScheduledExpensesSummary(string SelectedProvider, string SelectedUser, string DateFromCreate, string DateToCreate, string DateFromPurchase, string DateToPurchase, string AmountFrom, string AmountTo, string PurchaseRequestId, string PurchaseOrderTypeId, string Authorized, string Processed, string AuthorizedUser, string Active)
        {
            var data = Db.ExecuteSprocAccessor("pub_ScheduledExpenseSummary_GetList", ScheduledExpenseSummaryMapper.Mapper, SelectedProvider, SelectedUser, DateFromCreate, DateToCreate, DateFromPurchase, DateToPurchase, AmountFrom, AmountTo, PurchaseRequestId, PurchaseOrderTypeId, Authorized, Processed, AuthorizedUser, Active);

            return data.ToList();
        }

        public IEnumerable<ScheduledExpense> GetScheduledExpenses()
        {
            var data = Db.ExecuteSprocAccessor("pub_ScheduledExpense_GetList", ScheduledExpenseMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<ScheduledExpenseSummary> GetScheduledExpenseSummaryManaged(string SelectedUserAuthorized)
        {
            var data = Db.ExecuteSprocAccessor("pub_ScheduledExpenseSummaryManaged_GetList", ScheduledExpenseSummaryMapper.Mapper, SelectedUserAuthorized);

            return data.ToList();
        }

        public ScheduledExpenseDetail GetScheduledExpenseDetail(int scheduledExpenseDetailId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ScheduledExpenseDetail_Get", ScheduledExpenseDetailMapper.Mapper, scheduledExpenseDetailId);

            return GetFirstElement(data);
        }


        public IEnumerable<ScheduledExpenseDetail> GetScheduledExpenseDetails(int scheduledExpenseId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ScheduledExpenseDetail_GetList", ScheduledExpenseDetailMapper.Mapper, scheduledExpenseId);

            return data.ToList();
        }



        public void DeleteScheduledExpense(int scheduledExpenseId)
        {
            var command = Db.GetStoredProcCommand("pub_ScheduledExpense_Delete");
            Db.AddInParameter(command, "@ScheduledExpenseId", DbType.Int32, scheduledExpenseId);
            Db.ExecuteScalar(command);
        }

        public void DeleteScheduledExpenseDetailByScheduledId(int scheduledExpenseId)
        {
            var command = Db.GetStoredProcCommand("pub_ScheduledExpenseDetailByScheduledId_Delete");
            Db.AddInParameter(command, "@scheduledExpenseId", DbType.Int32, scheduledExpenseId);
            Db.ExecuteScalar(command);
        }

        public void DeleteScheduledExpenseDetail(int scheduledExpenseDetailId)
        {
            var command = Db.GetStoredProcCommand("pub_ScheduledExpenseDetail_Delete");
            Db.AddInParameter(command, "@ScheduledExpenseDetailId", DbType.Int32, scheduledExpenseDetailId);
            Db.ExecuteScalar(command);
        }

        public int AddScheduledExpense(ScheduledExpense ScheduledExpense)
        {
            var command = Db.GetStoredProcCommand("pub_ScheduledExpense_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, ScheduledExpense.Amount);
            Db.AddInParameter(command, "@Commentary", DbType.String, ScheduledExpense.Commentary);
            Db.AddInParameter(command, "@CreateDate", DbType.DateTime, DateTime.Now);
            Db.AddInParameter(command, "@IsAuthorized", DbType.Boolean, ScheduledExpense.IsAuthorized);
            Db.AddInParameter(command, "@ProveedorId", DbType.Int32, ScheduledExpense.ProveedorId);
            Db.AddInParameter(command, "@PurchaseDate", DbType.DateTime, ScheduledExpense.PurchaseDate);
            Db.AddInParameter(command, "@PurchaseOrderTypeId", DbType.Int32, 1);
            Db.AddInParameter(command, "@UserAuthorizedId", DbType.Int32, ScheduledExpense.UserAuthorizedId);
            Db.AddInParameter(command, "@UserId", DbType.Int32, ScheduledExpense.UserId);
            Db.AddInParameter(command, "@PurchaseRequestId", DbType.Int32, ScheduledExpense.PurchaseRequestId);
            Db.AddInParameter(command, "@Processed", DbType.Boolean, false);
            Db.AddInParameter(command, "@Managed", DbType.Boolean, false);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void AddScheduledExpenseDetail(ScheduledExpenseDetail ScheduledExpenseDetail)
        {
            var command = Db.GetStoredProcCommand("pub_ScheduledExpenseDetail_Insert");

            Db.AddInParameter(command, "@ArticleId", DbType.Int32, ScheduledExpenseDetail.ArticleId);
            Db.AddInParameter(command, "@Price", DbType.Double, ScheduledExpenseDetail.Price);
            Db.AddInParameter(command, "@Total", DbType.Double, ScheduledExpenseDetail.Total);
            Db.AddInParameter(command, "@Quantity", DbType.Int32, ScheduledExpenseDetail.Quantity);
            Db.AddInParameter(command, "@ScheduledExpenseId", DbType.Int32, ScheduledExpenseDetail.ScheduledExpenseId);
            Db.AddInParameter(command, "@Description", DbType.String, ScheduledExpenseDetail.Description);


            Db.ExecuteScalar(command);
        }

        public void UpdateScheduledExpense(ScheduledExpense ScheduledExpense)
        {
            var command = Db.GetStoredProcCommand("pub_ScheduledExpense_Update");

            Db.AddInParameter(command, "@ScheduledExpenseId", DbType.Int32, ScheduledExpense.ScheduledExpenseId);
            Db.AddInParameter(command, "@Amount", DbType.Double, ScheduledExpense.Amount);
            Db.AddInParameter(command, "@Commentary", DbType.String, ScheduledExpense.Commentary);
            Db.AddInParameter(command, "@CreateDate", DbType.DateTime, ScheduledExpense.CreateDate);
            Db.AddInParameter(command, "@IsAuthorized", DbType.Boolean, ScheduledExpense.IsAuthorized);
            Db.AddInParameter(command, "@ProveedorId", DbType.Int32, ScheduledExpense.ProveedorId);
            Db.AddInParameter(command, "@PurchaseDate", DbType.DateTime, ScheduledExpense.PurchaseDate);
            Db.AddInParameter(command, "@PurchaseOrderTypeId", DbType.Int32, 1);
            Db.AddInParameter(command, "@UserAuthorizedId", DbType.Int32, ScheduledExpense.UserAuthorizedId);
            Db.AddInParameter(command, "@UserId", DbType.Int32, ScheduledExpense.UserId);
            Db.AddInParameter(command, "@PurchaseRequestId", DbType.Int32, ScheduledExpense.PurchaseRequestId);
            Db.AddInParameter(command, "@Processed", DbType.Boolean, ScheduledExpense.Processed);
            Db.AddInParameter(command, "@Managed", DbType.Boolean, ScheduledExpense.Managed);
            Db.ExecuteScalar(command);
        }


        public void UpdateScheduledExpenseDetail(ScheduledExpenseDetail ScheduledExpenseDetail)
        {
            var command = Db.GetStoredProcCommand("pub_ScheduledExpenseDetail_Update");

            Db.AddInParameter(command, "@ScheduledExpenseDetailId", DbType.Int32, ScheduledExpenseDetail.ScheduledExpenseDetailId);
            Db.AddInParameter(command, "@ArticleId", DbType.Int32, ScheduledExpenseDetail.ArticleId);
            Db.AddInParameter(command, "@Price", DbType.Double, ScheduledExpenseDetail.Price);
            Db.AddInParameter(command, "@Total", DbType.Double, ScheduledExpenseDetail.Total);
            Db.AddInParameter(command, "@Quantity", DbType.Int32, ScheduledExpenseDetail.Quantity);
            Db.AddInParameter(command, "@ScheduledExpenseId", DbType.Int32, ScheduledExpenseDetail.ScheduledExpenseId);
            Db.ExecuteScalar(command);
        }

        public void UpdateScheduledExpenseProcessed(int scheduledExpenseId, bool processed)
        {
            var command = Db.GetStoredProcCommand("pub_ScheduledExpenseProcessed_Update");

            Db.AddInParameter(command, "@ScheduledExpenseId", DbType.Int32, scheduledExpenseId);
            Db.AddInParameter(command, "@Processed", DbType.Boolean, processed);
            Db.ExecuteScalar(command);
        }

        public void UpdateScheduledExpenseManaged(int scheduledExpenseId, bool managed)
        {
            var command = Db.GetStoredProcCommand("pub_ScheduledExpenseManaged_Update");

            Db.AddInParameter(command, "@ScheduledExpenseId", DbType.Int32, scheduledExpenseId);
            Db.AddInParameter(command, "@Managed", DbType.Boolean, managed);
            Db.ExecuteScalar(command);
        }
    }
}
