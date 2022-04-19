using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class CompanyProfile : Profile
    {

        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ReverseMap()
                .ForMember(c => c.WorkerIds, opt => opt.Ignore())
                .ForMember(c => c.Categories, opt => opt.Ignore())
                .ForMember(c => c.Players, opt => opt.Ignore())
                .ForMember(c => c.CompanyCategories, opt => opt.Ignore())
                .ForMember(c => c.CompanyPlayers, opt => opt.Ignore())
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c=>c.ContactDate, opt=>opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }

    }
}
