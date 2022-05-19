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
            company.CheckForNull();
            return context
                .Set<DiscountCode>()
                .Any(dc => dc.Companies != null && dc.Companies.Contains(company));
        }

        public static IQueryable<Category> GetCategories(this Company? company, DbContext context)
        {
            company.CheckForNull();
            if (!company.HasCodes(context))
            {
                throw new InvalidOperationException("Company do not have categories");
            }
            return context.Set<Company>().Where(c => c == company)
                .Include(c => c.DiscountCodes)
                .ThenInclude(dc => dc.Discount)
                .ThenInclude(d => d.Player)
                .ThenInclude(p => p.Categories)
                .SelectMany(c => c.DiscountCodes.SelectMany(dc => dc.Discount.Player.Categories))
                .Distinct();
        }
        public static IQueryable<Player> GetPlayers(this Company? company, DbContext context, Category? category)
        {
            company.CheckForNull();
            category.CheckForNull();
            if (!company.HasCodes(context))
            {
                throw new InvalidOperationException(
                    "Company do not have any categories, players and discounts assigned to it");
            }
            return context.Set<Company>()
                .Where(c => c == company)
                .Include(c => c.DiscountCodes)
                .ThenInclude(dc => dc.Discount.Player)
                .SelectMany(c => c.DiscountCodes.Select(dc => dc.Discount.Player))
                .Where(p => p.Categories.Contains(category));
        }
        public static IQueryable<Player> GetAllPlayers(this Company? company, DbContext context)
        {
            company.CheckForNull();
            if (!company.HasCodes(context))
            {
                throw new InvalidOperationException(
                    "Company do not have any categories, players and discounts assigned to it");
            }

            return context.Set<Company>().SelectMany(c => c.DiscountCodes.Select(dc => dc.Discount.Player)).Distinct();
        }
        public static bool HasPlayer(this Company company, Player? player, DbContext context)
        {
            return company.GetAllPlayers(context).Contains(player);
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

    }
}
