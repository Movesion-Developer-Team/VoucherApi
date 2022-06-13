using Core.Domain;

namespace Core.IRepositories
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {
        Task<DiscountType> FindDiscountType(int? discountTypeId);
        Task<IQueryable<DiscountType>> GetAllDiscountTypes();
        Task AssignDiscountCodesToDiscount(int? discountId, int? batchId, int numberOfDiscounts);
        Task<IQueryable<Discount>> GetAllDiscountsForPlayer(int playerId);
        Task<bool> CodesAreAlreadyInDb(List<DiscountCode> codes);
        Task<int?> GetBatchLimit(int discountId);
        Task<int?> AddBatch(Batch batch);
        Task<IEnumerable<Discount>?> GetAllGetAllDiscountsForPlayerOfCompany(int companyId, int playerId);
        Task AssignDiscountToCompany(int? discountId, int? companyId);
        Task<IQueryable<Batch>?> GetAllBatches();
        Task<long> OrderAmount(int discountId, int numberOfCodes);
        Task ReserveCodes(int? discountId, int userId, int numberOfCodes);

        Task<int?> GetDiscountLimit(int discountId);

    }

}
