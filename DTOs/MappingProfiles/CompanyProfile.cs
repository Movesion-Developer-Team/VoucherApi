using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class CompanyProfile : Profile
    {

        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ReverseMap();
        }

    }
}
