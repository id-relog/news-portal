using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using WebAppUI.Models.Home;

namespace WebAppUI.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly INewsArticleService _articleService;
        private readonly IArticleAnalyticsService _analyticsService;

        public HomeController(INewsArticleService articleService, IArticleAnalyticsService analyticsService)
        {
            _articleService = articleService;
            _analyticsService = analyticsService;
        }

        public IActionResult Index()
        {
            var published = _articleService.GetPublished(30);
            foreach (var article in published)
            {
                article.ViewsCount = _analyticsService.GetTotalViews(article.Id);
            }

            var model = new HomeIndexViewModel
            {
                TopArticles = published.OrderByDescending(x => x.ViewsCount).Take(5).ToList(),
                LatestArticles = published.OrderByDescending(x => x.PublishedAtUtc ?? x.CreatedAtUtc).Take(10).ToList()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous] 
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}