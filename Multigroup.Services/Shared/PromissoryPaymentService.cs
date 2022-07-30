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
    public class PromissoryPaymentService
    {
        private PromissoryPaymentRepository _promissoryPaymentRepository;
        private PromissoryRepository _promissoryRepository;
        private PaymentMethodsRepository _paymentMethodRepository;
        private PromissorySurrenderRepository _promissorySurrenderRepository;
        private CashierRepository _cashierRepository;
        private UserRepository _userRepository;
        private CashCycleRepository _cashCycleRepository;

        public PromissoryPaymentService()
        {
            _promissoryPaymentRepository = new PromissoryPaymentRepository();
            _paymentMethodRepository = new PaymentMethodsRepository();
            _promissoryRepository = new PromissoryRepository();
            _promissorySurrenderRepository = new PromissorySurrenderRepository();
            _cashierRepository = new CashierRepository();
            _userRepository = new UserRepository();
            _cashCycleRepository = new CashCycleRepository();
        }


        public PromissoryPayment GetPromissoryPaymentsById(int promissoryPaymentId)
        {
            PromissoryPayment pago = _promissoryPaymentRepository.GetPromissoryPaymentById(promissoryPaymentId);

            List<PromissoryPaymentMethod> Details = new List<PromissoryPaymentMethod>();

            List<PromissoryPaymentPromissory> promisories = new List<PromissoryPaymentPromissory>();

            foreach (PromissoryPaymentMethod item in _promissoryPaymentRepository.GetPromissoryPaymentMethodByPayment(pago.PromissoryPaymentId))
            {
                PromissoryPaymentMethod detalle = _promissoryPaymentRepository.GetPromissoryPaymentMethod(item.PromissoryPaymentMethodId);
                detalle.PaymentMethod = _paymentMethodRepository.GetPaymentMethodsById(detalle.PaymentMethodId).Name;
                Details.Add(detalle);
            }
            List<Promissory> promisoriesAll = new List<Promissory>();
            foreach (PromissoryPaymentPromissory item in _promissoryPaymentRepository.GetPromissoryPaymentPromissoryByPayment(pago.PromissoryPaymentId))
            {
                Promissory prom = _promissoryRepository.GetPromissoryById(item.PromissoryId);
                promisoriesAll.Add(prom);
                promisories.Add(item);
            
            }


            pago.PaymentMethods = Details;
            pago.Promissories = promisories;
            pago.PromissoriesAll = promisoriesAll;
            return pago;
        }

        public IEnumerable<PromissoryPayment> GetPromissoryPayments()
        {
            return _promissoryPaymentRepository.GetPromissoryPayments();
        }


        public IEnumerable<PromissoryPaymentSummary> GetPromissoryPaymentsSummary(int? SelectedClient, int? SelectedUser, string DateFromSystem, string DateToSystem, string DateFromPayment, string DateToPayment, string AmountFrom, string AmountTo)
        {
            if (SelectedClient == null)
                SelectedClient = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromSystem == "")
                DateFromSystem = "01/01/1900";
            if (DateToSystem == "")
                DateToSystem = "01/01/2100";
            if (DateFromPayment == "")
                DateFromPayment = "01/01/1900";
            if (DateToPayment == "")
                DateToPayment = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            return _promissoryPaymentRepository.GetPromissoryPaymentsSummary(SelectedClient.Value.ToString(), SelectedUser.Value.ToString(), DateFromSystem, DateToSystem, DateFromPayment, DateToPayment, AmountFrom, AmountTo);
        }

        public IEnumerable<PromissorySummary> GetPromissoryPaymentPromissories(int? SelectedClient, int? SelectedUser, string DateFromSystem, string DateToSystem, string DateFromPayment, string DateToPayment, string AmountFrom, string AmountTo)
        {
            if (SelectedClient == null)
                SelectedClient = 0;
            if (SelectedUser == null)
                SelectedUser = 0;
            if (DateFromSystem == "")
                DateFromSystem = "01/01/1900";
            if (DateToSystem == "")
                DateToSystem = "01/01/2100";
            if (DateFromPayment == "")
                DateFromPayment = "01/01/1900";
            if (DateToPayment == "")
                DateToPayment = "01/01/2100";
            if (AmountFrom == "")
                AmountFrom = "-100000000";
            if (AmountTo == "")
                AmountTo = "100000000";
            return _promissoryPaymentRepository.GetPromissoryPaymentPromissories(SelectedClient.Value.ToString(), SelectedUser.Value.ToString(), DateFromSystem, DateToSystem, DateFromPayment, DateToPayment, AmountFrom, AmountTo);
        }

        public PromissoryPaymentMethod GetPromissoryPaymentMethod(int promissoryPaymentMethodId)
        {
            return _promissoryPaymentRepository.GetPromissoryPaymentMethod(promissoryPaymentMethodId);
        }

        public IEnumerable<PromissoryPaymentMethod> GetPromissoryPaymentMethods()
        {
            return _promissoryPaymentRepository.GetPromissoryPaymentMethods();
        }

        public PromissoryPaymentPromissory GetPromissoryPaymentPromissory(int promissoryPaymentPromissoryId)
        {
            return _promissoryPaymentRepository.GetPromissoryPaymentPromissory(promissoryPaymentPromissoryId);
        }

        public IEnumerable<PromissoryPaymentPromissory> GetPromissoryPaymentPromissory()
        {
            return _promissoryPaymentRepository.GetPromissoryPaymentromissorys();
        }

        public IEnumerable<PromissorySurrenderPartner> GetPromissorySurrenderPartnerPaidByPayment(int PromissoryPayment)
        {
            return _promissorySurrenderRepository.GetPromissorySurrenderPartnerPaidByPayment(PromissoryPayment);
        }

        public void AddPromissoryPayment(PromissoryPayment PromissoryPayment)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {

                    UserCashier userCashier = _userRepository.GetUserCashierByUserId(PromissoryPayment.UserId);

                    CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);

                    int payment = _promissoryPaymentRepository.AddPromissoryPayment(PromissoryPayment);

                    foreach(PromissoryPaymentMethod detail in PromissoryPayment.PaymentMethods)
                    {
                        detail.PromissoryPaymentId = payment;
                        detail.Saving = userCashier.CashierId.ToString();
                        detail.Cycle = cashCycle.CashCycleId.ToString();
                        _promissoryPaymentRepository.AddPromissoryPaymentMethod(detail);
                    }

                    foreach (PromissoryPaymentPromissory detail in PromissoryPayment.Promissories)
                    {
                        detail.PromissoryPaymentId = payment;
                        _promissoryPaymentRepository.AddPromissoryPaymentPromissory(detail);

                        Promissory promissory = _promissoryRepository.GetPromissoryById(detail.PromissoryId);
                        promissory.isPaid = true;
                        _promissoryRepository.UpdatePromissory(promissory);

                        foreach (PromissoryPartner detailPromi in _promissoryRepository.GetPromissoryPartners(detail.PromissoryId))
                        {
                            PromissorySurrenderPartner partner = new PromissorySurrenderPartner();
                            partner.PartnerId = detailPromi.PartnerId;
                            partner.PromissoryId = detail.PromissoryId;
                            partner.Amount = detailPromi.Amount;
                            partner.PromissoryPaymentId = payment;
                            _promissorySurrenderRepository.AddPromissorySurrenderPartner(partner);
                        }
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

        public void DeletePromissoryPayment(int promissoryPaymentId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    PromissoryPayment promissory = GetPromissoryPaymentsById(promissoryPaymentId);
                    foreach (PromissoryPaymentPromissory detail in promissory.Promissories)
                    {
                        Promissory promiss = _promissoryRepository.GetPromissoryById(detail.PromissoryId);
                        promiss.isPaid = false;
                        _promissoryRepository.UpdatePromissory(promiss);
                    }
                    _promissoryPaymentRepository.DeletePromissoryPayment(promissoryPaymentId);
                    _promissoryPaymentRepository.DeletePromissoryPaymentMethod(promissoryPaymentId);
                    _promissoryPaymentRepository.DeletePromissoryPaymentPromissory(promissoryPaymentId);
                    _promissorySurrenderRepository.DeletePromissorySurrenderPartner(promissoryPaymentId);

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


        public void UpdatePromissoryPayment(PromissoryPayment PromissoryPayment)
        {

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    UserCashier userCashier = _userRepository.GetUserCashierByUserId(PromissoryPayment.UserId);

                    CashCycle cashCycle = _cashCycleRepository.GetCashCycleByCashierId(userCashier.CashierId);

                    _promissoryPaymentRepository.UpdatePromissoryPayment(PromissoryPayment);

                    _promissoryPaymentRepository.DeletePromissoryPaymentMethod(PromissoryPayment.PromissoryPaymentId);
                    _promissoryPaymentRepository.DeletePromissoryPaymentPromissory(PromissoryPayment.PromissoryPaymentId);
                    _promissorySurrenderRepository.DeletePromissorySurrenderPartner(PromissoryPayment.PromissoryPaymentId);

                    foreach (PromissoryPaymentMethod detail in PromissoryPayment.PaymentMethods)
                    {
                        detail.PromissoryPaymentId = PromissoryPayment.PromissoryPaymentId;
                        detail.Saving = userCashier.CashierId.ToString();
                        detail.Cycle = cashCycle.CashCycleId.ToString();
                        _promissoryPaymentRepository.AddPromissoryPaymentMethod(detail);
                    }

                    foreach (PromissoryPaymentPromissory detail in PromissoryPayment.Promissories)
                    {
                        detail.PromissoryPaymentId = PromissoryPayment.PromissoryPaymentId;
                        _promissoryPaymentRepository.AddPromissoryPaymentPromissory(detail);

                        foreach (PromissoryPartner detailPromi in _promissoryRepository.GetPromissoryPartners(detail.PromissoryId))
                        {
                            PromissorySurrenderPartner partner = new PromissorySurrenderPartner();
                            partner.PartnerId = detailPromi.PartnerId;
                            partner.PromissoryId = detail.PromissoryId;
                            partner.Amount = detailPromi.Amount;
                            partner.PromissoryPaymentId = PromissoryPayment.PromissoryPaymentId;
                            _promissorySurrenderRepository.AddPromissorySurrenderPartner(partner);
                        }
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
