using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IUserBanService : IService<UserBanDTO>
    {
        UserBanDTO? GetActiveBan(string userId);
        bool IsBanned(string userId);
        bool BanUser(string userId, string bannedByUserId, string reason, string evidenceMessage, Guid? evidenceCommentId);
    }
}

