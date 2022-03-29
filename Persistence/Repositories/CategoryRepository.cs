using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public VoucherContext VoucherContext => Context as VoucherContext;
        public CategoryRepository(DbContext context) : base(context)
        {
        }
    }
}
