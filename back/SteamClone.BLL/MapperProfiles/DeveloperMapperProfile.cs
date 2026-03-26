using AutoMapper;
using SteamClone.BLL.Dtos.Developer;
using SteamClone.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SteamClone.BLL.MapperProfiles
{
    public class DeveloperMapperProfile : Profile
    {
        public DeveloperMapperProfile()
        {
            // DeveloperEntity -> DeveloperDto
            CreateMap<DeveloperEntity, DeveloperDto>();

            // CreateDeveloperDto -> DeveloperEntity
            CreateMap<CreateDeveloperDto, DeveloperEntity>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // UpdateDeveloperDto -> DeveloperEntity
            CreateMap<UpdateDeveloperDto, DeveloperEntity>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}