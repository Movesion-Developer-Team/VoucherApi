using System.Runtime.InteropServices;
using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        public VoucherContext VoucherContext => Context as VoucherContext;
        public PlayerRepository(DbContext context) : base(context)
        {
        }

        public async Task AssignCategoryToPlayer(int playerId, int categoryId)
        {
            var player = await VoucherContext.Players
                .Where(p=>p.Id == playerId)
                .Include(p=>p.Categories)
                .FirstAsync();

            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            var category = await VoucherContext.Categories.FindAsync(categoryId);
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            Update(player);

            player.Categories ??= new List<Category>();
            player.Categories.Add(category);
            await VoucherContext.SaveChangesAsync();
        }
    }
}
