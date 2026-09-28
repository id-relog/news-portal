using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.entity;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class ArticleCommentService(IArticleCommentRepository _repo, IMapper _mapper) : IArticleCommentService
    {
        public bool Add(ArticleCommentDTO entity)
        {
            if (!Validate(entity)) return false;

            var model = _mapper.Map<ArticleComment>(entity);
            model.CreatedAtUtc = DateTime.UtcNow;
            model.Status = CommentStatus.Approved;

            return _repo.Add(model);
        }

        public bool Edit(ArticleCommentDTO entity)
        {
            if (!Validate(entity)) return false;
            var model = _mapper.Map<ArticleComment>(entity);
            return _repo.Edit(model);
        }

        public bool Delete(Guid entityId) => _repo.Delete(entityId);

        public ArticleCommentDTO? Get(Guid entityId) => _mapper.Map<ArticleCommentDTO?>(_repo.Get(entityId));

        public List<ArticleCommentDTO> Get() => _mapper.Map<List<ArticleCommentDTO>>(_repo.Get());

        public List<ArticleCommentDTO> GetForArticle(Guid articleId) =>
            _mapper.Map<List<ArticleCommentDTO>>(_repo.GetForArticle(articleId));

        public List<ArticleCommentDTO> GetPending(int take = 200) =>
            _mapper.Map<List<ArticleCommentDTO>>(_repo.GetPending(take));

        public bool Approve(Guid commentId, string moderatedByUserId) =>
            _repo.Moderate(commentId, CommentStatus.Approved, moderatedByUserId, DateTime.UtcNow);

        public bool Reject(Guid commentId, string moderatedByUserId) =>
            _repo.Moderate(commentId, CommentStatus.Rejected, moderatedByUserId, DateTime.UtcNow);

        public bool Validate(ArticleCommentDTO dto)
        {
            if (dto.ArticleId == Guid.Empty) return false;
            if (string.IsNullOrWhiteSpace(dto.DisplayName) || dto.DisplayName.Length > 128) return false;
            if (string.IsNullOrWhiteSpace(dto.Body) || dto.Body.Length > 4000) return false;
            return true;
        }
    }
}

