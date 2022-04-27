using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;
using DTOs.ResponseDtos;

namespace DTOs.MappingProfiles
{
    public class CompanyProfile : Profile
    {

        public CompanyProfile()
        {
           

            CreateMap<Company, CreateNewCompanyBodyDto>()
                .ReverseMap()
                .ForMember(c => c.Workers, opt => opt.Ignore())
                .ForMember(c => c.Players, opt => opt.Ignore())
                .ForMember(c => c.CompanyPlayers, opt => opt.Ignore())
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.ContactDate, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Company, CompanyBodyDto>()
                .ReverseMap()
                .ForMember(c => c.Workers, opt => opt.Ignore())
                .ForMember(c => c.Players, opt => opt.Ignore())
                .ForMember(c => c.CompanyPlayers, opt => opt.Ignore())
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.ContactDate, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateNewCompanyBodyDto, CompanyBodyDto>()
                .ForMember(cb => cb.Id, opt => opt.Ignore())
                .ReverseMap();

        }

    }
}
