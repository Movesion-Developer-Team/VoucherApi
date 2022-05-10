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

        public async Task<int?> GenerateInvitationCodeForEmployees(DateTime startDate, DateTime endDate, int companyId)
        {
            if (startDate > endDate)
            {
                throw new InvalidOperationException("Expiration date should be later than code initiation date");
            }
            var code = new InvitationCode
            {
                InviteCode = StringExtensions.RandomCodeGenerator(15),
                StartDate = startDate,
                ExpireDate = endDate,
                CompanyId = companyId
            };
            var id = await AddAsync(code);
            await VoucherContext.SaveChangesAsync();
            return id;

        }

    }
}
