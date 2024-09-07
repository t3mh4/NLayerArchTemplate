using AutoMapper;
using NLayerArchTemplate.Dtos.User;
using NLayerArchTemplate.Entities;

namespace NLayerArchTemplate.Business.MapperProfile;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserSaveDto, TblUser>()
            .ForMember(f => f.Password, opt => opt.Ignore());
    }
}
