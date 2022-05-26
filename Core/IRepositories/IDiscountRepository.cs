using Core.Domain;

namespace Core.IRepositories
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {
        Task<DiscountType> FindDiscountType(int? discountTypeId);
        Task<IQueryable<DiscountType>> GetAllDiscountTypes();
        Task AssignDiscountCodesToCompany(int? discountId, int? companyId, int numberOfDiscounts, double price);
        Task<IQueryable<Discount>> GetAllDiscountsForPlayer(int playerId);
        Task<Discount?> GetDiscountWithCodes(int discountId);
        Task<bool> CodesAreAlreadyInDb(List<DiscountCode> codes);
        Task<int?> GetDiscountLimit(int discountId);
        Task<int?> AddBatch(Batch batch);
        Task<IEnumerable<Discount>?> GetAllGetAllDiscountsForPlayerOfCompany(int companyId, int playerId);
        Task AssignDiscountToCompany(int? discountId, int? companyId);

    }

}
