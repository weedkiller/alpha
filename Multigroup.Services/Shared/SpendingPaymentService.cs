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
    public class SpendingPaymentService
    {
        private SpendingPaymentRepository _spendingPaymentRepository;
        private SpendingRepository _spendingRepository;
        private PaymentMethodsRepository _paymentMethodRepository;
        private CashierRepository _cashierRepository;
        private UserRepository _userRepository;
        private CashCycleRepository _cashCycleRepository;
        private ProviderRepository _providerRepository;

        public SpendingPaymentService()
        {
            _spendingPaymentRepository = new SpendingPaymentRepository();
            _spendingRepository = new SpendingRepository();
            _paymentMethodRepository = new PaymentMethodsRepository();
            _cashierRepository = new CashierRepository();
            _userRepository = new UserRepository();
            _cashCycleRepository = new CashCycleRepository();
            _providerRepository = new ProviderRepository();
        }


        public SpendingPayment GetSpendingPaymentsById(int spendingPaymentId)
        {
            SpendingPayment pago = _spendingPaymentRepository.GetSpendingPaymentById(spendingPaymentId);

            List<SpendingPaymentDetail> Details = new List<SpendingPaymentDetail>();

            List<SpendingPaymentPaymentMethod> payments = new List<SpendingPaymentPaymentMethod>();

            foreach (SpendingPaymentDetail item in _spendingPaymentRepository.GetSpendingPaymentDetails(pago.SpendingPaymentId))
            {
                SpendingPaymentDetail detalle = _spendingPaymentRepository.GetSpendingPaymentDetail(item.SpendingPaymentDetailId);
                detalle.Spending = _spendingRepository.GetSpendingById(detalle.SpendingId).Description;
                Details.Add(detalle);
            }

            foreach (SpendingPaymentPaymentMethod item in _spendingPaymentRepository.GetSpendingPaymentPaymentMethods(pago.SpendingPaymentId))
            {
                SpendingPaymentPaymentMethod detalle = _spendingPaymentRepository.GetSpendingPaymentPaymentMethod(item.SpendingPaymentPaymentMethodId);
                detalle.PaymentMethod = _paymentMethodRepository.GetPaymentMethodsById(detalle.PaymentMethodId).Name;
                payments.Add(detalle);
            }

            pago.Details = Details;
            pago.PaymentMethods = payments;

            return pago;
        }

        public IEnumerable<SpendingPayment> GetSpendingPayments()
        {
            return _spendingPaymentRepository.GetSpendingPayments();
        }

        public IEnumerable<SpendingPayment> GetSallaryPayments()
        {
            return _spendingPaymentRepository.GetSallaryPayments();
        }

        public IEnumerable<SpendingPaymentPaymentMethod> GetSpendingPaymentPaymentMethods(int spendingPaymentId)
        {
            return _spendingPaymentRepository.GetSpendingPaymentPaymentMethods(spendingPaymentId);
        }

        public SpendingPaymentPaymentMethod GetSpendingPaymentPaymentMethodById(int spendingPaymentPaymentMethodId)
        {
            return _spendingPaymentRepository.GetSpendingPaymentPaymentMethod(spendingPaymentPaymentMethodId);
        }

        public IEnumerable<SpendingPaymentPaymentMethod> GetSpendingPaymentPaymentMethodsByPM(int paymentMethodId)
        {
            return _spendingPaymentRepository.GetSpendingPaymentPaymentMethodsByPM(paymentMethodId);
        }

        public IEnumerable<SpendingPaymentSummary> GetSpendingPaymentSummary(int? SelectedProvider, int? SelectedUser, string DateFromExecution, string DateToExecution, string DateFromSystem, string DateToSystem, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance)
        {
            if (SelectedProvider == null)
                SelectedProvider = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromExecution == "")
                DateFromExecution = "01/01/1900";
            if (DateToExecution == "")
                DateToExecution = "01/01/2100";
            if (DateFromSystem == "")
                DateFromSystem = "01/01/1900";
            if (DateToSystem == "")
                DateToSystem = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            if (BalanceFrom == "")
                BalanceFrom = "-100000000";
            if (BalanceTo == "")
                BalanceTo = "100000000";
            if (Receipt == "")
                Receipt = "0";
            return _spendingPaymentRepository.GetSpendingPaymentsSummary(SelectedProvider.Value.ToString(), SelectedUser.Value.ToString(), DateFromExecution, DateToExecution, DateFromSystem, DateToSystem, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance);
        }

        public IEnumerable<SpendingPaymentDetailSummary> GetSpendingPaymentDetailSummary(int? SelectedProvider, int? SelectedUser, string DateFromExecution, string DateToExecution, string DateFromSystem, string DateToSystem, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance)
        {
            if (SelectedProvider == null)
                SelectedProvider = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromExecution == "")
                DateFromExecution = "01/01/1900";
            if (DateToExecution == "")
                DateToExecution = "01/01/2100";
            if (DateFromSystem == "")
                DateFromSystem = "01/01/1900";
            if (DateToSystem == "")
                DateToSystem = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            if (BalanceFrom == "")
                BalanceFrom = "-100000000";
            if (BalanceTo == "")
                BalanceTo = "100000000";
            if (Receipt == "")
                Receipt = "0";
            return _spendingPaymentRepository.GetSpendingPaymentsDetailSummary(SelectedProvider.Value.ToString(), SelectedUser.Value.ToString(), DateFromExecution, DateToExecution, DateFromSystem, DateToSystem, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance);
        }

        public IEnumerable<SpendingPaymentSummary> GetSallaryPaymentSummary(int? SelectedProvider, int? SelectedUser, string DateFromExecution, string DateToExecution, string DateFromSystem, string DateToSystem, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance)
        {
            if (SelectedProvider == null)
                SelectedProvider = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromExecution == "")
                DateFromExecution = "01/01/1900";
            if (DateToExecution == "")
                DateToExecution = "01/01/2100";
            if (DateFromSystem == "")
                DateFromSystem = "01/01/1900";
            if (DateToSystem == "")
                DateToSystem = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            if (BalanceFrom == "")
                BalanceFrom = "-100000000";
            if (BalanceTo == "")
                BalanceTo = "100000000";
            if (Receipt == "")
                Receipt = "0";
            return _spendingPaymentRepository.GetSallaryPaymentsSummary(SelectedProvider.Value.ToString(), SelectedUser.Value.ToString(), DateFromExecution, DateToExecution, DateFromSystem, DateToSystem, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance);
        }

        public IEnumerable<SpendingPaymentDetailSummary> GetSallaryPaymentDetailSummary(int? SelectedProvider, int? SelectedUser, string DateFromExecution, string DateToExecution, string DateFromSystem, string DateToSystem, string AmountFrom, string AmountTo, string BalanceFrom, string BalanceTo, string Receipt, string PositiveBalance)
        {
            if (SelectedProvider == null)
                SelectedProvider = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromExecution == "")
                DateFromExecution = "01/01/1900";
            if (DateToExecution == "")
                DateToExecution = "01/01/2100";
            if (DateFromSystem == "")
                DateFromSystem = "01/01/1900";
            if (DateToSystem == "")
                DateToSystem = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            if (BalanceFrom == "")
                BalanceFrom = "-100000000";
            if (BalanceTo == "")
                BalanceTo = "100000000";
            if (Receipt == "")
                Receipt = "0";
            return _spendingPaymentRepository.GetSallaryPaymentsDetailSummary(SelectedProvider.Value.ToString(), SelectedUser.Value.ToString(), DateFromExecution, DateToExecution, DateFromSystem, DateToSystem, AmountFrom, AmountTo, BalanceFrom, BalanceTo, Receipt, PositiveBalance);
        }

        public SpendingPaymentDetail GetSpendingPaymentDetail(int spendingPaymentDetailId)
        {
            return _spendingPaymentRepository.GetSpendingPaymentDetail(spendingPaymentDetailId);
        }

        public IEnumerable<SpendingPaymentDetail> GetSpendingPaymentDetails(int spendingPaymentId)
        {
            return _spendingPaymentRepository.GetSpendingPaymentDetails(spendingPaymentId);
        }

        public void AddSpendingPayment(SpendingPayment SpendingPayment)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    SpendingPayment.SpendingPaymentTypeId = (int)SpendingPaymentType.SpendingPayment;
                    int spendingPayment = _spendingPaymentRepository.AddSpendingPayment(SpendingPayment);

                    foreach (SpendingPaymentDetail detail in SpendingPayment.Details)
                    {
                        detail.SpendingPaymentId = spendingPayment;
                        _spendingPaymentRepository.AddSpendingPaymentDetail(detail);

                        Spending spending = _spendingRepository.GetSpendingById(detail.SpendingId);
                        spending.Balance = spending.Balance - detail.Total;
                        _spendingRepository.UpdateSpending(spending);
                    }

                    UserCashier userCashier = _userRepository.GetUserCashierByUserId(SpendingPayment.UserId);

                    CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);

                    foreach (SpendingPaymentPaymentMethod payment in SpendingPayment.PaymentMethods)
                    {
                        payment.SpendingPaymentId = spendingPayment;
                        payment.Saving = userCashier.CashierId.ToString();
                        payment.Cycle = cashCycle.CashCycleId.ToString();
                        _spendingPaymentRepository.AddSpendingPaymentPaymentMethod(payment);
                    }

                    if (SpendingPayment.ProveedorId.HasValue)
                    {
                        Provider provider = _providerRepository.GetProviderById(SpendingPayment.ProveedorId.Value);
                        provider.Balance = provider.Balance - SpendingPayment.Amount;
                        _providerRepository.UpdateProvider(provider);
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

        public void AddSallaryPayment(SpendingPayment SpendingPayment)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    UserCashier userCashier = _userRepository.GetUserCashierByUserId(SpendingPayment.UserId);

                    CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);

                    SpendingPayment.SpendingPaymentTypeId = (int)SpendingPaymentType.SallaryPayment;
                    int spendingPayment = _spendingPaymentRepository.AddSpendingPayment(SpendingPayment);

                    Provider provider = _providerRepository.GetProviderById(SpendingPayment.ProveedorId.Value);
                    provider.Balance = provider.Balance - SpendingPayment.Amount;
                    _providerRepository.UpdateProvider(provider);

                    foreach (SpendingPaymentDetail detail in SpendingPayment.Details)
                    {
                        detail.SpendingPaymentId = spendingPayment;
                        _spendingPaymentRepository.AddSpendingPaymentDetail(detail);

                        Spending spending = _spendingRepository.GetSpendingById(detail.SpendingId);
                        spending.Balance = spending.Balance - detail.Total;
                        _spendingRepository.UpdateSpending(spending);
                    }

                    foreach (SpendingPaymentPaymentMethod payment in SpendingPayment.PaymentMethods)
                    {
                        payment.SpendingPaymentId = spendingPayment;
                        payment.Saving = userCashier.CashierId.ToString();
                        payment.Cycle = cashCycle.CashCycleId.ToString();
                        _spendingPaymentRepository.AddSpendingPaymentPaymentMethod(payment);
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

        public void DeleteSpendingPayment(SpendingPayment spendingPayment)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    foreach (SpendingPaymentDetail spd in spendingPayment.Details)
                    {
                        Spending spen = _spendingRepository.GetSpendingById(spd.SpendingId);
                        spen.Balance = spen.Balance + spd.Total;
                        _spendingRepository.UpdateSpending(spen);
                    }

                    _spendingPaymentRepository.DeleteSpendingPayment(spendingPayment.SpendingPaymentId);
                    _spendingPaymentRepository.DeleteSpendingPaymentDetailBySpendingPaymentId(spendingPayment.SpendingPaymentId);
                    _spendingPaymentRepository.DeleteSpendingPaymentPMBySpendingPaymentId(spendingPayment.SpendingPaymentId);

                    if (spendingPayment.ProveedorId.HasValue)
                    {
                        Provider provider = _providerRepository.GetProviderById(spendingPayment.ProveedorId.Value);
                        provider.Balance = provider.Balance + spendingPayment.Amount;
                        _providerRepository.UpdateProvider(provider);
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

        public void UpdateSpendingPayment(SpendingPayment SpendingPayment)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    SpendingPayment old = _spendingPaymentRepository.GetSpendingPaymentById(SpendingPayment.SpendingPaymentId);

                    SpendingPayment.SpendingPaymentTypeId = (int)SpendingPaymentType.SpendingPayment;

                    UserCashier userCashier = _userRepository.GetUserCashierByUserId(SpendingPayment.UserId);



                    foreach (SpendingPaymentDetail spd in _spendingPaymentRepository.GetSpendingPaymentDetails(old.SpendingPaymentId))
                    {
                        Spending spen = _spendingRepository.GetSpendingById(spd.SpendingId);
                        spen.Balance = spen.Balance + spd.Total;
                        _spendingRepository.UpdateSpending(spen);
                    }

                    _spendingPaymentRepository.DeleteSpendingPaymentDetailBySpendingPaymentId(SpendingPayment.SpendingPaymentId);
                    _spendingPaymentRepository.DeleteSpendingPaymentPMBySpendingPaymentId(SpendingPayment.SpendingPaymentId);

                    if (SpendingPayment.ProveedorId.HasValue)
                    {
                        Provider provider = _providerRepository.GetProviderById(SpendingPayment.ProveedorId.Value);
                        provider.Balance = provider.Balance + old.Amount;
                        _providerRepository.UpdateProvider(provider);
                    }

                    _spendingPaymentRepository.UpdateSpendingPayment(SpendingPayment);

                    foreach (SpendingPaymentDetail detail in SpendingPayment.Details)
                    {
                        detail.SpendingPaymentId = SpendingPayment.SpendingPaymentId;
                        _spendingPaymentRepository.AddSpendingPaymentDetail(detail);

                        Spending spending = _spendingRepository.GetSpendingById(detail.SpendingId);
                        spending.Balance = spending.Balance - detail.Total;
                        _spendingRepository.UpdateSpending(spending);
                    }


                    CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);

                    foreach (SpendingPaymentPaymentMethod payment in SpendingPayment.PaymentMethods)
                    {
                        payment.SpendingPaymentId = SpendingPayment.SpendingPaymentId;
                        payment.Saving = userCashier.CashierId.ToString();
                        payment.Cycle = cashCycle.CashCycleId.ToString();
                        _spendingPaymentRepository.AddSpendingPaymentPaymentMethod(payment);
                    }

                    if (SpendingPayment.ProveedorId.HasValue)
                    {
                        Provider provider = _providerRepository.GetProviderById(SpendingPayment.ProveedorId.Value);
                        provider.Balance = provider.Balance - SpendingPayment.Amount;
                        _providerRepository.UpdateProvider(provider);
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

        public void UpdateSpendingPaymentShort(SpendingPayment SpendingPayment)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    SpendingPayment.SpendingPaymentTypeId = (int)SpendingPaymentType.SpendingPayment;

                    _spendingPaymentRepository.UpdateSpendingPayment(SpendingPayment);

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

        public void UpdateSallaryPayment(SpendingPayment SpendingPayment)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    SpendingPayment old = _spendingPaymentRepository.GetSpendingPaymentById(SpendingPayment.SpendingPaymentId);


                    SpendingPayment.SpendingPaymentTypeId = (int)SpendingPaymentType.SallaryPayment;

                    UserCashier userCashier = _userRepository.GetUserCashierByUserId(SpendingPayment.UserId);

                    foreach (SpendingPaymentDetail spd in _spendingPaymentRepository.GetSpendingPaymentDetails(old.SpendingPaymentId))
                    {
                        Spending spen = _spendingRepository.GetSpendingById(spd.SpendingId);
                        spen.Balance = spen.Balance + spd.Total;
                        _spendingRepository.UpdateSpending(spen);
                    }

                    _spendingPaymentRepository.DeleteSpendingPaymentDetailBySpendingPaymentId(SpendingPayment.SpendingPaymentId);
                    _spendingPaymentRepository.DeleteSpendingPaymentPMBySpendingPaymentId(SpendingPayment.SpendingPaymentId);

                        Provider provider = _providerRepository.GetProviderById(SpendingPayment.ProveedorId.Value);
                        provider.Balance = provider.Balance + old.Amount;
                        _providerRepository.UpdateProvider(provider);


                    _spendingPaymentRepository.UpdateSpendingPayment(SpendingPayment);



                    provider.Balance = provider.Balance - SpendingPayment.Amount;
                    _providerRepository.UpdateProvider(provider);

                    foreach (SpendingPaymentDetail detail in SpendingPayment.Details)
                    {
                        detail.SpendingPaymentId = SpendingPayment.SpendingPaymentId;
                        _spendingPaymentRepository.AddSpendingPaymentDetail(detail);

                        Spending spending = _spendingRepository.GetSpendingById(detail.SpendingId);
                        spending.Balance = spending.Balance - detail.Total;
                        _spendingRepository.UpdateSpending(spending);
                    }
                    CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);

                    foreach (SpendingPaymentPaymentMethod payment in SpendingPayment.PaymentMethods)
                    {
                        payment.SpendingPaymentId = SpendingPayment.SpendingPaymentId;
                        payment.Saving = userCashier.CashierId.ToString();
                        payment.Cycle = cashCycle.CashCycleId.ToString();
                        _spendingPaymentRepository.AddSpendingPaymentPaymentMethod(payment);
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

        public void UpdateSallaryPaymentShort(SpendingPayment SpendingPayment)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    SpendingPayment.SpendingPaymentTypeId = (int)SpendingPaymentType.SallaryPayment;

                    _spendingPaymentRepository.UpdateSpendingPayment(SpendingPayment);
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

        public void AddSpendingPaymentDetail(SpendingPaymentDetail SpendingPaymentDetail)
        {
            _spendingPaymentRepository.AddSpendingPaymentDetail(SpendingPaymentDetail);
        }

        public void DeleteSpendingPaymentDetail(int spendingPaymentDetailId)
        {
            _spendingPaymentRepository.DeleteSpendingPaymentDetail(spendingPaymentDetailId);
        }

        public void UpdateSpendingPaymentDetail(SpendingPaymentDetail SpendingPaymentDetail)
        {
            _spendingPaymentRepository.UpdateSpendingPaymentDetail(SpendingPaymentDetail);
        }

        public void AddSpendingPaymentPaymentMethod(SpendingPaymentPaymentMethod SpendingPaymentPaymentMethod)
        {
            _spendingPaymentRepository.AddSpendingPaymentPaymentMethod(SpendingPaymentPaymentMethod);
        }

        public void DeleteSpendingPaymentPaymentMethod(int spendingPaymentPaymentMethodId)
        {
            _spendingPaymentRepository.DeleteSpendingPaymentPaymentMethod(spendingPaymentPaymentMethodId);
        }

        public void UpdateSpendingPaymentPaymentMethod(SpendingPaymentPaymentMethod SpendingPaymentPaymentMethod)
        {
            _spendingPaymentRepository.UpdateSpendingPaymentPaymentMethod(SpendingPaymentPaymentMethod);
        }

        public void AddImputSpending(int spendingPaymentId, double amount, List<SpendingPaymentDetail> list)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    SpendingPayment sp = _spendingPaymentRepository.GetSpendingPaymentById(spendingPaymentId);
                    sp.Balance = sp.Balance - amount;
                    sp.SpendingPaymentTypeId = (int)SpendingPaymentType.SpendingPayment;
                    _spendingPaymentRepository.UpdateSpendingPayment(sp);

                    foreach (SpendingPaymentDetail spd in list)
                    {
                        spd.SpendingPaymentId = spendingPaymentId;
                        _spendingPaymentRepository.AddSpendingPaymentDetail(spd);

                        Spending spending = _spendingRepository.GetSpendingById(spd.SpendingId);
                        spending.Balance = spending.Balance - spd.Total;
                        _spendingRepository.UpdateSpending(spending);
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

        public void AddImputSallarySpending(int spendingPaymentId, double amount, List<SpendingPaymentDetail> list)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    SpendingPayment sp = _spendingPaymentRepository.GetSpendingPaymentById(spendingPaymentId);
                    sp.Balance = sp.Balance - amount;
                    sp.SpendingPaymentTypeId = (int)SpendingPaymentType.SallaryPayment;
                    _spendingPaymentRepository.UpdateSpendingPayment(sp);

                    foreach (SpendingPaymentDetail spd in list)
                    {
                        spd.SpendingPaymentId = spendingPaymentId;
                        _spendingPaymentRepository.AddSpendingPaymentDetail(spd);

                        Spending spending = _spendingRepository.GetSpendingById(spd.SpendingId);
                        spending.Balance = spending.Balance - spd.Total;
                        _spendingRepository.UpdateSpending(spending);
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

        public bool IsEditable(int spendingPaymentId)
        {
            bool editable = true;
            foreach (SpendingPaymentPaymentMethod pm in _spendingPaymentRepository.GetSpendingPaymentPaymentMethods(spendingPaymentId))
            {

                CashCycle caja = _cashCycleRepository.GetCashCycleByCashierId(int.Parse(pm.Saving));

                if (caja.CashCycleId != int.Parse(pm.Cycle))
                    editable = false;


                if (pm.ReconciledId != null)
                    editable = false;

                PaymentMethodVerification verif = _paymentMethodRepository.GetPaymentMethodVerificationBySpendingPayment(pm.SpendingPaymentPaymentMethodId);
                if (verif != null)
                    editable = false;
            }
            return editable;
        }

        public bool IsEditableSallary(int sallaryPaymentId)
        {
            bool editable = true;
            foreach (SpendingPaymentPaymentMethod pm in _spendingPaymentRepository.GetSpendingPaymentPaymentMethods(sallaryPaymentId))
            {

                CashCycle caja = _cashCycleRepository.GetCashCycleByCashierId(int.Parse(pm.Saving));

                if (caja.CashCycleId != int.Parse(pm.Cycle))
                    editable = false;


                if (pm.ReconciledId != null)
                    editable = false;

                PaymentMethodVerification verif = _paymentMethodRepository.GetPaymentMethodVerificationBySallaryPayment(pm.SpendingPaymentPaymentMethodId);
                if (verif != null)
                    editable = false;
            }
            return editable;
        }
    }
}
