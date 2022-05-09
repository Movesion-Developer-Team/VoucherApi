using Core.Domain;

namespace Core.IRepositories
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {

        Task ChangeCompanyName(int? companyId, string companyNewName);
        Task ChangeCompanyContactDate(int? companyId, DateTime newDate);
        Task AddUserToCompany(User user, int? companyId);
        Task AddPlayerToCompany(int playerId, int companyId);
        List<Tuple<string?, string?>> GetAllCompaniesWithPlayers();
        IQueryable<User> GetAllUsersOfCompany(int? companyId);
    }
}
