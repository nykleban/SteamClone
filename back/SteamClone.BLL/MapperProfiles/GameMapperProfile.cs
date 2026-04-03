using AutoMapper;
using SteamClone.BLL.Dtos.Game;
using SteamClone.DAL.Entities;

namespace SteamClone.BLL.MapperProfiles
{
    public class GameMapperProfile : Profile
    {
        public GameMapperProfile()
        {
            // GameEntity -> GameDto
            CreateMap<GameEntity, GameDto>();

            CreateMap<GameEntity, GameDto>()
                .ForMember(dest => dest.PreviewImage,
                    opt => opt.MapFrom(src => src.Images
                        .Where(i => i.IsPreview)
                        .Select(i => i.Name)
                        .FirstOrDefault()))
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src => src.Images
                        .Where(i => !i.IsPreview)
                        .Select(i => i.Name)
                        .ToList()));
            // UpdateGameDto -> GameEntity
            CreateMap<UpdateGameDto, GameEntity>()
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Developer, opt => opt.Ignore());


        }
    }
}
