using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class DiscountCodeRepository : GenericRepository<DiscountCode>, IDiscountCodeRepository
    {
        VoucherContext? VoucherContext => Context as VoucherContext;
        public DiscountCodeRepository(DbContext context) : base(context)
        {
        }
    }
}
