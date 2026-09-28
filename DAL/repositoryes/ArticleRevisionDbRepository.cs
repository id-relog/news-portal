using DAL.entity;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.repositoryes
{
    internal class ArticleRevisionRepository(AgencyContext _context) : IArticleRevisionRepository
    {
        public ArticleRevision? Get(Guid entityId) =>
            _context.ArticleRevisions.AsNoTracking().FirstOrDefault(x => x.Id == entityId);

        public List<ArticleRevision> Get() =>
            _context.ArticleRevisions.AsNoTracking().OrderByDescending(x => x.EditedAtUtc).ToList();

        public bool Add(ArticleRevision entity)
        {
            _context.ArticleRevisions.Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Edit(ArticleRevision entity)
        {
            var existing = _context.ArticleRevisions.Find(entity.Id);
            if (existing == null) return false;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(Guid entityId)
        {
            if (_context.ArticleRevisions.FirstOrDefault(x => x.Id == entityId) is not { } r)
                return false;
            _context.ArticleRevisions.Remove(r);
            return _context.SaveChanges() > 0;
        }

        public List<ArticleRevision> GetForArticle(Guid articleId) =>
            _context.ArticleRevisions
                .AsNoTracking()
                .Where(x => x.ArticleId == articleId)
                .OrderByDescending(x => x.RevisionNumber)
                .ToList();

        public int GetNextRevisionNumber(Guid articleId)
        {
            var max = _context.ArticleRevisions
                .AsNoTracking()
                .Where(x => x.ArticleId == articleId)
                .Select(x => (int?)x.RevisionNumber)
                .Max();

            return (max ?? 0) + 1;
        }
    }
}

