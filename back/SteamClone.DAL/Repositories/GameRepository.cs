using Microsoft.EntityFrameworkCore;
using SteamClone.DAL.Entities;

namespace SteamClone.DAL.Repositories
{
    public class GameRepository : GenericRepository<GameEntity>
    {
        private readonly AppDbContext _context;

        public GameRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<GameEntity> Games => GetAll();

        public async Task<GameEntity?> GetByNameAsync(string name)
        {
            return await _context.Games
                .Where(g => g.Name.ToLower().Trim() == name.ToLower().Trim())
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsAsync(string name)
        {
            return await Games
                .AsNoTracking()
                .AnyAsync(g => g.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> IsExistsAsync(string name, params int[] exceptionIds)
        {
            return await Games
                .AsNoTracking()
                .AnyAsync(g => g.Name.ToLower() == name.ToLower()
                && !exceptionIds.Contains(g.Id));
        }

        public IQueryable<GameEntity> GetCheaperThan(decimal price)
        {
            return _context.Games.AsNoTracking().Where(g => g.Price < price);
        }

        public IQueryable<GameEntity> GetByGenre(int genreId)
        {
            return _context.Games.AsNoTracking().Where(g => g.Genres.Any(genre => genre.Id == genreId));
        }
    }
}
