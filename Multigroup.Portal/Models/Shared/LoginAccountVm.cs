using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Shared
{
    public class LoginAccountVm
    {
        public LoginVm UserLogin { get; set; }

        public AccountVm Account { get; set; }
    }
}