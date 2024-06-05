using AutoMapper;
using NLayerArchTemplate.Dtos.User;
using NLayerArchTemplate.Entities;

namespace NLayerArchTemplate.Business.MapperProfile;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserSaveDto, TblUser>()
            .ForMember(f => f.Password, opt => opt.Ignore());
    }
}
