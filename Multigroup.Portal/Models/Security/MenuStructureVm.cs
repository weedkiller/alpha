using Multigroup.Portal.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Models.Security
{
    public class MenuStructureTableVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Parent { get; set; }
    }

    public class MenuStructureVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no debe superar los 50 caracteres")]
        public string Name { get; set; }
        public string Icon { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        public int? Order { get; set; }
        public SingleSelectVm ddlParents { get; set; }
    }
}