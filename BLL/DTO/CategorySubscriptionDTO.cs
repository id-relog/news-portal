namespace BLL.DTO
{
    public class CategorySubscriptionDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}

