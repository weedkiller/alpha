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
    public class EarningsAllocationService
    {
        private EarningsAllocationRepository _earningsAllocationRepository;
        private UserRepository _userRepository;
        private PartnerRepository _partnerRepository;

        public EarningsAllocationService()
        {
            _earningsAllocationRepository = new EarningsAllocationRepository();
            _userRepository = new UserRepository();
            _partnerRepository = new PartnerRepository();
        }


        public EarningsAllocation GetEarningsAllocationsById(int earningsAllocationId)
        {
            EarningsAllocation ganancia = _earningsAllocationRepository.GetEarningsAllocationById(earningsAllocationId);

            List<EarningsAllocationPartner> Partners = new List<EarningsAllocationPartner>();


            foreach (EarningsAllocationPartner item in _earningsAllocationRepository.GetEarningsAllocationPartners(ganancia.EarningsAllocationId))
            {
                EarningsAllocationPartner detalle = _earningsAllocationRepository.GetEarningsAllocationPartner(item.EarningsAllocationPartnerId);
                detalle.Partner = _partnerRepository.GetPartnerById(detalle.PartnerId).Name;
                detalle.Percentage = _partnerRepository.GetPartnerById(detalle.PartnerId).Percentage;
                Partners.Add(detalle);
            }

            ganancia.Partners = Partners;

            return ganancia;
        }

        public IEnumerable<EarningsAllocation> GetEarningsAllocations()
        {
            return _earningsAllocationRepository.GetEarningsAllocations();
        }


        public IEnumerable<EarningsAllocationSummary> GetEarningsAllocationSummary(int? SelectedUser, string DateFrom, string DateTo, string AmountFrom, string AmountTo)
        {
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFrom == "")
                DateFrom = "01/01/1900";
            if (DateTo == "")
                DateTo = "01/01/2100";        
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            
            return _earningsAllocationRepository.GetEarningsAllocationsSummary(SelectedUser.Value.ToString(), DateFrom, DateTo, AmountFrom, AmountTo);
        }     

        public EarningsAllocationPartner GetEarningsAllocationPartner(int earningsAllocationPartnerId)
        {
            return _earningsAllocationRepository.GetEarningsAllocationPartner(earningsAllocationPartnerId);
        }

        public IEnumerable<EarningsAllocationPartner> GetEarningsAllocationPartners(int earningsAllocationId)
        {
            return _earningsAllocationRepository.GetEarningsAllocationPartners(earningsAllocationId);
        }

        public IEnumerable<EarningsAllocationPartner> GetEarningsAllocationPartnersByParnerId(int partnerId)
        {

            List<EarningsAllocationPartner> list = new List<EarningsAllocationPartner>();
            var earnings = _earningsAllocationRepository.GetEarningsAllocationPartnersByPartnerId(partnerId);

            foreach (EarningsAllocationPartner ear in earnings)
            {
                ear.Partner = _partnerRepository.GetPartnerById(ear.PartnerId).Name;
                ear.Concept = _earningsAllocationRepository.GetEarningsAllocationById(ear.EarningsAllocationId).Concept;
                list.Add(ear);
            }

            return list;
        }

        public void AddEarningsAllocation(EarningsAllocation EarningsAllocation)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                  
                    int earnings = _earningsAllocationRepository.AddEarningsAllocation(EarningsAllocation);
                  

                    foreach (EarningsAllocationPartner detail in EarningsAllocation.Partners)
                    {
                        detail.EarningsAllocationId = earnings;
                        detail.Balance = detail.Amount;
                        _earningsAllocationRepository.AddEarningsAllocationPartner(detail);
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

        public void DeleteEarningsAllocation(int earningsAllocationId)
        {
            _earningsAllocationRepository.DeleteEarningsAllocation(earningsAllocationId);
            _earningsAllocationRepository.DeleteEarningsAllocationPartnerByEarningsAllocationId(earningsAllocationId);
        }

        public void UpdateEarningsAllocation(EarningsAllocation EarningsAllocation)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _earningsAllocationRepository.UpdateEarningsAllocation(EarningsAllocation);

                    _earningsAllocationRepository.DeleteEarningsAllocationPartnerByEarningsAllocationId(EarningsAllocation.EarningsAllocationId);
               
                    foreach (EarningsAllocationPartner detail in EarningsAllocation.Partners)
                    {
                        detail.EarningsAllocationId = EarningsAllocation.EarningsAllocationId;
                        detail.Balance = detail.Amount;
                        _earningsAllocationRepository.AddEarningsAllocationPartner(detail);
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



        public void AddEarningsAllocationDetail(EarningsAllocationPartner EarningsAllocationPartner)
        {
            _earningsAllocationRepository.AddEarningsAllocationPartner(EarningsAllocationPartner);
        }

        public void DeleteEarningsAllocationPartner(int earningsAllocationPartnerId)
        {
            _earningsAllocationRepository.DeleteEarningsAllocationPartner(earningsAllocationPartnerId);
        }

        public void UpdateEarningsAllocationDetail(EarningsAllocationPartner EarningsAllocationPartner)
        {
            _earningsAllocationRepository.UpdateEarningsAllocationPartner(EarningsAllocationPartner);
        }

    }
}
