using BLL.DTO;

namespace WebAppUI.Models.News
{
    public class NewsDetailsViewModel
    {
        public NewsArticleDTO Article { get; set; } = null!;
        public List<ArticleCommentDTO> Comments { get; set; } = new();
        public AddCommentViewModel NewComment { get; set; } = new();
    }
}

