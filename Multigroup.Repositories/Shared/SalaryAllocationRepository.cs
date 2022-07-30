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
    public class SalaryAllocationRepository : BaseRepository
    {
        public SalaryAllocation GetSalaryAllocationById(int salaryAllocationId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SalaryAllocation_Get", SalaryAllocationMapper.Mapper, salaryAllocationId);

            return GetFirstElement(data);
        }

        public IEnumerable<SalaryAllocationSummary> GetSalaryAllocationsSummary(string SelectedProvider, string DateFromStart, string DateToStart, string DateFromEnd, string DateToEnd, string AmountFrom, string AmountTo, string Authorized, string AuthorizedUser)
        {
            var data = Db.ExecuteSprocAccessor("pub_SalaryAllocationSummary_GetList", SalaryAllocationSummaryMapper.Mapper, SelectedProvider, DateFromStart, DateToStart, DateFromEnd, DateToEnd, AmountFrom, AmountTo, Authorized, AuthorizedUser);

            return data.ToList();
        }

        public IEnumerable<SalaryAllocationSummary> GetSalarySettlementSummary(string SelectedProvider, string Period)
        {
            var data = Db.ExecuteSprocAccessor("pub_SalarySettlementSummary_GetList", SalaryAllocationSummaryMapper.Mapper, SelectedProvider, Period);

            return data.ToList();
        }

        public IEnumerable<SalaryAllocationSummary> GetGenerateSalarySettlementSummary(string Period, string DatePeriod)
        {
            var data = Db.ExecuteSprocAccessor("pub_GenerateSalarySettlementSummary_GetList", SalaryAllocationSummaryMapper.Mapper, DatePeriod, Period);

            return data.ToList();
        }

        public IEnumerable<SalaryAllocation> GetSalaryAllocations()
        {
            var data = Db.ExecuteSprocAccessor("pub_SalaryAllocation_GetList", SalaryAllocationMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<SalaryAllocation> GetSalaryAllocationsByEmployee(int EmployeeId)
        {
            var data = Db.ExecuteSprocAccessor("pub_SalaryAllocationByEmployee_GetList", SalaryAllocationMapper.Mapper);

            return data.ToList();
        }

        public void DeleteSalaryAllocation(int salaryAllocationId)
        {
            var command = Db.GetStoredProcCommand("pub_SalaryAllocation_Delete");
            Db.AddInParameter(command, "@SalaryAllocationId", DbType.Int32, salaryAllocationId);
            Db.ExecuteScalar(command);
        }

        public int AddSalaryAllocation(SalaryAllocation SalaryAllocation)
        {
            DateTime start = new DateTime(SalaryAllocation.StartDate.Year, SalaryAllocation.StartDate.Month, 1);
            DateTime end = new DateTime(SalaryAllocation.EndDate.Year, SalaryAllocation.EndDate.Month, 1);
            var command = Db.GetStoredProcCommand("pub_SalaryAllocation_Insert");

            Db.AddInParameter(command, "@Amount", DbType.Double, SalaryAllocation.Amount);
            Db.AddInParameter(command, "@Description", DbType.String, SalaryAllocation.Description);
            Db.AddInParameter(command, "@EndDate", DbType.DateTime, end);
            Db.AddInParameter(command, "@IsAuthorized", DbType.Boolean, SalaryAllocation.IsAuthorized);
            Db.AddInParameter(command, "@ProveedorId", DbType.Int32, SalaryAllocation.ProveedorId);
            Db.AddInParameter(command, "@StartDate", DbType.DateTime, start);
            Db.AddInParameter(command, "@UserAuthorizedId", DbType.Int32, SalaryAllocation.UserAuthorizedId);
            Db.AddInParameter(command, "@LastMonth", DbType.Int32, SalaryAllocation.LastMonth);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdateSalaryAllocation(SalaryAllocation SalaryAllocation)
        {
            var command = Db.GetStoredProcCommand("pub_SalaryAllocation_Update");

            Db.AddInParameter(command, "@SalaryAllocationId", DbType.Int32, SalaryAllocation.SalaryAllocationId);
            Db.AddInParameter(command, "@Amount", DbType.Double, SalaryAllocation.Amount);
            Db.AddInParameter(command, "@Description", DbType.String, SalaryAllocation.Description);
            Db.AddInParameter(command, "@EndDate", DbType.DateTime, SalaryAllocation.EndDate);
            Db.AddInParameter(command, "@IsAuthorized", DbType.Boolean, SalaryAllocation.IsAuthorized);
            Db.AddInParameter(command, "@ProveedorId", DbType.Int32, SalaryAllocation.ProveedorId);
            Db.AddInParameter(command, "@StartDate", DbType.DateTime, SalaryAllocation.StartDate);
            Db.AddInParameter(command, "@UserAuthorizedId", DbType.Int32, SalaryAllocation.UserAuthorizedId);
            Db.AddInParameter(command, "@LastMonth", DbType.Int32, SalaryAllocation.LastMonth);
            Db.ExecuteScalar(command);
        }

    }
}
