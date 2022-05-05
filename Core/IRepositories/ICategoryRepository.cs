using Core.Domain;

namespace Core.IRepositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Player> GetAllCategoriesForPlayer(int playerId);
    }
}
