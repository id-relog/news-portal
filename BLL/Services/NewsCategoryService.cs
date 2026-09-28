using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.entity;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class NewsCategoryService(INewsCategoryRepository _repo, IMapper _mapper) : INewsCategoryService
    {
        public bool Add(NewsCategoryDTO entity)
        {
            if (!Validate(entity)) return false;
            var model = _mapper.Map<NewsCategory>(entity);
            return _repo.Add(model);
        }

        public bool Edit(NewsCategoryDTO entity)
        {
            if (!Validate(entity)) return false;
            var model = _mapper.Map<NewsCategory>(entity);
            return _repo.Edit(model);
        }

        public bool Delete(Guid entityId) => _repo.Delete(entityId);

        public NewsCategoryDTO? Get(Guid entityId) => _mapper.Map<NewsCategoryDTO?>(_repo.Get(entityId));

        public List<NewsCategoryDTO> Get() => _mapper.Map<List<NewsCategoryDTO>>(_repo.Get());

        public NewsCategoryDTO? GetBySlug(string slug) => _mapper.Map<NewsCategoryDTO?>(_repo.GetBySlug(slug));

        public bool Validate(NewsCategoryDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) return false;
            if (string.IsNullOrWhiteSpace(dto.Slug)) return false;
            if (dto.Name.Length > 128) return false;
            if (dto.Slug.Length > 128) return false;
            return true;
        }
    }
}

