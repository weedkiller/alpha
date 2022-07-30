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
    public class ContactCallRepository : BaseRepository
    {
        public ContactCall GetContactCallById(int ContactCallId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContactCall_Get", ContactCallMapper.Mapper, ContactCallId);

            return GetFirstElement(data);
        }


        public IEnumerable<ContactCall> GetContactCalls()
        {
            var data = Db.ExecuteSprocAccessor("pub_ContactCall_GetList", ContactCallMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<ContactCallState> GetContactCallStates()
        {
            var data = Db.ExecuteSprocAccessor("pub_ContactCallState_GetList", ContactCallStateMapper.Mapper);

            return data.ToList();
        }

        public IEnumerable<ContactCallSummary> GetContactCallSummaryByFilter(string states, string dateFrom, string dateTo, string managerId, string supervisorId, string sellerId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContactCallSummaryByFilter_GetList", ContactCallSummaryMapper.Mapper, states, dateFrom, dateTo, managerId, supervisorId, sellerId);

            return data.ToList();
        }

        public IEnumerable<ContactCallSummary> GetContactCallSummaries(int userId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ContactCallSummary_GetList", ContactCallSummaryMapper.Mapper, userId);

            return data.ToList();
        }


        public void Delete(int ContactCallId)
        {
            var command = Db.GetStoredProcCommand("pub_ContactCall_Delete");
            Db.AddInParameter(command, "@ContactCallId", DbType.Int32, ContactCallId);
            Db.ExecuteScalar(command);
        }

        public void AddContactCall(ContactCall ContactCall)
        {
            var command = Db.GetStoredProcCommand("pub_ContactCall_Insert");

            Db.AddInParameter(command, "@Name", DbType.String, ContactCall.Name);
            Db.AddInParameter(command, "@Surname", DbType.String, ContactCall.Surname);
            Db.AddInParameter(command, "@Vehicle", DbType.String, ContactCall.Vehicle);
            Db.AddInParameter(command, "@Phone", DbType.String, ContactCall.Phone);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, DateTime.Now);
            Db.AddInParameter(command, "@ContactDate", DbType.DateTime, ContactCall.ContactDate);
            Db.AddInParameter(command, "@ContactTime", DbType.String, ContactCall.ContactTime);
            Db.AddInParameter(command, "@Commentary", DbType.String, ContactCall.Commentary);
            Db.AddInParameter(command, "@ContactCallStateId", DbType.Int32, ContactCall.ContactCallStateId);
            Db.AddInParameter(command, "@ManagerId", DbType.Int32, ContactCall.ManagerId);
            Db.AddInParameter(command, "@SupervisorId", DbType.Int32, ContactCall.SupervisorId);
            Db.AddInParameter(command, "@SellerId", DbType.Int32, ContactCall.SellerId);

            Db.ExecuteScalar(command);
        }

        public void Update(ContactCall ContactCall)
        {
            var command = Db.GetStoredProcCommand("pub_ContactCall_Update");

            Db.AddInParameter(command, "@ContactCallId", DbType.Int32, ContactCall.ContactCallId);
            Db.AddInParameter(command, "@Name", DbType.String, ContactCall.Name);
            Db.AddInParameter(command, "@Surname", DbType.String, ContactCall.Surname);
            Db.AddInParameter(command, "@Vehicle", DbType.String, ContactCall.Vehicle);
            Db.AddInParameter(command, "@Phone", DbType.String, ContactCall.Phone);
            Db.AddInParameter(command, "@SystemDate", DbType.DateTime, ContactCall.SystemDate);
            Db.AddInParameter(command, "@ContactDate", DbType.DateTime, ContactCall.ContactDate);
            Db.AddInParameter(command, "@ContactTime", DbType.String, ContactCall.ContactTime);
            Db.AddInParameter(command, "@Commentary", DbType.String, ContactCall.Commentary);
            Db.AddInParameter(command, "@ContactCallStateId", DbType.Int32, ContactCall.ContactCallStateId);
            Db.AddInParameter(command, "@ManagerId", DbType.Int32, ContactCall.ManagerId);
            Db.AddInParameter(command, "@SupervisorId", DbType.Int32, ContactCall.SupervisorId);
            Db.AddInParameter(command, "@SellerId", DbType.Int32, ContactCall.SellerId);

            Db.ExecuteScalar(command);
        }
    }
}
