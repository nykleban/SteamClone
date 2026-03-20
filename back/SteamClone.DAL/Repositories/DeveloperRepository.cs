using SteamClone.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
            return await GetByNameAsync(name) != null;
        }
    }
}
