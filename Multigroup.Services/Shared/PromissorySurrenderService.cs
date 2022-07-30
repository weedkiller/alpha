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
    public class PromissorySurrenderService
    {
        private PromissorySurrenderRepository _promissorySurrenderRepository;
        private PromissoryRepository _promissoryRepository;
        private PaymentMethodsRepository _paymentMethodRepository;
        private CashierRepository _cashierRepository;
        private UserRepository _userRepository;
        private CashCycleRepository _cashCycleRepository;

        public PromissorySurrenderService()
        {
            _promissorySurrenderRepository = new PromissorySurrenderRepository();
            _promissoryRepository = new PromissoryRepository();
            _paymentMethodRepository = new PaymentMethodsRepository();
            _cashierRepository = new CashierRepository();
            _userRepository = new UserRepository();
            _cashCycleRepository = new CashCycleRepository();
        }


        public PromissorySurrender GetPromissorySurrendersById(int promissorySurrenderId)
        {
            PromissorySurrender rendicion = _promissorySurrenderRepository.GetPromissorySurrenderById(promissorySurrenderId);

            List<PromissorySurrenderMethod> Details = new List<PromissorySurrenderMethod>();

            List<PromissoryRendered> rendered = new List<PromissoryRendered>();

            foreach (PromissorySurrenderMethod item in _promissorySurrenderRepository.GetPromissorySurrenderMethodsBySurrender(rendicion.PromissorySurrenderId))
            {
                PromissorySurrenderMethod detalle = _promissorySurrenderRepository.GetPromissorySurrenderMethodById(item.PromissorySurrenderMethodId);
                detalle.PaymentMethod = _paymentMethodRepository.GetPaymentMethodsById(detalle.PaymentMethodId).Name;
                Details.Add(detalle);
            }

            List<Promissory> promisoriesAll = new List<Promissory>();
            foreach (PromissoryRendered item in _promissorySurrenderRepository.GetPromissoryRenderedBySurrender(rendicion.PromissorySurrenderId))
            {
                Promissory prom = _promissoryRepository.GetPromissoryWithPartnerAmount(item.PromissoryId, item.PromissorySurrenderId);
                promisoriesAll.Add(prom);
                rendered.Add(item);
            }

            rendicion.PaymentMethods = Details;
            rendicion.rendereds = rendered;
            rendicion.PromissoriesAll = promisoriesAll;

            return rendicion;
        }

        public IEnumerable<PromissorySurrender> GetPromissorySurrenders()
        {
            return _promissorySurrenderRepository.GetPromissorySurrenders();
        }


        public IEnumerable<PromissorySurrenderPartnerSummary> GetPromissorySurrendersPartnerSummary(int? SelectedPartner, int? SelectedUser, string DateFromPayment, string DateToPayment, string AmountFrom, string AmountTo, string Paid)
        {
            if (SelectedPartner == null)
                SelectedPartner = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromPayment == "")
                DateFromPayment = "01/01/1900";
            if (DateToPayment == "")
                DateToPayment = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            return _promissorySurrenderRepository.GetPromissorySurrendersPartnerSummary(SelectedPartner.Value.ToString(), SelectedUser.Value.ToString(), DateFromPayment, DateToPayment, AmountFrom, AmountTo, Paid);
        }

        public PromissorySurrenderPartner GetPromissorySurrenderPartnerById(int promissorySurrenderPartnerId)
        {
            return _promissorySurrenderRepository.GetPromissorySurrenderPartnerById(promissorySurrenderPartnerId);
        }


        public void AddPromissorySurrender(PromissorySurrender PromissorySurrender)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    int surrender = _promissorySurrenderRepository.AddPromissorySurrender(PromissorySurrender);

                    UserCashier userCashier = _userRepository.GetUserCashierByUserId(PromissorySurrender.UserId);

                    CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);

                    foreach (PromissorySurrenderMethod detail in PromissorySurrender.PaymentMethods)
                    {
                        detail.PromissorySurrenderId = surrender;
                        detail.Saving = userCashier.CashierId.ToString();
                        detail.Cycle = cashCycle.CashCycleId.ToString();
                        _promissorySurrenderRepository.AddPromissorySurrenderMethod(detail);
                    }

                    foreach (PromissoryRendered detail in PromissorySurrender.rendereds)
                    {
                        detail.PromissorySurrenderId = surrender;
                        _promissorySurrenderRepository.AddPromissoryRendered(detail);

                        PromissorySurrenderPartner partner = _promissorySurrenderRepository.GetPromissorySurrenderPartnerByPromissoryPartner(detail.PromissoryId, PromissorySurrender.PartnerId);
                        partner.IsPaid = true;
                        _promissorySurrenderRepository.UpdatePromissorySurrenderPartner(partner);
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

        public void DeletePromissorySurrender(int promissorySurrenderId)
        {
            _promissorySurrenderRepository.DeletePromissorySurrender(promissorySurrenderId);
        }

        public void UpdatePromissorySurrender(PromissorySurrender PromissorySurrender)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    UserCashier userCashier = _userRepository.GetUserCashierByUserId(PromissorySurrender.UserId);

                    CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);

                    _promissorySurrenderRepository.UpdatePromissorySurrender(PromissorySurrender);

                    _promissorySurrenderRepository.DeletePromissorySurrenderMethodBySurrenderId(PromissorySurrender.PromissorySurrenderId);
                    _promissorySurrenderRepository.DeletePromissoryRenderedBySurrenderId(PromissorySurrender.PromissorySurrenderId);

                    foreach (PromissorySurrenderMethod detail in PromissorySurrender.PaymentMethods)
                    {
                        detail.PromissorySurrenderMethodId = PromissorySurrender.PromissorySurrenderId;
                        detail.Saving = userCashier.CashierId.ToString();
                        detail.Cycle = cashCycle.CashCycleId.ToString();
                        _promissorySurrenderRepository.AddPromissorySurrenderMethod(detail);
                    }

                    foreach (PromissoryRendered detail in PromissorySurrender.rendereds)
                    {
                        detail.PromissorySurrenderId = PromissorySurrender.PromissorySurrenderId;
                        _promissorySurrenderRepository.AddPromissoryRendered(detail);

                        PromissorySurrenderPartner partner = _promissorySurrenderRepository.GetPromissorySurrenderPartnerByPromissory(detail.PromissoryId);
                        partner.IsPaid = true;
                        _promissorySurrenderRepository.UpdatePromissorySurrenderPartner(partner);
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
