using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class PlayerContactProfile : Profile
    {
        public PlayerContactProfile()
        {
            CreateMap<PlayerContact, PlayerContactDto>()
                .ReverseMap()
                .ForMember(pc=>pc.Player, opt=>opt.Ignore());
        }
    }
}
