namespace DAL.entity
{
    public class UserBan
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public string EvidenceMessage { get; set; } = null!;
        public Guid? EvidenceCommentId { get; set; }
        public DateTime BannedAtUtc { get; set; }
        public string BannedByUserId { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}

