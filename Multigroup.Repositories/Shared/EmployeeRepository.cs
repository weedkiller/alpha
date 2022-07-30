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
    public class EmployeeRepository : BaseRepository
    {
        public Provider GetProviderById(int providerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Provider_Get", ProviderMapper.Mapper, providerId);

            return GetFirstElement(data);
        }

        public IEnumerable<ProviderSummary> GetProvidersSummary(string ProviderType, string DateFrom, string DateTo, string BalanceFrom, string BalanceTo, string active)
        {
            var data = Db.ExecuteSprocAccessor("pub_EmployeeSummary_GetList", ProviderSummaryMapper.Mapper, ProviderType, DateFrom, DateTo, BalanceFrom, BalanceTo, active, "1");

            return data.ToList();
        }

        public IEnumerable<Provider> GetProviders()
        {
            var data = Db.ExecuteSprocAccessor("pub_Provider_GetList", ProviderMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<ProviderType> GetProviderTypes()
        {
            var data = Db.ExecuteSprocAccessor("pub_ProviderType_GetList", GenericMapper<ProviderType>.Mapper);

            return data.ToList();
        }

        public IEnumerable<EmployeeCurrentAcount> GetEmployeeCurrentAcount(string Providers, string BalanceFrom, string BalanceTo, string NotImput)
        {
            var data = Db.ExecuteSprocAccessor("pub_EmployeeCurrentAcount_GetList", EmployeeCurrentAcountMapper.Mapper, Providers, BalanceFrom, BalanceTo, NotImput);

            return data.ToList();
        }

        public IEnumerable<EmployeeCurrentAcountDetail> GetEmployeeCurrentAcountDetail(int providerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_EmployeeCurrentAcountDetail_GetList", EmployeeCurrentAcountDetailMapper.Mapper, providerId);

            return data.ToList();
        }

        public void AddProvider(Provider Provider)
        {
            var command = Db.GetStoredProcCommand("pub_Provider_Insert");

            Db.AddInParameter(command, "@Active", DbType.Boolean, true);
            Db.AddInParameter(command, "@Balance", DbType.Double, 0);
            Db.AddInParameter(command, "@Commentary", DbType.String, Provider.Commentary);
            Db.AddInParameter(command, "@Name", DbType.String, Provider.Name);
            Db.AddInParameter(command, "@Date", DbType.Date, DateTime.Now);
            Db.AddInParameter(command, "@Document", DbType.String, Provider.Document);
            Db.AddInParameter(command, "@Email", DbType.String, Provider.Email);
            Db.AddInParameter(command, "@IsEmployee", DbType.Boolean, true);
            Db.AddInParameter(command, "@ProviderTypeId", DbType.Int32, Provider.ProviderTypeId);
            Db.AddInParameter(command, "@Telephone", DbType.String, Provider.Telephone);
            Db.AddInParameter(command, "@UserId", DbType.Int32, Provider.UserId);

            Db.ExecuteScalar(command);
        }


        public void UpdateProvider(Provider Provider)
        {
            var command = Db.GetStoredProcCommand("pub_Provider_Update");

            Db.AddInParameter(command, "@ProviderId", DbType.Int32, Provider.ProviderId);
            Db.AddInParameter(command, "@Active", DbType.Boolean, Provider.Active);
            Db.AddInParameter(command, "@Balance", DbType.Double, Provider.Balance);
            Db.AddInParameter(command, "@Commentary", DbType.String, Provider.Commentary);
            Db.AddInParameter(command, "@Name", DbType.String, Provider.Name);
            Db.AddInParameter(command, "@Date", DbType.Date, Provider.Date);
            Db.AddInParameter(command, "@Document", DbType.String, Provider.Document);
            Db.AddInParameter(command, "@Email", DbType.String, Provider.Email);
            Db.AddInParameter(command, "@IsEmployee", DbType.Boolean, true);
            Db.AddInParameter(command, "@ProviderTypeId", DbType.Int32, Provider.ProviderTypeId);
            Db.AddInParameter(command, "@Telephone", DbType.String, Provider.Telephone);
            Db.AddInParameter(command, "@UserId", DbType.String, Provider.UserId);
            Db.ExecuteScalar(command);
        }
    }
}
