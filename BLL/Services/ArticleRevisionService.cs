using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.entity;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class ArticleRevisionService(IArticleRevisionRepository _repo, IMapper _mapper) : IArticleRevisionService
    {
        public bool Add(ArticleRevisionDTO entity)
        {
            if (!Validate(entity)) return false;
            var model = _mapper.Map<ArticleRevision>(entity);
            return _repo.Add(model);
        }

        public bool Edit(ArticleRevisionDTO entity)
        {
            if (!Validate(entity)) return false;
            var model = _mapper.Map<ArticleRevision>(entity);
            return _repo.Edit(model);
        }

        public bool Delete(Guid entityId) => _repo.Delete(entityId);
        public ArticleRevisionDTO? Get(Guid entityId) => _mapper.Map<ArticleRevisionDTO?>(_repo.Get(entityId));
        public List<ArticleRevisionDTO> Get() => _mapper.Map<List<ArticleRevisionDTO>>(_repo.Get());

        public List<ArticleRevisionDTO> GetForArticle(Guid articleId) =>
            _mapper.Map<List<ArticleRevisionDTO>>(_repo.GetForArticle(articleId));

        public ArticleRevisionDTO? CreateRevision(Guid articleId, string editedByUserId, string title, string? summary, string content)
        {
            if (articleId == Guid.Empty) return null;
            if (string.IsNullOrWhiteSpace(editedByUserId)) return null;
            if (string.IsNullOrWhiteSpace(title) || title.Length > 256) return null;
            if (string.IsNullOrWhiteSpace(content)) return null;

            var revision = new ArticleRevision
            {
                Id = Guid.NewGuid(),
                ArticleId = articleId,
                RevisionNumber = _repo.GetNextRevisionNumber(articleId),
                EditedByUserId = editedByUserId,
                EditedAtUtc = DateTime.UtcNow,
                Title = title,
                Summary = summary,
                Content = content
            };

            return _repo.Add(revision) ? _mapper.Map<ArticleRevisionDTO>(revision) : null;
        }

        public bool Validate(ArticleRevisionDTO dto)
        {
            if (dto.ArticleId == Guid.Empty) return false;
            if (dto.RevisionNumber <= 0) return false;
            if (string.IsNullOrWhiteSpace(dto.Title) || dto.Title.Length > 256) return false;
            if (string.IsNullOrWhiteSpace(dto.Content)) return false;
            if (string.IsNullOrWhiteSpace(dto.EditedByUserId)) return false;
            return true;
        }
    }
}

