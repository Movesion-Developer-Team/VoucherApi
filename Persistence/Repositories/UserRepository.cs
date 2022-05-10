using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public VoucherContext VoucherContext => (Context as VoucherContext)!;
        public UserRepository(DbContext context) : base(context)
        {
        }

        private async Task<bool> _RequestExists(string code, int userId)
        {
            var userRequests = VoucherContext.JoinRequests.Where(jr => jr.UserId == userId)
                .Include(jr => jr.InvitationCode)
                .Where(jr=>jr.InvitationCode != null)
                .AsQueryable();
            if (!(await userRequests.AnyAsync()))
            {
                return false;
            }

            var requestExists = await userRequests
                .Where(jr => jr.InvitationCode.InviteCode == code)
                .SingleOrDefaultAsync() != null;
            return requestExists;
        }

        public async Task SendJoinRequestToCompany(string code, int userId, string? message = null)
        {
            bool userExists = await ExistsAsync(userId);

            if (!userExists)
            {
                throw new InvalidOperationException("User not found");
            }

            var requestAlreadySent = await _RequestExists(code, userId);
            if (requestAlreadySent)
            {
                throw new InvalidOperationException("Request already sent");
            }
            var invitationCode = await VoucherContext.InvitationCodes.SingleOrDefaultAsync(c => c.InviteCode == code);
            if (invitationCode == null)
            {
                throw new ArgumentNullException(nameof(invitationCode), "Invalid code");
            }

            if (invitationCode.StartDate > DateTime.Now)
            {
                var error = $"Code will be valid from {invitationCode.StartDate.Value.Date} to {invitationCode.ExpireDate.Value.Date}";
                throw new InvalidOperationException(error);
            }

            if (invitationCode.ExpireDate < DateTime.Now)
            {
                throw new InvalidOperationException("Code is expired");
            }

            
            

            var joinRequest = new JoinRequest
            {
                Message = message,
                InvitationCodeId = invitationCode.Id,
                UserId = userId,
            };
            await VoucherContext.JoinRequests.AddAsync(joinRequest);
            await VoucherContext.SaveChangesAsync();
        }
    }
}
