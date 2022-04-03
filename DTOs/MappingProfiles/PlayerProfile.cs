using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            CreateMap<Player, PlayerDto>()
                .ForMember(p=>p.Companies, opt=>opt
                    .MapFrom(x=>x.CompanyPlayers.Select(a=>a.Company).ToList()))
                .ForMember(p=>p.Locations, opt=>opt
                    .MapFrom(x=>x.PlayerLocations.Select(y=>y.Location).ToList()))
                .ReverseMap();
        }
    }
}
