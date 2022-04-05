using Core.Domain;
using Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {

        VoucherContext? VoucherContext => Context as VoucherContext;
        public CompanyRepository(DbContext context) : base(context)
        {
            
        }

        public async Task AssignToCategory(int companyId, int categoryId)
        {
            var currentCompany = await VoucherContext!.Companies.FindAsync(companyId);
            if (currentCompany == null)
            {
                throw new NullReferenceException("Company not found");
            }

            var currentCategory = await VoucherContext.Categories.FindAsync(categoryId);
            if (currentCategory == null)
            {
                throw new NullReferenceException("Category not found");
            }

            currentCompany.Categories.Add(currentCategory);
        }

        public async Task AssignToCategory(int companyId, string categoryName)
        {
            var currentCompany = await VoucherContext!.Companies.FindAsync(companyId);
            if (currentCompany == null)
            {
                throw new NullReferenceException("Company not found");
            }

            var currentCategory = await VoucherContext!.Categories.FirstAsync(c => c.Name == categoryName);
            if (currentCategory == null)
            {
                throw new NullReferenceException("Category not found");
            }
            currentCompany.Categories.Add(currentCategory);
        }

        public async Task AssignToCategory(string companyName, string categoryName)
        {
            var currentCompany = await VoucherContext!.Companies.FirstAsync(c => c.Name == companyName);
            if (currentCompany == null)
            {
                throw new NullReferenceException("Company not found");
            }

            var currentCategory = await VoucherContext!.Categories.FirstAsync(c => c.Name == categoryName);
            if (currentCategory == null)
            {
                throw new NullReferenceException("Category not found");
            }
            currentCompany.Categories.Add(currentCategory);
        }

        public async Task ChangeCompanyName(int companyId, string companyNewName)
        {
            var currentCompany = await VoucherContext.Companies.FindAsync(companyId);
            if (currentCompany == null)
            {
                throw new NullReferenceException("Company not found");
            }

            currentCompany.Name = companyNewName;
        }

        public async Task ChangeCompanyContactDate(int companyId, DateTime newDate)
        {
            var currentCompany = await VoucherContext.Companies.FindAsync(companyId);
            if (currentCompany == null)
            {
                throw new NullReferenceException("Company not found");
            }

            currentCompany.ContactDate = newDate;
        }

        public Task AddWorkerToCompany(List<string> workerIds, int companyId)
        {
            var currentCompaniesWorkerList = VoucherContext.Companies
                .Where(c => c.Id == companyId)
                .Select(c => c.WorkerIds);


            currentCompaniesWorkerList.Append(workerIds);
            return Task.FromResult(currentCompaniesWorkerList);
        }
        
    }
}
