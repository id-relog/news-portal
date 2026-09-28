using DAL.entity;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.repositoryes
{
    internal class NewsTagRepository(AgencyContext _context) : INewsTagRepository
    {
        public bool Add(NewsTag entity)
        {
            _context.NewsTags.Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Edit(NewsTag entity)
        {
            var existing = _context.NewsTags.Find(entity.Id);
            if (existing == null) return false;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(Guid entityId)
        {
            if (_context.NewsTags.FirstOrDefault(x => x.Id == entityId) is not { } tag)
                return false;
            _context.NewsTags.Remove(tag);
            return _context.SaveChanges() > 0;
        }

        public NewsTag? Get(Guid entityId) =>
            _context.NewsTags.AsNoTracking().FirstOrDefault(x => x.Id == entityId);

        public List<NewsTag> Get() =>
            _context.NewsTags.AsNoTracking().OrderBy(x => x.Name).ToList();

        public NewsTag? GetBySlug(string slug) =>
            _context.NewsTags.AsNoTracking().FirstOrDefault(x => x.Slug == slug);
    }
}

