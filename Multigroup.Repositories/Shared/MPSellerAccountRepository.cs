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
    public class MPSellerAccountRepository : BaseRepository
    {
        public MPSellerAccount GetMPSellerAccountById(int MPSellerAccountId)
        {
            var data = Db.ExecuteSprocAccessor("pub_MPSellerAccount_Get", GenericMapper<MPSellerAccount>.Mapper, MPSellerAccountId);

            return GetFirstElement(data);
        }
        
        public IEnumerable<MPSellerAccountSummary> GetMPSellerAccounts()
        {
            var data = Db.ExecuteSprocAccessor("pub_MPSellerAccount_GetList", GenericMapper<MPSellerAccountSummary>.Mapper);

            return data.ToList();
        }

        public void Delete(int MPSellerAccountId)
        {
            var command = Db.GetStoredProcCommand("pub_MPSellerAccount_Delete");
            Db.AddInParameter(command, "@MPSellerAccountId", DbType.Int32, MPSellerAccountId);
            Db.ExecuteScalar(command);
        }

        public void AddMPSellerAccount(MPSellerAccount MPSellerAccounts)
        {
            var command = Db.GetStoredProcCommand("pub_MPSellerAccount_Insert");

            Db.AddInParameter(command, "@MPAccountId", DbType.String, MPSellerAccounts.MPAccountId);
            Db.AddInParameter(command, "@Pass", DbType.String, MPSellerAccounts.Pass);
            Db.AddInParameter(command, "@MaxAmount", DbType.Int32, MPSellerAccounts.MaxAmount);
            Db.AddInParameter(command, "@MinAmount", DbType.Int32, MPSellerAccounts.MinAmount);
            Db.AddInParameter(command, "@Token", DbType.String, MPSellerAccounts.Token);
            Db.AddInParameter(command, "@UserId", DbType.Int32, MPSellerAccounts.UserId);

            Db.ExecuteScalar(command);
        }

        public void AddMPPaymentInstallment(MPPaymentInstallment MPPaymentInstallment)
        {
            var command = Db.GetStoredProcCommand("pub_MPPaymentInstallment_Insert");

            Db.AddInParameter(command, "@amount", DbType.Double, MPPaymentInstallment.amount);
            Db.AddInParameter(command, "@date", DbType.DateTime, MPPaymentInstallment.date);
            Db.AddInParameter(command, "@InstallmentId", DbType.Int32, MPPaymentInstallment.InstallmentId);
            Db.AddInParameter(command, "@MPSellerAccountId", DbType.Int32, MPPaymentInstallment.MPSellerAccountId);

            Db.ExecuteScalar(command);
        }

        public void Update(MPSellerAccount MPSellerAccounts)
        {
            var command = Db.GetStoredProcCommand("pub_MPSellerAccount_Update");

            Db.AddInParameter(command, "@MPSellerAccountId", DbType.Int32, MPSellerAccounts.MPSellerAccountId);
            Db.AddInParameter(command, "@MPAccountId", DbType.String, MPSellerAccounts.MPAccountId);
            Db.AddInParameter(command, "@Pass", DbType.String, MPSellerAccounts.Pass);
            Db.AddInParameter(command, "@MaxAmount", DbType.Int32, MPSellerAccounts.MaxAmount);
            Db.AddInParameter(command, "@MinAmount", DbType.Int32, MPSellerAccounts.MinAmount);
            Db.AddInParameter(command, "@Token", DbType.String, MPSellerAccounts.Token);
            Db.AddInParameter(command, "@UserId", DbType.Int32, MPSellerAccounts.UserId);
            
            Db.ExecuteScalar(command);
        }

        public MPSellerAccount GetNextMPSellerAccount(DateTime date)
        {
            var data = Db.ExecuteSprocAccessor("pub_GetNextMPSellerAccount", GenericMapper<MPSellerAccount>.Mapper, date);

            return GetFirstElement(data);
        }

    }
}
