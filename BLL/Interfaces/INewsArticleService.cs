using BLL.DTO;

namespace BLL.Interfaces
{
    public interface INewsArticleService : IService<NewsArticleDTO>
    {
        List<NewsArticleDTO> GetPublished(int take = 50);
        List<NewsArticleDTO> GetByCategory(Guid categoryId, int take = 50);
        NewsArticleDTO? GetBySlug(string slug);
        List<NewsArticleDTO> GetPendingReview(int take = 200);
        bool SubmitForReview(Guid articleId);
        bool Approve(Guid articleId, string reviewedByUserId);
        bool Reject(Guid articleId, string reviewedByUserId, string note);
        bool Archive(Guid articleId);
    }
}

