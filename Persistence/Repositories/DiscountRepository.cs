using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        public VoucherContext VoucherContext => Context as VoucherContext;
        public DiscountRepository(DbContext context) : base(context)
        {
            
        }
    }
}
