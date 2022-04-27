using Core.Domain;
using Core.IRepositories;
using DTOs;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class 
        CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {

        VoucherContext? VoucherContext => Context as VoucherContext;
        public CompanyRepository(DbContext context) : base(context)
        {
            
        }

        public async Task ChangeCompanyName(int? companyId, string companyNewName)
        {
            var currentCompany = await VoucherContext.Companies.FindAsync(companyId);
            if (currentCompany == null)
            {
                throw new NullReferenceException("Company not found");
            }

            currentCompany.Name = companyNewName;
        }

        public async Task ChangeCompanyContactDate(int? companyId, DateTime newDate)
        {
            var currentCompany = await VoucherContext.Companies.FindAsync(companyId);
            if (currentCompany == null)
            {
                throw new NullReferenceException("Company not found");
            }

            currentCompany.ContactDate = newDate;
        }

        public Task AddWorkerToCompany(User user, int? companyId)
        {
            var currentCompany = VoucherContext.Companies.First(c => c.Id == companyId);
            Update(currentCompany);
            if (currentCompany.Workers == null)
            {
                currentCompany.Workers = new List<User>();
            }

            void Action() => currentCompany.Workers.Add(user);

            return Task.Run((Action) Action);
        }

        
    }
}
