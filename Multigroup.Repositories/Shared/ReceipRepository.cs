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
    public class ReceipRepository : BaseRepository
    {
        public Receip GetReceipById(int ReceipId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Receip_Get", ReceipMapper.Mapper, ReceipId);

            return GetFirstElement(data);
        }

        public Receip GetReceipByNumber(string number)
        {
            var data = Db.ExecuteSprocAccessor("pub_ReceipByNumber_Get", ReceipMapper.Mapper, number);

            return GetFirstElement(data);
        }


        public IEnumerable<Receip> GetReceips()
        {
            var data = Db.ExecuteSprocAccessor("pub_Receip_GetList", ReceipMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<ReceipSummary> GetReceipByFilter(int? status)
        {
            var data = Db.ExecuteSprocAccessor("pub_ReceipByFilter_GetList", ReceipSummaryMapper.Mapper, status);

            return data.ToList();
        }

        public void Delete(int contractId)
        {
            var command = Db.GetStoredProcCommand("pub_Receip_Delete");
            Db.AddInParameter(command, "@ReceipId", DbType.Int32, contractId);
            Db.ExecuteScalar(command);
        }

        public int AddReceip(Receip contract)
        {
            var command = Db.GetStoredProcCommand("pub_Receip_Insert");
           
            Db.AddInParameter(command, "@Number", DbType.String, contract.Number);
            Db.AddInParameter(command, "@User", DbType.Int32, contract.User);
            Db.AddInParameter(command, "@Status", DbType.Int32, contract.Status);
            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int Update(Receip contract)
        {
            var command = Db.GetStoredProcCommand("pub_Receip_Update");

            Db.AddInParameter(command, "@ReceipId", DbType.Int32, contract.ReceipId);
            Db.AddInParameter(command, "@Number", DbType.String, contract.Number);
            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public Receip GetContractByNumber(long numero)
        {
            var data = Db.ExecuteSprocAccessor("pub_ReceipByNumber_GetList", ReceipMapper.Mapper, numero);

            return GetFirstElement(data);
        }

        public void UpdateReceipStatus(int receipId, int status, int? user)
        {
            var command = Db.GetStoredProcCommand("pub_ReceipStatus_Update");

            Db.AddInParameter(command, "@ReceipId", DbType.Int32, receipId);
            Db.AddInParameter(command, "@PaperStatus", DbType.Int32, status);
            Db.AddInParameter(command, "@User", DbType.Int32, user);

            Db.ExecuteScalar(command);
        }

    }

}
