using System.Drawing;
using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.MappingProfiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<BaseImage, BaseImageBodyDto>()
                .ReverseMap();
        }
    }
}
