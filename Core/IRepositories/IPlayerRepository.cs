using Core.Domain;

namespace Core.IRepositories
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        Task AssignCategoryToPlayer(int playerId, int categoryId);
        Task<bool> DeleteCategoryFromPlayer(int playerId, int categoryId);

        Task AssignDiscountTypeToPlayer(int? playerId, int? discountTypeId);
        Task<IQueryable<DiscountType>> GetAllDiscountTypesForPlayer(int? playerId);
    }


}
