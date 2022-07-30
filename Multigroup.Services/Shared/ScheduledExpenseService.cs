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
    public class ScheduledExpenseService
    {
        private ScheduledExpenseRepository _scheduledExpenseRepository;
        private ArticleRepository _articleRepository;
        private PurchaseRequestRepository _purchaseRequestRepository;

        public ScheduledExpenseService()
        {
            _scheduledExpenseRepository = new ScheduledExpenseRepository();
            _articleRepository = new ArticleRepository();
            _purchaseRequestRepository = new PurchaseRequestRepository();
        }


        public ScheduledExpense GetScheduledExpensesById(int scheduledExpenseId)
        {
            ScheduledExpense gasto = _scheduledExpenseRepository.GetScheduledExpenseById(scheduledExpenseId);

            List<ScheduledExpenseDetail> Details = new List<ScheduledExpenseDetail>();

            foreach (ScheduledExpenseDetail item in _scheduledExpenseRepository.GetScheduledExpenseDetails(gasto.ScheduledExpenseId))
            {
                ScheduledExpenseDetail detalle = _scheduledExpenseRepository.GetScheduledExpenseDetail(item.ScheduledExpenseDetailId);
                detalle.Article = _articleRepository.GetArticleById(detalle.ArticleId).Name;
                Details.Add(detalle);
            }

            gasto.Details = Details;

            return gasto;
        }

        public IEnumerable<ScheduledExpense> GetScheduledExpenses()
        {
            return _scheduledExpenseRepository.GetScheduledExpenses();
        }

        public IEnumerable<ScheduledExpenseSummary> GetScheduledExpensesSummary(int? SelectedProvider, int? SelectedUser, string DateFromCreate, string DateToCreate, string DateFromPurchase, string DateToPurchase, string AmountFrom, string AmountTo, string PurchaseRequestId, string Authorized, string Processed, string AuthorizedUser, string Active)
        {
            if (SelectedProvider == null)
                SelectedProvider = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromCreate == "")
                DateFromCreate = "01/01/1900";
            if (DateToCreate == "")
                DateToCreate = "01/01/2100";
            if (DateFromPurchase == "")
                DateFromPurchase = "01/01/1900";
            if (DateToPurchase == "")
                DateToPurchase = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            if (PurchaseRequestId == "")
                PurchaseRequestId = "0";
            return _scheduledExpenseRepository.GetScheduledExpensesSummary(SelectedProvider.Value.ToString(), SelectedUser.Value.ToString(), DateFromCreate, DateToCreate, DateFromPurchase, DateToPurchase, AmountFrom, AmountTo, PurchaseRequestId, "1", Authorized, Processed, AuthorizedUser, Active);
        }

        public IEnumerable<ScheduledExpenseSummary> GetScheduledExpenseSummaryManaged(int SelectedUserAuthorized)
        {
            return _scheduledExpenseRepository.GetScheduledExpenseSummaryManaged(SelectedUserAuthorized.ToString());
        }

        public ScheduledExpenseDetail GetScheduledExpenseDetail(int scheduledExpenseDetailId)
        {
            return _scheduledExpenseRepository.GetScheduledExpenseDetail(scheduledExpenseDetailId);
        }

        public IEnumerable<ScheduledExpenseDetail> GetScheduledExpenseDetails(int scheduledExpenseId)
        {
            return _scheduledExpenseRepository.GetScheduledExpenseDetails(scheduledExpenseId);
        }

        public void AddScheduledExpense(ScheduledExpense ScheduledExpense)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    int scheduled = _scheduledExpenseRepository.AddScheduledExpense(ScheduledExpense);
            
                    if (ScheduledExpense.PurchaseRequestId.HasValue)
                    {
                        _purchaseRequestRepository.UpdatePurchaseRequestProcessed(ScheduledExpense.PurchaseRequestId.Value, true);
                    }
                    foreach (ScheduledExpenseDetail detail in ScheduledExpense.Details)
                    {
                        detail.ScheduledExpenseId = scheduled;
                        _scheduledExpenseRepository.AddScheduledExpenseDetail(detail);
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

        public void DeleteScheduledExpense(int scheduledExpenseId)
        {
            _scheduledExpenseRepository.DeleteScheduledExpense(scheduledExpenseId);
        }

        public void UpdateScheduledExpense(ScheduledExpense ScheduledExpense)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _scheduledExpenseRepository.UpdateScheduledExpense(ScheduledExpense);
                    _scheduledExpenseRepository.DeleteScheduledExpenseDetailByScheduledId(ScheduledExpense.ScheduledExpenseId);
                    foreach (ScheduledExpenseDetail detail in ScheduledExpense.Details)
                    {
                        detail.ScheduledExpenseId = ScheduledExpense.ScheduledExpenseId;
                        _scheduledExpenseRepository.AddScheduledExpenseDetail(detail);
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

        public void AddScheduledExpenseDetail(ScheduledExpenseDetail ScheduledExpenseDetail)
        {
            _scheduledExpenseRepository.AddScheduledExpenseDetail(ScheduledExpenseDetail);
        }

        public void DeleteScheduledExpenseDetail(int scheduledExpenseDetailId)
        {
            _scheduledExpenseRepository.DeleteScheduledExpenseDetail(scheduledExpenseDetailId);
        }

        public void UpdateScheduledExpenseDetail(ScheduledExpenseDetail ScheduledExpenseDetail)
        {
            _scheduledExpenseRepository.UpdateScheduledExpenseDetail(ScheduledExpenseDetail);
        }
    }
}
