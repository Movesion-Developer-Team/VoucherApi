using System.Drawing;
using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.Resolvers
{
    public class ImageResolver : IMemberValueResolver<BaseImage, BaseImageBodyDto, byte[], Image?>
    {
        public Image Resolve(BaseImage source, BaseImageBodyDto destination, byte[] sourceMember, Image? destMember, ResolutionContext context)
        {
            
            destMember = Image.FromStream(new MemoryStream(sourceMember));

            return destMember;
        }
    }
}
