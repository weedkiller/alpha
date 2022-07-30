using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Enums;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Shared
{
    public class UserTypeHistoryTableVm
    {
        public IEnumerable<UserTypeHistoryListVm> UserTypeHistoryList { get; set; }

        public static UserTypeHistoryTableVm ToViewModel(IEnumerable<UserTypeHistory> entities)
        {
            return new UserTypeHistoryTableVm
            {
                UserTypeHistoryList = UserTypeHistoryListVm.ToViewModelList(entities),
            };
        }
    }

    public class UserTypeHistoryListVm
    {
        public int UserTypeHistoryId { get; set; }
        public DateTime Date { get; set; }
        public string UserTypeOld { get; set; }
        public string UserTypeNew { get; set; }
        public string Observations { get; set; }

        public static IEnumerable<UserTypeHistoryListVm> ToViewModelList(IEnumerable<UserTypeHistory> entities)
        {
            return entities.Select(x => new UserTypeHistoryListVm
            {
                UserTypeHistoryId = x.UserTypeHistoryId,
                Date = x.Date,
                UserTypeOld = x.UserTypeOld,
                UserTypeNew = x.UserTypeNew,
                Observations = x.Observations
            });
        }
    }
}