using BLL.DTO;
using BLL.Interfaces;
using DAL.entity;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class ArticleAnalyticsService(IArticleViewEventRepository _repo) : IArticleAnalyticsService
    {
        public bool TrackView(Guid articleId, string? viewerUserId, string? ipHash, string userAgent)
        {
            if (articleId == Guid.Empty) return false;

            var evt = new ArticleViewEvent
            {
                Id = Guid.NewGuid(),
                ArticleId = articleId,
                ViewerUserId = viewerUserId,
                IpHash = ipHash,
                UserAgent = userAgent ?? string.Empty,
                ViewedAtUtc = DateTime.UtcNow
            };

            return _repo.Add(evt);
        }

        public int GetTotalViews(Guid articleId)
        {
            if (articleId == Guid.Empty) return 0;
            return _repo.CountViews(articleId);
        }

        public List<ArticleViewStatsDTO> TopArticlesByViews(DateTime fromUtc, DateTime toUtc, int take = 20)
        {
            var dict = _repo.CountViewsByArticle(fromUtc, toUtc, take);
            return dict.Select(kv => new ArticleViewStatsDTO { ArticleId = kv.Key, Views = kv.Value }).ToList();
        }
    }
}

