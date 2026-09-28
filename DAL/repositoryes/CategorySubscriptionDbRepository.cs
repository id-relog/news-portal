using DAL.entity;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.repositoryes
{
    internal class CategorySubscriptionRepository(AgencyContext _context) : ICategorySubscriptionRepository
    {
        public CategorySubscription? Get(Guid entityId) =>
            _context.CategorySubscriptions.AsNoTracking().Include(x => x.Category).FirstOrDefault(x => x.Id == entityId);

        public List<CategorySubscription> Get() =>
            _context.CategorySubscriptions.AsNoTracking().Include(x => x.Category).OrderByDescending(x => x.CreatedAtUtc).ToList();

        public bool Add(CategorySubscription entity)
        {
            _context.CategorySubscriptions.Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Edit(CategorySubscription entity)
        {
            var existing = _context.CategorySubscriptions.Find(entity.Id);
            if (existing == null) return false;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(Guid entityId)
        {
            if (_context.CategorySubscriptions.FirstOrDefault(x => x.Id == entityId) is not { } s)
                return false;
            _context.CategorySubscriptions.Remove(s);
            return _context.SaveChanges() > 0;
        }

        public List<CategorySubscription> GetForUser(string userId) =>
            _context.CategorySubscriptions.AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToList();

        public bool Unsubscribe(string userId, Guid categoryId)
        {
            var entity = _context.CategorySubscriptions.FirstOrDefault(x => x.UserId == userId && x.CategoryId == categoryId);
            if (entity == null) return false;
            _context.CategorySubscriptions.Remove(entity);
            return _context.SaveChanges() > 0;
        }
    }
}

