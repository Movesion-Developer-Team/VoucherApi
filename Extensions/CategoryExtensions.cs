using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Extensions
{
    public static class CategoryExtensions
    {
        public static async Task<bool> HasImage(this Category? category, DbContext context)
        {
            return await context.Set<Image>()
                .Where(i => i.CategoryId == category.Id)
                .AnyAsync();
        }

        public static async Task DeleteImage(this Category? category, DbContext context)
        {
            var image = await context.Set<Image>()
                .Where(i => i.CategoryId == category.Id)
                .FirstOrDefaultAsync();
            image.CheckForNull();
            context.Remove(image);
            await context.SaveChangesAsync();
        }
    }
}
