using kusach_igi.Data;
using Microsoft.AspNetCore.Mvc;

namespace kusach_igi.Controllers;

public class NewsController : Controller
{
    public IActionResult Index(string? category, string? query)
    {
        return View(PortalContentRepository.GetNews(category, query));
    }

    [HttpGet("/news/{slug}")]
    public IActionResult Details(string slug)
    {
        var article = PortalContentRepository.GetArticle(slug);
        if (article is null)
        {
            return NotFound();
        }

        return View(article);
    }
}
