namespace DAL.entity
{
    public class CategorySubscription
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public NewsCategory Category { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; }
    }
}

