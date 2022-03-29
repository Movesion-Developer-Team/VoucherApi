using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public VoucherContext VoucherContext => Context as VoucherContext;
        public LocationRepository(DbContext context) : base(context)
        {
        }
    }
}
