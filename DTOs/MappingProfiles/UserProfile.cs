using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ReverseMap();
        }
    }
}
