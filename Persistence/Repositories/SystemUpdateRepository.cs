using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class SystemUpdateRepository : GenericRepository<SystemUpdate>, ISystemUpdateRepository
    {
        VoucherContext? VoucherContext => Context as VoucherContext;

        public SystemUpdateRepository(DbContext context) : base(context)
        {
        }

        public DateTimeOffset? GetLastUpdate()
        {
            return VoucherContext.SystemUpdates.Max(su => su.UpdateDate);
        }
    }
}
