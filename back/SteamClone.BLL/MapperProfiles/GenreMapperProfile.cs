using AutoMapper;
using SteamClone.BLL.Dtos.Genre;
using SteamClone.DAL.Entities;

namespace SteamClone.BLL.MapperProfiles
{
    public class GenreMapperProfile : Profile
    {
        public GenreMapperProfile()
        {
            // GenreEntity -> GenreDto
            CreateMap<GenreEntity, GenreDto>();

            // CreateGenreDto -> GenreEntity
            CreateMap<CreateGenreDto, GenreEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Games, opt => opt.Ignore());

            // UpdateGenreDto -> GenreEntity
            CreateMap<UpdateGenreDto, GenreEntity>()
                .ForMember(dest => dest.Games, opt => opt.Ignore());
        }
    }
}
