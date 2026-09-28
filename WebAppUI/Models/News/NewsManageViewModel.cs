using BLL.DTO;

namespace WebAppUI.Models.News
{
    public class NewsManageViewModel
    {
        public List<NewsArticleDTO> Articles { get; set; } = new();
    }
}

