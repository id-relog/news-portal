using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.entity;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class NewsSourceService(INewsSourceRepository _repo, IMapper _mapper) : INewsSourceService
    {
        public bool Add(NewsSourceDTO entity)
        {
            if (!Validate(entity)) return false;
            var model = _mapper.Map<NewsSource>(entity);
            model.CreatedAtUtc = DateTime.UtcNow;
            return _repo.Add(model);
        }

        public bool Edit(NewsSourceDTO entity)
        {
            if (!Validate(entity)) return false;
            var model = _mapper.Map<NewsSource>(entity);
            return _repo.Edit(model);
        }

        public bool Delete(Guid entityId) => _repo.Delete(entityId);
        public NewsSourceDTO? Get(Guid entityId) => _mapper.Map<NewsSourceDTO?>(_repo.Get(entityId));
        public List<NewsSourceDTO> Get() => _mapper.Map<List<NewsSourceDTO>>(_repo.Get());

        public bool Validate(NewsSourceDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 128) return false;
            if (dto.Url != null && dto.Url.Length > 512) return false;
            return true;
        }
    }
}

