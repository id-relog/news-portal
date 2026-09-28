using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.entity;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class CategorySubscriptionService(ICategorySubscriptionRepository _repo, IMapper _mapper) : ICategorySubscriptionService
    {
        public bool Add(CategorySubscriptionDTO entity)
        {
            if (!Validate(entity)) return false;

            var model = _mapper.Map<CategorySubscription>(entity);
            model.CreatedAtUtc = DateTime.UtcNow;
            return _repo.Add(model);
        }

        public bool Edit(CategorySubscriptionDTO entity)
        {
            if (!Validate(entity)) return false;
            var model = _mapper.Map<CategorySubscription>(entity);
            return _repo.Edit(model);
        }

        public bool Delete(Guid entityId) => _repo.Delete(entityId);
        public CategorySubscriptionDTO? Get(Guid entityId) => _mapper.Map<CategorySubscriptionDTO?>(_repo.Get(entityId));
        public List<CategorySubscriptionDTO> Get() => _mapper.Map<List<CategorySubscriptionDTO>>(_repo.Get());

        public List<CategorySubscriptionDTO> GetForUser(string userId) =>
            _mapper.Map<List<CategorySubscriptionDTO>>(_repo.GetForUser(userId));

        public bool Unsubscribe(string userId, Guid categoryId) => _repo.Unsubscribe(userId, categoryId);

        public bool Validate(CategorySubscriptionDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId)) return false;
            if (dto.CategoryId == Guid.Empty) return false;
            return true;
        }
    }
}

