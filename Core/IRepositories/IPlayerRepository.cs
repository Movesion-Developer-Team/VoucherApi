using Core.Domain;

namespace Core.IRepositories
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        Task AssignCategoryToPlayer(int playerId, int categoryId);
        Task<bool> DeleteCategoryFromPlayer(int playerId, int categoryId);

        Task AssignDiscountTypeToPlayer(int? playerId, int? discountTypeId);
        Task<IQueryable<DiscountType>> GetAllDiscountTypesForPlayer(int? playerId);
        Task AssignPlayerToCompany(int? companyId, int? playerId);
        Task AddImageToPlayer(BaseImage baseImage, int? playerId);
        Task<IQueryable<Player>> GetAll(bool withImage);
        Task<BaseImage> GetImageOfPlayer(int playerId);

    }


}
