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
    public class CashierService
    {
        private CashierRepository _CashierRepository;
        private CashCycleRepository _cashCycleRepository;

        public CashierService()
        {
            _CashierRepository = new CashierRepository();
            _cashCycleRepository = new CashCycleRepository();
        }


        public Cashier GetCashiersById(int CashierId)
        {
            return _CashierRepository.GetCashierById(CashierId);
        }

        public IEnumerable<Cashier> GetCashiers()
        {
            return _CashierRepository.GetCashiers();
        }

        public Cashier GetCashiersByUserId(int userId)
        {
            return _CashierRepository.GetCashiersByUserId(userId);
        }

        public void AddCashier(Cashier Cashier)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    int cashier = _CashierRepository.AddCashier(Cashier);
                    CashCycle cashCycle = new CashCycle();
                    cashCycle.CashierId = cashier;
                    cashCycle.CycleNumber = 1;
                    cashCycle.Closed = false;
                    cashCycle.Validate = false;
                    _cashCycleRepository.AddCashCycle(cashCycle);
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

        public void DeleteCashier(int CashierId)
        {
            _CashierRepository.DeleteCashier(CashierId);
        }

        public void UpdateCashier(Cashier Cashier)
        {
            _CashierRepository.UpdateCashier(Cashier);

        }
    }
}
