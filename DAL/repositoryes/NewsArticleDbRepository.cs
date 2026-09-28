using DAL.entity;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.repositoryes
{
    internal class NewsArticleRepository(AgencyContext _context) : INewsArticleRepository
    {
        public NewsArticle? Get(Guid entityId) =>
            _context.NewsArticles
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .FirstOrDefault(x => x.Id == entityId);

        public List<NewsArticle> Get() =>
            _context.NewsArticles
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToList();

        public bool Add(NewsArticle entity)
        {
            _context.NewsArticles.Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Edit(NewsArticle entity)
        {
            var existing = _context.NewsArticles.Find(entity.Id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(Guid entityId)
        {
            if (_context.NewsArticles.FirstOrDefault(x => x.Id == entityId) is not { } article)
                return false;

            _context.NewsArticles.Remove(article);
            return _context.SaveChanges() > 0;
        }

        public List<NewsArticle> GetPublished(int take = 50) =>
            _context.NewsArticles
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .Where(x => x.Status == ArticleStatus.Published)
                .OrderByDescending(x => x.PublishedAtUtc ?? x.CreatedAtUtc)
                .Take(take)
                .ToList();

        public List<NewsArticle> GetByCategory(Guid categoryId, int take = 50) =>
            _context.NewsArticles
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .Where(x => x.CategoryId == categoryId && x.Status == ArticleStatus.Published)
                .OrderByDescending(x => x.PublishedAtUtc ?? x.CreatedAtUtc)
                .Take(take)
                .ToList();

        public NewsArticle? GetBySlug(string slug) =>
            _context.NewsArticles
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .FirstOrDefault(x => x.Slug == slug);

        public bool SetStatus(Guid articleId, ArticleStatus status, DateTime? publishedAtUtc)
        {
            var article = _context.NewsArticles.Find(articleId);
            if (article == null) return false;

            article.Status = status;
            article.PublishedAtUtc = publishedAtUtc;
            if (status != ArticleStatus.Published)
            {
                article.PublishedAtUtc = null;
            }
            article.UpdatedAtUtc = DateTime.UtcNow;
            return _context.SaveChanges() > 0;
        }

        public bool SetTags(Guid articleId, List<string> tagNames)
        {
            var article = _context.NewsArticles
                .Include(x => x.Tags)
                .FirstOrDefault(x => x.Id == articleId);
            if (article == null) return false;

            var normalized = tagNames
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existingTags = _context.NewsTags.ToList();
            var selectedTagIds = new List<Guid>();

            foreach (var name in normalized)
            {
                var tag = existingTags.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (tag == null)
                {
                    tag = new NewsTag
                    {
                        Id = Guid.NewGuid(),
                        Name = name,
                        Slug = name.ToLowerInvariant().Replace(' ', '-'),
                        CreatedAtUtc = DateTime.UtcNow
                    };
                    _context.NewsTags.Add(tag);
                    existingTags.Add(tag);
                }
                selectedTagIds.Add(tag.Id);
            }

            var toRemove = article.Tags.Where(x => !selectedTagIds.Contains(x.TagId)).ToList();
            if (toRemove.Count > 0)
            {
                _context.ArticleTags.RemoveRange(toRemove);
            }

            var existingForArticle = article.Tags.Select(x => x.TagId).ToHashSet();
            var toAdd = selectedTagIds
                .Where(tagId => !existingForArticle.Contains(tagId))
                .Select(tagId => new ArticleTag { ArticleId = articleId, TagId = tagId });
            _context.ArticleTags.AddRange(toAdd);

            return _context.SaveChanges() >= 0;
        }
    }
}

