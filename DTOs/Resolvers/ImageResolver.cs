using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;

namespace DTOs.Resolvers
{
    public class ImageResolver : IMemberValueResolver<Player, PlayerOnlyBodyDto, object, object?>
    {
        public object Resolve(Player source, PlayerOnlyBodyDto destination, object sourceMember, object destMember, ResolutionContext context)
        {
            var srcMember = sourceMember as BaseImage;
            destMember = System.Drawing.Image.FromStream(new MemoryStream(srcMember.Content));

            return destMember;
        }
    }
}
