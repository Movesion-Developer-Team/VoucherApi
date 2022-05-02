using System.Reflection;
using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.MappingProfiles
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            

            CreateMap<Player, CreateNewPlayerBodyDto>()
                .ReverseMap()
                .ForMember(p => p.Companies, opt => opt.Ignore())
                .ForMember(p => p.Categories, opt => opt.Ignore())
                .ForMember(p => p.CompanyPlayers, opt => opt.Ignore())
                .ForMember(p => p.Discounts, opt => opt.Ignore())
                .ForMember(p => p.Locations, opt => opt.Ignore())
                .ForMember(p => p.PlayerContacts, opt => opt.Ignore())
                .ForMember(p => p.PlayerLocations, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Player, PlayerBodyDto>()
                .ReverseMap()
                .ForMember(p => p.Companies, opt => opt.Ignore())
                .ForMember(p => p.Categories, opt => opt.Ignore())
                .ForMember(p => p.CompanyPlayers, opt => opt.Ignore())
                .ForMember(p => p.Discounts, opt => opt.Ignore())
                .ForMember(p => p.Locations, opt => opt.Ignore())
                .ForMember(p => p.PlayerContacts, opt => opt.Ignore())
                .ForMember(p => p.PlayerLocations, opt => opt.Ignore())
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateNewPlayerBodyDto, PlayerBodyDto>()
                .ForMember(pb => pb.Id, opt => opt.Ignore())
                .ReverseMap();
        }

       
    }
}
