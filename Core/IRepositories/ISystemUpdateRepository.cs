using Core.Domain;

namespace Core.IRepositories
{
    public interface ISystemUpdateRepository : IGenericRepository<SystemUpdate>
    {
        DateTimeOffset? GetLastUpdate();
    }
}
