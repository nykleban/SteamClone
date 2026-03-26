using Microsoft.EntityFrameworkCore;
using SteamClone.DAL.Entities;

namespace SteamClone.DAL.Repositories
{
    public class DeveloperRepository : GenericRepository<DeveloperEntity>
    {
        private readonly AppDbContext _context;

        public DeveloperRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IQueryable<DeveloperEntity> Developers => GetAll();

        public async Task<DeveloperEntity?> GetByNameAsync(string name)
        {
            return await _context.Developers
                .Where(d => d.Name.ToLower().Trim() == name.ToLower().Trim())
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsAsync(string name)
        {
            return await Developers
                .AsNoTracking()
                .AnyAsync(d => d.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> IsExistsAsync(string name, params int[] exceptionIds)
        {
            return await Developers
                .AsNoTracking()
                .AnyAsync(d => d.Name.ToLower() == name.ToLower()
                && !exceptionIds.Contains(d.Id));
        }
    }
}
