using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            CreateMap<Player, PlayerDto>()
                .ReverseMap()
                .ForMember(p => p.Companies, opt => opt.Ignore())
                .ForMember(p => p.Category, opt => opt.Ignore())
                .ForMember(p => p.CompanyPlayers, opt => opt.Ignore())
                .ForMember(p => p.Discounts, opt => opt.Ignore())
                .ForMember(p => p.Locations, opt => opt.Ignore())
                .ForMember(p => p.PlayerContacts, opt => opt.Ignore())
                .ForMember(p => p.PlayerLocations, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
