using System.Globalization;
using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;
using DTOs.MethodDto;

namespace DTOs.MappingProfiles
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<DiscountBodyDto, Discount>()
                .ReverseMap();
            CreateMap<CreateNewDiscountBodyDto, Discount>()
                .ReverseMap();
            CreateMap<CsvCodeDto, DiscountCode>();
            CreateMap<DiscountType, DiscountTypeBodyDto>()
                .ReverseMap();
            CreateMap<DiscountType, DiscountTypeNameBodyDto>();

            CreateMap<DiscountCode, UserDiscountCodeBodyDto>()
                .ForMember(ud => ud.DiscountTypeName,
                    opt => opt.MapFrom(dc => dc.Discount.DiscountType.Name))
                .ForMember(ud => ud.OrderDateTime, 
                    opt => opt.MapFrom(dc=>dc.OrderTime.Value.LocalDateTime))
                .ForMember(ud => ud.PriceInPoints, 
                    opt => opt.MapFrom(dc => dc.Discount.PriceInPoints))
                .ForMember(ud => ud.FinalPrice, 
                    opt => opt.MapFrom(dc => dc.Discount.FinalPrice))
                .ForMember(ud => ud.PlayerName, 
                    opt => opt.MapFrom(dc => dc.Discount.Player.ShortName));

        }


    }
}
