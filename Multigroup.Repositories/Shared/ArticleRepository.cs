using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared.Mappers;
using Multigroup.Repositories.Shared.Resources;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Multigroup.Repositories.Shared
{
    public class ArticleRepository : BaseRepository
    {
        public Article GetArticleById(int articleId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Article_Get", ArticleMapper.Mapper, articleId);

            return GetFirstElement(data);
        }

        public IEnumerable<ArticleSummary> GetArticlesSummary(int? headings)
        {
            var data = Db.ExecuteSprocAccessor("pub_ArticleSummary_GetList", ArticleSummaryMapper.Mapper, headings);

            return data.ToList();
        }

        public IEnumerable<Article> GetArticles()
        {
            var data = Db.ExecuteSprocAccessor("pub_Article_GetList", ArticleMapper.Mapper);

            return data.ToList();
        }

        public Heading GetHeading(int headingId)
        {
            var data = Db.ExecuteSprocAccessor("pub_Heading_Get", GenericMapper<Heading>.Mapper, headingId);

            return GetFirstElement(data);
        }


        public IEnumerable<Heading> GetHeadings()
        {
            var data = Db.ExecuteSprocAccessor("pub_Heading_GetList", GenericMapper<Heading>.Mapper);

            return data.ToList();
        }

     

        public void DeleteArticle(int articleId)
        {
            var command = Db.GetStoredProcCommand("pub_Article_Delete");
            Db.AddInParameter(command, "@ArticleId", DbType.Int32, articleId);
            Db.ExecuteScalar(command);
        }

        public void DeleteHeading(int headingId)
        {
            var command = Db.GetStoredProcCommand("pub_Heading_Delete");
            Db.AddInParameter(command, "@HeadingId", DbType.Int32, headingId);
            Db.ExecuteScalar(command);
        }

        public void AddArticle(Article Article)
        {
            var command = Db.GetStoredProcCommand("pub_Article_Insert");

            Db.AddInParameter(command, "@name", DbType.String, Article.Name);
            Db.AddInParameter(command, "@HeadingId", DbType.Int32, Article.HeadingId);

            Db.ExecuteScalar(command);
        }

        public void AddHeading(Heading Heading)
        {
            var command = Db.GetStoredProcCommand("pub_Heading_Insert");

            Db.AddInParameter(command, "@name", DbType.String, Heading.Name);

            Db.ExecuteScalar(command);
        }

        public void UpdateArticle(Article Article)
        {
            var command = Db.GetStoredProcCommand("pub_Article_Update");

            Db.AddInParameter(command, "@ArticleId", DbType.Int32, Article.ArticleId);
            Db.AddInParameter(command, "@Name", DbType.String, Article.Name);
            Db.AddInParameter(command, "@HeadingId", DbType.Int32, Article.HeadingId);
            Db.ExecuteScalar(command);
        }

        public void UpdateHeading(Heading Heading)
        {
            var command = Db.GetStoredProcCommand("pub_Heading_Update");

            Db.AddInParameter(command, "@HeadingId", DbType.Int32, Heading.HeadingId);
            Db.AddInParameter(command, "@Name", DbType.String, Heading.Name);
            Db.ExecuteScalar(command);
        }
    }
}
