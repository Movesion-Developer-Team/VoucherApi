using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;
using DTOs.MethodDto;
using DTOs.ResponseDtos;

namespace DTOs.MappingProfiles
{
    public class CompanyProfile : Profile
    {

        public CompanyProfile()
        {
           

            CreateMap<Company, CreateNewCompanyBodyDto>()
                .ReverseMap()
                .ForMember(c => c.Users, opt => opt.Ignore())
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.ContactDate, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Company, CompanyBodyDto>()
                .ReverseMap()
                .ForMember(c => c.Users, opt => opt.Ignore())
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.ContactDate, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateNewCompanyBodyDto, CompanyBodyDto>()
                .ForMember(cb => cb.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Tuple<string?, string?>, CompaniesWithPlayersBodyDto>()
                .ForMember(cw => cw.CompanyName, opt => opt.MapFrom(t => t.Item1))
                .ForMember(cw => cw.PlayerName, opt => opt.MapFrom(t => t.Item2));



        }

    }
}
