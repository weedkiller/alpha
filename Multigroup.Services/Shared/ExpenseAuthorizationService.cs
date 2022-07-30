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
    public class ExpenseAuthorizationService
    {
        private ExpenseAuthorizationRepository _expenseAuthorizationRepository;
        private ArticleRepository _articleRepository;
        private PurchaseRequestRepository _purchaseRequestRepository;
        private ScheduledExpenseRepository _scheduledExpenseRepository;

        public ExpenseAuthorizationService()
        {
            _expenseAuthorizationRepository = new ExpenseAuthorizationRepository();
            _articleRepository = new ArticleRepository();
            _purchaseRequestRepository = new PurchaseRequestRepository();
            _scheduledExpenseRepository = new ScheduledExpenseRepository();
        }


        public ExpenseAuthorization GetExpenseAuthorizationsById(int expenseAuthorizationId)
        {
            ExpenseAuthorization gasto = _expenseAuthorizationRepository.GetExpenseAuthorizationById(expenseAuthorizationId);

            List<ExpenseAuthorizationDetail> Details = new List<ExpenseAuthorizationDetail>();

            foreach (ExpenseAuthorizationDetail item in _expenseAuthorizationRepository.GetExpenseAuthorizationDetails(gasto.ExpenseAuthorizationId))
            {
                ExpenseAuthorizationDetail detalle = _expenseAuthorizationRepository.GetExpenseAuthorizationDetail(item.ExpenseAuthorizationDetailId);
                detalle.Article = _articleRepository.GetArticleById(detalle.ArticleId).Name;
                Details.Add(detalle);
            }

            gasto.Details = Details;

            return gasto;
        }

        public IEnumerable<ExpenseAuthorization> GetExpenseAuthorizations()
        {
            return _expenseAuthorizationRepository.GetExpenseAuthorizations();
        }

        public IEnumerable<ExpenseAuthorizationSummary> GetExpenseAuthorizationSummary(int? SelectedProvider, int? SelectedUser, string DateFromCreate, string DateToCreate, string DateFromPurchase, string DateToPurchase, string AmountFrom, string AmountTo, string PurchaseRequestId, int? SelectedUserBuyer, string Processed)
        {
            if (SelectedProvider == null)
                SelectedProvider = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (SelectedUserBuyer == null)
                SelectedUserBuyer = 0;
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
            return _expenseAuthorizationRepository.GetExpenseAuthorizationsSummary(SelectedProvider.Value.ToString(), SelectedUser.Value.ToString(), DateFromCreate, DateToCreate, DateFromPurchase, DateToPurchase, AmountFrom, AmountTo, PurchaseRequestId, "1", SelectedUserBuyer.ToString(), Processed);
        }

        public IEnumerable<ExpenseAuthorizationSummary> GetExpenseAuthorizationSummaryManaged(int SelectedUserAuthorized)
        {
            return _expenseAuthorizationRepository.GetExpenseAuthorizationSummaryManaged(SelectedUserAuthorized.ToString());
        }

        public ExpenseAuthorizationDetail GetExpenseAuthorizationDetail(int expenseAuthorizationDetailId)
        {
            return _expenseAuthorizationRepository.GetExpenseAuthorizationDetail(expenseAuthorizationDetailId);
        }

        public IEnumerable<ExpenseAuthorizationDetail> GetExpenseAuthorizationDetails(int expenseAuthorizationId)
        {
            return _expenseAuthorizationRepository.GetExpenseAuthorizationDetails(expenseAuthorizationId);
        }

        public void AddExpenseAuthorization(ExpenseAuthorization ExpenseAuthorization)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    int expense = _expenseAuthorizationRepository.AddExpenseAuthorization(ExpenseAuthorization);

                    if (ExpenseAuthorization.PurchaseRequestId.HasValue)
                    {
                        _purchaseRequestRepository.UpdatePurchaseRequestProcessed(ExpenseAuthorization.PurchaseRequestId.Value, true);
                    }

                    if (ExpenseAuthorization.ScheduledExpenseId.HasValue)
                    {
                        _scheduledExpenseRepository.UpdateScheduledExpenseProcessed(ExpenseAuthorization.ScheduledExpenseId.Value, true);
                    }

                    foreach (ExpenseAuthorizationDetail detail in ExpenseAuthorization.Details)
                    {
                        detail.ExpenseAuthorizationId = expense;
                        _expenseAuthorizationRepository.AddExpenseAuthorizationDetail(detail);
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

        public void DeleteExpenseAuthorization(int expenseAuthorizationId)
        {
            _expenseAuthorizationRepository.DeleteExpenseAuthorization(expenseAuthorizationId);
        }

        public void UpdateExpenseAuthorization(ExpenseAuthorization ExpenseAuthorization)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _expenseAuthorizationRepository.UpdateExpenseAuthorization(ExpenseAuthorization);

                    _expenseAuthorizationRepository.DeleteExpenseAuthorizationDetailByExpenseId(ExpenseAuthorization.ExpenseAuthorizationId);
                    foreach (ExpenseAuthorizationDetail detail in ExpenseAuthorization.Details)
                    {
                        detail.ExpenseAuthorizationId = ExpenseAuthorization.ExpenseAuthorizationId;
                        _expenseAuthorizationRepository.AddExpenseAuthorizationDetail(detail);
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

        public void AddExpenseAuthorizationDetail(ExpenseAuthorizationDetail ExpenseAuthorizationDetail)
        {
            _expenseAuthorizationRepository.AddExpenseAuthorizationDetail(ExpenseAuthorizationDetail);
        }

        public void DeleteExpenseAuthorizationDetail(int expenseAuthorizationDetailId)
        {
            _expenseAuthorizationRepository.DeleteExpenseAuthorizationDetail(expenseAuthorizationDetailId);
        }

        public void UpdateExpenseAuthorizationDetail(ExpenseAuthorizationDetail ExpenseAuthorizationDetail)
        {
            _expenseAuthorizationRepository.UpdateExpenseAuthorizationDetail(ExpenseAuthorizationDetail);
        }
    }
}
