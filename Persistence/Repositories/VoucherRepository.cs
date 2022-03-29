using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {

        public VoucherContext VoucherContext => Context as VoucherContext;
        public VoucherRepository(DbContext context) : base(context)
        {
        }
        



    }
}
