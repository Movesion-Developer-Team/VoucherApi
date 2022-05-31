using Core.Domain;

namespace Core.IRepositories
{
    public interface IInvitationCodeRepository : IGenericRepository<InvitationCode>
    {
        Task<int?> GenerateInvitationCodeForEmployees(DateTimeOffset startDate, DateTimeOffset endDate, int companyId);
    }
}
