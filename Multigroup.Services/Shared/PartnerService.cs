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
    public class PartnerService
    {
        private PartnerRepository _partnerRepository;

        public PartnerService()
        {
            _partnerRepository = new PartnerRepository();
        }

        public Partner GetPartner(int PartnerId)
        {
            return _partnerRepository.GetPartnerById(PartnerId);
        }

        public PercentageDefinition GetPercentageDefinitionById(int PercentageDefinitionId)
        {
            PercentageDefinition percentage = _partnerRepository.GetPercentageDefinitionById(PercentageDefinitionId);
            List<PercentageDefinitionPartner> Partners = new List<PercentageDefinitionPartner>();
            foreach (PercentageDefinitionPartner item in _partnerRepository.GetPercentageDefinitionPartners(percentage.PercentageDefinitionId))
            {
                PercentageDefinitionPartner partner = _partnerRepository.GetPercentageDefinitionPartner(item.PercentageDefinitionPartnerId);
                partner.Name = _partnerRepository.GetPartnerById(partner.PartnerId).Name;
                Partners.Add(partner);
            }

            percentage.Partners = Partners;

            return percentage;
        }

        public IEnumerable<Partner> GetPartners()
        {
            return _partnerRepository.GetPartners();
        }

        public IEnumerable<PercentageDefinition> GetPercentageDefinitions()
        {
            return _partnerRepository.GetPercentageDefinitions();
        }

        public void AddPartner(Partner partner)
        {
            _partnerRepository.AddPartner(partner);
        }

        public void AddPercentageDefinition(PercentageDefinition PercentageDefinition)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    int percentage = _partnerRepository.AddPercentageDefinition(PercentageDefinition);

                    foreach (PercentageDefinitionPartner partner in PercentageDefinition.Partners)
                    {
                        partner.PercentageDefinitionId = percentage;
                        _partnerRepository.AddPercentageDefinitionPartner(partner);
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

        public void DeletePercentageDefinition(int percentageDefinitionId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _partnerRepository.DeletePercentageDefinitionPartners(percentageDefinitionId);
                    _partnerRepository.DeletePercentageDefinition(percentageDefinitionId);
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

        public void UpdatePercentageDefinition(PercentageDefinition PercentageDefinition)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _partnerRepository.UpdatePercentageDefinition(PercentageDefinition);

                    _partnerRepository.DeletePercentageDefinitionPartners(PercentageDefinition.PercentageDefinitionId);
                    foreach (PercentageDefinitionPartner partner in PercentageDefinition.Partners)
                    {
                        partner.PercentageDefinitionId = PercentageDefinition.PercentageDefinitionId;
                        _partnerRepository.AddPercentageDefinitionPartner(partner);
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

        public void DeletePartners(int partnerId)
        {
            _partnerRepository.Delete(partnerId);
        }

        public void UpdatePartners(Partner partner)
        {
            _partnerRepository.Update(partner);
        }

        public IEnumerable<BalancePartner> GetBalancePartner(string PartnerId, string asignation)
        {
            return _partnerRepository.GetBalancePartner(PartnerId, asignation);
        }
    }
}
