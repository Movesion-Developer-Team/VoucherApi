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

        private async Task<bool> SpecificRequestExists(string code, int userId)
        {
            var exists = await GetRequestOfUser(code, userId) != null;
            return exists;
        }

        private async Task<JoinRequest?> GetRequestOfUser(string code, int userId)
        {
            if (!await HasAnyRequests(code, userId))
            {
                return null;
            }

            return await GetAllRequestsForUser(userId).Result
                .Where(jr => jr.InvitationCode.InviteCode == code)
                .SingleOrDefaultAsync();
        }

        private async Task<IQueryable<JoinRequest>> GetAllRequestsForUser(int userId)
        {
            return await Task.Run(()=>VoucherContext.JoinRequests.Where(jr => jr.UserId == userId)
                .Include(jr => jr.InvitationCode)
                .Where(jr => jr.InvitationCode != null));
        }

        private async Task<bool> HasAnyRequests(string code, int userId)
        {
            var userRequests = await GetAllRequestsForUser(userId);
            if (!(await userRequests.AnyAsync()))
            {
                return false;
            }

            return true;
        }

        private async Task<bool> RequestDeclined(string code, int userId)
        {
            if (!await SpecificRequestExists(code, userId))
            {
                return false;
            }

            var request = await GetRequestOfUser(code, userId);
            return request.Declined;
        }

        private async Task<bool> IsAssignedToCompany(int userId)
        {
            var user = await VoucherContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            return user.CompanyId != null;

        }

        public async Task SendJoinRequestToCompany(string code, int userId, string? message = null)
        {
            bool userExists = await ExistsAsync(userId);

            if (!userExists)
            {
                throw new InvalidOperationException("User not found");
            }

            if (await IsAssignedToCompany(userId))

            {
                throw new InvalidOperationException("User already assigned to company");
            }
            var requestDeclined = await RequestDeclined(code, userId);
            if (requestDeclined)
            {
                throw new InvalidOperationException(
                    "Request declined by company. Please, request another invitation code from Mobility Manager.");
            }

            var requestAlreadySent = await SpecificRequestExists(code, userId);
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
                Declined = false
            };
            await VoucherContext.JoinRequests.AddAsync(joinRequest);
            await VoucherContext.SaveChangesAsync();
        }

        
    }
}
