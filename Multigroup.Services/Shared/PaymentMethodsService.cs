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
    public class PaymentMethodsService
    {
        private PaymentMethodsRepository _paymentMethodsRepository;
        private ContractRepository _contractRepository;

        public PaymentMethodsService()
        {
            _paymentMethodsRepository = new PaymentMethodsRepository();
            _contractRepository = new ContractRepository();
        }

        public PaymentMethod GetPaymentMethods(int PaymentMethodId)
        {
            return _paymentMethodsRepository.GetPaymentMethodsById(PaymentMethodId);
        }

        public PaymentMethod GetPaymentMethodssById(int PaymentMethodId)
        {
            return _paymentMethodsRepository.GetPaymentMethodsById(PaymentMethodId);
        }

        public IEnumerable<PaymentMethod> GetPaymentMethodss()
        {
            return _paymentMethodsRepository.GetPaymentMethodss();
        }

        public IEnumerable<PaymentMethodSummary> GetPaymentMethodsSummary()
        {
            return _paymentMethodsRepository.GetPaymentMethodsSummary();
        }

        public IEnumerable<PaymentPreferencePaymentMethods> GetPaymentMethodsByPaymentPreference(int paymentMethodPreferenceId)
        {
            return _paymentMethodsRepository.GetPaymentMethodByPaymentPreference(paymentMethodPreferenceId);
        }

        public IEnumerable<PaymentMethodReconciled> GetPaymentMethodReconciled()
        {
            return _paymentMethodsRepository.GetPaymentMethodReconciled();
        }

        public PaymentMethodReconciled GetPaymentByReceipNumber(int paymentMethodReconciledId)
        {
            return _paymentMethodsRepository.GetPaymentByReceipNumber(paymentMethodReconciledId);
        }

        public IEnumerable<PaymentMethodReconciledSummary> GetPaymentMethodReconciledSummary(int paymentMethodId)
        {
            return _paymentMethodsRepository.GetPaymentMethodReconciledSummary(paymentMethodId);
        }

        public PaymentMethodReconciledSummary GetPaymentMethodReconciledSummaryLast(int paymentMethodId)
        {
            return _paymentMethodsRepository.GetPaymentMethodReconciledSummaryLast(paymentMethodId);
        }

        public IEnumerable<PaymentMethodReconciledLast> GetPaymentMethodReconciledLast()
        {
            return _paymentMethodsRepository.GetPaymentMethodReconciledLast();
        }

        public void AddPaymentMethods(PaymentMethod paymentMethods)
        {
            _paymentMethodsRepository.AddPaymentMethods(paymentMethods);
        }

        public void DeletePaymentMethodss(int PaymentMethodId)
        {
            _paymentMethodsRepository.Delete(PaymentMethodId);
        }

        public void UpdatePaymentMethodss(PaymentMethod paymentMethods)
        {
            _paymentMethodsRepository.Update(paymentMethods);
        }

        public IEnumerable<PaymentPreference> GetPaymentPreference()
        {
            return _contractRepository.GetPaymentPreference();
        }

        public PaymentPreference GetPaymentPreferenceById(int paymentPreference)
        {
            return _contractRepository.GetPaymentPreferenceById(paymentPreference);
        }

        public void AddPaymentPreference(PaymentPreference paymentPreference, IEnumerable<PaymentPreferencePaymentMethods> details)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    int id = _paymentMethodsRepository.AddPaymentPreference(paymentPreference);

                    foreach (PaymentPreferencePaymentMethods detail in details)
                    {
                        detail.PaymentPreferenceId = id;
                        _paymentMethodsRepository.AddPaymentPreferencePM(detail);
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


        public void UpdatePaymentPreference(PaymentPreference paymentPreference, IEnumerable<PaymentPreferencePaymentMethods> details)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _paymentMethodsRepository.UpdatePaymentPreference(paymentPreference);
                    _paymentMethodsRepository.DeletePaymentMethodByPaymentPreference(paymentPreference.PaymentPreferenceId);
                    foreach (PaymentPreferencePaymentMethods detail in details)
                    {
                        detail.PaymentPreferenceId = paymentPreference.PaymentPreferenceId;
                        _paymentMethodsRepository.AddPaymentPreferencePM(detail);
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

    }
}
