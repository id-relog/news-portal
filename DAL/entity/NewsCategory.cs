namespace DAL.entity
{
    public class NewsCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public DateTime CreatedAtUtc { get; set; }
    }
}

