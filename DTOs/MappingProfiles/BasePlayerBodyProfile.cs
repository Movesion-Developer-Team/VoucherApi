using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.MappingProfiles
{
    public class BasePlayerBodyProfile : Profile
    {
        public BasePlayerBodyProfile()
        {
            CreateMap<Player, BasePlayerBody>()
                .ForMember(d => d.PlayerDto, opt => opt.MapFrom(o => o))
                .ForMember(d => d.Id, opt => opt.MapFrom(o => o.Id));

        }
    }
}
