using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Shared
{
    public class UserFilterVm
    {
        public IEnumerable<string> SelectedZones { get; set; }
        public IEnumerable<SelectListItem> ListZones { get; set; }
        public IEnumerable<string> SelectedStatus { get; set; }
        public IEnumerable<SelectListItem> ListStatus { get; set; }
    }

    public class UserListVm
    {
        public string Names { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Active { get; set; }
        public string RoleName { get; set; }
        public double Limit { get; set; }

        public static IEnumerable<UserListVm> FromDomainModel(IEnumerable<UserSummary> users)
        {
            List<UserListVm> listUsers = new List<UserListVm>();

            foreach (var user in users)
            {
                listUsers.Add(new UserListVm
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Names = user.Names,
                    Surname = user.Surname,
                    UserName = user.UserName,
                    Active = user.Active? "Activo" : "Inactivo",
                    RoleName = user.RoleName,
                    Limit = user.Limit
                });
            }
            return listUsers;
        }
    }
}