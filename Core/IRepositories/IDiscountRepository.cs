using Core.Domain;

namespace Core.IRepositories
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {
        Task<DiscountType> FindDiscountType(int? discountTypeId);
        Task<IQueryable<DiscountType>> GetAllDiscountTypes();
        Task AssignDiscountCodesToCompany(int? discountId, int? companyId, int numberOfDiscounts);
        Task<IQueryable<Discount>> GetAllDiscountsForPlayer(int playerId);
        Task<Discount?> GetDiscountWithCodes(int discountId);
        Task<bool> CodesAreAlreadyInDb(List<DiscountCode> codes);
        Task<int?> GetDiscountLimit(int discountId);
        Task<int?> AddBatch(Batch batch);
        Task<IQueryable<Discount>?> GetAllGetAllDiscountsForPlayerOfCompany(int companyId, int playerId);
    }

}
