using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.entity;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class NewsArticleService(INewsArticleRepository _repo, IMapper _mapper) : INewsArticleService
    {
        public bool Add(NewsArticleDTO entity)
        {
            if (!Validate(entity)) return false;

            var model = _mapper.Map<NewsArticle>(entity);
            model.CreatedAtUtc = DateTime.UtcNow;
            model.UpdatedAtUtc = model.CreatedAtUtc;
            model.Status = ArticleStatus.Draft;
            model.PublishedAtUtc = null;

            var added = _repo.Add(model);
            if (!added) return false;

            return _repo.SetTags(model.Id, entity.Tags);
        }

        public bool Edit(NewsArticleDTO entity)
        {
            if (!Validate(entity)) return false;

            var model = _mapper.Map<NewsArticle>(entity);
            model.UpdatedAtUtc = DateTime.UtcNow;
            var updated = _repo.Edit(model);
            if (!updated) return false;

            return _repo.SetTags(model.Id, entity.Tags);
        }

        public bool Delete(Guid entityId) => _repo.Delete(entityId);

        public NewsArticleDTO? Get(Guid entityId) => _mapper.Map<NewsArticleDTO?>(_repo.Get(entityId));

        public List<NewsArticleDTO> Get() => _mapper.Map<List<NewsArticleDTO>>(_repo.Get());

        public List<NewsArticleDTO> GetPublished(int take = 50) => _mapper.Map<List<NewsArticleDTO>>(_repo.GetPublished(take));

        public List<NewsArticleDTO> GetByCategory(Guid categoryId, int take = 50) =>
            _mapper.Map<List<NewsArticleDTO>>(_repo.GetByCategory(categoryId, take));

        public NewsArticleDTO? GetBySlug(string slug) => _mapper.Map<NewsArticleDTO?>(_repo.GetBySlug(slug));

        public List<NewsArticleDTO> GetPendingReview(int take = 200)
        {
            var list = _repo.Get()
                .Where(x => x.Status == ArticleStatus.PendingReview)
                .OrderByDescending(x => x.UpdatedAtUtc)
                .Take(take)
                .ToList();
            return _mapper.Map<List<NewsArticleDTO>>(list);
        }

        public bool SubmitForReview(Guid articleId) =>
            _repo.SetStatus(articleId, ArticleStatus.PendingReview, null);

        public bool Approve(Guid articleId, string reviewedByUserId)
        {
            if (string.IsNullOrWhiteSpace(reviewedByUserId)) return false;

            var ok = _repo.SetStatus(articleId, ArticleStatus.Published, DateTime.UtcNow);
            if (!ok) return false;

            var dto = Get(articleId);
            if (dto == null) return true;
            dto.ReviewedAtUtc = DateTime.UtcNow;
            dto.ReviewedByUserId = reviewedByUserId;
            dto.ReviewNote = null;
            return Edit(dto);
        }

        public bool Reject(Guid articleId, string reviewedByUserId, string note)
        {
            if (string.IsNullOrWhiteSpace(reviewedByUserId)) return false;

            var dto = Get(articleId);
            if (dto == null) return false;

            dto.Status = ArticleStatusDTO.Draft;
            dto.ReviewedAtUtc = DateTime.UtcNow;
            dto.ReviewedByUserId = reviewedByUserId;
            dto.ReviewNote = string.IsNullOrWhiteSpace(note) ? "Отклонено" : note;
            dto.PublishedAtUtc = null;
            return Edit(dto);
        }

        public bool Archive(Guid articleId) =>
            _repo.SetStatus(articleId, ArticleStatus.Archived, null);

        public bool Validate(NewsArticleDTO dto)
        {
            if (dto.CategoryId == Guid.Empty) return false;
            if (string.IsNullOrWhiteSpace(dto.Title) || dto.Title.Length > 256) return false;
            if (string.IsNullOrWhiteSpace(dto.Slug) || dto.Slug.Length > 256) return false;
            if (string.IsNullOrWhiteSpace(dto.Content)) return false;
            if (string.IsNullOrWhiteSpace(dto.AuthorUserId)) return false;
            return true;
        }
    }
}

