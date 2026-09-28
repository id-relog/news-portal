using DAL.entity;

namespace DAL.Interfaces
{
    public interface IUserBanRepository : IRepository<UserBan>
    {
        UserBan? GetActiveBan(string userId);
        bool SetActive(Guid banId, bool isActive);
    }
}

