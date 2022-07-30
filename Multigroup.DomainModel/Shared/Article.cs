using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multigroup.DomainModel.Shared
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Name { get; set; }
        public Heading Heading { get; set; }
        public int HeadingId { get; set; }
    }

    public class ArticleSummary
    {
        public int ArticleId { get; set; }
        public string Name { get; set; }
        public string Heading { get; set; }
    }


    public class Heading
    {
        public int HeadingId { get; set; }
        public string Name { get; set; }

    }
}
