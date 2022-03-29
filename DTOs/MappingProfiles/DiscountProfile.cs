using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Discount, DiscountDto>()
                .ForMember(d => d.StartDate, opt => opt
                    .MapFrom(x => x.ValidityPeriod.StartDate))
                .ForMember(d => d.EndDate, opt => opt
                    .MapFrom(x => x.ValidityPeriod.EndDate))
                .ReverseMap()
                .ForMember(s => s.Player, opt => opt.Ignore())
                .ForPath(s => s.ValidityPeriod.StartDate, opt => opt
                    .MapFrom(s => s.StartDate))
                .ForPath(s => s.ValidityPeriod.EndDate, opt => opt
                    .MapFrom(s => s.EndDate)) 
                .ForMember(d=>d.UsageTimes, opt=>opt
                    .Ignore());
        }
    }
}
