using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppUI.Auth;
using WebAppUI.Models.Analytics;

namespace WebAppUI.Controllers
{
    [Authorize(Roles = $"{AppRoles.Analyst},{AppRoles.Administrator}")]
    public class AnalyticsController : Controller
    {
        private readonly IArticleAnalyticsService _analytics;
        private readonly INewsArticleService _articles;

        public AnalyticsController(IArticleAnalyticsService analytics, INewsArticleService articles)
        {
            _analytics = analytics;
            _articles = articles;
        }

        public IActionResult Index(int days = 7, int take = 10)
        {
            if (days < 1) days = 1;
            if (days > 90) days = 90;
            if (take < 1) take = 1;
            if (take > 50) take = 50;

            var toUtc = DateTime.UtcNow;
            var fromUtc = toUtc.AddDays(-days);
            var top = _analytics.TopArticlesByViews(fromUtc, toUtc, take);

            var all = _articles.Get().ToDictionary(a => a.Id, a => a.Title);
            var items = top.Select(x => new TopArticleViewItem
            {
                ArticleId = x.ArticleId,
                Title = all.TryGetValue(x.ArticleId, out var t) ? t : x.ArticleId.ToString(),
                Views = x.Views
            }).ToList();

            return View(new AnalyticsIndexViewModel
            {
                Days = days,
                Take = take,
                Items = items
            });
        }
    }
}

