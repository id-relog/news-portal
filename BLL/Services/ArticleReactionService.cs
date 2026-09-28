using BLL.Interfaces;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class ArticleReactionService(IArticleReactionsRepository _repo) : IArticleReactionService
    {
        public bool SetReaction(Guid articleId, string userId, bool isLike)
        {
            if (articleId == Guid.Empty || string.IsNullOrWhiteSpace(userId)) return false;
            return _repo.SetReaction(articleId, userId, isLike);
        }

        public (int likes, int dislikes) GetCounts(Guid articleId)
        {
            if (articleId == Guid.Empty) return (0, 0);
            return _repo.GetCounts(articleId);
        }
    }
}
