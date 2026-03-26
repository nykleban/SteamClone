using Microsoft.EntityFrameworkCore;
using SteamClone.DAL.Entities;

namespace SteamClone.DAL.Repositories
{
    public class GameImageRepository : GenericRepository<GameImageEntity>
    {
        private readonly AppDbContext _context;

        public GameImageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<GameImageEntity> GetByGameId(int gameId)
        {
            return _context.GameImages
                .AsNoTracking()
                .Where(gi => gi.GameId == gameId);
        }

        public async Task<GameImageEntity?> GetByGameName(string gameName)
        {
            return await _context.GameImages
                .Include(gi => gi.Game)
                .FirstOrDefaultAsync(gi => gi.Game != null && gi.Game.Name.ToLower().Trim() == gameName.ToLower().Trim());
        }
    }
}
