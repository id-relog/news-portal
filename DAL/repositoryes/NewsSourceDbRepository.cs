using DAL.entity;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.repositoryes
{
    internal class NewsSourceRepository(AgencyContext _context) : INewsSourceRepository
    {
        public bool Add(NewsSource entity)
        {
            _context.NewsSources.Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Edit(NewsSource entity)
        {
            var existing = _context.NewsSources.Find(entity.Id);
            if (existing == null) return false;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(Guid entityId)
        {
            if (_context.NewsSources.FirstOrDefault(x => x.Id == entityId) is not { } s)
                return false;
            _context.NewsSources.Remove(s);
            return _context.SaveChanges() > 0;
        }

        public NewsSource? Get(Guid entityId) =>
            _context.NewsSources.AsNoTracking().FirstOrDefault(x => x.Id == entityId);

        public List<NewsSource> Get() =>
            _context.NewsSources.AsNoTracking().OrderBy(x => x.Name).ToList();
    }
}

