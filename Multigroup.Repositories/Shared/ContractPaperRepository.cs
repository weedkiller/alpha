using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared.Mappers;
using Multigroup.Repositories.Shared.Resources;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Multigroup.Repositories.Shared
{
    public class ContractPaperRepository : BaseRepository
    {
        public ContractPaper GetContractPaperById(int contractPaperId)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaperContract_Get", ContractPaperMapper.Mapper, contractPaperId);

            return GetFirstElement(data);
        }


        public IEnumerable<ContractPaper> GetContractPapers()
        {
            var data = Db.ExecuteSprocAccessor("pub_PaperContract_GetList", ContractPaperMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<ContractPaperSummary> GetContractPaperByFilter(int? status, int? contractType)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaperContractByFilter_GetList", ContractPaperSummaryMapper.Mapper, status, contractType);

            return data.ToList();
        }

        public void Delete(int contractId)
        {
            var command = Db.GetStoredProcCommand("pub_PaperContract_Delete");
            Db.AddInParameter(command, "@ContractId", DbType.Int32, contractId);
            Db.ExecuteScalar(command);
        }

        public int AddContractPaper(ContractPaper contract)
        {
            var command = Db.GetStoredProcCommand("pub_PaperContract_Insert");
           
            Db.AddInParameter(command, "@Number", DbType.Int32, contract.Number);
            Db.AddInParameter(command, "@User", DbType.Int32, contract.User);
            Db.AddInParameter(command, "@Status", DbType.Int32, contract.Status);
            Db.AddInParameter(command, "@ContractType", DbType.Int32, contract.ContractType);
            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int Update(ContractPaper contract)
        {
            var command = Db.GetStoredProcCommand("pub_PaperContract_Update");

            Db.AddInParameter(command, "@PaperContractId", DbType.Int32, contract.PaperContractId);
            Db.AddInParameter(command, "@Number", DbType.Int32, contract.Number);
            Db.AddInParameter(command, "@ContractType", DbType.Int32, contract.ContractType);
            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public ContractPaper GetContractByNumber(int numero, string type)
        {
            var data = Db.ExecuteSprocAccessor("pub_PaperContractByNumber_GetList", ContractPaperMapper.Mapper, numero, type);

            return GetFirstElement(data);
        }

        public void UpdateContractPaperStatus(int contractId, int status, int? user)
        {
            var command = Db.GetStoredProcCommand("pub_PaperContractStatus_Update");

            Db.AddInParameter(command, "@PaperContractId", DbType.Int32, contractId);
            Db.AddInParameter(command, "@PaperStatus", DbType.Int32, status);
            Db.AddInParameter(command, "@User", DbType.Int32, user);

            Db.ExecuteScalar(command);
        }

    }

}
