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
            CreateMap<UploadCsvToDiscountDto, Discount>();
        }
    }
}
