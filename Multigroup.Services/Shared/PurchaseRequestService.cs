using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Multigroup.Services.Shared
{
    public class PurchaseRequestService
    {
        private PurchaseRequestRepository _purchaseRequestRepository;
        private ArticleRepository _articleRepository;

        public PurchaseRequestService()
        {
            _purchaseRequestRepository = new PurchaseRequestRepository();
            _articleRepository = new ArticleRepository();
        }


        public PurchaseRequest GetPurchaseRequestsById(int purchaseRequestId)
        {
            PurchaseRequest gasto = _purchaseRequestRepository.GetPurchaseRequestById(purchaseRequestId);

            List<PurchaseRequestDetail> Details = new List<PurchaseRequestDetail>();

            foreach (PurchaseRequestDetail item in _purchaseRequestRepository.GetPurchaseRequestDetails(gasto.PurchaseRequestId))
            {
                PurchaseRequestDetail detalle = _purchaseRequestRepository.GetPurchaseRequestDetail(item.PurchaseRequestDetailId);
                detalle.Article = _articleRepository.GetArticleById(detalle.ArticleId).Name;
                Details.Add(detalle);
            }

            gasto.Details = Details;

            return gasto;
        }

        public IEnumerable<PurchaseRequest> GetPurchaseRequests()
        {
            return _purchaseRequestRepository.GetPurchaseRequests();
        }

        public IEnumerable<Urgency> GetUrgencys()
        {
            return _purchaseRequestRepository.GetUrgencys();
        }

        public IEnumerable<PurchaseRequestSummary> GetPurchaseRequestsSummary(int? SelectedUrgency, int? SelectedProvider, int? SelectedUser, int? SelectedUserAuthorized, string DateFromCreate, string DateToCreate, string DateFromNeed, string DateToNeed, string AmountFrom, string AmountTo, string Active, string Processed)
        {
            if (SelectedProvider == null)
                SelectedProvider = 0;
            if (SelectedUrgency == null)
                SelectedUrgency = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (SelectedUserAuthorized == null)
                SelectedUserAuthorized = 0;
            if (DateFromCreate == "")
                DateFromCreate = "01/01/1900";
            if (DateToCreate == "")
                DateToCreate = "01/01/2100";
            if (DateFromNeed == "")
                DateFromNeed = "01/01/1900";
            if (DateToNeed == "")
                DateToNeed = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            return _purchaseRequestRepository.GetPurchaseRequestsSummary(SelectedUrgency.Value.ToString(), SelectedProvider.Value.ToString(), SelectedUser.Value.ToString(), SelectedUserAuthorized.Value.ToString(), DateFromCreate, DateToCreate, DateFromNeed, DateToNeed, AmountFrom, AmountTo, Active, Processed);
        }

        public IEnumerable<PurchaseRequestSummary> GetPurchaseRequestsSummaryManaged(int SelectedUserAuthorized)
        {
            return _purchaseRequestRepository.GetPurchaseRequestsSummaryManaged(SelectedUserAuthorized.ToString());
        }

        public PurchaseRequestDetail GetPurchaseRequestDetail(int purchaseRequestDetailId)
        {
            return _purchaseRequestRepository.GetPurchaseRequestDetail(purchaseRequestDetailId);
        }

        public IEnumerable<PurchaseRequestDetail> GetPurchaseRequestDetails(int purchaseRequestId)
        {
            return _purchaseRequestRepository.GetPurchaseRequestDetails(purchaseRequestId);
        }

        public void AddPurchaseRequest(PurchaseRequest PurchaseRequest)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    int purchase = _purchaseRequestRepository.AddPurchaseRequest(PurchaseRequest);

                    foreach(PurchaseRequestDetail detail in PurchaseRequest.Details)
                    {
                        detail.PurchaseRequestId = purchase;
                        _purchaseRequestRepository.AddPurchaseRequestDetail(detail);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    throw ex;
                }
            }

        }

        public void DeletePurchaseRequest(int purchaseRequestId)
        {
            _purchaseRequestRepository.DeletePurchaseRequest(purchaseRequestId);
        }

        public void UpdatePurchaseRequest(PurchaseRequest PurchaseRequest)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _purchaseRequestRepository.UpdatePurchaseRequest(PurchaseRequest);

                    _purchaseRequestRepository.DeletePurchaseRequestDetailByPurchaseId(PurchaseRequest.PurchaseRequestId);
                    foreach (PurchaseRequestDetail detail in PurchaseRequest.Details)
                    {
                        detail.PurchaseRequestId = PurchaseRequest.PurchaseRequestId;
                        _purchaseRequestRepository.AddPurchaseRequestDetail(detail);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public void AddPurchaseRequestDetail(PurchaseRequestDetail PurchaseRequestDetail)
        {
            _purchaseRequestRepository.AddPurchaseRequestDetail(PurchaseRequestDetail);
        }

        public void DeletePurchaseRequestDetail(int purchaseRequestDetailId)
        {
            _purchaseRequestRepository.DeletePurchaseRequestDetail(purchaseRequestDetailId);
        }

        public void UpdatePurchaseRequestDetail(PurchaseRequestDetail PurchaseRequestDetail)
        {
            _purchaseRequestRepository.UpdatePurchaseRequestDetail(PurchaseRequestDetail);
        }
    }
}
