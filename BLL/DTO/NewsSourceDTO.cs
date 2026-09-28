namespace BLL.DTO
{
    public class NewsSourceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Url { get; set; }
        public bool IsTrusted { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}

