using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using Core.Domain;
using Core.IRepositories;
using Extensions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public VoucherContext VoucherContext => Context as VoucherContext;
        public CategoryRepository(DbContext context) : base(context)
        {
        }
        public async Task<IQueryable<Category>> GetAllCategoriesForPlayer(int playerId)
        {

            try
            {
                var player = await VoucherContext.Players
                    .Where(p => p.Id == playerId)
                    .Select(p => p)
                    .Include(p => p.Categories)
                    .AsQueryable()
                    .FirstAsync();
                if (player.Categories == null)
                {
                    throw new InvalidOperationException("No categories assigned to the Player");
                }

                return player.Categories.AsQueryable();

            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

        }

        public async Task<IQueryable<Category>> GetAllCategoriesForCompany(int companyId)
        {
            var company = await VoucherContext
                .Companies
                .FindAsync(companyId);

            return await company.GetCategories(VoucherContext);
        }

        public async Task<IQueryable<Player>> GetAllPlayersForCategoryAndCompany(int companyId, int categoryId)
        {
            var company = await VoucherContext.Companies.FindAsync(companyId);
            var category = await VoucherContext.Categories.FindAsync(categoryId);
            company.CheckForNull(nameof(company));
            category.CheckForNull(nameof(category));
            return await company.GetPlayersOfCategory(VoucherContext, category);
        }

        public async Task AddImageToCategory(BaseImage baseImage, int? categoryId)
        {
            var category = await VoucherContext.Categories.FindAsync(categoryId);
            category.CheckForNull(nameof(category));
            if (await category.HasImage(VoucherContext))
            {
                await category.DeleteImage(VoucherContext);
            }
            baseImage.CategoryId = categoryId;
            await VoucherContext.Images.AddAsync(baseImage);
            await VoucherContext.SaveChangesAsync();
        }
    }
}
