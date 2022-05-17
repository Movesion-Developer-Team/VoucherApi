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
             return Task.Run(()=>VoucherContext.DiscountsTypes.Select(d => d));
        }

        public async Task<DiscountType> FindDiscountType(int? discountTypeId)
        {
            var discountType = await VoucherContext.DiscountsTypes.FindAsync(discountTypeId);
            discountType.CheckForNull();
            return discountType;
        }


    }
}
