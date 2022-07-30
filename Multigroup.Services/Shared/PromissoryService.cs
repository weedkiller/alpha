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
    public class PromissoryService
    {
        private PromissoryRepository _promissoryRepository;
        private PartnerRepository _partnerRepository;

        public PromissoryService()
        {
            _promissoryRepository = new PromissoryRepository();
            _partnerRepository = new PartnerRepository();
        }


        public Promissory GetPromissorysById(int promissoryId)
        {
            Promissory pagare = _promissoryRepository.GetPromissoryById(promissoryId);

            return pagare;
        }

        public IEnumerable<Promissory> GetPromissorys()
        {
            return _promissoryRepository.GetPromissorys();
        }

        public IEnumerable<Promissory> GetPromissoriesNotPaid(int clientId)
        {
            return _promissoryRepository.GetPromissorys().Where(x => x.isPaid == false).Where(x => x.ClientId == clientId).OrderBy(y => y.CollectionDate);
        }

        public IEnumerable<Promissory> GetPromissoriesByPartnerNotPaid(int partnerId)
        {
            return _promissoryRepository.GetPromissoriesByPartnerNotPaid(partnerId).OrderBy(y => y.CollectionDate);
        }

        public IEnumerable<PromissorySummary> GetPromissorysSummary(int? SelectedClient, int? SelectedUser, string DateFromBroadcast, string DateToBroadcast, string DateFromCollection, string DateToCollection, string AmountFrom, string AmountTo, string Paid, string Number)
        {
            if (SelectedClient == null)
                SelectedClient = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromBroadcast == "")
                DateFromBroadcast = "01/01/1900";
            if (DateToBroadcast == "")
                DateToBroadcast = "01/01/2100";
            if (DateFromCollection == "")
                DateFromCollection = "01/01/1900";
            if (DateToCollection == "")
                DateToCollection = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            if (Number == "")
                Number = "0";
            return _promissoryRepository.GetPromissorysSummary(SelectedClient.Value.ToString(), SelectedUser.Value.ToString(), DateFromBroadcast, DateToBroadcast, DateFromCollection, DateToCollection, AmountFrom, AmountTo, Paid, Number);
        }

        public PromissoryPartner GetPromissoryDetail(int promissoryPartnerId)
        {
            return _promissoryRepository.GetPromissoryPartner(promissoryPartnerId);
        }

        public IEnumerable<PromissoryPartner> GetPromissoryPartners(int promissoryId)
        {
            return _promissoryRepository.GetPromissoryPartners(promissoryId);
        }

        public void AddPromissory(Promissory Promissory)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    int promissory = _promissoryRepository.AddPromissory(Promissory);

                    foreach(PercentageDefinitionPartner detail in _partnerRepository.GetPercentageDefinitionPartners(Promissory.PercentageDefinitionId))
                    {
                        PromissoryPartner partner = new PromissoryPartner();
                        partner.PartnerId = detail.PartnerId;
                        partner.PromissoryId = promissory;
                        partner.Amount = Promissory.Amount * detail.Percentage / 100;
                        _promissoryRepository.AddPromissoryPartner(partner);
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

        public void DeletePromissory(int promissoryId)
        {
            _promissoryRepository.DeletePromissory(promissoryId);
        }

        public void UpdatePromissory(Promissory Promissory)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    _promissoryRepository.UpdatePromissory(Promissory);

                    _promissoryRepository.DeletePromissoryPartner(Promissory.PromissoryId);

                    foreach (PercentageDefinitionPartner detail in _partnerRepository.GetPercentageDefinitionPartners(Promissory.PercentageDefinitionId))
                    {
                        PromissoryPartner partner = new PromissoryPartner();
                        partner.PartnerId = detail.PartnerId;
                        partner.PromissoryId = Promissory.PromissoryId;
                        partner.Amount = Promissory.Amount * detail.Percentage / 100;
                        _promissoryRepository.AddPromissoryPartner(partner);
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
    }
}
