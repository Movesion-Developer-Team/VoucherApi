using System.Diagnostics;
using AutoMapper;
using Core.Domain;
using CsvHelper;
using DTOs.BodyDtos;
using DTOs.MethodDto;
using Enum;
using Microsoft.EntityFrameworkCore;

namespace Extensions
{
    public static class DiscountExtensions
    {
        public static bool HasAvailableDiscountCodes(this CompanyPortfolio portfolio, DbContext context)
        {

            try
            {
                var check = context
                    .Set<DiscountCode>()
                    .Where(dc => dc.CompanyPortfolioId == portfolio.Id)
                    .Any(dc => (dc.IsAssignedToCompany == false || dc.IsAssignedToCompany == null) && dc.UsageLimit > 0);
                return check;

            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public static async Task<int> GetCompanyLimit(this CompanyPortfolio portfolio, DbContext context)
        {
            if (!portfolio.HasAvailableDiscountCodes(context))
            {
                return 0;
            }

            var discount = await context.Set<Discount>().FindAsync(portfolio.DiscountId);
            discount.CheckForNull(nameof(discount));
            var discountType = await context.Set<DiscountType>().FindAsync(discount.DiscountTypeId);

            if (discount.DiscountType == null)
            {
                throw new InvalidOperationException("DiscountType is not assigned or not included in query");
            }
            if (discount.DiscountType.Name == DiscountTypes.PromotionalCode.ToString())
            {
                return context.Set<DiscountCode>()
                    .Where(dc => dc.CompanyPortfolioId == portfolio.Id && dc.UsageLimit>0)
                    .Select(dc => dc.UsageLimit)
                    .Max() ?? 0;
            }
            return await context
                .Set<DiscountCode>()
                .Where(dc => dc.CompanyPortfolioId == portfolio.Id && dc.IsAssignedToUser!=true && dc.UsageLimit>0)
                .CountAsync();
        }

        public static async Task<int> GetBatchLimit(this Batch? batch, DbContext context)
        {
            var discountType = await context.Set<DiscountType>().FindAsync(batch.DiscountTypeId);

            if (batch.DiscountTypeId == null)
            {
                throw new InvalidOperationException("Forbidden: batch must contain discount type");
            }

            if (discountType.Name == DiscountTypes.PromotionalCode.ToString())
            {
                return await context.Set<DiscountCode>()
                    .Where(dc => dc.CompanyPortfolioId == null && dc.UsageLimit > 0)
                    .Select(dc => dc.UsageLimit)
                    .MaxAsync() ?? 0;
            }

            return await context.Set<DiscountCode>()
                .Where(dc => dc.BatchId == batch.Id && dc.CompanyPortfolioId == null 
                                                    && dc.UsageLimit>0 && dc.IsAssignedToCompany == false
                                                    && dc.IsAssignedToUser == false).CountAsync();
        }

        public static async Task<bool> CodesAreAssignedToAnyCompany(this IQueryable<DiscountCode> discountCodes)
        {
            return await discountCodes.Select(c => c.CompanyPortfolioId).AnyAsync();
        }

        public static async Task<Tuple<IQueryable<DiscountCode>, string?>> ChooseCodes(this Batch batch, int quantity, DbContext context)
        {
            IQueryable<DiscountCode> discountCodes;


            var checkDiscountType = await context.Set<DiscountType>().FindAsync(batch.DiscountTypeId);
            checkDiscountType.CheckForNull(nameof(checkDiscountType));
            if (checkDiscountType.Name == DiscountTypes.PromotionalCode.ToString())
            {
                discountCodes = context.Set<DiscountCode>().Where(dc => dc.BatchId == batch.Id && 
                                                                        (dc.IsAssignedToCompany == false || dc.IsAssignedToCompany == null) && dc.UsageLimit>0)
                    .Select(dc=>dc);
                var discountType = DiscountTypes.PromotionalCode.ToString();
                return Tuple.Create(discountCodes, discountType)!;
            }

            

            discountCodes = context.Set<DiscountCode>()
                .Where(dc => dc.BatchId == batch.Id && (dc.IsAssignedToCompany == false || dc.IsAssignedToCompany == null) && dc.UsageLimit > 0)
                .Take(quantity);
            return Tuple.Create(discountCodes, checkDiscountType.Name);
        }

        public static async Task OneUserUsageTypeCodesAssignToCompany(this IQueryable<DiscountCode> discountCodes, CompanyPortfolio? companyPortfolio,
            double price, DbContext context)
        {
            if (await discountCodes.CodesAreAssignedToAnyCompany())
            {
                throw new InvalidOperationException(
                    "Codes are already assigned to some company. Please, use reassignment");
            }

            context.UpdateRange(discountCodes);
            await discountCodes.ForEachAsync(dc=>
            {
                dc.IsAssignedToCompany = true;
                dc.CompanyPortfolioId = companyPortfolio.CompanyId;
                
            });
            await context.SaveChangesAsync();
        }

        public static async Task MultiUserUsageTypeCodeAssignToCompany(this IQueryable<DiscountCode> discountCodes, CompanyPortfolio? companyPortfolio,
            int quantity, double price, DbContext context)
        {
            
            if (await discountCodes.CodesAreAssignedToAnyCompany())
            {
                throw new InvalidOperationException(
                    "Codes are already assigned to some company. Please, use reassignment");
            }

            var maxAmongAllCodesAvailability = discountCodes.Max(dc => dc.UsageLimit);
            var code = await discountCodes.Where(dc => dc.UsageLimit == maxAmongAllCodesAvailability).FirstAsync();
            context.Update(code);
            var restUsage = code.UsageLimit - quantity;

            if (restUsage < 0)
            {
                throw new InvalidOperationException("Cannot assign usage limit more than available");
            }
            code.UsageLimit = restUsage;
            code.IsAssignedToCompany = false;
            await context.SaveChangesAsync();

            var assignedCode = new DiscountCode
            {
                Code = code.Code,
                BatchId = code.BatchId,
                UsageLimit = quantity,
                IsAssignedToUser = false,
                IsAssignedToCompany = true,
                PlayerId = code.PlayerId
            };

            await context.Set<DiscountCode>().AddAsync(assignedCode);
            await context.SaveChangesAsync();

        }

        public static async Task AssignToCompany(this Tuple<IQueryable<DiscountCode>, string?> discountCodeInfo, CompanyPortfolio? companyPortfolio,
            int quantity, double price, DbContext context)
        {
            if (discountCodeInfo.Item2 == DiscountTypes.PromotionalCode.ToString())
            {
                if (quantity == null)
                {
                    throw new InvalidOperationException("Please provide a quantity also in assignment method");
                }

                await discountCodeInfo.Item1.MultiUserUsageTypeCodeAssignToCompany(companyPortfolio, quantity, price, context);
            }
            else
            {
                await discountCodeInfo.Item1.OneUserUsageTypeCodesAssignToCompany(companyPortfolio, price, context);
            }
        }

        public static async Task ReassignToOtherCompany(this IQueryable<DiscountCode> codes, int companyId, double price, DbContext context)
        {
            context.Set<DiscountCode>().UpdateRange(codes);
            var portfolioIds = codes.Select(c => c.CompanyPortfolioId).Distinct();
            if (portfolioIds.Count() != 1)
            {
                throw new InvalidOperationException("Codes are assigned to different companies or vouchers, " +
                                                    "which is forbidden in the current case, because reassignment should be applied " +
                                                    "to the sequence of codes related to the same Voucher and the same Company");
            }

            var companyPortfolio = await context.Set<CompanyPortfolio>().FindAsync(portfolioIds.ElementAt(0));
            companyPortfolio.CheckForNull(nameof(companyPortfolio));
            var discountId = companyPortfolio.DiscountId;
            if (companyPortfolio.CompanyId == companyId)
            {
                throw new InvalidOperationException("You are trying to reassign codes to the same company");
            }
            var portfolioForReassignment = await context.Set<CompanyPortfolio>()
                .Where(cp => cp.DiscountId == discountId && cp.CompanyId == companyId)
                .FirstOrDefaultAsync();
            if (portfolioForReassignment == null)
            {
                throw new InvalidOperationException("Discount is not assigned to the company");
            }
            await codes.ForEachAsync(c =>
            {
                c.CompanyPortfolioId = portfolioForReassignment.Id;
            });

            await context.SaveChangesAsync();
        }

        public static async Task<bool> CompanyHasDiscount(this Company? company, Discount? discount, DbContext context)
        {
            return await context.Set<CompanyPortfolio>()
                .Where(o => o.DiscountId == discount.Id && o.CompanyId == company.Id)
                .AnyAsync();
        }

        public static async Task<IQueryable<DiscountCode>>? ChooseAssignedDiscountCodes(this CompanyPortfolio? portfolio, int quantity, DbContext context) 
        {
            var discount = await context.Set<Discount>().FindAsync(portfolio.DiscountId);
            discount.CheckForNull(nameof(discount));
            var discountType = await context.Set<DiscountType>().FindAsync(discount.DiscountTypeId);
            discountType.CheckForNull(nameof(discountType));

            var company = await context.Set<Company>().FindAsync(portfolio.CompanyId);
            var player = await context.Set<Player>().FindAsync(discount.PlayerId);

            if (!await company.HasPlayer(player, context))
            {
                throw new InvalidOperationException($"Player {player.ShortName} is not assigned to the current company");
            }

            var assignedCodes = context.Set<DiscountCode>()
                .Where(dc => dc.CompanyPortfolioId == portfolio.Id 
                             && dc.TemporaryReserved == false
                             && dc.IsAssignedToUser == false)
                .Select(dc=>dc);

            if (!portfolio.HasAvailableDiscountCodes(context))
            {
                throw new InvalidOperationException("Company does not have any codes for related discount");
            }

            var assignedCodesAvailable = assignedCodes.Where(dc => dc.UsageLimit > 0);
            if (!assignedCodesAvailable.Any())
            {
                throw new InvalidOperationException("Company does not have available discount codes for the current discount");
            }

            if (discountType.Name == DiscountTypes.PromotionalCode.ToString())
            {
                var maxUsageAvailablePerCode = await assignedCodesAvailable.MaxAsync(dc => dc.UsageLimit);
                var promotionCode = assignedCodesAvailable.Where(dc => dc.UsageLimit == maxUsageAvailablePerCode);
                return promotionCode;
            }

            return assignedCodesAvailable;

        }

        public static Task<List<DiscountCode>> WriteToDiscountCodes(this CsvReader csv, IMapper mapper)
        {
            return Task.Run(() => mapper.ProjectTo<DiscountCode>(csv.GetRecords<CsvCodeDto>().AsQueryable()).ToList());
        }

        public static bool CheckIfCodesAlreadyInDatabase(this List<DiscountCode> codes, IQueryable<string?> codesInDb)
        {
            
            var checking = codes.Select(dc => dc.Code).Intersect(codesInDb);
            return checking.Any();

        }

        public static async Task AssignCodesToUserTemporary(this IQueryable<DiscountCode> codes, int userId, DbContext context)
        {
            context.Set<DiscountCode>().UpdateRange(codes);
            await codes.ForEachAsync(dc =>
            {
                dc.UserId = userId;
                dc.TemporaryReserved = true;
            });
            await context.SaveChangesAsync();
        }

    }
}
