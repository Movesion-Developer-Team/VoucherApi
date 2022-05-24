using System.Reflection.Metadata.Ecma335;
using Core.Domain;
using Core.IRepositories;
using Extensions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class InvitationCodeRepository : GenericRepository<InvitationCode>, IInvitationCodeRepository
    {
        VoucherContext VoucherContext => Context as VoucherContext;
        public InvitationCodeRepository(DbContext context) : base(context)
        {
        }

        public async Task<int?> GenerateInvitationCodeForEmployees(DateTimeOffset startDate, DateTimeOffset endDate, int companyId)
        {
            if (startDate > endDate)
            {
                throw new InvalidOperationException("Expiration date should be later than code initiation date");
            }
            var code = new InvitationCode
            {
                InviteCode = StringExtensions.RandomCodeGenerator(15),
                StartDate = startDate.UtcDateTime,
                ExpireDate = endDate.UtcDateTime,
                CompanyId = companyId
            };
            var id = await AddAsync(code);
            await VoucherContext.SaveChangesAsync();
            return id;

        }

    }
}
