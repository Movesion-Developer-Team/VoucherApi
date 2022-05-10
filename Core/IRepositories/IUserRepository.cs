using Core.Domain;

namespace Core.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task SendJoinRequestToCompany(string code, int userId, string? message = null);
    }
}
