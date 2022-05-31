using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class PurchaseRepository : GenericRepository<Purchase>, IPurchaseRepository
    {
        public VoucherContext VoucherContext => Context as VoucherContext;
        public PurchaseRepository(DbContext context) : base(context)
        {
        }
    }
}
