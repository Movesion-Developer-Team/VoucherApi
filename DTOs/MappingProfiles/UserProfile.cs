using AutoMapper;
using Core.Domain;
using Microsoft.AspNetCore.Identity;

namespace DTOs.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {

            CreateMap<IdentityUser, UserDto>()
                .ForMember(u => u.Id, opt => opt.MapFrom(iu => iu.Id))
                .ForMember(u => u.UserName, opt => opt.MapFrom(iu => iu.UserName));
        }
    }
}
