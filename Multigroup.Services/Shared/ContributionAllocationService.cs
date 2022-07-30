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
    public class ContributionAllocationService
    {
        private ContributionAllocationRepository _contributionAllocationRepository;
        private UserRepository _userRepository;
        private PartnerRepository _partnerRepository;

        public ContributionAllocationService()
        {
            _contributionAllocationRepository = new ContributionAllocationRepository();
            _userRepository = new UserRepository();
            _partnerRepository = new PartnerRepository();
        }


        public ContributionAllocation GetContributionAllocationsById(int contributionAllocationId)
        {
            ContributionAllocation ganancia = _contributionAllocationRepository.GetContributionAllocationById(contributionAllocationId);

            List<ContributionAllocationPartner> Partners = new List<ContributionAllocationPartner>();


            foreach (ContributionAllocationPartner item in _contributionAllocationRepository.GetContributionAllocationPartners(ganancia.ContributionAllocationId))
            {
                ContributionAllocationPartner detalle = _contributionAllocationRepository.GetContributionAllocationPartner(item.ContributionAllocationPartnerId);
                detalle.Partner = _partnerRepository.GetPartnerById(detalle.PartnerId).Name;
                detalle.Percentage = _partnerRepository.GetPartnerById(detalle.PartnerId).Percentage;
                Partners.Add(detalle);
            }

            ganancia.Partners = Partners;

            return ganancia;
        }

        public IEnumerable<ContributionAllocation> GetContributionAllocations()
        {
            return _contributionAllocationRepository.GetContributionAllocations();
        }


        public IEnumerable<ContributionAllocationSummary> GetContributionAllocationSummary(int? SelectedUser, string DateFrom, string DateTo, string AmountFrom, string AmountTo)
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

            return _contributionAllocationRepository.GetContributionAllocationsSummary(SelectedUser.Value.ToString(), DateFrom, DateTo, AmountFrom, AmountTo);
        }

        public ContributionAllocationPartner GetContributionAllocationPartner(int contributionAllocationPartnerId)
        {
            return _contributionAllocationRepository.GetContributionAllocationPartner(contributionAllocationPartnerId);
        }

        public IEnumerable<ContributionAllocationPartner> GetContributionAllocationPartnersByParnerId(int partnerId)
        {

            List<ContributionAllocationPartner> list = new List<ContributionAllocationPartner>();
            var contribution = _contributionAllocationRepository.GetContributionAllocationPartnersByPartnerId(partnerId);

            foreach (ContributionAllocationPartner ear in contribution)
            {
                ear.Partner = _partnerRepository.GetPartnerById(ear.PartnerId).Name;
                ear.Concept = _contributionAllocationRepository.GetContributionAllocationById(ear.ContributionAllocationId).Concept;
                list.Add(ear);
            }

            return list;
        }

        public IEnumerable<ContributionAllocationPartner> GetContributionAllocationPartners(int contributionAllocationId)
        {
            return _contributionAllocationRepository.GetContributionAllocationPartners(contributionAllocationId);
        }

        public void AddContributionAllocation(ContributionAllocation ContributionAllocation)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    int contribution = _contributionAllocationRepository.AddContributionAllocation(ContributionAllocation);


                    foreach (ContributionAllocationPartner detail in ContributionAllocation.Partners)
                    {
                        detail.ContributionAllocationId = contribution;
                        detail.Balance = detail.Amount;
                        _contributionAllocationRepository.AddContributionAllocationPartner(detail);
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

        public void DeleteContributionAllocation(int contributionAllocationId)
        {
            _contributionAllocationRepository.DeleteContributionAllocation(contributionAllocationId);
            _contributionAllocationRepository.DeleteContributionAllocationPartnerByContributionAllocationId(contributionAllocationId);
        }

        public void UpdateContributionAllocation(ContributionAllocation ContributionAllocation)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _contributionAllocationRepository.UpdateContributionAllocation(ContributionAllocation);

                    _contributionAllocationRepository.DeleteContributionAllocationPartnerByContributionAllocationId(ContributionAllocation.ContributionAllocationId);

                    foreach (ContributionAllocationPartner detail in ContributionAllocation.Partners)
                    {
                        detail.ContributionAllocationId = ContributionAllocation.ContributionAllocationId;
                        detail.Balance = detail.Amount;
                        _contributionAllocationRepository.AddContributionAllocationPartner(detail);
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



        public void AddContributionAllocationDetail(ContributionAllocationPartner ContributionAllocationPartner)
        {
            _contributionAllocationRepository.AddContributionAllocationPartner(ContributionAllocationPartner);
        }

        public void DeleteContributionAllocationPartner(int contributionAllocationPartnerId)
        {
            _contributionAllocationRepository.DeleteContributionAllocationPartner(contributionAllocationPartnerId);
        }

        public void UpdateContributionAllocationDetail(ContributionAllocationPartner ContributionAllocationPartner)
        {
            _contributionAllocationRepository.UpdateContributionAllocationPartner(ContributionAllocationPartner);
        }

    }
}
