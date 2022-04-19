using AutoMapper;
using Core.Domain;

namespace DTOs.MappingProfiles
{
    public class VoucherProfile : Profile
    {
        public VoucherProfile()
        {
            CreateMap<Voucher, VoucherDto>()
                .ReverseMap()
                .ForMember(v=>v.Category, opt=>opt.Ignore())
                .ForMember(v=>v.Discount, opt=>opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
