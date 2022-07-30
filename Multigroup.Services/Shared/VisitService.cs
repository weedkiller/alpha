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
    public class VisitService
    {
        private VisitRepository _visitRepository;
        private UserService _userService;
        private PaymentRepository _paymentRepository;
        private ContractRepository _contractRepository;
        private CashierRepository _cashierRepository;
        private UserRepository _userRepository;
        private CashCycleRepository _cashCycleRepository;

        public VisitService()
        {
            _visitRepository = new VisitRepository();
            _paymentRepository = new PaymentRepository();
            _userService = new UserService();
            _contractRepository = new ContractRepository();
            _cashierRepository = new CashierRepository();
            _userRepository = new UserRepository();
            _cashCycleRepository = new CashCycleRepository();
        }
        public IEnumerable<AssignVisit> GetAssignVisitByZoneAndPaymentDate(int? ZoneId, int? PaymentDateId, int UserId)
        {
            return _visitRepository.GetAssignVisitByZoneAndPaymentDate(ZoneId, PaymentDateId,  UserId);
        }
        public IEnumerable<VisitSummary> GetVisitsByFilters(string status, string zones, string collectors, string DateTo, string DateFrom)
        {
            return _visitRepository.GetVisitsByFilters(status, zones, collectors, DateTo, DateFrom);
        }

        public IEnumerable<AgencyPaymentHistory> GetAgencyPaymentHistoryByCustomer(int customerId)
        {
            return _visitRepository.GetAgencyPaymentHistoryByCustomer(customerId);
        }

        public IEnumerable<AgencyPaymentHistory> GetAgencyPaymentHistoryByCustomerLastMonth(int customerId)
        {
            return _visitRepository.GetAgencyPaymentHistoryByCustomer(customerId);
        }
        
        public void ConfirmStatus(int VisitId)
        {
            _visitRepository.ChangeStatus(VisitId, 16);
        }

        public void CancelStatus(int VisitId)
        {
            _visitRepository.ChangeStatus(VisitId, 12);
        }

        public void GenerateAgency(int CustomerId, DateTime? AgencyDate, string observationsAgency, int userId)
        {
            AgencyPayment agencyPayment = new AgencyPayment();
            agencyPayment.CustomerId = CustomerId;
            agencyPayment.ScheduledPaymentDate = AgencyDate;
            agencyPayment.Observations = observationsAgency;
            _visitRepository.InsertAgencyPayment(agencyPayment, userId);
        }
        public void GenerateVisit(int CustomerId, DateTime? ScheduledVisitDate, double AmountOwed, double CommissionRate, int ZoneId, string observationsCollector, int userId)
        {
            Visit visit = new Visit();
            visit.CustomerId = CustomerId;
            visit.ScheduledVisitDate = ScheduledVisitDate;
            visit.ConcretedVisitDate = null;
            visit.CollectorId = _userService.GetZoneCollector(ZoneId).UserId;
            visit.CollectorComission = CommissionRate;
            visit.AmountOwed = AmountOwed;
            visit.AmountCollected = null;
            visit.TotalComission = null;
            visit.Status = 10;
            visit.CollectorExpensesDesc = null;
            visit.CollectorExpensesAmount = null;
            visit.Observations = null;
            _visitRepository.Insert(visit, observationsCollector, userId);
        }

        public void RepeatAsignation(int monthToRepeat, int yearToRepeat, int monthToApply, int yearToApply)
        {

            _visitRepository.RepeatAsignation(monthToRepeat, yearToRepeat, monthToApply, yearToApply);
        }

        public Contract GetContractByCustomerId(int customerId)
        {
           return _contractRepository.GetContractByCustomerId(customerId);
        }

        public void CollectorVisit(int VisitId, DateTime? ConcretedVisitDate, double AmountCollected, int status, string CollectorExpensesDesc, double CollectorExpensesAmount, string Observations, string ReceipNumber, int? contractId, int user)
        {
            Visit visit = _visitRepository.GetVisitById(VisitId);

            visit.ConcretedVisitDate = ConcretedVisitDate;
            visit.AmountCollected = AmountCollected;
            visit.TotalComission = AmountCollected * visit.CollectorComission / 100;
            visit.Status = status;
            visit.CollectorExpensesDesc = CollectorExpensesDesc;
            visit.CollectorExpensesAmount = CollectorExpensesAmount;
            visit.Observations = Observations;
            _visitRepository.Update(visit);
            PaymentService payment = new PaymentService();
            Payment pago = new Payment();
            pago.PaymentDate = DateTime.Now.Date;
            pago.PaymentMethod = "6";
            pago.type = "1";
            pago.Observations = Observations;
            pago.ReceipNumber = ReceipNumber;
            pago.Amount = AmountCollected;
            if (contractId != null)
                pago.ContractNumber = contractId.Value;
            else
                pago.ContractNumber = _contractRepository.GetContractByCustomerId(visit.CustomerId).ContractId;
            payment.Insert(pago, user);
        }

        public IEnumerable<AssignPaymentPreferenceCollector> GetAssignPaymentPreferenceSummaryPaymentCollector(string Telemarketer, string Collector, string Zone, string Customer, string DateVisitFrom, string DateVisitTo, string DatePaymentFrom, string DatePaymentTo, string IsSurrender, string IsAssign, string IsRegister)
        {
            return _visitRepository.GetAssignPaymentPreferenceSummaryPaymentCollector(Telemarketer, Collector, Zone, Customer, DateVisitFrom, DateVisitTo, DatePaymentFrom, DatePaymentTo, IsSurrender, IsAssign, IsRegister);
        }

        public AssignPaymentPreferenceCollector GetAssignPaymentPreferenceCollectorById(int id)
        {
            return _visitRepository.GetAssignPaymentPreferenceCollectorById(id);
        }

        public IEnumerable<CollectorAssigmmentSummary> GetCollectorAssigmmentsByAssignPaymentAssign(int id)
        {
            return _visitRepository.GetCollectorAssigmmentsByAssignPaymentAssign(id);
        }

        public void AddCollectorAssigmment(CollectorAssigmment collector)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _visitRepository.UpdateCollectorAssigmmentStatus(collector);
                    _visitRepository.AddCollectorAssigmment(collector);
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

        public void AddCollectorSurrender(CollectorSurrender rendicion, IEnumerable<AssignPaymentPreferenceCollector> asignaciones, IEnumerable<CollectorSurrenderPM> PaymentMethods)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    UserCashier userCashier = _userRepository.GetUserCashierByUserId(rendicion.UserId);

                    CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);

                    var rend = _visitRepository.AddCollectorSurrender(rendicion);
                    foreach (AssignPaymentPreferenceCollector asig in asignaciones)
                    {
                        InstallmentSurrender cuota = new InstallmentSurrender();
                        cuota.Amount = asig.Amount;
                        cuota.CollectorSurrenderId = rend;
                        cuota.AssignPaymentPreferenceId = asig.AssignPaymentPreferenceId;
                        _visitRepository.AddInstallmentSurrender(cuota);
                    }

                    foreach (CollectorSurrenderPM payment in PaymentMethods)
                    {
                        payment.CollectorSurrenderId = rend;
                        payment.Saving = userCashier.CashierId.ToString();
                        payment.Cycle = cashCycle.CashCycleId.ToString();
                        _visitRepository.AddCollectorSurrenderPM(payment);
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

        public void AddCollectorAdvanced(CollectorAdvancement avance, int pmethod)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    var rendiciones = GetCollectorSurrendorByUser(avance.CollectorId);

                    var monto = avance.Amount;


                    foreach (CollectorSurrender rendicion in rendiciones)
                    {
                        double totalPm = 0;

                        if (rendicion.Balance > monto)
                        {
                            rendicion.Balance = rendicion.Balance - monto;
                            totalPm = monto;
                            monto = monto - rendicion.Balance;
                        }
                        else
                        {
                            monto = monto - rendicion.Balance;
                            rendicion.Balance = 0;
                            totalPm = rendicion.Balance;
                        }
                        

                        CollectorSurrenderPM pm = new CollectorSurrenderPM();
                        pm.Amount = totalPm;
                        pm.PaymentMethodId = pmethod;

                        UpdateCollectorSurrender(rendicion, pm);

                        if (monto <= 0)
                            break;
                    }

                    if (monto > 0)
                    {
                        avance.Amount = monto;
                        avance.Balance = monto;
                        CollectorAdvancementPM pma = new CollectorAdvancementPM();
                        pma.Amount = monto;
                        pma.PaymentMethodId = pmethod;

                        int av = _visitRepository.AddCollectorAdvancement(avance);
                        pma.CollectorAdvancementId = av;
                        _visitRepository.AddCollectorAdvancementPM(pma);
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

        public void AddImputSurrender(CollectorAdvancement imputacion)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    var rendiciones = GetCollectorSurrendorByUser(imputacion.CollectorId);

                    var avances = GetCollectorAdvancementByUser(imputacion.CollectorId);
                    

                    var monto = imputacion.Amount;
                    var montoA = imputacion.Amount;


                    foreach (CollectorSurrender rendicion in rendiciones)
                    {

                        if (rendicion.Balance > monto)
                        {
                            rendicion.Balance = rendicion.Balance - monto;
                            monto = monto - rendicion.Balance;
                        }
                        else
                        {
                            monto = monto - rendicion.Balance;
                            rendicion.Balance = 0;
                        }

                        _visitRepository.UpdateCollectorSurrender(rendicion);

                        if (monto <= 0)
                            break;
                    }

                    foreach (CollectorAdvancement avance in avances)
                    {


                        if (avance.Balance > montoA)
                        {
                            avance.Balance = avance.Balance - montoA;
                            montoA = montoA - avance.Balance;
                        }
                        else
                        {
                            montoA = montoA - avance.Balance;
                            avance.Balance = 0;
                        }



                        _visitRepository.UpdateCollectorAdvancement(avance);

                        if (montoA <= 0)
                            break;
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

        public void UpdateCollectorSurrender(CollectorSurrender rendicion, CollectorSurrenderPM payment)
        {
                try
                {
                    UserCashier userCashier = _userRepository.GetUserCashierByUserId(rendicion.UserId);

                    CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);

                    _visitRepository.UpdateCollectorSurrender(rendicion);


                if (payment != null)
                {

                    payment.CollectorSurrenderId = rendicion.UserId;
                    payment.Saving = userCashier.CashierId.ToString();
                    payment.Cycle = cashCycle.CashCycleId.ToString();
                    payment.CollectorSurrenderId = rendicion.CollectorSurrenderId;
                    _visitRepository.AddCollectorSurrenderPM(payment);
                }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
        }

        public IEnumerable<CollectorState> GetCollectorState()
        {
            return _visitRepository.GetCollectorState();
        }

        public InstallmentSurrender GetInstallmentSurrenderById(int id)
        {
            return _visitRepository.GetInstallmentSurrenderById(id);
        }

        public IEnumerable<CollectorSurrender> GetCollectorSurrendorByUser(int id)
        {
            return _visitRepository.GetCollectorSurrendorByUser(id);
        }

        public IEnumerable<CollectorAdvancement> GetCollectorAdvancementByUser(int id)
        {
            return _visitRepository.GetCollectorAdvancementByUser(id);
        }

        public IEnumerable<InstallmentSurrenderSummary> GetInstallmentSurrenderSummary(string collector)
        {
            return _visitRepository.GetInstallmentSurrenderSummary(collector);
        }
    }
};
