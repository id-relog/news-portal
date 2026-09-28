using DAL.entity;

namespace DAL.Interfaces
{
    public interface INewsTagRepository : IRepository<NewsTag>
    {
        NewsTag? GetBySlug(string slug);
    }
}

