using DAL.entity;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.repositoryes
{
    internal class NewsCategoryRepository(AgencyContext _context) : INewsCategoryRepository
    {
        public bool Add(NewsCategory entity)
        {
            _context.NewsCategories.Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Edit(NewsCategory entity)
        {
            var existing = _context.NewsCategories.Find(entity.Id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(Guid entityId)
        {
            if (_context.NewsCategories.FirstOrDefault(x => x.Id == entityId) is not { } cat)
                return false;

            _context.NewsCategories.Remove(cat);
            return _context.SaveChanges() > 0;
        }

        public NewsCategory? Get(Guid entityId) =>
            _context.NewsCategories.AsNoTracking().FirstOrDefault(x => x.Id == entityId);

        public List<NewsCategory> Get() =>
            _context.NewsCategories.AsNoTracking().OrderBy(x => x.Name).ToList();

        public NewsCategory? GetBySlug(string slug) =>
            _context.NewsCategories.AsNoTracking().FirstOrDefault(x => x.Slug == slug);
    }
}

