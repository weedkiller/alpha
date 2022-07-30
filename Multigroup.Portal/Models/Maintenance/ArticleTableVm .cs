using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multigroup.Portal.Models.Maintenance
{

    public class ArticleFilterVm
    {
        public IEnumerable<string> SelectedHeading { get; set; }
        public IEnumerable<SelectListItem> ListHeading { get; set; }
    }
    public class ArticleTableVm
    {
        public IEnumerable<ArticleListVm> ArticleList { get; set; }

        public static ArticleTableVm ToViewModel(IEnumerable<ArticleSummary> entities)
        {
            return new ArticleTableVm
            {
                ArticleList = ArticleListVm.ToViewModelList(entities),
            };
        }
    }

    public class ArticleListVm
    {
        public int ArticleId { get; set; }
        public string Name { get; set; }
        public string Heading { get; set; }
        public static IEnumerable<ArticleListVm> ToViewModelList(IEnumerable<ArticleSummary> entities)
        {
            return entities.Select(x => new ArticleListVm
            {
                ArticleId = x.ArticleId,
                Name = x.Name,
                Heading = x.Heading,
            });
        }
    }
}