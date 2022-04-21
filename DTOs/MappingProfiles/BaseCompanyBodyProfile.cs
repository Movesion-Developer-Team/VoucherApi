using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.MappingProfiles
{
    public class BaseCompanyBodyProfile : Profile
    {
        public BaseCompanyBodyProfile()
        {
            CreateMap<Company, BaseCompanyBody>()
                .ForMember(d => d.CompanyDto, opt => opt.MapFrom(o => o))
                .ForMember(d => d.Id, opt => opt.MapFrom(o => o.Id));
        }
    }
}
