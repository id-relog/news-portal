using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.entity;
using DAL.Interfaces;

namespace BLL.Services
{
    internal class UserBanService(IUserBanRepository _banRepo, IArticleCommentRepository _commentRepo, IMapper _mapper) : IUserBanService
    {
        public bool Add(UserBanDTO entity)
        {
            if (!Validate(entity)) return false;
            var model = _mapper.Map<UserBan>(entity);
            return _banRepo.Add(model);
        }

        public bool Edit(UserBanDTO entity)
        {
            if (!Validate(entity)) return false;
            var model = _mapper.Map<UserBan>(entity);
            return _banRepo.Edit(model);
        }

        public bool Delete(Guid entityId) => _banRepo.Delete(entityId);
        public UserBanDTO? Get(Guid entityId) => _mapper.Map<UserBanDTO?>(_banRepo.Get(entityId));
        public List<UserBanDTO> Get() => _mapper.Map<List<UserBanDTO>>(_banRepo.Get());

        public UserBanDTO? GetActiveBan(string userId) => _mapper.Map<UserBanDTO?>(_banRepo.GetActiveBan(userId));
        public bool IsBanned(string userId) => _banRepo.GetActiveBan(userId) != null;

        public bool BanUser(string userId, string bannedByUserId, string reason, string evidenceMessage, Guid? evidenceCommentId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return false;
            if (string.IsNullOrWhiteSpace(bannedByUserId)) return false;
            if (string.IsNullOrWhiteSpace(reason)) reason = "Нарушение правил";
            if (string.IsNullOrWhiteSpace(evidenceMessage)) evidenceMessage = "(пусто)";

            var ban = new UserBan
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BannedByUserId = bannedByUserId,
                Reason = reason.Length > 256 ? reason[..256] : reason,
                EvidenceMessage = evidenceMessage.Length > 4000 ? evidenceMessage[..4000] : evidenceMessage,
                EvidenceCommentId = evidenceCommentId,
                BannedAtUtc = DateTime.UtcNow,
                IsActive = true
            };

            var ok = _banRepo.Add(ban);
            if (!ok) return false;

            _commentRepo.DeleteByAuthorUserId(userId);
            return true;
        }

        public bool Validate(UserBanDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId)) return false;
            if (string.IsNullOrWhiteSpace(dto.BannedByUserId)) return false;
            if (string.IsNullOrWhiteSpace(dto.Reason)) return false;
            if (string.IsNullOrWhiteSpace(dto.EvidenceMessage)) return false;
            return true;
        }
    }
}

