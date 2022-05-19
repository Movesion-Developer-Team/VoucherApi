using Core.Domain;

namespace Core.IRepositories
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {
        Task<DiscountType> FindDiscountType(int? discountTypeId);
        Task<IQueryable<DiscountType>> GetAllDiscountTypes();
        Task AssignDiscountCodesToCompany(int? discountId, int? companyId, int numberOfDiscounts);
        Task<IQueryable<Discount>> GetAllDiscountsForPlayer(int playerId);
    }

}
