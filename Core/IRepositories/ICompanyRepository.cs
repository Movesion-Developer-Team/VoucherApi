using Core.Domain;

namespace Core.IRepositories
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {

        Task ChangeCompanyName(int? companyId, string companyNewName);
        Task ChangeCompanyContactDate(int? companyId, DateTime newDate);
        Task AddUserToCompany(User user, int? companyId);
        Task<List<Tuple<string?, string?>>> GetAllCompaniesWithPlayers();
        IQueryable<User> GetAllUsersOfCompany(int? companyId);
        Task<IQueryable<JoinRequest>> GetAllJoinRequests(int companyId);
        Task<IQueryable<Player>> GetAllPlayersForOneCompany(int companyId);
        Task<bool> AcceptRequest(int requestId, int userId);
        Task DeclineRequest(int requestId);
        Task<IQueryable<Player?>?> SearchForPlayerOfCompany(string? playerName, int companyId);
    }
}
