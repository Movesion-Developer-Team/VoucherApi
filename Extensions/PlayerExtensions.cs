using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Extensions
{
    public static class PlayerExtensions
    {

        public static async Task<bool> HasImage(this Player? player, DbContext context)
        {
            return await context.Set<BaseImage>()
                .Where(i => i.PlayerId == player.Id)
                .AnyAsync();
        }

        public static async Task DeleteImage(this Player? player, DbContext context)
        {
            var image = await context.Set<BaseImage>()
                .Where(i => i.PlayerId == player.Id)
                .FirstOrDefaultAsync();
            image.CheckForNull(nameof(image));
            context.Remove(image);
            await context.SaveChangesAsync();
        }

    }
}
