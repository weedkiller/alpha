using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared.Mappers;
using Multigroup.Repositories.Shared.Resources;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Multigroup.Repositories.Shared
{
    public class UserRepository : BaseRepository
    {
        public IEnumerable<User> GetUsers()
        {
            var users = Db.ExecuteSprocAccessor(StoredProcedures.pub_User_GetList, UserMapper.Mapper);
            return users.ToList();
        }

        public IEnumerable<User> GetUsersCollection()
        {
            var users = Db.ExecuteSprocAccessor("pub_UserCollection_GetList", UserMapper.Mapper);
            return users.ToList();
        }

        public IEnumerable<User> GetSellers()
        {
            var users = Db.ExecuteSprocAccessor("pub_User_GetSellers", UserMapper.Mapper);
            return users.ToList();
        }
        
        public User GetZoneCollector(int ZoneId)
        {
            var users = Db.ExecuteSprocAccessor("query_zone_collector", GenericMapper<User>.Mapper, ZoneId);
            return GetFirstElement(users);
        }
        public User GetUserByUserName(string userName)
        {
            var users = Db.ExecuteSprocAccessor(StoredProcedures.pub_UserByUserName_Get, UserMapper.Mapper, userName);
            return GetFirstElement(users);
        }
        public IEnumerable<UserSummary> GetUserSummary()
        {
            var users = Db.ExecuteSprocAccessor("pub_UserSummary_GetList", UserSummaryMapper.Mapper);
            return users.ToList();
        }

        public IEnumerable<UserType> GetUserTypes()
        {
            var users = Db.ExecuteSprocAccessor("pub_UserType_GetList", UserTypeMapper.Mapper);
            return users.ToList();
        }

        public IEnumerable<UserCashier> GetUserCashiers()
        {
            var users = Db.ExecuteSprocAccessor("pub_UserCashier_GetList", UserCashierMapper.Mapper);
            return users.ToList();
        }

        public IEnumerable<BaseEntity> GetUsersByType(int type)
        {
            var users = Db.ExecuteSprocAccessor("pub_UserComboByFilter_GetList", GenericMapper<BaseEntity>.Mapper, type);
            return users.ToList();
        }

        public User GetUser(int userId)
        {
            var data = Db.ExecuteSprocAccessor(StoredProcedures.pub_User_Get, UserMapper.Mapper, userId);
            return data.FirstOrDefault();
        }

        public UserCashier GetUserCashierByUserId(int userId)
        {
            var data = Db.ExecuteSprocAccessor("pub_UserCashier_Get", UserCashierMapper.Mapper, userId);
            return data.FirstOrDefault();
        }

        public User GetUserByMail(string email)
        {
            var data = Db.ExecuteSprocAccessor(StoredProcedures.pub_UserByEmail_Get, UserMapper.Mapper, email);
            return data.FirstOrDefault();
        }

        public User UserLogin(User user)
        {
            var data = Db.ExecuteSprocAccessor(StoredProcedures.pub_User_Login, UserMapper.Mapper, user.UserName, user.Password);
            return data.FirstOrDefault();
        }

        public IEnumerable<User> GetUsersWithoutActivity(int monthCount)
        {
            var users = Db.ExecuteSprocAccessor("pub_UserWithoutActivity_GetList", UserMapper.Mapper, monthCount);
            return users.ToList();
        }
        
        public int Insert(User user)
        {
            var command = Db.GetStoredProcCommand(StoredProcedures.pub_User_Insert);

            Db.AddInParameter(command, "@Names", DbType.String, user.Names);
            Db.AddInParameter(command, "@Surname", DbType.String, user.Surname);
            Db.AddInParameter(command, "@Email", DbType.String, user.Email);
            Db.AddInParameter(command, "@Password", DbType.String, user.Password);
            Db.AddInParameter(command, "@UserName", DbType.String, user.UserName);
            Db.AddInParameter(command, "@Active", DbType.String, user.Active);
            Db.AddInParameter(command, "@UserType", DbType.Int32, user.UserType);
            Db.AddInParameter(command, "@IdentificationNumber", DbType.Int32, user.IdentificationNumber);
            Db.AddInParameter(command, "@IdGerente", DbType.Int32, user.IdGerente);
            Db.AddInParameter(command, "@IdSupervisor", DbType.Int32, user.IdSupervisor);

            var id = Db.ExecuteScalar(command);

            return (id != null)? int.Parse(id.ToString()) : 0;
        }

        public int AddUserTypeHistory(int newType, int oldType, string observations, DateTime date, int userId)
        {
            var command = Db.GetStoredProcCommand("pub_UserTypeHistory_Insert");

            Db.AddInParameter(command, "@Date", DbType.DateTime, date);
            Db.AddInParameter(command, "@UserId", DbType.Int32, userId);
            Db.AddInParameter(command, "@UserTypeOld", DbType.Int32, oldType);
            Db.AddInParameter(command, "@UserTypeNew", DbType.Int32, newType);
            Db.AddInParameter(command, "@Observations", DbType.String, observations);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int AddUserLogin(string userName, string Name, DateTime date)
        {
            var command = Db.GetStoredProcCommand("pub_UserLogin_Insert");

            Db.AddInParameter(command, "@Date", DbType.DateTime, date);
            Db.AddInParameter(command, "@Name", DbType.String, Name);
            Db.AddInParameter(command, "@userName", DbType.String, userName);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public int AddUserCashier(int UserId, int CashierId)
        {
            var command = Db.GetStoredProcCommand("pub_UserCashier_Insert");
            Db.AddInParameter(command, "@UserId", DbType.String, UserId);
            Db.AddInParameter(command, "@CashierId", DbType.String, CashierId);

            var id = Db.ExecuteScalar(command);

            return (id != null) ? int.Parse(id.ToString()) : 0;
        }

        public void UpdateZonexUser(int zoneId, int userId)
        {
            var command = Db.GetStoredProcCommand("pub_UserZones_Insert");

            Db.AddInParameter(command, "@UserId", DbType.Int32, userId);
            Db.AddInParameter(command, "@ZoneId", DbType.Int32, zoneId);

            Db.ExecuteScalar(command);
        }

        public void DeleteZonexUser(int userId)
        {
            var command = Db.GetStoredProcCommand("pub_UserZonesxUser_Delete");

            Db.AddInParameter(command, "@UserId", DbType.Int32, userId);

            Db.ExecuteScalar(command);
        }

        public void DeleteUserCashier(int userCashierId)
        {
            var command = Db.GetStoredProcCommand("pub_UserCashier_Delete");

            Db.AddInParameter(command, "@UserCashierId", DbType.Int32, userCashierId);

            Db.ExecuteScalar(command);
        }

        public IEnumerable<UserTypeHistory> GetUserTypeHistoryByUserId(int userId)
        {
            var data = Db.ExecuteSprocAccessor("pub_UserTypeHistoryByUser_GetList", GenericMapper<UserTypeHistory>.Mapper, userId);

            return data.ToList();
        }

        public IEnumerable<Zone> GetZonesByIdUser(int userId)
        {
            var data = Db.ExecuteSprocAccessor("pub_ZonesxUser_GetList", ZoneMapper.Mapper, userId);
            return data.ToList();
        }

        public void Update(User user)
        {
            var command = Db.GetStoredProcCommand(StoredProcedures.pub_User_Update);

            Db.AddInParameter(command, "@UserId", DbType.Int32, user.UserId);
            Db.AddInParameter(command, "@Names", DbType.String, user.Names);
            Db.AddInParameter(command, "@Surname", DbType.String, user.Surname);
            Db.AddInParameter(command, "@Email", DbType.String, user.Email);
            Db.AddInParameter(command, "@Password", DbType.String, user.Password);
            Db.AddInParameter(command, "@Active", DbType.String, user.Active);
            Db.AddInParameter(command, "@UserType", DbType.Int32, user.UserType);
            Db.AddInParameter(command, "@IdentificationNumber", DbType.Int32, user.IdentificationNumber);
            Db.AddInParameter(command, "@IdGerente", DbType.Int32, user.IdGerente);
            Db.AddInParameter(command, "@IdSupervisor", DbType.Int32, user.IdSupervisor);

            Db.ExecuteScalar(command);
        }

        public void UpdateLimit(int id, double Limit)
        {
            var command = Db.GetStoredProcCommand("pub_userLimit_update");

            Db.AddInParameter(command, "@UserId", DbType.Int32, id);
            Db.AddInParameter(command, "@Limit", DbType.Double, Limit);

            Db.ExecuteScalar(command);
        }

        public IEnumerable<User> GetUsersMigration()
        {
            var users = Db.ExecuteSprocAccessor("pub_UserMigration_GetList", UserMapper.Mapper);
            return users.ToList();
        }


        public User GetUser(int type, bool active)
        {
            var data = Db.ExecuteSprocAccessor(StoredProcedures.pub_User_Get, UserMapper.Mapper, type, active );
            return data.FirstOrDefault();
        }
    }
}
