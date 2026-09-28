using DAL.entity;

namespace DAL.Interfaces
{
    public interface ICategorySubscriptionRepository : IRepository<CategorySubscription>
    {
        List<CategorySubscription> GetForUser(string userId);
        bool Unsubscribe(string userId, Guid categoryId);
    }
}

