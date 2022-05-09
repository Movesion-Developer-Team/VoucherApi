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

        public async Task AddUserToCompany(User user, int? companyId)
        {
            var currentCompany = await VoucherContext.Companies.FirstAsync(c => c.Id == companyId);
            Update(currentCompany);
            if (currentCompany.Users == null)
            {
                currentCompany.Users = new List<User>();
            }

            await Task.Run(() => currentCompany.Users.Add(user));
        }

        public async Task AddPlayerToCompany(int playerId, int companyId)
        {
            var player = await VoucherContext.Players.FindAsync(playerId);
            if (player == null)
            {
                throw new ArgumentNullException(nameof(playerId), "Player not found");
            }
            var company = await VoucherContext.Companies.Where(c => c.Id == companyId).Include(c => c.Players).FirstAsync();
            if (company == null)
            {
                throw new ArgumentNullException(nameof(companyId), "Company not found");
            }

            var collection = company.Players;
            if (collection != null)
            {
                if (collection.Contains(player)) throw new InvalidOperationException("Player is already assigned");
            }

            Update(company);

            company.Players ??= new List<Player>();
            void PlayersAction() => company.Players.Add(player);

            await Task.Run(PlayersAction);
        }

        public List<Tuple<string?, string?>> GetAllCompaniesWithPlayers()
        {
            var companies = VoucherContext.Companies.Select(c => c)
                    .Include(c => c.Players)
                    .AsQueryable();


            List<Tuple<string?, string?>> companiesWithPlayers = new List<Tuple<string?, string?>>();
            foreach (var company in companies)
            {
                if (company.Players != null)
                {
                    foreach (var player in company.Players)
                    {

                        companiesWithPlayers.Add(Tuple.Create(company.Name, player.ShortName));
                    }
                }
                else
                {
                    companiesWithPlayers.Add(Tuple.Create<string?, string?>(company.Name, null));
                }

            }


            return companiesWithPlayers;
        }

        public IQueryable<User> GetAllUsersOfCompany(int? companyId)
        {
            var users = VoucherContext.Users.Where(u => u.CompanyId == companyId).Select(u => u);
            return users;
        }
    }
}
