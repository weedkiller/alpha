using Multigroup.Portal.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Multigroup.DomainModel.Shared;

namespace Multigroup.Portal.Models.Account
{
    public class UserProfileVm
    {
        public string UserName { get; set; }
        public string Names { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public SingleSelectVm ddlSites { get; set; }
        public ImageVm Image { get; set; }
    }
}