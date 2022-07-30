using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Multigroup.Services.Shared
{
    public class ReceipService
    {
        private ReceipRepository _ReceipRepository;

        public ReceipService()
        {
            _ReceipRepository = new ReceipRepository();
        }

        public Receip GetReceip(int receipId)
        {
            return _ReceipRepository.GetReceipById(receipId);
        }

        public Receip GetReceipByNumber(string number)
        {
            return _ReceipRepository.GetReceipByNumber(number);
        }


        public IEnumerable<Receip> GetReceips()
        {
            return _ReceipRepository.GetReceips();
        }


        public IEnumerable<ReceipSummary> GetContractByFilter(int? status)
        {
            return _ReceipRepository.GetReceipByFilter(status);
        }

        public bool AddReceip(Receip contract, long desde, long hasta)
        {
            bool existe = false;
            for (long i = desde; i < hasta + 1; i++)
            {
                if (ExisteReceipByNumer(i))
                {
                    existe = true;
                }
                else
                {
                    contract.Status = 3;
                    contract.User = null;
                    contract.Number = i.ToString();
                    int contrato = _ReceipRepository.AddReceip(contract);
                }
                
            }
            return existe;
        }

        public void UpdateReceipStatus(int receipId, int status, int? user)
        {
            _ReceipRepository.UpdateReceipStatus(receipId, status, user);
        }

        public bool ExisteReceipByNumer(long Number)
        {
            Receip clientes = _ReceipRepository.GetContractByNumber(Number);
            
            bool existe = false;
            if (clientes != null)
            {
                existe = true;
            }
            return existe;
        }

        public void UpdateReceip(Receip contract)
        {
            _ReceipRepository.Update(contract);
        }

        public void DeleteReceip(int ReceipId)
        {
            _ReceipRepository.Delete(ReceipId);
        }

    }
}
