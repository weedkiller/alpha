using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class User
    {
        public int UserId { get; set; }
        public  string Names { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public bool Active { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public int? UserType { get; set; }
        public string IdentificationNumber { get; set; }
        public IEnumerable<Zone> Zones { get; set; }
        public int? IdGerente { get; set; }
        public int? IdSupervisor { get; set; }
        public double Limit { get; set; }

        //public int? CountryId { get; set; }
    }

    public class UserSummary : User
    {
        public string RoleName { get; set; }
    }

    public class UserType
    {
        public int UserTypeId { get; set; }
        public string Description { get; set; }
        public bool IsCommission { get; set; }

        public double Commission { get; set; }
}

    public class UserZone
    {
        public int UserZoneId { get; set; }
        public int UserId { get; set; }
        public int ZoneId { get; set; }
    }

    public class UserTypeHistory
    {
        public int UserTypeHistoryId { get; set; }
        public DateTime Date { get; set; }
        public string UserTypeOld { get; set; }
        public string UserTypeNew { get; set; }
        public string Observations { get; set; }
    }

    public class UserLogin
    {
        public int UserLoginId { get; set; }
        public string userName { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public class UserCashier
    {
        public int UserCashierId { get; set; }
        public int UserId { get; set; }
        public int CashierId { get; set; }
    }
}
