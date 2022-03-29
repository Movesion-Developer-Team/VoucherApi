using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dto=>dto.Agencies, 
                    opt=>opt.MapFrom(x=>x.AgencyCategories
                        .Select(y=>y.Agency).ToList()))
                .ReverseMap();
        }
    }
}
