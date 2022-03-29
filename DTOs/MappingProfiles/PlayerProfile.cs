using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            CreateMap<Player, PlayerDto>()
                .ForMember(p=>p.Agencies, opt=>opt
                    .MapFrom(x=>x.AgencyPlayers.Select(a=>a.Agency).ToList()))
                .ForMember(p=>p.Locations, opt=>opt
                    .MapFrom(x=>x.PlayerLocations.Select(y=>y.Location).ToList()))
                .ReverseMap();
        }
    }
}
