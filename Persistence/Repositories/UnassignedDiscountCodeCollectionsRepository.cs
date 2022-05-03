using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class UnassignedDiscountCodeCollectionsRepository : GenericRepository<UnassignedDiscountCodeCollection>, IUnassignedDiscountCodeCollectionsRepository
    {
        VoucherContext? VoucherContext => Context as VoucherContext;
        public UnassignedDiscountCodeCollectionsRepository(DbContext context) : base(context)
        {
        }
    }
}
