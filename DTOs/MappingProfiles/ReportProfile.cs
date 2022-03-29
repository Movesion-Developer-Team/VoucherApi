using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<Report, ReportDto>()
                .ReverseMap();
        }
    }
}
