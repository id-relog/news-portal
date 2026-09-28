using BLL.DTO;

namespace WebAppUI.Models.Home
{
    public class HomeIndexViewModel
    {
        public List<NewsArticleDTO> TopArticles { get; set; } = new();
        public List<NewsArticleDTO> LatestArticles { get; set; } = new();
    }
}
