namespace DAL.entity
{
    public class NewsArticle
    {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }
        public NewsCategory Category { get; set; } = null!;

        public Guid? SourceId { get; set; }
        public NewsSource? Source { get; set; }

        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Summary { get; set; }
        public string? ImageUrl { get; set; }
        public string Content { get; set; } = null!;

        public string AuthorUserId { get; set; } = null!;

        public ArticleStatus Status { get; set; }
        public DateTime? PublishedAtUtc { get; set; }
        public DateTime? ReviewedAtUtc { get; set; }
        public string? ReviewedByUserId { get; set; }
        public string? ReviewNote { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }

        public List<ArticleTag> Tags { get; set; } = new();
    }
}

