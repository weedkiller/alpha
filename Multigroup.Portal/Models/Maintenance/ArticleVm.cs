using Multigroup.DomainModel.Shared;
using Multigroup.DomainModel.Shared.Enums;
using Multigroup.Portal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Multigroup.Portal.Models.Shared;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Maintenance
{
    public class ArticleVm : BaseVm
    {
        public int ArticleId { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public SingleSelectVm ddlHeading { get; set; }


    public Article ToModelObject()
        {
            return new Article
            {
                ArticleId = ArticleId,
                Name = Name,
                HeadingId = GetNullableValue(ddlHeading).Value,
            };
        }

        public static ArticleVm FromDomainModel(Article entity)
        {
            return new ArticleVm
            {
                ArticleId = entity.ArticleId,
                Name = entity.Name,
            };
        }
    }
}