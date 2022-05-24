using Core.Domain;
using Core.IRepositories;
using DTOs.BodyDtos;
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
             return await Task.Run(()=>VoucherContext.DiscountTypes.Select(d => d));
        }

        public async Task<DiscountType> FindDiscountType(int? discountTypeId)
        {
            var discountType = await VoucherContext.DiscountTypes.FindAsync(discountTypeId);
            discountType.CheckForNull();
            return discountType;
        }

        public async Task AssignDiscountCodesToCompany(int? discountId, int? companyId, int numberOfDiscounts)
        {
            var company = await VoucherContext
                .Companies
                .FindAsync(companyId);
            company.CheckForNull();
            var discount = await VoucherContext
                .Discounts.Where(dc => dc.Id == discountId)
                .Include(dc => dc.DiscountType)
                .FirstOrDefaultAsync();
            discount.CheckForNull();
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
            if(quantity > limit)
            {
                throw new InvalidOperationException($"Discount has only {limit} codes currently");
            }
            await discount
                .ChooseCodes(numberOfDiscounts, VoucherContext)
                .AssignToCompany(company, quantity, VoucherContext);
        }

        public async Task<IQueryable<Discount>> GetAllDiscountsForPlayer(int playerId)
        {
            return await Task.Run(()=>VoucherContext.Discounts.Where(d => d.PlayerId == playerId));
        }

        public async Task<Discount?> GetDiscountWithCodes(int discountId)
        {
            return await VoucherContext.Discounts.Where(d => d.Id == discountId).Include(d => d.DiscountCodes).SingleOrDefaultAsync();
        }

        public Task<bool> CodesAreAlreadyInDb(List<DiscountCode> codes)
        {
            var codesInDb = VoucherContext.DiscountCodes.Select(dc => dc.Code);
            return Task.Run(()=>codes.CheckIfCodesAlreadyInDatabase(codesInDb));
        }

        public async Task<int?> GetDiscountLimit(int discountId)
        {
            var discount = await VoucherContext.Discounts
                .Where(d=>d.Id == discountId)
                .Include(d=>d.DiscountType)
                .SingleOrDefaultAsync();

            discount.CheckForNull();

            return await Task.Run(()=>discount.GetLimit(VoucherContext));

        }

    }
}
