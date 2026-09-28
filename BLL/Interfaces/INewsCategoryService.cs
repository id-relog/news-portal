using BLL.DTO;

namespace BLL.Interfaces
{
    public interface INewsCategoryService : IService<NewsCategoryDTO>
    {
        NewsCategoryDTO? GetBySlug(string slug);
    }
}

