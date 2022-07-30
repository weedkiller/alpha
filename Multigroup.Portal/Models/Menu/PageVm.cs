using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Menu
{
    public class PageVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no debe superar los 50 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [AllowHtml]
        public string HtmlContent { get; set; }
    }
}