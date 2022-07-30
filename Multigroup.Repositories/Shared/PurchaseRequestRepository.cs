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
    public class PurchaseRequestRepository : BaseRepository
    {
        public PurchaseRequest GetPurchaseRequestById(int purchaseRequestId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PurchaseRequest_Get", PurchaseRequestMapper.Mapper, purchaseRequestId);

            return GetFirstElement(data);
        }

        public IEnumerable<PurchaseRequestSummary> GetPurchaseRequestsSummary(string SelectedUrgency, string SelectedProvider, string SelectedUser, string SelectedUserAuthorized, string DateFromCreate, string DateToCreate, string DateFromNeed, string DateToNeed, string AmountFrom, string AmountTo, string Active, string Processed)
        {
            var data = Db.ExecuteSprocAccessor("pub_PurchaseRequestSummary_GetList", PurchaseRequestSummaryMapper.Mapper, SelectedUrgency, SelectedProvider, SelectedUser, SelectedUserAuthorized, DateFromCreate, DateToCreate, DateFromNeed, DateToNeed, AmountFrom, AmountTo, Active, Processed);

            return data.ToList();
        }

        public IEnumerable<PurchaseRequestSummary> GetPurchaseRequestsSummaryManaged(string SelectedUserAuthorized)
        {
            var data = Db.ExecuteSprocAccessor("pub_PurchaseRequestSummaryManaged_GetList", PurchaseRequestSummaryMapper.Mapper,SelectedUserAuthorized);

            return data.ToList();
        }

        public IEnumerable<PurchaseRequest> GetPurchaseRequests()
        {
            var data = Db.ExecuteSprocAccessor("pub_PurchaseRequest_GetList", PurchaseRequestMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<Urgency> GetUrgencys()
        {
            var data = Db.ExecuteSprocAccessor("pub_Urgency_GetList", UrgencyMapper.Mapper);

            return data.ToList();
        }

        public PurchaseRequestDetail GetPurchaseRequestDetail(int purchaseRequestDetailId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PurchaseRequestDetail_Get", PurchaseRequestDetailMapper.Mapper, purchaseRequestDetailId);

            return GetFirstElement(data);
        }


        public IEnumerable<PurchaseRequestDetail> GetPurchaseRequestDetails(int purchaseRequestId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PurchaseRequestDetail_GetList", PurchaseRequestDetailMapper.Mapper, purchaseRequestId);

            return data.ToList();
        }



        public void DeletePurchaseRequest(int purchaseRequestId)
        {
            var command = Db.GetStoredProcCommand("pub_PurchaseRequest_Delete");
            Db.AddInParameter(command, "@PurchaseRequestId", DbType.Int32, purchaseRequestId);
            Db.ExecuteScalar(command);
        }

        public void DeletePurchaseRequestDetail(int purchaseRequestDetailId)
        {
            var command = Db.GetStoredProcCommand("pub_PurchaseRequestDetail_Delete");
            Db.AddInParameter(command, "@PurchaseRequestDetailId", DbType.Int32, purchaseRequestDetailId);
            Db.ExecuteScalar(command);
        }

        public void DeletePurchaseRequestDetailByPurchaseId(int purchaseRequestId)
        {
            var command = Db.GetStoredProcCommand("pub_PurchaseRequestDetailByPurchaseId_Delete");
            Db.AddInParameter(command, "@purchaseRequestId", DbType.Int32, purchaseRequestId);
            Db.ExecuteScalar(command);
        }

        public int AddPurchaseRequest(PurchaseRequest PurchaseRequest)
        {
            var command = Db.GetStoredProcCommand("pub_PurchaseRequest_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, PurchaseRequest.Amount);
            Db.AddInParameter(command, "@Commentary", DbType.String, PurchaseRequest.Commentary);
            Db.AddInParameter(command, "@CreateDate", DbType.DateTime, DateTime.Now);
            Db.AddInParameter(command, "@Active", DbType.Boolean, 1);
            Db.AddInParameter(command, "@ProveedorId", DbType.Int32, PurchaseRequest.ProveedorId);
            Db.AddInParameter(command, "@NeedDate", DbType.DateTime, PurchaseRequest.NeedDate);
            Db.AddInParameter(command, "@RequestTypeId", DbType.Int32, 1);
            Db.AddInParameter(command, "@UserId", DbType.Int32, PurchaseRequest.UserId);
            Db.AddInParameter(command, "@UrgencyId", DbType.Int32, PurchaseRequest.UrgencyId);
            Db.AddInParameter(command, "@UserAuthorizedId", DbType.Int32, PurchaseRequest.UserAuthorizedId);
            Db.AddInParameter(command, "@Processed", DbType.Boolean, false);
            Db.AddInParameter(command, "@Managed", DbType.Boolean, false);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }


        public void AddPurchaseRequestDetail(PurchaseRequestDetail PurchaseRequestDetail)
        {
            var command = Db.GetStoredProcCommand("pub_PurchaseRequestDetail_Insert");

            Db.AddInParameter(command, "@ArticleId", DbType.Int32, PurchaseRequestDetail.ArticleId);
            Db.AddInParameter(command, "@Price", DbType.Double, PurchaseRequestDetail.Price);
            Db.AddInParameter(command, "@Total", DbType.Double, PurchaseRequestDetail.Total);
            Db.AddInParameter(command, "@Quantity", DbType.Int32, PurchaseRequestDetail.Quantity);
            Db.AddInParameter(command, "@PurchaseRequestId", DbType.Int32, PurchaseRequestDetail.PurchaseRequestId);
            Db.AddInParameter(command, "@Description", DbType.String, PurchaseRequestDetail.Description);

            Db.ExecuteScalar(command);
        }

        public void UpdatePurchaseRequest(PurchaseRequest PurchaseRequest)
        {
            var command = Db.GetStoredProcCommand("pub_PurchaseRequest_Update");

            Db.AddInParameter(command, "@PurchaseRequestId", DbType.Int32, PurchaseRequest.PurchaseRequestId);
            Db.AddInParameter(command, "@Amount", DbType.Double, PurchaseRequest.Amount);
            Db.AddInParameter(command, "@Commentary", DbType.String, PurchaseRequest.Commentary);
            Db.AddInParameter(command, "@CreateDate", DbType.DateTime, PurchaseRequest.CreateDate);
            Db.AddInParameter(command, "@Active", DbType.Boolean, PurchaseRequest.Active);
            Db.AddInParameter(command, "@ProveedorId", DbType.Int32, PurchaseRequest.ProveedorId);
            Db.AddInParameter(command, "@NeedDate", DbType.DateTime, PurchaseRequest.NeedDate);
            Db.AddInParameter(command, "@RequestTypeId", DbType.Int32, 1);
            Db.AddInParameter(command, "@UserId", DbType.Int32, PurchaseRequest.UserId);
            Db.AddInParameter(command, "@UrgencyId", DbType.Int32, PurchaseRequest.UrgencyId);
            Db.AddInParameter(command, "@UserAuthorizedId", DbType.Int32, PurchaseRequest.UserAuthorizedId);
            Db.AddInParameter(command, "@Processed", DbType.Boolean, PurchaseRequest.Processed);
            Db.AddInParameter(command, "@Managed", DbType.Boolean, PurchaseRequest.Managed);
            Db.ExecuteScalar(command);
        }

        public void UpdatePurchaseRequestProcessed(int purchaseRequestId, bool processed)
        {
            var command = Db.GetStoredProcCommand("pub_PurchaseRequestProcessed_Update");

            Db.AddInParameter(command, "@PurchaseRequestId", DbType.Int32, purchaseRequestId);
            Db.AddInParameter(command, "@Processed", DbType.Boolean, processed);
            Db.ExecuteScalar(command);
        }

        public void UpdatePurchaseRequestManaged(int purchaseRequestId, bool managed)
        {
            var command = Db.GetStoredProcCommand("pub_PurchaseRequestManaged_Update");

            Db.AddInParameter(command, "@PurchaseRequestId", DbType.Int32, purchaseRequestId);
            Db.AddInParameter(command, "@Managed", DbType.Boolean, managed);
            Db.ExecuteScalar(command);
        }

        public void UpdatePurchaseRequestDetail(PurchaseRequestDetail PurchaseRequestDetail)
        {
            var command = Db.GetStoredProcCommand("pub_PurchaseRequestDetail_Update");

            Db.AddInParameter(command, "@PurchaseRequestDetailId", DbType.Int32, PurchaseRequestDetail.PurchaseRequestDetailId);
            Db.AddInParameter(command, "@ArticleId", DbType.Int32, PurchaseRequestDetail.ArticleId);
            Db.AddInParameter(command, "@Price", DbType.Double, PurchaseRequestDetail.Price);
            Db.AddInParameter(command, "@Total", DbType.Double, PurchaseRequestDetail.Total);
            Db.AddInParameter(command, "@Quantity", DbType.Int32, PurchaseRequestDetail.Quantity);
            Db.AddInParameter(command, "@PurchaseRequestId", DbType.Int32, PurchaseRequestDetail.PurchaseRequestId);
            Db.ExecuteScalar(command);
        }
    }
}
