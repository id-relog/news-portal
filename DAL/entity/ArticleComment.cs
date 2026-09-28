namespace DAL.entity
{
    public class ArticleComment
    {
        public Guid Id { get; set; }

        public Guid ArticleId { get; set; }
        public NewsArticle Article { get; set; } = null!;

        public string? AuthorUserId { get; set; }
        public string DisplayName { get; set; } = null!;

        public string Body { get; set; } = null!;
        public CommentStatus Status { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModeratedAtUtc { get; set; }
        public string? ModeratedByUserId { get; set; }
    }
}
