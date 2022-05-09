using System.Runtime.CompilerServices;
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

        public async Task<bool> DeleteCategoryFromPlayer(int playerId, int categoryId)
        {
            Player? player;
            try
            {
                player = await VoucherContext.Players.Where(p => p.Id == playerId)
                    .Include(p => p.Categories)
                    .FirstAsync();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Player not found. Internal error: {ex.Message}");
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException($"Player not found. Internal error: {ex.Message}");
            }
            
            var category = await VoucherContext.Categories.FindAsync(categoryId);
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category),"CategoryId not found");
            }

            Update(player);
            bool deleted;
            try
            {
                deleted = player.Categories.Remove(category);
                await Complete();
                return (deleted);
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }
    }
}
