using System.Runtime.CompilerServices;
using Core.Domain;
using Core.IRepositories;
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
            if(company == null)
            {
                throw new InvalidOperationException("Company not found");
            }

            var companyWithPlayers = await VoucherContext
                .Companies.Where(c => c.Id == companyId)
                .Include(c => c.Players)
                .FirstAsync();
            if (companyWithPlayers.Players == null)
            {
                throw new InvalidOperationException("Company does not have any players assigned");
            }

            var companyWithPlayersAndCategories = VoucherContext.Companies
                .Where(c => c.Id == companyId)
                .Include(c => c.Players)
                .ThenInclude(p => p.Categories);
            if (!companyWithPlayersAndCategories.Any())
            {
                throw new InvalidOperationException("Players pf the company does not contain any categories");
            }

            var result = companyWithPlayersAndCategories
                .SelectMany(c => c.Players)
                .Distinct()
                .SelectMany(p => p.Categories)
                .Distinct();

            return result;
        }

        public async Task<IQueryable<Player>> GetAllPlayersForCategoryAndCompany(int companyId, int categoryId)
        {
            var company = await Task.Run(()=>VoucherContext
                .Companies
                .Where(c => c.Id == companyId));
            if (!company.Any())
            {
                throw new InvalidOperationException("Company not found");
            }

            if (!company.Select(c => c.Players).Any())
            {
                throw new InvalidOperationException("Company does not have any Players assigned");
            }

            var players = await Task.Run(()=>company
                .Where(c =>
                    c.Players.All(p =>
                        p.Categories.All(c =>
                            c.Id == categoryId)))
                .SelectMany(c => c.Players));

            return players;
        }

        public async Task AddImageToCategory(Image image, int? categoryId)
        {
            image.CategoryId = categoryId;
            await VoucherContext.Images.AddAsync(image);
            await VoucherContext.SaveChangesAsync();
        }
    }
}
