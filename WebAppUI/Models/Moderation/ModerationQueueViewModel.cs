using BLL.DTO;

namespace WebAppUI.Models.Moderation
{
    public class ModerationQueueViewModel
    {
        public List<ArticleCommentDTO> PendingComments { get; set; } = new();
    }
}

