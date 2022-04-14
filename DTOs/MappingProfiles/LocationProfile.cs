using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<Location, LocationDto>()
                .ReverseMap()
                .ForMember(d=>d.Players, opt=>opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
