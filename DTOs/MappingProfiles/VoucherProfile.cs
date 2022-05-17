using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.MappingProfiles
{
    public class VoucherProfile : Profile
    {
        public VoucherProfile()
        {
            
            CreateMap<Voucher, CreateNewVoucherBodyDto>()
                .ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
