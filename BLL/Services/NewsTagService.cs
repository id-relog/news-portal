using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.entity;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class NewsTagService(INewsTagRepository _repo, IMapper _mapper) : INewsTagService
    {
        public bool Add(NewsTagDTO entity)
        {
            if (!Validate(entity)) return false;
            var model = _mapper.Map<NewsTag>(entity);
            return _repo.Add(model);
        }

        public bool Edit(NewsTagDTO entity)
        {
            if (!Validate(entity)) return false;
            var model = _mapper.Map<NewsTag>(entity);
            return _repo.Edit(model);
        }

        public bool Delete(Guid entityId) => _repo.Delete(entityId);
        public NewsTagDTO? Get(Guid entityId) => _mapper.Map<NewsTagDTO?>(_repo.Get(entityId));
        public List<NewsTagDTO> Get() => _mapper.Map<List<NewsTagDTO>>(_repo.Get());

        public NewsTagDTO? GetBySlug(string slug) => _mapper.Map<NewsTagDTO?>(_repo.GetBySlug(slug));

        public bool Validate(NewsTagDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 128) return false;
            if (string.IsNullOrWhiteSpace(dto.Slug) || dto.Slug.Length > 128) return false;
            return true;
        }
    }
}

