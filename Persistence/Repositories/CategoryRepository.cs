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
        public Task<Player> GetAllCategoriesForPlayer(int playerId)
        {

             try 
             { 
                 return VoucherContext.Players
                        .Where(p => p.Id == playerId)
                        .Select(p => p)
                        .Include(p => p.Categories)
                        .AsQueryable()
                        .FirstAsync();
                    
             }
             catch (ArgumentNullException ex)
             {
                    throw new ArgumentNullException(ex.Message);
             }

        }

    }
}
