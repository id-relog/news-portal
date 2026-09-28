using DAL.entity;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.repositoryes
{
    internal class ArticleCommentRepository(AgencyContext _context) : IArticleCommentRepository
    {
        public ArticleComment? Get(Guid entityId) =>
            _context.ArticleComments.AsNoTracking().FirstOrDefault(x => x.Id == entityId);

        public List<ArticleComment> Get() =>
            _context.ArticleComments.AsNoTracking().OrderByDescending(x => x.CreatedAtUtc).ToList();

        public bool Add(ArticleComment entity)
        {
            _context.ArticleComments.Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Edit(ArticleComment entity)
        {
            var existing = _context.ArticleComments.Find(entity.Id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(Guid entityId)
        {
            if (_context.ArticleComments.FirstOrDefault(x => x.Id == entityId) is not { } c)
                return false;

            _context.ArticleComments.Remove(c);
            return _context.SaveChanges() > 0;
        }

        public List<ArticleComment> GetForArticle(Guid articleId) =>
            _context.ArticleComments
                .AsNoTracking()
                .Where(x => x.ArticleId == articleId && x.Status == CommentStatus.Approved)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToList();

        public List<ArticleComment> GetPending(int take = 200) =>
            _context.ArticleComments
                .AsNoTracking()
                .Where(x => x.Status == CommentStatus.Pending)
                .OrderBy(x => x.CreatedAtUtc)
                .Take(take)
                .ToList();

        public bool Moderate(Guid commentId, CommentStatus status, string moderatedByUserId, DateTime moderatedAtUtc)
        {
            var comment = _context.ArticleComments.Find(commentId);
            if (comment == null) return false;

            comment.Status = status;
            comment.ModeratedByUserId = moderatedByUserId;
            comment.ModeratedAtUtc = moderatedAtUtc;
            return _context.SaveChanges() > 0;
        }

        public bool DeleteByAuthorUserId(string authorUserId)
        {
            var comments = _context.ArticleComments.Where(x => x.AuthorUserId == authorUserId).ToList();
            if (comments.Count == 0) return true;
            _context.ArticleComments.RemoveRange(comments);
            return _context.SaveChanges() > 0;
        }
    }
}

