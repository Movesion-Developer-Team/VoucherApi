using Core.Domain;

namespace Core.IRepositories
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        Task AssignToCategory(int? companyId, int? categoryId);
        Task AssignToCategory(int? companyId, string? categoryName);
        Task AssignToCategory(string? companyName, string? categoryName);
        Task ChangeCompanyName(int companyId, string companyNewName);
        Task ChangeCompanyContactDate(int companyId, DateTime newDate);
        Task AddWorkerToCompany(string workerId, int companyId);


    }
}
