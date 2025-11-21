using AutoMapper;
using LW_4_3_5_Daryev_PI231.Models;
using LW_4_3_5_Daryev_PI231.DTOs;

namespace LW_4_3_5_Daryev_PI231.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GamingAsset, AssetDTO>();

            CreateMap<CreateAssetDTO, GamingAsset>();
            CreateMap<UpdateAssetDTO, GamingAsset>();
        }
    }
}