using Core.Domain;

namespace Core.IRepositories
{
    public interface IInvitationCodeRepository : IGenericRepository<InvitationCode>
    {
        Task<int?> GenerateInvitationCodeForEmployees(DateTime startDate, DateTime endDate, int companyId);
    }
}
