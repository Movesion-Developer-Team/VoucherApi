using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {

        VoucherContext VoucherContext => Context as VoucherContext;
        public CompanyRepository(DbContext context) : base(context)
        {
        }


    }
}
