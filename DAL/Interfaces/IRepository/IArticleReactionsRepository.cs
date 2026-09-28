using DAL.entity;

namespace DAL.Interfaces
{
    public interface IArticleReactionsRepository : IRepository<ArticleReaction>
    {
        bool SetReaction(Guid articleId, string userId, bool isLike);
        (int likes, int dislikes) GetCounts(Guid articleId);
    }
}
