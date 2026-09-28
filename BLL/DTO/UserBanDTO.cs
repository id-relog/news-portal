namespace BLL.DTO
{
    public class UserBanDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string EvidenceMessage { get; set; } = string.Empty;
        public Guid? EvidenceCommentId { get; set; }
        public DateTime BannedAtUtc { get; set; }
        public string BannedByUserId { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}

