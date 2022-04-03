using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dto=>dto.Companies, 
                    opt=>opt.MapFrom(x=>x.CompanyCategories
                        .Select(y=>y.Company).ToList()))
                .ReverseMap();
        }
    }
}
