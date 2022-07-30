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
    public class RetirementPartnerService
    {
        private RetirementPartnerRepository _retirementPartnerRepository;
        private EarningsAllocationRepository _earningsAllocationRepository;
        private PaymentMethodsRepository _paymentMethodRepository;
        private PromissorySurrenderRepository _promissorySurrenderRepository;
        private CashierRepository _cashierRepository;
        private UserRepository _userRepository;
        private CashCycleRepository _cashCycleRepository;

        public RetirementPartnerService()
        {
            _retirementPartnerRepository = new RetirementPartnerRepository();
            _paymentMethodRepository = new PaymentMethodsRepository();
            _earningsAllocationRepository = new EarningsAllocationRepository();
            _promissorySurrenderRepository = new PromissorySurrenderRepository();
            _cashierRepository = new CashierRepository();
            _userRepository = new UserRepository();
            _cashCycleRepository = new CashCycleRepository();
        }


        public RetirementPartner GetRetirementPartnersById(int retirementPartnerId)
        {
            RetirementPartner retirement = _retirementPartnerRepository.GetRetirementPartnerById(retirementPartnerId);

            List<RetirementPartnerPaymentMethod> Details = new List<RetirementPartnerPaymentMethod>();

            List<RetirementPartnerAllocation> allocations = new List<RetirementPartnerAllocation>();

            foreach (RetirementPartnerPaymentMethod item in _retirementPartnerRepository.GetRetirementPartnerPaymentMethodByRet(retirement.RetirementPartnerId))
            {
                RetirementPartnerPaymentMethod detalle = _retirementPartnerRepository.GetRetirementPartnerPaymentMethod(item.RetirementPartnerPaymentMethodId);
                detalle.PaymentMethod = _paymentMethodRepository.GetPaymentMethodsById(detalle.PaymentMethodId).Name;
                Details.Add(detalle);
            }
            List<EarningsAllocationPartner> allocationsAll = new List<EarningsAllocationPartner>();
            foreach (RetirementPartnerAllocation item in _retirementPartnerRepository.GetRetirementPartnerAllocationByRet(retirement.RetirementPartnerId))
            {
                EarningsAllocationPartner ear = _earningsAllocationRepository.GetEarningsAllocationPartner(item.EarningsAllocationPartnerId);
                ear.Concept = _earningsAllocationRepository.GetEarningsAllocationById(ear.EarningsAllocationId).Concept;
                allocationsAll.Add(ear);
                allocations.Add(item);
            
            }


            retirement.PaymentMethods = Details;
            retirement.Allocations = allocations;
            retirement.AllocationsAll = allocationsAll;
            return retirement;
        }

        public IEnumerable<RetirementPartner> GetRetirementPartners()
        {
            return _retirementPartnerRepository.GetRetirementPartners();
        }


        public IEnumerable<RetirementPartnerSummary> GetRetirementPartnersSummary(int? SelectedPartner, int? SelectedUser, string DateFromSystem, string DateToSystem, string DateFrom, string DateTo, string AmountFrom, string AmountTo)
        {
            if (SelectedPartner == null)
                SelectedPartner = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromSystem == "")
                DateFromSystem = "01/01/1900";
            if (DateToSystem == "")
                DateToSystem = "01/01/2100";
            if (DateFrom == "")
                DateFrom = "01/01/1900";
            if (DateTo == "")
                DateTo = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            return _retirementPartnerRepository.GetRetirementPartnersSummary(SelectedPartner.Value.ToString(), SelectedUser.Value.ToString(), DateFromSystem, DateToSystem, DateFrom, DateTo, AmountFrom, AmountTo);
        }
  

        public RetirementPartnerPaymentMethod GetRetirementPartnerPaymentMethod(int retirementPartnerPaymentMethodId)
        {
            return _retirementPartnerRepository.GetRetirementPartnerPaymentMethod(retirementPartnerPaymentMethodId);
        }

        public IEnumerable<RetirementPartnerPaymentMethod> GetRetirementPartnerPaymentMethods()
        {
            return _retirementPartnerRepository.GetRetirementPartnerPaymentMethods();
        }

        public RetirementPartnerAllocation GetRetirementPartnerAllocation(int retirementPartnerAllocationId)
        {
            return _retirementPartnerRepository.GetRetirementPartnerAllocation(retirementPartnerAllocationId);
        }

        public IEnumerable<RetirementPartnerAllocation> GetRetirementPartnerAllocation()
        {
            return _retirementPartnerRepository.GetRetirementPartnerAllocations();
        }

        public void AddRetirementPartner(RetirementPartner RetirementPartner)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    UserCashier userCashier = _userRepository.GetUserCashierByUserId(RetirementPartner.UserId);

                    CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);

                    int payment = _retirementPartnerRepository.AddRetirementPartner(RetirementPartner);

                    foreach(RetirementPartnerPaymentMethod detail in RetirementPartner.PaymentMethods)
                    {
                        detail.RetirementPartnerId = payment;
                        detail.Saving = userCashier.CashierId.ToString();
                        detail.Cycle = cashCycle.CashCycleId.ToString();
                        _retirementPartnerRepository.AddRetirementPartnerPaymentMethod(detail);
                    }

                    foreach (RetirementPartnerAllocation detail in RetirementPartner.Allocations)
                    {
                        EarningsAllocationPartner earning = _earningsAllocationRepository.GetEarningsAllocationPartner(detail.EarningsAllocationPartnerId);
                        earning.Balance = 0;
                        _earningsAllocationRepository.UpdateEarningsAllocationPartner(earning);

                        detail.RetirementPartnerId = payment;
                        detail.Amount = earning.Amount;
                        _retirementPartnerRepository.AddRetirementPartnerAllocation(detail);
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

        public void DeleteRetirementPartner(int retirementPartnerId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    RetirementPartner retirement = GetRetirementPartnersById(retirementPartnerId);
                    foreach (RetirementPartnerAllocation detail in retirement.Allocations)
                    {
                        EarningsAllocationPartner ear = _earningsAllocationRepository.GetEarningsAllocationPartner(detail.EarningsAllocationPartnerId);
                        ear.Balance = ear.Amount;
                        _earningsAllocationRepository.UpdateEarningsAllocationPartner(ear);
                    }
                    _retirementPartnerRepository.DeleteRetirementPartner(retirementPartnerId);
                    _retirementPartnerRepository.DeleteRetirementPartnerPaymentMethod(retirementPartnerId);
                    _retirementPartnerRepository.DeleteRetirementPartnerAllocation(retirementPartnerId);


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
