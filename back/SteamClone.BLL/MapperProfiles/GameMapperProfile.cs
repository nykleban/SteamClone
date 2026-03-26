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

            // CreateGameDto -> GameEntity
            CreateMap<CreateGameDto, GameEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Developer, opt => opt.Ignore());

            // UpdateGameDto -> GameEntity
            CreateMap<UpdateGameDto, GameEntity>()
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Developer, opt => opt.Ignore());
        }
    }
}
