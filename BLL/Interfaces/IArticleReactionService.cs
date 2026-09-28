namespace BLL.Interfaces
{
    public interface IArticleReactionService
    {
        bool SetReaction(Guid articleId, string userId, bool isLike);
        (int likes, int dislikes) GetCounts(Guid articleId);
    }
}
