using System.Reflection;
using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;
using DTOs.Resolvers;

namespace DTOs.MappingProfiles
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            

            CreateMap<Player, CreateNewPlayerBodyDto>()
                .ReverseMap()
                .ForMember(p => p.Categories, opt => opt.Ignore())
                .ForMember(p => p.Discounts, opt => opt.Ignore())
                .ForMember(p => p.Locations, opt => opt.Ignore())
                .ForMember(p => p.PlayerContacts, opt => opt.Ignore())
                .ForMember(p => p.PlayerLocations, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Player, PlayerOnlyBodyDto>()
                .ReverseMap();

            CreateMap<Player, PlayerWithCategoriesAndDiscountTypesBodyDto>()
                .ForMember(pb=>pb.DiscountTypes, opt=>opt.MapFrom(p=>p.DiscountsTypes))
                .ForMember(pb=>pb.Categories, opt=>opt.MapFrom(p=>p.Categories))
                .ReverseMap()
                .ForMember(p => p.Categories, opt => opt.Ignore())
                .ForMember(p => p.Discounts, opt => opt.Ignore())
                .ForMember(p => p.Locations, opt => opt.Ignore())
                .ForMember(p => p.PlayerContacts, opt => opt.Ignore())
                .ForMember(p => p.PlayerLocations, opt => opt.Ignore())
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p=>p.DiscountsTypes, opt=>opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateNewPlayerBodyDto, PlayerWithCategoriesAndDiscountTypesBodyDto>()
                .ForMember(pb => pb.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Player, PlayerWithCategoriesBodyDto>()
                .ForMember(pc => pc.Player, opt => opt.MapFrom(p => p))
                .ForMember(pc => pc.Categories, opt => opt.MapFrom(p => p.Categories));

        }



       
    }
}
