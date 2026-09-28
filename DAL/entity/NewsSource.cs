namespace DAL.entity
{
    public class NewsSource
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Url { get; set; }
        public bool IsTrusted { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}

