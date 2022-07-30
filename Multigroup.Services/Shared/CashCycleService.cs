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
    public class CashCycleService
    {
        private CashCycleRepository _cashCycleRepository;
        private PaymentMethodsRepository _PaymentMethodsRepository;
        private SpendingRepository _spendingRepository;
        private SpendingPaymentRepository _spendingPaymentRepository;
        private PromissoryPaymentRepository _promissoryPaymentRepository;
        private PromissorySurrenderRepository _promissorySurrenderRepository;
        private RetirementPartnerRepository _retirementPartnerRepository;
        private IncomePartnerRepository _incomePartnerRepository;
        private CashierRepository _cashierRepository;
        private PaymentRepository _paymentRepository;

        public CashCycleService()
        {
            _cashCycleRepository = new CashCycleRepository();
            _PaymentMethodsRepository = new PaymentMethodsRepository();
            _spendingRepository = new SpendingRepository();
            _spendingPaymentRepository = new SpendingPaymentRepository();
            _promissoryPaymentRepository = new PromissoryPaymentRepository();
            _promissorySurrenderRepository = new PromissorySurrenderRepository();
            _retirementPartnerRepository = new RetirementPartnerRepository();
            _incomePartnerRepository = new IncomePartnerRepository();
            _cashierRepository = new CashierRepository();
            _paymentRepository = new PaymentRepository();
        }


        public CashCycle GetCashCyclesById(int CashCycleId)
        {
            return _cashCycleRepository.GetCashCycleById(CashCycleId);
        }

        public CashCycle GetCashCycleByCashierId(int CashierId)
        {
            return _cashCycleRepository.GetCashCycleByCashierId(CashierId);
        }

        public double GetAmountByCashierPM(int CashierId, int PaymentMehodId)
        {
            PaymentMethod pm = _PaymentMethodsRepository.GetPaymentMethodsById(PaymentMehodId);

            CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(CashierId);

            double total = 0;
            if (!pm.Consolidated)
            {
                CashCycle cs = _cashCycleRepository.getlastBalance(CashierId);
                if (cs != null)
                {
                    CashCyclePaymentMethod cspm = _cashCycleRepository.GetCashCyclePaymentMethodByCashCyclePM(cs.CashCycleId, pm.PaymentMethodId);
                    total = cspm.Balance;
                }
            }

            IEnumerable<IncomePartnerPaymentMethod> incomes = _incomePartnerRepository.GetIncomePartnerPaymentMethodsByCashCycle(cashCycle.CashCycleId, cashCycle.CashierId, pm.PaymentMethodId);
            foreach (IncomePartnerPaymentMethod income in incomes)
            {
                total = total + income.Amount;
            }

            IEnumerable<RetirementPartnerPaymentMethod> retirements = _retirementPartnerRepository.GetRetirementPartnerPaymentMethodsByCashCycle(cashCycle.CashCycleId, cashCycle.CashierId, pm.PaymentMethodId);
            foreach (RetirementPartnerPaymentMethod retirement in retirements)
            {
                total = total - retirement.Amount;
            }

            IEnumerable<PromissoryPaymentMethod> promissorys = _promissoryPaymentRepository.GetPromissoryPaymentMethodsByCashCycle(cashCycle.CashCycleId, cashCycle.CashierId, pm.PaymentMethodId);
            foreach (PromissoryPaymentMethod promissory in promissorys)
            {
                total = total + promissory.Amount;
            }

            IEnumerable<PromissorySurrenderMethod> surrrenders = _promissorySurrenderRepository.GetPromissorySurrendersMethodsByCashCycle(cashCycle.CashCycleId, cashCycle.CashierId, pm.PaymentMethodId);
            foreach (PromissorySurrenderMethod surrrender in surrrenders)
            {
                total = total - surrrender.Amount;
            }

            IEnumerable<SpendingPaymentMethod> spendings = _spendingRepository.GetSpendingsPaymentMethodsByCashCycle(cashCycle.CashCycleId, cashCycle.CashierId, pm.PaymentMethodId);
            foreach (SpendingPaymentMethod spending in spendings)
            {
                total = total - spending.Amount;
            }

            IEnumerable<SpendingPaymentPaymentMethod> spendingPayments = _spendingPaymentRepository.GetSpendingsPaymentPMsByCashCycle(cashCycle.CashCycleId, cashCycle.CashierId, pm.PaymentMethodId);
            foreach (SpendingPaymentPaymentMethod spendingPayment in spendingPayments)
            {
                total = total - spendingPayment.Amount;
            }

            IEnumerable<TransferCashierPaymentMethod> cashiersPos = _cashierRepository.GetTransferCashierPaymentMethodPosByCashCycle(cashCycle.CashCycleId, cashCycle.CashierId, pm.PaymentMethodId);
            foreach (TransferCashierPaymentMethod cashierPos in cashiersPos)
            {
                total = total + cashierPos.Amount;
            }

            IEnumerable<TransferCashierPaymentMethod> cashiersNeg = _cashierRepository.GetTransferCashierPaymentMethodNegByCashCycle(cashCycle.CashCycleId, cashCycle.CashierId, pm.PaymentMethodId);
            foreach (TransferCashierPaymentMethod cashierNeg in cashiersNeg)
            {
                total = total - cashierNeg.Amount;
            }

            IEnumerable<PaymentPaymentMethod> paymetPayments = _paymentRepository.GetPaymentPaymentMethodByCashCycle(cashCycle.CashCycleId, cashCycle.CashierId, pm.PaymentMethodId);
            foreach (PaymentPaymentMethod payment in paymetPayments)
            {
                total = total + payment.Amount;
            }

            return total;
        }

        public IEnumerable<CashCycle> GetCashCycles()
        {
            return _cashCycleRepository.GetCashCycles();
        }

        public IEnumerable<Cycle> GetCycles()
        {
            return _cashCycleRepository.GetCycles();
        }

        public IEnumerable<CashCycleSummary> GetCashCycleSummaryOpen()
        {

            IEnumerable<CashCycleSummary> cashOpen = _cashCycleRepository.GetCashCycleSummaryOpen();

            List<CashCycleSummary> list = new List<CashCycleSummary>();

            foreach(CashCycleSummary cash in cashOpen)
            {
                CashCycle cs = _cashCycleRepository.getlastBalance(cash.CashierId);
                if(cs != null)
                {
                    cash.Date = cs.Date.Value;
                    cash.Time = cs.Time;
                }
                list.Add(cash);
            }

            return list;

        }

        public IEnumerable<CashCycleSummary> GetCashCycleSummaryClose(int cashierId)
        {
            return _cashCycleRepository.GetCashCycleSummaryClose(cashierId);
        }

        public IEnumerable<CashCyclePaymentMethodSummary> GetCashCyclePMSummaryOpen(int cashCycleId, int cashierId)
        {
            List<CashCyclePaymentMethodSummary> list = new List<CashCyclePaymentMethodSummary>();

            IEnumerable<PaymentMethod> paymentMethods = _PaymentMethodsRepository.GetPaymentMethodss();
            foreach (PaymentMethod pm in paymentMethods)
            {
                CashCyclePaymentMethodSummary cashCyclePaymentMethod = new CashCyclePaymentMethodSummary();
                cashCyclePaymentMethod.CashCycleId = cashCycleId;
                cashCyclePaymentMethod.PaymentMethodId = pm.PaymentMethodId;
                cashCyclePaymentMethod.PaymentMethod = pm.Name;
                double inicio = 0;
                double totalIngreso = 0;
                double totalEgreso = 0;
                if (!pm.Consolidated)
                {
                    CashCycle cs = _cashCycleRepository.getlastBalance(cashierId);
                    if (cs != null)
                    {
                        CashCyclePaymentMethod cspm = _cashCycleRepository.GetCashCyclePaymentMethodByCashCyclePM(cs.CashCycleId, pm.PaymentMethodId);
                        inicio = cspm.Balance;
                    }
                }

                IEnumerable<IncomePartnerPaymentMethod> incomes = _incomePartnerRepository.GetIncomePartnerPaymentMethodsByCashCycle(cashCycleId, cashierId, pm.PaymentMethodId);
                foreach (IncomePartnerPaymentMethod income in incomes)
                {
                    totalIngreso = totalIngreso + income.Amount;
                }

                IEnumerable<RetirementPartnerPaymentMethod> retirements = _retirementPartnerRepository.GetRetirementPartnerPaymentMethodsByCashCycle(cashCycleId, cashierId, pm.PaymentMethodId);
                foreach (RetirementPartnerPaymentMethod retirement in retirements)
                {
                    totalEgreso = totalEgreso + retirement.Amount;
                }

                IEnumerable<PromissoryPaymentMethod> promissorys = _promissoryPaymentRepository.GetPromissoryPaymentMethodsByCashCycle(cashCycleId, cashierId, pm.PaymentMethodId);
                foreach (PromissoryPaymentMethod promissory in promissorys)
                {
                    totalIngreso = totalIngreso + promissory.Amount;
                }

                IEnumerable<PromissorySurrenderMethod> surrrenders = _promissorySurrenderRepository.GetPromissorySurrendersMethodsByCashCycle(cashCycleId, cashierId, pm.PaymentMethodId);
                foreach (PromissorySurrenderMethod surrrender in surrrenders)
                {
                    totalEgreso = totalEgreso + surrrender.Amount;
                }

                IEnumerable<SpendingPaymentMethod> spendings = _spendingRepository.GetSpendingsPaymentMethodsByCashCycle(cashCycleId, cashierId, pm.PaymentMethodId);
                foreach (SpendingPaymentMethod spending in spendings)
                {
                    totalEgreso = totalEgreso + spending.Amount;
                }

                IEnumerable<SpendingPaymentPaymentMethod> spendingPayments = _spendingPaymentRepository.GetSpendingsPaymentPMsByCashCycle(cashCycleId, cashierId, pm.PaymentMethodId);
                foreach (SpendingPaymentPaymentMethod spendingPayment in spendingPayments)
                {
                    totalEgreso = totalEgreso + spendingPayment.Amount;
                }

                IEnumerable<TransferCashierPaymentMethod> cashiersPos = _cashierRepository.GetTransferCashierPaymentMethodPosByCashCycle(cashCycleId, cashierId, pm.PaymentMethodId);
                foreach (TransferCashierPaymentMethod cashierPos in cashiersPos)
                {
                    totalIngreso = totalIngreso + cashierPos.Amount;
                }

                IEnumerable<TransferCashierPaymentMethod> cashiersNeg = _cashierRepository.GetTransferCashierPaymentMethodNegByCashCycle(cashCycleId, cashierId, pm.PaymentMethodId);
                foreach (TransferCashierPaymentMethod cashierNeg in cashiersNeg)
                {
                    totalEgreso = totalEgreso + cashierNeg.Amount;
                }

                IEnumerable<PaymentPaymentMethod> paymetPayments = _paymentRepository.GetPaymentPaymentMethodByCashCycle(cashCycleId, cashierId, pm.PaymentMethodId);
                foreach (PaymentPaymentMethod payment in paymetPayments)
                {
                    totalIngreso = totalIngreso + payment.Amount;
                }

                cashCyclePaymentMethod.OLD = inicio;
                cashCyclePaymentMethod.Entry = totalIngreso;
                cashCyclePaymentMethod.Egress = totalEgreso;
                cashCyclePaymentMethod.Balance = inicio + totalIngreso - totalEgreso;

                list.Add(cashCyclePaymentMethod);
            }

            return list;
        }

        public IEnumerable<CashCyclePayment> GetCashCyclePayments(int cashCycleId, int cashierId)
        {
            return _cashCycleRepository.GetCashCyclePayments(cashCycleId, cashierId);
        }

        public void AddCashCycle(CashCycle CashCycle)
        {
            _cashCycleRepository.AddCashCycle(CashCycle);
        }

        public void DeleteCashCycle(int CashCycleId)
        {
            _cashCycleRepository.DeleteCashCycle(CashCycleId);
        }

        public CashCycle LastCashCycleByCashier(int CashierId)
        {
            return _cashCycleRepository.getlastBalance(CashierId);
        }

        public void CloseCashCycle(CashCycle CashCycle)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    CashCycle.Closed = true;
                    CashCycle.Date = DateTime.Now.Date;
                    CashCycle.Time = DateTime.Now.ToString("HH:mm");
                    

                    IEnumerable<PaymentMethod> paymentMethods = _PaymentMethodsRepository.GetPaymentMethodss();
                    foreach(PaymentMethod pm in paymentMethods)
                    {
                        CashCyclePaymentMethod cashCyclePaymentMethod = new CashCyclePaymentMethod();
                        cashCyclePaymentMethod.CashCycleId = CashCycle.CashCycleId;
                        cashCyclePaymentMethod.PaymentMethodId = pm.PaymentMethodId;
                        double total = 0;
                        if(!pm.Consolidated)
                        {
                            CashCycle cs = _cashCycleRepository.getlastBalance(CashCycle.CashierId);
                            if (cs != null)
                            {
                                CashCyclePaymentMethod cspm = _cashCycleRepository.GetCashCyclePaymentMethodByCashCyclePM(cs.CashCycleId, pm.PaymentMethodId);
                                total = cspm.Balance;
                            }
                        }

                        IEnumerable<IncomePartnerPaymentMethod> incomes = _incomePartnerRepository.GetIncomePartnerPaymentMethodsByCashCycle(CashCycle.CashCycleId, CashCycle.CashierId, pm.PaymentMethodId);
                        foreach(IncomePartnerPaymentMethod income in incomes)
                        {
                            total = total + income.Amount;
                        }

                        IEnumerable<RetirementPartnerPaymentMethod> retirements = _retirementPartnerRepository.GetRetirementPartnerPaymentMethodsByCashCycle(CashCycle.CashCycleId, CashCycle.CashierId, pm.PaymentMethodId);
                        foreach(RetirementPartnerPaymentMethod retirement in retirements)
                        {
                            total = total - retirement.Amount;
                        }

                        IEnumerable<PromissoryPaymentMethod> promissorys = _promissoryPaymentRepository.GetPromissoryPaymentMethodsByCashCycle(CashCycle.CashCycleId, CashCycle.CashierId, pm.PaymentMethodId);
                        foreach (PromissoryPaymentMethod promissory in promissorys)
                        {
                            total = total + promissory.Amount;
                        }

                        IEnumerable<PromissorySurrenderMethod> surrrenders = _promissorySurrenderRepository.GetPromissorySurrendersMethodsByCashCycle(CashCycle.CashCycleId, CashCycle.CashierId, pm.PaymentMethodId);
                        foreach (PromissorySurrenderMethod surrrender in surrrenders)
                        {
                            total = total - surrrender.Amount;
                        }

                        IEnumerable<SpendingPaymentMethod> spendings = _spendingRepository.GetSpendingsPaymentMethodsByCashCycle(CashCycle.CashCycleId, CashCycle.CashierId, pm.PaymentMethodId);
                        foreach (SpendingPaymentMethod spending in spendings)
                        {
                            total = total - spending.Amount;
                        }

                        IEnumerable<SpendingPaymentPaymentMethod> spendingPayments = _spendingPaymentRepository.GetSpendingsPaymentPMsByCashCycle(CashCycle.CashCycleId, CashCycle.CashierId, pm.PaymentMethodId);
                        foreach (SpendingPaymentPaymentMethod spendingPayment in spendingPayments)
                        {
                            total = total - spendingPayment.Amount;
                        }

                        IEnumerable<TransferCashierPaymentMethod> cashiersPos = _cashierRepository.GetTransferCashierPaymentMethodPosByCashCycle(CashCycle.CashCycleId, CashCycle.CashierId, pm.PaymentMethodId);
                        foreach (TransferCashierPaymentMethod cashierPos in cashiersPos)
                        {
                            total = total + cashierPos.Amount;
                        }

                        IEnumerable<TransferCashierPaymentMethod> cashiersNeg = _cashierRepository.GetTransferCashierPaymentMethodNegByCashCycle(CashCycle.CashCycleId, CashCycle.CashierId, pm.PaymentMethodId);
                        foreach (TransferCashierPaymentMethod cashierNeg in cashiersNeg)
                        {
                            total = total - cashierNeg.Amount;
                        }

                        IEnumerable<PaymentPaymentMethod> paymetPayments = _paymentRepository.GetPaymentPaymentMethodByCashCycle(CashCycle.CashCycleId, CashCycle.CashierId, pm.PaymentMethodId);
                        foreach (PaymentPaymentMethod payment in paymetPayments)
                        {
                            total = total + payment.Amount;
                        }

                        cashCyclePaymentMethod.Balance = total;
                        _cashCycleRepository.AddCashCyclePaymentMethod(cashCyclePaymentMethod);
                    }

                    _cashCycleRepository.UpdateCashCycle(CashCycle);

                    CashCycle cashCycleNew = new CashCycle();
                    cashCycleNew.CashierId = CashCycle.CashierId;
                    cashCycleNew.CycleNumber = CashCycle.CycleNumber + 1;
                    cashCycleNew.Closed = false;
                    cashCycleNew.Validate = false;
                    _cashCycleRepository.AddCashCycle(cashCycleNew);

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

        public void Transfer(TransferCashier transferCashier, string paymentMethod)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    transferCashier.Date = DateTime.Now.Date;
                    transferCashier.Time = DateTime.Now.ToString("HH:mm");
                    var tc = _cashierRepository.AddTransferCashier(transferCashier);

                    CashCycle cashCycleOrigin = _cashCycleRepository.GetCashCycleByCashierId(transferCashier.OriginCashierId);

                    CashCycle cashCycleDestiny = _cashCycleRepository.GetCashCycleByCashierId(transferCashier.DestinyCashierId);

                    TransferCashierPaymentMethod origin = new TransferCashierPaymentMethod();
                    origin.TransferCashierId = tc;
                    origin.PaymentMethodId = int.Parse(paymentMethod);
                    origin.Entry = false;
                    origin.Amount = transferCashier.Amount;
                    origin.Saving = cashCycleOrigin.CashierId.ToString(); 
                    origin.Cycle = cashCycleOrigin.CashCycleId.ToString();

                    TransferCashierPaymentMethod destiny = new TransferCashierPaymentMethod();
                    destiny.TransferCashierId = tc;
                    destiny.PaymentMethodId = int.Parse(paymentMethod);
                    destiny.Entry = true;
                    destiny.Amount = transferCashier.Amount;
                    destiny.Saving = cashCycleDestiny.CashierId.ToString();
                    destiny.Cycle = cashCycleDestiny.CashCycleId.ToString();

                    _cashierRepository.AddTransferCashierPaymentMethod(origin);
                    _cashierRepository.AddTransferCashierPaymentMethod(destiny);

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

        public void TransferPM(TransferCashier transferCashier, string paymentMethodOrigin, string paymentMethodDestiny)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    transferCashier.Date = DateTime.Now.Date;
                    transferCashier.Time = DateTime.Now.ToString("HH:mm");
                    var tc = _cashierRepository.AddTransferCashier(transferCashier);
                    CashCycle cashCycleOrigin = null;
                    CashCycle cashCycleDestiny = null;
                 
                    if (transferCashier.OriginCashierId != 0)
                        cashCycleOrigin = _cashCycleRepository.GetCashCycleByCashierId(transferCashier.OriginCashierId);

                  
                    if(transferCashier.DestinyCashierId != 0)
                        cashCycleDestiny = _cashCycleRepository.GetCashCycleByCashierId(transferCashier.DestinyCashierId);

                    TransferCashierPaymentMethod origin = new TransferCashierPaymentMethod();
                    origin.TransferCashierId = tc;
                    origin.PaymentMethodId = int.Parse(paymentMethodOrigin);
                    origin.Entry = false;
                    origin.Amount = transferCashier.Amount;
                    if (transferCashier.OriginCashierId != 0)
                    {
                        origin.Saving = cashCycleOrigin.CashierId.ToString();
                        origin.Cycle = cashCycleOrigin.CashCycleId.ToString();
                    }
                    else
                    {
                        origin.Saving = "0";
                        origin.Cycle = "0";
                    }

                    TransferCashierPaymentMethod destiny = new TransferCashierPaymentMethod();
                    destiny.TransferCashierId = tc;
                    destiny.PaymentMethodId = int.Parse(paymentMethodDestiny);
                    destiny.Entry = true;
                    destiny.Amount = transferCashier.Amount;
                    if (transferCashier.DestinyCashierId != 0)
                    {
                        destiny.Saving = cashCycleDestiny.CashierId.ToString();
                        destiny.Cycle = cashCycleDestiny.CashCycleId.ToString();
                    }
                    else
                    {
                        destiny.Saving = "0";
                        destiny.Cycle = "0";
                    }

                    _cashierRepository.AddTransferCashierPaymentMethod(origin);
                    _cashierRepository.AddTransferCashierPaymentMethod(destiny);

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
