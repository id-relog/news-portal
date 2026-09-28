namespace DAL.entity
{
    public class ArticleRevision
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public NewsArticle Article { get; set; } = null!;

        public int RevisionNumber { get; set; }

        public string Title { get; set; } = null!;
        public string? Summary { get; set; }
        public string Content { get; set; } = null!;

        public string EditedByUserId { get; set; } = null!;
        public DateTime EditedAtUtc { get; set; }
    }
}

