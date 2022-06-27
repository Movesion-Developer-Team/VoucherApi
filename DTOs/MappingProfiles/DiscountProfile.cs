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
                    opt => opt.MapFrom(dc => 
                        $"{dc.OrderTime.Value.LocalDateTime.ToString("ddd", CultureInfo.InvariantCulture)}" +
                        $" {dc.OrderTime.Value.LocalDateTime.ToString("MMM", CultureInfo.InvariantCulture)}" +
                        $" {dc.OrderTime.Value.LocalDateTime.ToString("dd", CultureInfo.InvariantCulture)}" +
                        $" {dc.OrderTime.Value.LocalDateTime.ToString("yyyy", CultureInfo.InvariantCulture)}" +
                        $" {dc.OrderTime.Value.LocalDateTime.TimeOfDay.Hours}:" +
                        $"{dc.OrderTime.Value.LocalDateTime.TimeOfDay.Minutes}:" +
                        $"{dc.OrderTime.Value.LocalDateTime.TimeOfDay.Seconds} " +
                        $"GMT{dc.OrderTime.Value.LocalDateTime.ToString("zz", CultureInfo.InvariantCulture)}00" ))
                .ForMember(ud => ud.PriceInPoints, 
                    opt => opt.MapFrom(dc => dc.Discount.PriceInPoints))
                .ForMember(ud => ud.FinalPrice, 
                    opt => opt.MapFrom(dc => dc.Discount.FinalPrice))
                .ForMember(ud => ud.PlayerName, 
                    opt => opt.MapFrom(dc => dc.Discount.Player.ShortName));

        }


    }
}
