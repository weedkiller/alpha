using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Multigroup.Services.Shared
{
    public class MovementService
    {
        private MovementRepository _movementRepository;
        private PaymentMethodsRepository _paymentMethodsRepository;
        private SpendingRepository _spendingRepository;
        private SpendingPaymentRepository _spendingPaymentRepository;
        private PromissoryPaymentRepository _promissoryPaymentRepository;
        private PromissorySurrenderRepository _promissorySurrenderRepository;
        private RetirementPartnerRepository _retirementPartnerRepository;
        private IncomePartnerRepository _incomePartnerRepository;
        private CashierRepository _cashierRepository;
        private PaymentRepository _paymentRepository;

        public MovementService()
        {
            _movementRepository = new MovementRepository();
            _paymentMethodsRepository = new PaymentMethodsRepository();
            _spendingRepository = new SpendingRepository();
            _spendingPaymentRepository = new SpendingPaymentRepository();
            _promissoryPaymentRepository = new PromissoryPaymentRepository();
            _promissorySurrenderRepository = new PromissorySurrenderRepository();
            _retirementPartnerRepository = new RetirementPartnerRepository();
            _incomePartnerRepository = new IncomePartnerRepository();
            _cashierRepository = new CashierRepository();
            _paymentRepository = new PaymentRepository();
        }

        public MovementType GetMovementType(int movementTypeId)
        {
            return _movementRepository.GetMovementTypeById(movementTypeId);
        }

        public IEnumerable<MovementType> GetMovementTypes()
        {
            return _movementRepository.GetMovementTypes();
        }

        public IEnumerable<MovementResume> GetMovementResume(string DateFrom, string DateTo, string DateSystemFrom, string DateSystemTo, string SelectedUser, string SelectedPaymentMethod, string SelectedCashier, string Cycle, string _selectedMovementType, string _selectedVerified, string amountFrom, string amountTo)
        {
            return _movementRepository.GetMovementResume(DateFrom, DateTo, DateSystemFrom, DateSystemTo, SelectedUser, SelectedPaymentMethod, SelectedCashier, Cycle, _selectedMovementType, _selectedVerified, amountFrom, amountTo);
        }

        public IEnumerable<MovementBalance> GetMovementBalance()
        {
            List<MovementBalance> balances = new List<MovementBalance>();

            IEnumerable<PaymentMethod> pms = _paymentMethodsRepository.GetPaymentMethodss();
            foreach(PaymentMethod pm in pms)
            {
                MovementBalance balance = new MovementBalance();
                balance.PaymentMethod = pm.Name;

                PaymentMethodReconciledSummary reconciled = _paymentMethodsRepository.GetPaymentMethodReconciledSummaryLast(pm.PaymentMethodId);
                balance.User = (reconciled != null) ? reconciled.User : " - ";
                balance.ConciliationDate = (reconciled != null) ? reconciled.ReconciledDate : (DateTime?)null;



                double total = 0;

                IEnumerable<IncomePartnerPaymentMethod> incomes = _incomePartnerRepository.GetIncomePartnerPaymentMethodsByBalance(pm.PaymentMethodId);
                foreach (IncomePartnerPaymentMethod income in incomes)
                {
                    total = total + income.Amount;
                }

                IEnumerable<RetirementPartnerPaymentMethod> retirements = _retirementPartnerRepository.GetRetirementPartnerPaymentMethodsBalance(pm.PaymentMethodId);
                foreach (RetirementPartnerPaymentMethod retirement in retirements)
                {
                    total = total - retirement.Amount;
                }

                IEnumerable<PromissoryPaymentMethod> promissorys = _promissoryPaymentRepository.GetPromissoryPaymentMethodsBalance(pm.PaymentMethodId);
                foreach (PromissoryPaymentMethod promissory in promissorys)
                {
                    total = total + promissory.Amount;
                }

                IEnumerable<PromissorySurrenderMethod> surrrenders = _promissorySurrenderRepository.GetPromissorySurrendersMethodsBalance(pm.PaymentMethodId);
                foreach (PromissorySurrenderMethod surrrender in surrrenders)
                {
                    total = total - surrrender.Amount;
                }

                IEnumerable<SpendingPaymentMethod> spendings = _spendingRepository.GetSpendingsPaymentMethodsBalance(pm.PaymentMethodId);
                foreach (SpendingPaymentMethod spending in spendings)
                {
                    total = total - spending.Amount;
                }

                IEnumerable<SpendingPaymentPaymentMethod> spendingPayments = _spendingPaymentRepository.GetSpendingsPaymentPMsBalance(pm.PaymentMethodId);
                foreach (SpendingPaymentPaymentMethod spendingPayment in spendingPayments)
                {
                    total = total - spendingPayment.Amount;
                }

                IEnumerable<TransferCashierPaymentMethod> cashiersPos = _cashierRepository.GetTransferCashierPaymentMethodPosBalance(pm.PaymentMethodId);
                foreach (TransferCashierPaymentMethod cashierPos in cashiersPos)
                {
                    total = total + cashierPos.Amount;
                }

                IEnumerable<TransferCashierPaymentMethod> cashiersNeg = _cashierRepository.GetTransferCashierPaymentMethodNegBalance(pm.PaymentMethodId);
                foreach (TransferCashierPaymentMethod cashierNeg in cashiersNeg)
                {
                    total = total - cashierNeg.Amount;
                }

                IEnumerable<PaymentPaymentMethod> paymetPayments = _paymentRepository.GetPaymentPaymentMethodBalance(pm.PaymentMethodId);
                foreach (PaymentPaymentMethod payment in paymetPayments)
                {
                    total = total + payment.Amount;
                }

                balance.Amount = total;

                balances.Add(balance);
            }

            return balances;
        }

        public IEnumerable<Verified> GetVerified()
        {
            return _movementRepository.GetVerified();
        }

        public void VerifiedMovement(int movementId, string movementType)
        {
            PaymentMethodVerification verification = new PaymentMethodVerification();
            verification.MovementID = movementId;
            verification.MovementType = movementType;
            verification.VerificationDate = DateTime.Now;
            verification.Time = DateTime.Now.ToString("HH:mm");
            _paymentMethodsRepository.AddPaymentMethodVerification(verification);

        }

    }
}
