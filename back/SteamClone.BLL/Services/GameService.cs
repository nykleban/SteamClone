using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SteamClone.BLL.Dtos.Game;
using SteamClone.DAL.Entities;
using SteamClone.DAL.Repositories;

namespace SteamClone.BLL.Services
{
    public class GameService
    {
        private readonly GameRepository _gameRepository;
        private readonly GenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GameService(GameRepository gameRepository, GenreRepository genreRepository, IMapper mapper)
        {
            _gameRepository = gameRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _gameRepository.Games
                .Include(g => g.Genres)
                .ToListAsync();

            var dtos = _mapper.Map<List<GameDto>>(entities);

            return ServiceResponse.Success("Список ігор отримано", dtos);
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var entity = await _gameRepository.Games
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Гра з id '{id}' не знайдена");
            }

            var dto = _mapper.Map<GameDto>(entity);

            return ServiceResponse.Success("Гру отримано", dto);
        }

        public async Task<ServiceResponse> CreateAsync(CreateGameDto dto)
        {
            if (await _gameRepository.IsExistsAsync(dto.Name))
            {
                return ServiceResponse.Error($"Гра '{dto.Name}' вже існує");
            }

            var entity = _mapper.Map<GameEntity>(dto);

            // Attach genres
            if (dto.GenreIds.Count > 0)
            {
                var genres = await _genreRepository.Genres
                    .Where(g => dto.GenreIds.Contains(g.Id))
                    .ToListAsync();

                entity.Genres = genres;
            }

            bool res = await _gameRepository.CreateAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося додати гру");
            }

            var responseDto = _mapper.Map<GameDto>(entity);

            return ServiceResponse.Success($"Гра '{dto.Name}' успішно додана", responseDto);
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateGameDto dto)
        {
            var entity = await _gameRepository.Games
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(g => g.Id == dto.Id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Гра з id '{dto.Id}' не знайдена");
            }

            if (await _gameRepository.IsExistsAsync(dto.Name, dto.Id))
            {
                return ServiceResponse.Error($"Гра '{dto.Name}' вже існує");
            }

            string oldName = entity.Name;

            _mapper.Map(dto, entity);

            if (dto.GenreIds.Count > 0)
            {
                var genres = await _genreRepository.Genres
                    .Where(g => dto.GenreIds.Contains(g.Id))
                    .ToListAsync();

                entity.Genres = genres;
            }
            else
            {
                entity.Genres = [];
            }

            bool res = await _gameRepository.UpdateAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося змінити гру");
            }

            var responseDto = _mapper.Map<GameDto>(entity);

            return ServiceResponse.Success($"Гра '{oldName}' успішно змінена", responseDto);
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var entity = await _gameRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Гра з id '{id}' не знайдена");
            }

            bool res = await _gameRepository.DeleteAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося видалити гру");
            }

            return ServiceResponse.Success($"Гра '{entity.Name}' успішно видалена");
        }
    }
}
