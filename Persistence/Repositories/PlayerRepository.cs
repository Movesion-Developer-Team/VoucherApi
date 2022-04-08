using System.Runtime.InteropServices;
using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        public VoucherContext VoucherContext => Context as VoucherContext;
        public PlayerRepository(DbContext context) : base(context)
        {
        }

    }
}
