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
    public class IncomePartnerService
    {
        private IncomePartnerRepository _incomePartnerRepository;
        private ContributionAllocationRepository _contributionAllocationRepository;
        private PaymentMethodsRepository _paymentMethodRepository;
        private PromissorySurrenderRepository _promissorySurrenderRepository;
        private CashierRepository _cashierRepository;
        private UserRepository _userRepository;
        private CashCycleRepository _cashCycleRepository;

        public IncomePartnerService()
        {
            _incomePartnerRepository = new IncomePartnerRepository();
            _paymentMethodRepository = new PaymentMethodsRepository();
            _contributionAllocationRepository = new ContributionAllocationRepository();
            _promissorySurrenderRepository = new PromissorySurrenderRepository();
            _cashierRepository = new CashierRepository();
            _userRepository = new UserRepository();
            _cashCycleRepository = new CashCycleRepository();
        }


        public IncomePartner GetIncomePartnersById(int incomePartnerId)
        {
            IncomePartner income = _incomePartnerRepository.GetIncomePartnerById(incomePartnerId);

            List<IncomePartnerPaymentMethod> Details = new List<IncomePartnerPaymentMethod>();

            List<IncomePartnerAllocation> allocations = new List<IncomePartnerAllocation>();

            foreach (IncomePartnerPaymentMethod item in _incomePartnerRepository.GetIncomePartnerPaymentMethodByRet(income.IncomePartnerId))
            {
                IncomePartnerPaymentMethod detalle = _incomePartnerRepository.GetIncomePartnerPaymentMethod(item.IncomePartnerPaymentMethodId);
                detalle.PaymentMethod = _paymentMethodRepository.GetPaymentMethodsById(detalle.PaymentMethodId).Name;
                Details.Add(detalle);
            }
            List<ContributionAllocationPartner> allocationsAll = new List<ContributionAllocationPartner>();
            foreach (IncomePartnerAllocation item in _incomePartnerRepository.GetIncomePartnerAllocationByRet(income.IncomePartnerId))
            {
                ContributionAllocationPartner ear = _contributionAllocationRepository.GetContributionAllocationPartner(item.ContributionAllocationPartnerId);
                ear.Concept = _contributionAllocationRepository.GetContributionAllocationById(ear.ContributionAllocationId).Concept;
                allocationsAll.Add(ear);
                allocations.Add(item);
            
            }


            income.PaymentMethods = Details;
            income.Allocations = allocations;
            income.AllocationsAll = allocationsAll;
            return income;
        }

        public IEnumerable<IncomePartner> GetIncomePartners()
        {
            return _incomePartnerRepository.GetIncomePartners();
        }


        public IEnumerable<IncomePartnerSummary> GetIncomePartnersSummary(int? SelectedPartner, int? SelectedUser, string DateFromSystem, string DateToSystem, string DateFrom, string DateTo, string AmountFrom, string AmountTo)
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
            return _incomePartnerRepository.GetIncomePartnersSummary(SelectedPartner.Value.ToString(), SelectedUser.Value.ToString(), DateFromSystem, DateToSystem, DateFrom, DateTo, AmountFrom, AmountTo);
        }
  

        public IncomePartnerPaymentMethod GetIncomePartnerPaymentMethod(int incomePartnerPaymentMethodId)
        {
            return _incomePartnerRepository.GetIncomePartnerPaymentMethod(incomePartnerPaymentMethodId);
        }

        public IEnumerable<IncomePartnerPaymentMethod> GetIncomePartnerPaymentMethods()
        {
            return _incomePartnerRepository.GetIncomePartnerPaymentMethods();
        }

        public IncomePartnerAllocation GetIncomePartnerAllocation(int incomePartnerAllocationId)
        {
            return _incomePartnerRepository.GetIncomePartnerAllocation(incomePartnerAllocationId);
        }

        public IEnumerable<IncomePartnerAllocation> GetIncomePartnerAllocation()
        {
            return _incomePartnerRepository.GetIncomePartnerAllocations();
        }

        public void AddIncomePartner(IncomePartner IncomePartner)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    UserCashier userCashier = _userRepository.GetUserCashierByUserId(IncomePartner.UserId);

                    CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);

                    int payment = _incomePartnerRepository.AddIncomePartner(IncomePartner);

                    foreach(IncomePartnerPaymentMethod detail in IncomePartner.PaymentMethods)
                    {
                        detail.IncomePartnerId = payment;
                        detail.Saving = userCashier.CashierId.ToString();
                        detail.Cycle = cashCycle.CashCycleId.ToString();
                        _incomePartnerRepository.AddIncomePartnerPaymentMethod(detail);
                    }

                    foreach (IncomePartnerAllocation detail in IncomePartner.Allocations)
                    {
                        ContributionAllocationPartner contribution = _contributionAllocationRepository.GetContributionAllocationPartner(detail.ContributionAllocationPartnerId);
                        contribution.Balance = 0;
                        _contributionAllocationRepository.UpdateContributionAllocationPartner(contribution);

                        detail.IncomePartnerId = payment;
                        detail.Amount = contribution.Amount;
                        _incomePartnerRepository.AddIncomePartnerAllocation(detail);
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

        public void DeleteIncomePartner(int incomePartnerId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    IncomePartner income = GetIncomePartnersById(incomePartnerId);
                    foreach (IncomePartnerAllocation detail in income.Allocations)
                    {
                        ContributionAllocationPartner ear = _contributionAllocationRepository.GetContributionAllocationPartner(detail.ContributionAllocationPartnerId);
                        ear.Balance = ear.Amount;
                        _contributionAllocationRepository.UpdateContributionAllocationPartner(ear);
                    }
                    _incomePartnerRepository.DeleteIncomePartner(incomePartnerId);
                    _incomePartnerRepository.DeleteIncomePartnerPaymentMethod(incomePartnerId);
                    _incomePartnerRepository.DeleteIncomePartnerAllocation(incomePartnerId);


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
