using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public VoucherContext Voucher => (Context as VoucherContext)!;
        public UserRepository(DbContext context) : base(context)
        {
        }


    }
}
