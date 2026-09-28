using DAL.entity;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.repositoryes
{
    internal class UserBanRepository(AgencyContext _context) : IUserBanRepository
    {
        public bool Add(UserBan entity)
        {
            _context.UserBans.Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Edit(UserBan entity)
        {
            var existing = _context.UserBans.Find(entity.Id);
            if (existing == null) return false;
            _context.Entry(existing).CurrentValues.SetValues(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(Guid entityId)
        {
            if (_context.UserBans.FirstOrDefault(x => x.Id == entityId) is not { } b)
                return false;
            _context.UserBans.Remove(b);
            return _context.SaveChanges() > 0;
        }

        public UserBan? Get(Guid entityId) =>
            _context.UserBans.AsNoTracking().FirstOrDefault(x => x.Id == entityId);

        public List<UserBan> Get() =>
            _context.UserBans.AsNoTracking().OrderByDescending(x => x.BannedAtUtc).ToList();

        public UserBan? GetActiveBan(string userId) =>
            _context.UserBans.AsNoTracking().FirstOrDefault(x => x.UserId == userId && x.IsActive);

        public bool SetActive(Guid banId, bool isActive)
        {
            var b = _context.UserBans.Find(banId);
            if (b == null) return false;
            b.IsActive = isActive;
            return _context.SaveChanges() > 0;
        }
    }
}

