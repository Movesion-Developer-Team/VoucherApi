using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.MappingProfiles
{
    public class JoinRequestProfile : Profile
    {
        public JoinRequestProfile()
        {
            CreateMap<JoinRequest, JoinRequestBodyDto>();
        }
    }
}
