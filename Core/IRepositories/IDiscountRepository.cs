using Core.Domain;

namespace Core.IRepositories
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {
        Task<DiscountType> FindDiscountType(int? discountTypeId);
        Task<IQueryable<DiscountType>> GetAllDiscountTypes();
        Task AssignDiscountCodesToCompany(int? discountId, int? companyId, int? batchId, int numberOfDiscounts, double price);
        Task<IQueryable<Discount>> GetAllDiscountsForPlayer(int playerId);
        Task<bool> CodesAreAlreadyInDb(List<DiscountCode> codes);
        Task<int?> GetBatchLimit(int discountId);
        Task<int?> AddBatch(Batch batch);
        Task<IEnumerable<Discount>?> GetAllGetAllDiscountsForPlayerOfCompany(int companyId, int playerId);
        Task AssignDiscountToCompany(int? discountId, int? companyId);
        Task<IQueryable<Batch>?> GetAllBatches();
        Task<long> OrderAmount(int? discountId, int? companyId, int numberOfCodes);
        Task ReserveCodes(int? discountId, int? companyId, int userId, int numberOfCodes);

    }

}
