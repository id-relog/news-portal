using DAL.entity;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.repositoryes
{
    internal class ArticleReactionsRepository(AgencyContext _context) : IArticleReactionsRepository
    {
        public ArticleReaction? Get(Guid entityId) =>
            _context.ArticleReactions.AsNoTracking().FirstOrDefault(x => x.Id == entityId);

        public List<ArticleReaction> Get() =>
            _context.ArticleReactions.AsNoTracking().OrderByDescending(x => x.ReactedAtUtc).ToList();

        public bool Add(ArticleReaction entity)
        {
            _context.ArticleReactions.Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Edit(ArticleReaction entity)
        {
            var existing = _context.ArticleReactions.Find(entity.Id);
            if (existing == null) return false;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(Guid entityId)
        {
            if (_context.ArticleReactions.FirstOrDefault(x => x.Id == entityId) is not { } reaction)
                return false;

            _context.ArticleReactions.Remove(reaction);
            return _context.SaveChanges() > 0;
        }

        public bool SetReaction(Guid articleId, string userId, bool isLike)
        {
            var existing = _context.ArticleReactions
                .FirstOrDefault(x => x.ArticleId == articleId && x.UserId == userId);

            if (existing == null)
            {
                _context.ArticleReactions.Add(new ArticleReaction
                {
                    Id = Guid.NewGuid(),
                    ArticleId = articleId,
                    UserId = userId,
                    IsLike = isLike,
                    ReactedAtUtc = DateTime.UtcNow
                });
            }
            else
            {
                existing.IsLike = isLike;
                existing.ReactedAtUtc = DateTime.UtcNow;
            }

            return _context.SaveChanges() > 0;
        }

        public (int likes, int dislikes) GetCounts(Guid articleId)
        {
            var likes = _context.ArticleReactions.AsNoTracking().Count(x => x.ArticleId == articleId && x.IsLike);
            var dislikes = _context.ArticleReactions.AsNoTracking().Count(x => x.ArticleId == articleId && !x.IsLike);
            return (likes, dislikes);
        }
    }
}
