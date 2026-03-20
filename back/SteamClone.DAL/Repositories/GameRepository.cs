using Microsoft.EntityFrameworkCore;
using SteamClone.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SteamClone.DAL.Repositories
{
    public class GameRepository : GenericRepository<GameEntity>
    {
        private readonly AppDbContext _context;
        public GameRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<GameEntity> Genres => GetAll();

        public IQueryable<GameEntity> GetCheaperThan(decimal price)
        {
            return _context.Games.AsNoTracking().Where(g => g.Price < price);
        }
        public IQueryable<GameEntity> GetByGenre(int genreId)
        {
            return _context.Games.AsNoTracking().Where(g => g.Genres.Any(game => game.Id == genreId));
        }
    }
}
