using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class AgencyRepository : GenericRepository<Agency>, IAgencyRepository
    {

        VoucherContext VoucherContext => Context as VoucherContext;
        public AgencyRepository(DbContext context) : base(context)
        {
        }


    }
}
