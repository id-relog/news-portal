namespace BLL.DTO
{
    public class ArticleRevisionDTO
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public int RevisionNumber { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string Content { get; set; } = string.Empty;

        public string EditedByUserId { get; set; } = string.Empty;
        public DateTime EditedAtUtc { get; set; }
    }
}

