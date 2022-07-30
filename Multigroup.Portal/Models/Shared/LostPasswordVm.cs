using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Shared
{
    public class LostPasswordVm
    {
        [Required(ErrorMessage = "Necesitamos su nombre de usuario para recuperar la contraseña")]
        public string UserName { get; set; }
    }
}