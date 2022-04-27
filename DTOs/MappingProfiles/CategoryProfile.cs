using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CreateNewCategoryBodyDto>()
                .ReverseMap()
                .ForMember(d => d.Players, opt => opt.Ignore())
                .ForMember(d => d.Vouchers, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Category, CategoryBodyDto>()
                .ReverseMap()
                .ForMember(d => d.Players, opt => opt.Ignore())
                .ForMember(c=>c.Id, opt=>opt.Ignore())
                .ForMember(d => d.Vouchers, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateNewCategoryBodyDto, CategoryBodyDto>()
                .ForMember(cb=>cb.Id, opt=>opt.Ignore())
                .ReverseMap();

        }
    }
}
