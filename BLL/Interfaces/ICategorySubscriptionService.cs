using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ICategorySubscriptionService : IService<CategorySubscriptionDTO>
    {
        List<CategorySubscriptionDTO> GetForUser(string userId);
        bool Unsubscribe(string userId, Guid categoryId);
    }
}

