using System.Linq.Expressions;
using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class DiscountTypeRepository : GenericRepository<DiscountType>, IDiscountTypeRepository
    {
        public VoucherContext VoucherContext => Context as VoucherContext;
        public DiscountTypeRepository(DbContext context) : base(context)
        {
        }
    }
}
