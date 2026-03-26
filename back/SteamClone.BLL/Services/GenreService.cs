using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SteamClone.BLL.Dtos.Genre;
using SteamClone.DAL.Entities;
using SteamClone.DAL.Repositories;

namespace SteamClone.BLL.Services
{
    public class GenreService
    {
        private readonly GenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenreService(GenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _genreRepository.Genres
                .ToListAsync();

            var dtos = _mapper.Map<List<GenreDto>>(entities);

            return ServiceResponse.Success("Список жанрів отримано", dtos);
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var entity = await _genreRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Жанр з id '{id}' не знайдений");
            }

            var dto = _mapper.Map<GenreDto>(entity);

            return ServiceResponse.Success("Жанр отримано", dto);
        }

        public async Task<ServiceResponse> CreateAsync(CreateGenreDto dto)
        {
            if (await _genreRepository.IsExitsAsync(dto.Name))
            {
                return ServiceResponse.Error($"Жанр '{dto.Name}' вже існує");
            }

            var entity = _mapper.Map<GenreEntity>(dto);

            bool res = await _genreRepository.CreateAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося додати жанр");
            }

            var responseDto = _mapper.Map<GenreDto>(entity);

            return ServiceResponse.Success($"Жанр '{dto.Name}' успішно доданий", responseDto);
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateGenreDto dto)
        {
            var entity = await _genreRepository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Жанр з id '{dto.Id}' не знайдений");
            }

            var existing = await _genreRepository.GetByNameAsync(dto.Name);
            if (existing != null && existing.Id != dto.Id)
            {
                return ServiceResponse.Error($"Жанр '{dto.Name}' вже існує");
            }

            string oldName = entity.Name;

            _mapper.Map(dto, entity);

            bool res = await _genreRepository.UpdateAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося змінити жанр");
            }

            var responseDto = _mapper.Map<GenreDto>(entity);

            return ServiceResponse.Success($"Жанр '{oldName}' успішно змінений", responseDto);
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var entity = await _genreRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Жанр з id '{id}' не знайдений");
            }

            bool res = await _genreRepository.DeleteAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося видалити жанр");
            }

            return ServiceResponse.Success($"Жанр '{entity.Name}' успішно видалений");
        }
    }
}
