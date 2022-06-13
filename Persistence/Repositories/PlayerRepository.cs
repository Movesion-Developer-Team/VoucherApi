using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Core.Domain;
using Core.IRepositories;
using Extensions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        public VoucherContext VoucherContext => Context as VoucherContext;
        public PlayerRepository(DbContext context) : base(context)
        {
        }

        public async Task<IQueryable<Player>> GetAll(bool withImage)
        {
            if (withImage)
            {
                return await Task.Run(() => VoucherContext.Players.Select(p => p).Include(p => p.Image).AsQueryable());
            }
            else
            {
                return await GetAll();
            }
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

            if (player.Categories != null)
            {
                if (player.Categories.Any(c => c.Id == categoryId))
                {
                    throw new InvalidOperationException("Category is already assigned to the Player");
                }
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

        public async Task AssignDiscountTypeToPlayer(int? playerId, int? discountTypeId)
        {
            var discountType = await VoucherContext.DiscountTypes.FindAsync(discountTypeId);
            discountType.CheckForNull(nameof(discountType));
            var player = await VoucherContext.Players.FindAsync(playerId);
            player.CheckForNull(nameof(player));
            if (player.DiscountsTypes == null)
            {
                player.DiscountsTypes = new List<DiscountType>();
            }

            if (player.DiscountsTypes.Contains(discountType))
            {
                throw new InvalidOperationException("This discount type is already assigned to the player");
            }
            Update(player);
            player.DiscountsTypes!.Add(discountType);
            await Complete();
        }

        public async Task AssignPlayerToCompany(int? companyId, int? playerId)
        {
            var company = await VoucherContext.Companies
                .Where(c=>c.Id == companyId)
                .Include(c=>c.Players)
                .FirstOrDefaultAsync();
            company.CheckForNull(nameof(company));
            var player = await VoucherContext.Players.FindAsync(playerId);
            player.CheckForNull(nameof(player));

            if (await company.HasPlayer(player, VoucherContext))
            {
                throw new InvalidOperationException("Player is already assigned to the company");
            }

            VoucherContext.Update(company);

            if (!await company.HasAnyPlayer(VoucherContext))
            {
                company.Players = new List<Player>();
            }
            
            company.Players.Add(player);
            await VoucherContext.SaveChangesAsync();
        }

        public async Task<IQueryable<DiscountType>> GetAllDiscountTypesForPlayer(int? playerId)
        {
            var player = await VoucherContext.Players
                .Where(p => p.Id == playerId)
                .Include(p => p.DiscountsTypes)
                .FirstAsync();
            player.CheckForNull(nameof(player));
            player.DiscountsTypes.CheckEnumerableForNull();
            return player.DiscountsTypes.AsQueryable();

        }

        public async Task AddImageToPlayer(BaseImage baseImage, int? playerId)
        {
            var player = await VoucherContext.Players.FindAsync(playerId);
            player.CheckForNull(nameof(player));
            if (await player.HasImage(VoucherContext))
            {
                await player.DeleteImage(VoucherContext);
            }
            baseImage.PlayerId = playerId;
            await VoucherContext.Images.AddAsync(baseImage);
            await VoucherContext.SaveChangesAsync();
            
        }



    }
}
