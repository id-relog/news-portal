using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IArticleAnalyticsService
    {
        bool TrackView(Guid articleId, string? viewerUserId, string? ipHash, string userAgent);
        int GetTotalViews(Guid articleId);
        List<ArticleViewStatsDTO> TopArticlesByViews(DateTime fromUtc, DateTime toUtc, int take = 20);
    }
}

