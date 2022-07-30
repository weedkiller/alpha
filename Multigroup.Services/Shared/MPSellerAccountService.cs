using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mercadopago;
using System.Collections;

namespace Multigroup.Services.Shared
{
    public class MPSellerAccountService
    {
        private MPSellerAccountRepository _MPSellerAccountRepository;

        public MPSellerAccountService()
        {
            _MPSellerAccountRepository = new MPSellerAccountRepository();
        }

        public MPSellerAccount GetMPSellerAccount(int MPSellerAccountId)
        {
            return _MPSellerAccountRepository.GetMPSellerAccountById(MPSellerAccountId);
        }

        public MPSellerAccount GetMPSellerAccountsById(int MPSellerAccountId)
        {
            return _MPSellerAccountRepository.GetMPSellerAccountById(MPSellerAccountId);
        }

        public IEnumerable<MPSellerAccountSummary> GetMPSellerAccounts()
        {
            return _MPSellerAccountRepository.GetMPSellerAccounts();
        }
        public IEnumerable<MPSellerAccountSummary> GetWidget()
        {
            var data = _MPSellerAccountRepository.GetMPSellerAccounts();

            // METODO PARA TRAER DATOS DE MP
           //foreach (var dato in data)  
           // {
           //     MP mp = new MP(dato.MPAccountId.ToString(), dato.Token);

           //     Hashtable balance = mp.get("/users/215245850/mercadopago_account/balance", true);

           //     dato.Amount = 100;
           //     var datos = balance["response"];
           //     dato.Color = datos.ToString();
           // }

            return data;
        }
        public void AddMPSellerAccount(MPSellerAccount MPSellerAccount)
        {
            _MPSellerAccountRepository.AddMPSellerAccount(MPSellerAccount);
        }

        public void DeleteMPSellerAccounts(int MPSellerAccountId)
        {
            _MPSellerAccountRepository.Delete(MPSellerAccountId);
        }

        public void UpdateMPSellerAccounts(MPSellerAccount MPSellerAccount)
        {
            _MPSellerAccountRepository.Update(MPSellerAccount);
        }

        public MPSellerAccount GetNextMPSellerAccount(DateTime date)
        {
            return _MPSellerAccountRepository.GetNextMPSellerAccount(date);
        }

        public void AddMPPaymentInstallment(int mpSellerAccountId, int installmentId, DateTime date, Double amount)
        {
            MPPaymentInstallment mpPaymentInstallment = new MPPaymentInstallment();
            mpPaymentInstallment.MPSellerAccountId = mpSellerAccountId;
            mpPaymentInstallment.InstallmentId = installmentId;
            mpPaymentInstallment.date = date;
            mpPaymentInstallment.amount = amount;

            AddMPPaymentInstallment(mpPaymentInstallment);

        }

        public void AddMPPaymentInstallment(MPPaymentInstallment mpPaymentInstallment)
        {
            _MPSellerAccountRepository.AddMPPaymentInstallment(mpPaymentInstallment);
        }
    }
}
