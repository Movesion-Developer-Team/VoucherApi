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

        public Task<IQueryable<DiscountType>> GetAllDiscountTypes()
        {
             return Task.Run(()=>VoucherContext.DiscountTypes.Select(d => d));
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
                .Discounts.FindAsync(discountId);
            discount.CheckForNull();
            if (discount.HasAvailableDiscountCodes(VoucherContext))
            {
                throw new InvalidOperationException("Discount is not available currently");

            }
            if (numberOfDiscounts == 0)
            {
                throw new InvalidOperationException("Please provide a number higher than 0");
            }

            var quantity = Math.Abs((int)numberOfDiscounts);
            var limit = discount.GetLimit(VoucherContext);
            if (limit is null or 0)
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



    }
}
