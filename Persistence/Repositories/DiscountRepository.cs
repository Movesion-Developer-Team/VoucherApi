using System.Globalization;
using System.Text;
using Core.Domain;
using Core.IRepositories;
using CsvHelper;
using CsvHelper.Configuration;
using Enum;
using Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.FileProviders.Composite;

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

        public async Task AssignDiscountCodesToDiscount(int? discountId, int? batchId, int numberOfDiscounts)
        {
            var discount = await VoucherContext
                .Discounts.Where(dc => dc.Id == discountId)
                .FirstOrDefaultAsync();
            discount.CheckForNull(nameof(discount));

            var batch = await VoucherContext.Batches.FindAsync(batchId);
            batch.CheckForNull(nameof(batch));
            
            if (numberOfDiscounts == 0)
            {
                throw new InvalidOperationException("Please provide a number higher than 0");
            }

            
            var quantity = Math.Abs((int)numberOfDiscounts);
            var limit = await batch!.GetBatchFreeCodesLimit(VoucherContext);
            if (limit is 0)
            {
                throw new InvalidOperationException("Batch is not available");
            }
            if (quantity > limit)
            {
                throw new InvalidOperationException($"Batch has only {limit} codes currently");
            }


            await (await batch.ChooseMonoUserCodesFromBatch(VoucherContext))
                .AssignToDiscount(discount, quantity, VoucherContext);
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
                .ThenInclude(d => d.Companies)
                .SingleOrDefaultAsync();

            playerWithCompanies.Discounts.CheckEnumerableForNull();

            return playerWithCompanies.Discounts.Where(d => d.Companies.Contains(company));

        }

        public Task<bool> CodesAreAlreadyInDb(List<DiscountCode> codes)
        {
            var codesInDb = VoucherContext.DiscountCodes.Select(dc => dc.Code);
            return Task.Run(() => codes.CheckIfCodesAlreadyInDatabase(codesInDb));
        }

        public async Task<int?> GetDiscountLimit(int discountId)
        {
            var discount = await VoucherContext.Discounts.FindAsync(discountId);
            discount.CheckForNull(nameof(discount));
            return await discount.GetDiscountLimit(VoucherContext);
        }

        public async Task<int?> GetBatchLimit(int batchId)
        {
            var batch = await VoucherContext.Batches.FindAsync(batchId);

            batch.CheckForNull(nameof(batch));

            return await batch.GetBatchFreeCodesLimit(VoucherContext);

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
                .SingleOrDefaultAsync();

            var company = await VoucherContext.Companies
                .Where(c => c.Id == companyId)
                .Include(c => c.Players)
                .SingleOrDefaultAsync();

            discount.CheckForNull(nameof(discount));
            company.CheckForNull(nameof(company));

            if (discount.PlayerId == null)
            {
                throw new InvalidOperationException("Discount must belong to some player");
            }

            if (company.Players == null)
            {
                throw new InvalidOperationException("Discount belongs to the player that is not assigned to the company");
            }
            if (!company.Players.Contains(discount.Player))
            {
                throw new InvalidOperationException("Discount belongs to the player that is not assigned to the company");
            }

            if (await company.CompanyHasDiscount(discount, VoucherContext))
            {
                throw new InvalidOperationException("Discount is already assigned to the company");
            }

            
            await VoucherContext.SaveChangesAsync();
            await VoucherContext.CompanyPortfolios.AddAsync(new CompanyPortfolio
            {
                CompanyId = companyId,
                DiscountId = discountId
            });

        }

        public async Task<IQueryable<Batch>?> GetAllBatches()
        {
            return await Task.Run(() => VoucherContext.Batches.Select(b=>b));
        }

        public async Task<long> OrderAmount(int discountId, int numberOfCodes)
        {
            
            
            var discount = await VoucherContext.Discounts.FindAsync(discountId);
            var discountType = await VoucherContext.DiscountTypes.FindAsync(discount.DiscountTypeId);
            discount.CheckForNull(nameof(discount));

            if (numberOfCodes == 0)
            {
                return 0;
            }

            if (discount.FinalPrice == null || discount.FinalPrice == 0)
            {
                return 0;
            }
            if (discountType.Name == DiscountTypes.PromotionalCode.ToString())
            {
                throw new InvalidOperationException("Promotional codes cannot be purchased");
            }

            var amount = (long)discount.FinalPrice * numberOfCodes;
            return amount;
        }

        public async Task ReserveCodes(int? discountId, int userId, int numberOfCodes)
        {
            var discount = await VoucherContext.Discounts.FindAsync(discountId);
            discount.CheckForNull(nameof(discount));
            var discountType = await VoucherContext.DiscountTypes.FindAsync(discountId);
            discountType.CheckForNull(nameof(discountType));

            if (discountType.Name == DiscountTypes.PromotionalCode.ToString())
            {
                throw new InvalidOperationException("Promotion codes currently are not implemented into the reservation system");
            }
            var codes = await discount.ChooseAssignedMonoUserCodes(numberOfCodes, VoucherContext);
            await codes.AssignCodesToUserTemporary(userId, VoucherContext);
        }

        

    }
}
