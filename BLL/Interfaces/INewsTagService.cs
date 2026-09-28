using BLL.DTO;

namespace BLL.Interfaces
{
    public interface INewsTagService : IService<NewsTagDTO>
    {
        NewsTagDTO? GetBySlug(string slug);
    }
}

