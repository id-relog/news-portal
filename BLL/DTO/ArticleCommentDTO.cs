namespace BLL.DTO
{
    public class ArticleCommentDTO
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }

        public string? AuthorUserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public CommentStatusDTO Status { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}

