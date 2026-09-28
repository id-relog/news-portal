using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IArticleCommentService : IService<ArticleCommentDTO>
    {
        List<ArticleCommentDTO> GetForArticle(Guid articleId);
        List<ArticleCommentDTO> GetPending(int take = 200);
        bool Approve(Guid commentId, string moderatedByUserId);
        bool Reject(Guid commentId, string moderatedByUserId);
    }
}

