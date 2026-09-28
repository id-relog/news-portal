namespace DAL.entity
{
    public class ArticleViewEvent
    {
        public Guid Id { get; set; }

        public Guid ArticleId { get; set; }
        public NewsArticle Article { get; set; } = null!;

        public string? ViewerUserId { get; set; }
        public string? IpHash { get; set; }
        public string UserAgent { get; set; } = string.Empty;

        public DateTime ViewedAtUtc { get; set; }
    }
}

