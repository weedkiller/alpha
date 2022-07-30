using Microsoft.Practices.EnterpriseLibrary.Data;
using Multigroup.DomainModel.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.Repositories.Shared.Mappers
{
    public class ArticleMapper
    {
        public static IRowMapper<Article> Mapper
        {
            get
            {
                var mapper = MapBuilder<Article>.MapAllProperties()
                    .DoNotMap(m => m.Heading)
                    .Build();
                return mapper;
            }
        }
    }

    public class ArticleSummaryMapper
    {
        public static IRowMapper<ArticleSummary> Mapper
        {
            get
            {
                var mapper = MapBuilder<ArticleSummary>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }

    public class HeadingMapper
    {
        public static IRowMapper<Heading> Mapper
        {
            get
            {
                var mapper = MapBuilder<Heading>.MapAllProperties()
                    .Build();
                return mapper;
            }
        }
    }
}
