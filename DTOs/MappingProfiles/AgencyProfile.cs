using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class AgencyProfile : Profile
    {

        public AgencyProfile()
        {
            CreateMap<Agency, AgencyDto>()
                .ReverseMap();
        }

    }
}
