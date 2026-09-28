using BLL.DTO;

namespace WebAppUI.Models.News
{
    public class NewsIndexViewModel
    {
        public List<NewsArticleDTO> Articles { get; set; } = new();
        public string? Query { get; set; }
        public string? Category { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public List<NewsCategoryDTO> Categories { get; set; } = new();
    }
}

