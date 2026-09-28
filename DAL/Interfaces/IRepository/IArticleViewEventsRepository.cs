using DAL.entity;

namespace DAL.Interfaces
{
    public interface IArticleViewEventRepository : IRepository<ArticleViewEvent>
    {
        int CountViews(Guid articleId);
        int CountViews(Guid articleId, DateTime fromUtc, DateTime toUtc);
        Dictionary<Guid, int> CountViewsByArticle(DateTime fromUtc, DateTime toUtc, int take = 20);
    }
}

