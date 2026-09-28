using DAL.entity;

namespace DAL.Interfaces
{
    public interface INewsArticleRepository : IRepository<NewsArticle>
    {
        List<NewsArticle> GetPublished(int take = 50);
        List<NewsArticle> GetByCategory(Guid categoryId, int take = 50);
        NewsArticle? GetBySlug(string slug);
        bool SetStatus(Guid articleId, ArticleStatus status, DateTime? publishedAtUtc);
        bool SetTags(Guid articleId, List<string> tagNames);
    }
}

