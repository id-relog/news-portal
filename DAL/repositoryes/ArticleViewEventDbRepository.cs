using DAL.entity;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.repositoryes
{
    internal class ArticleViewEventRepository(AgencyContext _context) : IArticleViewEventRepository
    {
        public ArticleViewEvent? Get(Guid entityId) =>
            _context.ArticleViewEvents.AsNoTracking().FirstOrDefault(x => x.Id == entityId);

        public List<ArticleViewEvent> Get() =>
            _context.ArticleViewEvents.AsNoTracking().OrderByDescending(x => x.ViewedAtUtc).ToList();

        public bool Add(ArticleViewEvent entity)
        {
            _context.ArticleViewEvents.Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Edit(ArticleViewEvent entity)
        {
            var existing = _context.ArticleViewEvents.Find(entity.Id);
            if (existing == null) return false;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(Guid entityId)
        {
            if (_context.ArticleViewEvents.FirstOrDefault(x => x.Id == entityId) is not { } e)
                return false;
            _context.ArticleViewEvents.Remove(e);
            return _context.SaveChanges() > 0;
        }

        public int CountViews(Guid articleId) =>
            _context.ArticleViewEvents.AsNoTracking()
                .Count(x => x.ArticleId == articleId);

        public int CountViews(Guid articleId, DateTime fromUtc, DateTime toUtc) =>
            _context.ArticleViewEvents.AsNoTracking()
                .Count(x => x.ArticleId == articleId && x.ViewedAtUtc >= fromUtc && x.ViewedAtUtc <= toUtc);

        public Dictionary<Guid, int> CountViewsByArticle(DateTime fromUtc, DateTime toUtc, int take = 20) =>
            _context.ArticleViewEvents.AsNoTracking()
                .Where(x => x.ViewedAtUtc >= fromUtc && x.ViewedAtUtc <= toUtc)
                .GroupBy(x => x.ArticleId)
                .Select(g => new { ArticleId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(take)
                .ToDictionary(x => x.ArticleId, x => x.Count);
    }
}

