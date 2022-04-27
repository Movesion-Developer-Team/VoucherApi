using Core.Domain;

namespace Core.IRepositories
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        
        Task ChangeCompanyName(int? companyId, string companyNewName);
        Task ChangeCompanyContactDate(int? companyId, DateTime newDate);
        Task AddWorkerToCompany(User user, int? companyId);


    }
}
