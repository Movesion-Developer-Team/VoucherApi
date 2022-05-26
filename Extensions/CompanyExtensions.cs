using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Extensions
{
    public static class CompanyExtensions
    {
        public static bool HasCodes(this Company? company, DbContext context)
        {
            company.CheckForNull(nameof(company));
            return context
                .Set<Offer>()
                .Where(o => o.CompanyId == company.Id)
                .Select(o=>o.Availability>0).Any();
        }
        public static async Task<IQueryable<Category>> GetCategories(this Company? company, DbContext context)
        {
            company.CheckForNull(nameof(company));
            if (!await company.HasAnyPlayer(context))
            {
                throw new InvalidOperationException("Company do not have categories");
            }
            return context.Set<Company>().Where(c => c == company)
                .Include(c => c.Players)
                .ThenInclude(p=>p.Categories)
                .SelectMany(c=>c.Players
                    .SelectMany(p=>p.Categories))
                .Distinct();
        }
        public static IQueryable<Player> GetPlayers(this Company? company, DbContext context, Category? category)
        {
            company.CheckForNull(nameof(company));
            category.CheckForNull(nameof(category));
            if (!company.HasCodes(context))
            {
                throw new InvalidOperationException(
                    "Company do not have any categories, players and discounts assigned to it");
            }

            return context.Set<Company>()
                .Where(c => c == company)
                .Include(c => c.Players)
                .ThenInclude(p => p.Categories)
                .SelectMany(c => c.Players)
                .Where(p => p.Categories.Contains(category));
        }
        public static async Task<IQueryable<Player>> GetAllPlayers(this Company? company, DbContext context)
        {
            company.CheckForNull(nameof(company));
            var companyWIthPlayers = await context.Set<Company>()
                .Where(c => c.Id == company.Id)
                .Include(c => c.Players)
                .SingleOrDefaultAsync();
            if (companyWIthPlayers == null)
            {
                throw new ArgumentNullException(nameof(companyWIthPlayers), "Company not found");
            }

            if (companyWIthPlayers.Players == null)
            {
                throw new ArgumentNullException(nameof(companyWIthPlayers), "Company does not have players");
            }

            return companyWIthPlayers.Players.AsQueryable();

        }
        public static async Task<bool> HasAnyPlayer(this Company company, DbContext context)
        {
            var players = await company.GetAllPlayers(context);
            return company.Players.Any();
        }
        public static async Task<bool> HasPlayer(this Company? company, Player? player, DbContext context)
        {
            if (!await company.HasAnyPlayer(context))
            {
                return false;
            }

            return context.Set<Company>()
                .Where(c => c.Id == company.Id)
                .Include(c => c.Players)
                .SelectMany(c => c.Players)
                .Contains(player);
        }
        public static ICollection<Company> Initialize(this ICollection<Company>? companies)
        {
            if (companies == null)
            {
                companies = new List<Company>();
                return companies;
            }

            throw new InvalidOperationException("Lis contains elements");
        }
        public static async Task<bool> PlayerIsAssigned(this Company? company, Player? player, DbContext context)
        {
            company.CheckForNull(nameof(company));
            player.CheckForNull(nameof(player));

            context.Set<Player>()
                .Where(p => p.Id == player.Id)
                .Include(p => p.Companies)
                .SelectMany(p => p.Companies)
                .CheckQueryForNull();

            return await context.Set<Player>()
                .Where(p => p.Id == player.Id)
                .Include(p => p.Companies)
                .SelectMany(p => p.Companies)
                .ContainsAsync(company);


        }
    }
}
