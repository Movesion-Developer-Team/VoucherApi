using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ReverseMap()
                .ForMember(d => d.Companies, opt => opt.Ignore())
                .ForMember(d => d.CompanyCategories, opt => opt.Ignore())
                .ForMember(d => d.Players, opt => opt.Ignore())
                .ForMember(d => d.Vouchers, opt => opt.Ignore());
        }
    }
}
