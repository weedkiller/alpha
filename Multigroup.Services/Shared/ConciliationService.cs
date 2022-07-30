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
    public class ConciliatioService
    {
        private CashCycleRepository _cashCycleRepository;
        private PaymentMethodsRepository _paymentMethodsRepository;
        private SpendingRepository _spendingRepository;
        private SpendingPaymentRepository _spendingPaymentRepository;
        private PromissoryPaymentRepository _promissoryPaymentRepository;
        private PromissorySurrenderRepository _promissorySurrenderRepository;
        private RetirementPartnerRepository _retirementPartnerRepository;
        private IncomePartnerRepository _incomePartnerRepository;
        private CashierRepository _cashierRepository;
        private MovementRepository _movementRepository;
        private PaymentRepository _paymentRepository;

        public ConciliatioService()
        {
            _cashCycleRepository = new CashCycleRepository();
            _paymentMethodsRepository = new PaymentMethodsRepository();
            _spendingRepository = new SpendingRepository();
            _spendingPaymentRepository = new SpendingPaymentRepository();
            _promissoryPaymentRepository = new PromissoryPaymentRepository();
            _promissorySurrenderRepository = new PromissorySurrenderRepository();
            _retirementPartnerRepository = new RetirementPartnerRepository();
            _incomePartnerRepository = new IncomePartnerRepository();
            _cashierRepository = new CashierRepository();
            _movementRepository = new MovementRepository();
            _paymentRepository = new PaymentRepository();
        }

        public double GetBalance(int PaymentMethodId)
        {
                double total = 0;

                IEnumerable<IncomePartnerPaymentMethod> incomes = _incomePartnerRepository.GetIncomePartnerPaymentMethodsByBalance(PaymentMethodId);
                foreach (IncomePartnerPaymentMethod income in incomes)
                {
                    total = total + income.Amount;
                }

                IEnumerable<RetirementPartnerPaymentMethod> retirements = _retirementPartnerRepository.GetRetirementPartnerPaymentMethodsBalance(PaymentMethodId);
                foreach (RetirementPartnerPaymentMethod retirement in retirements)
                {
                    total = total - retirement.Amount;
                }

                IEnumerable<PromissoryPaymentMethod> promissorys = _promissoryPaymentRepository.GetPromissoryPaymentMethodsBalance(PaymentMethodId);
                foreach (PromissoryPaymentMethod promissory in promissorys)
                {
                    total = total + promissory.Amount;
                }

                IEnumerable<PromissorySurrenderMethod> surrrenders = _promissorySurrenderRepository.GetPromissorySurrendersMethodsBalance(PaymentMethodId);
                foreach (PromissorySurrenderMethod surrrender in surrrenders)
                {
                    total = total - surrrender.Amount;
                }

                IEnumerable<SpendingPaymentMethod> spendings = _spendingRepository.GetSpendingsPaymentMethodsBalance(PaymentMethodId);
                foreach (SpendingPaymentMethod spending in spendings)
                {
                    total = total - spending.Amount;
                }

                IEnumerable<SpendingPaymentPaymentMethod> spendingPayments = _spendingPaymentRepository.GetSpendingsPaymentPMsBalance(PaymentMethodId);
                foreach (SpendingPaymentPaymentMethod spendingPayment in spendingPayments)
                {
                    total = total - spendingPayment.Amount;
                }

                IEnumerable<TransferCashierPaymentMethod> cashiersPos = _cashierRepository.GetTransferCashierPaymentMethodPosBalance(PaymentMethodId);
                foreach (TransferCashierPaymentMethod cashierPos in cashiersPos)
                {
                    total = total + cashierPos.Amount;
                }

                IEnumerable<TransferCashierPaymentMethod> cashiersNeg = _cashierRepository.GetTransferCashierPaymentMethodNegBalance(PaymentMethodId);
                foreach (TransferCashierPaymentMethod cashierNeg in cashiersNeg)
                {
                    total = total - cashierNeg.Amount;
                }

                IEnumerable<PaymentPaymentMethod> paymetPayments = _paymentRepository.GetPaymentPaymentMethodBalance(PaymentMethodId);
                foreach (PaymentPaymentMethod payment in paymetPayments)
                {
                    total = total + payment.Amount;
                }

            return total;
        }

        public void AddPaymentMethodReconciled(PaymentMethodReconciled payment)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    var saldo = GetBalance(payment.PaymentMethodId);
                    payment.Amount = saldo;
                    var conciliation = _paymentMethodsRepository.AddPaymentMethodReconciled(payment);
                    _movementRepository.ReconciledMovement(payment.PaymentMethodId, conciliation);
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
