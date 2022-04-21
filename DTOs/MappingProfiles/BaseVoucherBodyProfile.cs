using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.MappingProfiles
{
    public class BaseVoucherBodyProfile : Profile
    {
        public BaseVoucherBodyProfile()
        {
            CreateMap<Voucher, BaseVoucherBody>()
                .ForMember(d => d.VoucherDto, opt => opt.MapFrom(o => o))
                .ForMember(d => d.Id, opt => opt.MapFrom(o => o.Id));
        }
    }
}
