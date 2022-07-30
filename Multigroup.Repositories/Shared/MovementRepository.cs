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
    public class MovementRepository : BaseRepository
    {
        public MovementType GetMovementTypeById(int movementTypeId)
        {
            var data = Db.ExecuteSprocAccessor("pub_MovementType_Get", MovementTypeMapper.Mapper, movementTypeId);

            return GetFirstElement(data);
        }

        public IEnumerable<MovementType> GetMovementTypes()
        {
            var data = Db.ExecuteSprocAccessor("pub_MovementType_GetList", MovementTypeMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<MovementResume> GetMovementResume(string DateFrom, string DateTo, string DateSystemFrom, string DateSystemTo, string SelectedUser, string SelectedPaymentMethod, string SelectedCashier, string Cycle, string _selectedMovementType, string _selectedVerified, string amountFrom, string amountTo)
        {
            var data = Db.ExecuteSprocAccessor("pub_MovementResume_GetList", MovementResumeMapper.Mapper, DateFrom, DateTo, DateSystemFrom, DateSystemTo, SelectedUser, SelectedPaymentMethod, SelectedCashier, Cycle, _selectedMovementType + ",", _selectedVerified, amountFrom, amountTo);

            return data.ToList();
        }

        public IEnumerable<MovementBalance> GetMovementBalance()
        {
            var data = Db.ExecuteSprocAccessor("pub_MovementBalance_GetList", MovementBalanceMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<Verified> GetVerified()
        {
            var data = Db.ExecuteSprocAccessor("pub_Verified_GetList", VerifiedMapper.Mapper);

            return data.ToList();
        }

        public void ReconciledMovement(int paymentMethodId, int conciliationId)
        {
            var command = Db.GetStoredProcCommand("pub_ReconciledMovement_Update");

            Db.AddInParameter(command, "@PaymentMethodId", DbType.Int32, paymentMethodId);
            Db.AddInParameter(command, "@ConciliationId", DbType.Int32, conciliationId);

            Db.ExecuteScalar(command);
        }

    }
}
