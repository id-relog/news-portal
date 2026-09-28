using DAL.entity;
using DAL.Interfaces;

namespace DAL.Interfaces
{
    public interface INewsCategoryRepository : IRepository<NewsCategory>
    {
        NewsCategory? GetBySlug(string slug);
    }
}

