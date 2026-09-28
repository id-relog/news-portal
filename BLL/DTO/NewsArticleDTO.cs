namespace BLL.DTO
{
    public class NewsArticleDTO
    {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? ImageUrl { get; set; }
        public string Content { get; set; } = string.Empty;

        public string AuthorUserId { get; set; } = string.Empty;

        public ArticleStatusDTO Status { get; set; }
        public DateTime? PublishedAtUtc { get; set; }
        public DateTime? ReviewedAtUtc { get; set; }
        public string? ReviewedByUserId { get; set; }
        public string? ReviewNote { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }

        public int ViewsCount { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}

