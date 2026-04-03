using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SteamClone.BLL.Dtos.Game;
using SteamClone.BLL.Settings;
using SteamClone.DAL.Entities;
using SteamClone.DAL.Repositories;

namespace SteamClone.BLL.Services
{
    public class GameService
    {
        private readonly GameRepository _gameRepository;
        private readonly GenreRepository _genreRepository;
        private readonly FileService _fileService;
        private readonly IMapper _mapper;

        public GameService(GameRepository gameRepository, IMapper mapper, GenreRepository genreRepository, FileService fileService)
        {
            _gameRepository = gameRepository;
            _mapper = mapper;
            _genreRepository = genreRepository;
            _fileService = fileService;
        }

        private async Task SaveImagesAsync(GameEntity entity, CreateGameDto dto)
        {
            string guid = Guid.NewGuid().ToString();
            string folderPath = Path.Combine(StaticFilesSettings.Games, guid);
            if (dto.PreviewImage != null)
            {
                var res = await _fileService.SaveImageAsync(dto.PreviewImage, folderPath);
                if (res.IsSuccess)
                {
                    var previewImage = new GameImageEntity
                    {
                        IsPreview = true,
                        Name = $"{guid}/{res.Payload!}",
                    };
                    entity.Images.Add(previewImage);
                }
            }

            if (dto.Images.Count > 0)
            {
                var res = await _fileService.SaveImagesAsync(dto.Images, folderPath);

                foreach (var r in res)
                {
                    if (r.IsSuccess)
                    {
                        var image = new GameImageEntity
                        {
                            IsPreview = false,
                            Name = $"{guid}/{r.Payload!}"
                        };
                        entity.Images.Add(image);
                    }
                }
            }
        }

        public async Task<ServiceResponse> CreateAsync(CreateGameDto dto)
        {
            var entity = _mapper.Map<GameEntity>(dto);

            entity.Genres = await _genreRepository.Genres
                .Where(g => dto.Genres.Select(g => g.ToLower()).Contains(g.Name.ToLower()))
                .ToListAsync();

            // images
            await SaveImagesAsync(entity, dto);

            var res = await _gameRepository.CreateAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error("Не вдалося додати гру");
            }

            return ServiceResponse.Success($"Гра '{dto.Name}' успішно додана");
        }

        private string? GetGameFolderGuid(GameEntity entity)
        {
            var anyImage = entity.Images.FirstOrDefault();
            if (anyImage == null) return null;

            // формат "{guid}/{filename}"
            return anyImage.Name.Split('/')[0];
        }
        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var entity = await _gameRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return ServiceResponse.Error($"Гра з id '{id}' не знайдена");
            }


            // delete images
            var folderGuid = GetGameFolderGuid(entity);
            if (folderGuid != null)
            {
                _fileService.DeleteImage(Path.Combine(StaticFilesSettings.Games, folderGuid));
            }
            var res = await _gameRepository.DeleteAsync(entity);
            if (!res)
            {
                return ServiceResponse.Error($"Не вдалося видалити гру '{entity.Name}'");
            }
            return ServiceResponse.Success($"Гра '{entity.Name}' успішно видалена");


        }

        private async Task SaveImagesAsync(GameEntity entity, IFormFile? previewImage, List<IFormFile> images)
        {
            string guid = Guid.NewGuid().ToString();
            string folderPath = Path.Combine(StaticFilesSettings.Games, guid);

            if (previewImage != null)
            {
                var res = await _fileService.SaveImageAsync(previewImage, folderPath);
                if (res.IsSuccess)
                    entity.Images.Add(new GameImageEntity { IsPreview = true, Name = $"{guid}/{res.Payload!}" });
            }

            if (images.Count > 0)
            {
                var res = await _fileService.SaveImagesAsync(images, folderPath);
                foreach (var r in res)
                    if (r.IsSuccess)
                        entity.Images.Add(new GameImageEntity { IsPreview = false, Name = $"{guid}/{r.Payload!}" });
            }
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateGameDto dto)
        {
            var entity = await _gameRepository.GetByIdAsync(dto.Id);

            if (entity == null)
                return ServiceResponse.Error($"Гра з id '{dto.Id}' не знайдена");

            string oldName = entity.Name;

            bool hasNewPreview = dto.PreviewImage != null; 
            bool hasNewImages = dto.Images != null && dto.Images.Count > 0;

            if (hasNewPreview || hasNewImages)
            {
                string? guid = GetGameFolderGuid(entity);
                if (guid != null)
                    _fileService.DeleteFolder(Path.Combine(StaticFilesSettings.Games, guid));

                entity.Images.Clear();
                await SaveImagesAsync(entity, dto.PreviewImage, dto.Images);
            }

            _mapper.Map(dto, entity);

            entity.Genres = await _genreRepository.Genres
                .Where(g => dto.Genres.Contains(g.Id))
                .ToListAsync();

            bool res = await _gameRepository.UpdateAsync(entity);

            if (!res) 
                return ServiceResponse.Error("Не вдалося оновити гру");


            return ServiceResponse.Success($"Гра '{oldName}' успішно оновлена");
        }
    }
}
