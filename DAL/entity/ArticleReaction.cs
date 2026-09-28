namespace DAL.entity
{
    public class ArticleReaction
    {
        public Guid Id { get; set; }

        public Guid ArticleId { get; set; }
        public NewsArticle Article { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public bool IsLike { get; set; }
        public DateTime ReactedAtUtc { get; set; }
    }
}
