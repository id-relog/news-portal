using DAL.entity;

namespace DAL.Interfaces
{
    public interface IArticleRevisionRepository : IRepository<ArticleRevision>
    {
        List<ArticleRevision> GetForArticle(Guid articleId);
        int GetNextRevisionNumber(Guid articleId);
    }
}

