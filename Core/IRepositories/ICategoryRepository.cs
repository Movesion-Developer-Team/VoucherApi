using Core.Domain;

namespace Core.IRepositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IQueryable<Category>> GetAllCategoriesForPlayer(int playerId);
        Task<IQueryable<Category>> GetAllCategoriesForCompany(int companyId);
        Task<IQueryable<Player>> GetAllPlayersForCategoryAndCompany(int companyId, int categoryId);
    }
}
