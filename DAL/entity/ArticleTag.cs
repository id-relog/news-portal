namespace DAL.entity
{
    public class ArticleTag
    {
        public Guid ArticleId { get; set; }
        public NewsArticle Article { get; set; } = null!;

        public Guid TagId { get; set; }
        public NewsTag Tag { get; set; } = null!;
    }
}

