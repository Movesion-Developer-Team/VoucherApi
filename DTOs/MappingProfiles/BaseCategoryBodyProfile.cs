using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.MappingProfiles
{
    public class BaseCategoryBodyProfile : Profile
    {
        public BaseCategoryBodyProfile()
        {
            CreateMap<Category, BaseCategoryBody>()
                .ForMember(d => d.CategoryDto, opt => opt.MapFrom(o => o))
                .ForMember(d => d.Id, opt => opt.MapFrom(o => o.Id));
        }
    }
}
