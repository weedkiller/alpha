using Multigroup.DomainModel.Shared;
using Multigroup.Repositories.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Multigroup.Services.Shared
{
    public class ArticleService
    {
        private ArticleRepository _articleRepository;

        public ArticleService()
        {
            _articleRepository = new ArticleRepository();
        }


        public Article GetArticlesById(int articleId)
        {
            return _articleRepository.GetArticleById(articleId);
        }

        public IEnumerable<Article> GetArticles()
        {
            return _articleRepository.GetArticles();
        }

        public IEnumerable<ArticleSummary> GetArticlesSummary(int? headings)
        {
            return _articleRepository.GetArticlesSummary(headings);
        }

        public Heading GetHeading(int headingId)
        {
            return _articleRepository.GetHeading(headingId);
        }

        public IEnumerable<Heading> GetHeadings()
        {
            return _articleRepository.GetHeadings();
        }

        public void AddArticle(Article Article)
        {
            _articleRepository.AddArticle(Article);
        }

        public void DeleteArticle(int articleId)
        {
            _articleRepository.DeleteArticle(articleId);
        }

        public void UpdateArticle(Article Article)
        {
            _articleRepository.UpdateArticle(Article);
        }

        public void AddHeading(Heading Heading)
        {
            _articleRepository.AddHeading(Heading);
        }

        public void DeleteHeading(int headingId)
        {
            _articleRepository.DeleteHeading(headingId);
        }

        public void UpdateHeading(Heading Heading)
        {
            _articleRepository.UpdateHeading(Heading);
        }
    }
}
