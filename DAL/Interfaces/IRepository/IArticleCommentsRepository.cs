using DAL.entity;

namespace DAL.Interfaces
{
    public interface IArticleCommentRepository : IRepository<ArticleComment>
    {
        List<ArticleComment> GetForArticle(Guid articleId);
        List<ArticleComment> GetPending(int take = 200);
        bool Moderate(Guid commentId, CommentStatus status, string moderatedByUserId, DateTime moderatedAtUtc);
        bool DeleteByAuthorUserId(string authorUserId);
    }
}

