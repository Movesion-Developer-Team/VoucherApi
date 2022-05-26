using Core.Domain;
using Core.IRepositories;
using Extensions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        public VoucherContext VoucherContext => Context as VoucherContext;
        public DiscountRepository(DbContext context) : base(context)
        {

        }

        public async Task<IQueryable<DiscountType>> GetAllDiscountTypes()
        {
            return await Task.Run(() => VoucherContext.DiscountTypes.Select(d => d));
        }

        public async Task<DiscountType> FindDiscountType(int? discountTypeId)
        {
            var discountType = await VoucherContext.DiscountTypes.FindAsync(discountTypeId);
            discountType.CheckForNull(nameof(discountType));
            return discountType;
        }

        public async Task AssignDiscountCodesToCompany(int? discountId, int? companyId, int numberOfDiscounts, double price)
        {
            var company = await VoucherContext
                .Companies
                .FindAsync(companyId);
            company.CheckForNull(nameof(company));
            var discount = await VoucherContext
                .Discounts.Where(dc => dc.Id == discountId)
                .Include(dc => dc.DiscountType)
                .Include(d=>d.Player)
                .FirstOrDefaultAsync();
            discount.CheckForNull(nameof(discount));
            var isAssigned = await company.PlayerIsAssigned(discount.Player, VoucherContext);
            if (!isAssigned)
            {
                throw new InvalidOperationException(
                    "Player of selected discount is not assigned to the current company");
            }

            if (!discount.HasAvailableDiscountCodes(VoucherContext))
            {
                throw new InvalidOperationException("Discount is not available currently");

            }
            if (numberOfDiscounts == 0)
            {
                throw new InvalidOperationException("Please provide a number higher than 0");
            }

            var quantity = Math.Abs((int)numberOfDiscounts);
            var limit = discount.GetLimit(VoucherContext);
            if (limit is 0)
            {
                throw new InvalidOperationException("Discount is not available currently");
            }
            if (quantity > limit)
            {
                throw new InvalidOperationException($"Discount has only {limit} codes currently");
            }
            await discount
                .ChooseCodes(numberOfDiscounts, VoucherContext)
                .AssignToCompany(company, quantity, price, VoucherContext);
        }

        public async Task<IQueryable<Discount>> GetAllDiscountsForPlayer(int playerId)
        {
            return await Task.Run(() => VoucherContext.Discounts.Where(d => d.PlayerId == playerId));
        }

        public async Task<IEnumerable<Discount>?> GetAllGetAllDiscountsForPlayerOfCompany(int companyId, int playerId)
        {
            var company = await VoucherContext.Companies.FindAsync(companyId);
            company.CheckForNull(nameof(company));
            var player = await VoucherContext.Players.FindAsync(playerId);
            player.CheckForNull(nameof(player));
            if (!await company.HasPlayer(player, VoucherContext))
            {
                throw new InvalidOperationException("Player is not assigned to the company");
            }
            var playerWithCompanies = await VoucherContext.Players.Where(p => p.Id == player.Id)
                .Include(c => c.Companies)
                .Where(p => p.Companies.Contains(company))
                .Include(p => p.Discounts)
                .ThenInclude(d=>d.Companies)
                .SingleOrDefaultAsync();

            playerWithCompanies.Discounts.CheckEnumerableForNull();
            
            return playerWithCompanies.Discounts.Where(d=>d.Companies.Contains(company));

        }

        public async Task<Discount?> GetDiscountWithCodes(int discountId)
        {
            return await VoucherContext.Discounts.Where(d => d.Id == discountId).Include(d => d.DiscountCodes).SingleOrDefaultAsync();
        }

        public Task<bool> CodesAreAlreadyInDb(List<DiscountCode> codes)
        {
            var codesInDb = VoucherContext.DiscountCodes.Select(dc => dc.Code);
            return Task.Run(() => codes.CheckIfCodesAlreadyInDatabase(codesInDb));
        }

        public async Task<int?> GetDiscountLimit(int discountId)
        {
            var discount = await VoucherContext.Discounts
                .Where(d => d.Id == discountId)
                .Include(d => d.DiscountType)
                .SingleOrDefaultAsync();

            discount.CheckForNull(nameof(discount));

            return await Task.Run(() => discount.GetLimit(VoucherContext));

        }

        public async Task<int?> AddBatch(Batch batch)
        {
            await VoucherContext.Batches.AddAsync(batch);
            await Complete();
            return batch.Id;
        }

        public async Task AssignDiscountToCompany(int? discountId, int? companyId)
        {
            var discount = await VoucherContext.Discounts
                .Where(d => d.Id == discountId)
                .Include(d => d.Companies)
                .Include(d=>d.Player)
                .SingleOrDefaultAsync();

            var company = await VoucherContext.Companies
                .Where(c=>c.Id == companyId)
                .Include(c=>c.Players)
                .SingleOrDefaultAsync();

            discount.CheckForNull(nameof(discount));
            company.CheckForNull(nameof(company));
            if (discount.Player == null)
            {
                throw new InvalidOperationException("Discount must belong to some player");
            }
            
            if (discount.Companies != null)
            {
                if (discount.Companies.Contains(company))
                {
                    throw new InvalidOperationException("Discount is already assigned to the company");
                }

                if (company.Players == null)
                {
                    throw new InvalidOperationException("Discount belongs to the player that is not assigned to the company");

                }
                if (!company.Players.Contains(discount.Player))
                {
                    throw new InvalidOperationException("Discount belongs to the player that is not assigned to the company");

                }
                VoucherContext.Update(discount);
                discount.Companies.Add(company);
                await VoucherContext.SaveChangesAsync();
            }
            else
            {
                VoucherContext.Update(discount);
                discount.Companies = new List<Company>();
                discount.Companies.Add(company);
                await VoucherContext.SaveChangesAsync();

            }
        }


    }
}
