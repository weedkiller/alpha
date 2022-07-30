using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Multigroup.Services.Shared
{
    public class ContractPaperService
    {
        private ContractPaperRepository _contractPaperRepository;

        public ContractPaperService()
        {
            _contractPaperRepository = new ContractPaperRepository();
        }

        public ContractPaper GetContractPaper(int PaperContractId)
        {
            return _contractPaperRepository.GetContractPaperById(PaperContractId);
        }


        public IEnumerable<ContractPaper> GetContractPapers()
        {
            return _contractPaperRepository.GetContractPapers();
        }

        public IEnumerable<ContractPaperSummary> GetContractByFilter(int? status, int? contractType)
        {
            return _contractPaperRepository.GetContractPaperByFilter(status, contractType);
        }

        public bool AddContractPaper(ContractPaper contract, int desde, int hasta, string contractType)
        {
            bool existe = false;
            for (int i = desde; i < hasta + 1; i++)
            {
                if (ExisteContractPaperByNumer(i, contractType))
                {
                    existe = true;
                }
                else
                {
                    contract.Status = 3;
                    contract.User = null;
                    contract.Number = i;
                    contract.ContractType = int.Parse(contractType);
                    int contrato = _contractPaperRepository.AddContractPaper(contract);
                }
                
            }
            return existe;
        }

        public void UpdateContractPaperStatus(int contractId, int status, int? user)
        {
            _contractPaperRepository.UpdateContractPaperStatus(contractId, status, user);
        }

        public bool ExisteContractPaperByNumer(int Number, string type)
        {
            ContractPaper clientes = _contractPaperRepository.GetContractByNumber(Number, type);
            
            bool existe = false;
            if (clientes != null)
            {
                existe = true;
            }
            return existe;
        }

        public void UpdateContractPaper(ContractPaper contract)
        {
            _contractPaperRepository.Update(contract);
        }

        public void DeleteContractPaper(int contractPaperId)
        {
            _contractPaperRepository.Delete(contractPaperId);
        }

    }
}
