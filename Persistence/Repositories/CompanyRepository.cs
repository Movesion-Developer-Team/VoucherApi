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

        public async Task AddWorkerToCompany(User user, int? companyId)
        {
            var currentCompany = await VoucherContext.Companies.FirstAsync(c => c.Id == companyId);
            Update(currentCompany);
            if (currentCompany.Workers == null)
            {
                currentCompany.Workers = new List<User>();
            }

            void Action() => currentCompany.Workers.Add(user);

            await Task.Run((Action)Action);
        }

        public async Task AddPlayerToCompany(int playerId, int companyId)
        {
            var player = await VoucherContext.Players.FindAsync(playerId);
            if (player == null)
            {
                throw new ArgumentNullException(nameof(playerId), "Player not found");
            }
            var company = await VoucherContext.Companies.FindAsync(companyId);
            if (company == null)
            {
                throw new ArgumentNullException(nameof(companyId), "Company not found");
            }

            Update(company);
            company.Players ??= new List<Player>();
            void Action() => company.Players.Add(player);

            await Task.Run((Action)Action);
        }

    }
}
