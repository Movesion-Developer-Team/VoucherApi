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
        public static bool HasAvailableDiscountCodes(this Discount discount, DbContext context)
        {

            try
            {
                var check = context
                    .Set<Discount>()
                    .Where(d => d.Id == discount.Id)
                    .Include(d => d.DiscountCodes)
                    .SelectMany(d => d.DiscountCodes)
                    .Any(dc => dc.IsAssignedToCompany == false || dc.IsAssignedToCompany == null);
                return check;

            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public static int GetLimit(this Discount discount, DbContext context)
        {
            if (!discount.HasAvailableDiscountCodes(context))
            {
                return 0;
            }

            if (discount.DiscountType == null)
            {
                throw new InvalidOperationException("DiscountType is not assigned or not included in query");
            }
            if (discount.DiscountType.Name == DiscountTypes.PromotionalCode.ToString())
            {
                return context.Set<Discount>()
                    .Where(d => d.Id == discount.Id)
                    .Include(d => d.DiscountCodes)
                    .SelectMany(d => d.DiscountCodes.Where(dc => dc.IsAssignedToCompany == false))
                    .Select(dc => dc.UsageLimit)
                    .Max() ?? 0;
            }
            return context.Set<Discount>()
                .Where(d => d.Id == discount.Id)
                .Include(d => d.DiscountCodes)
                .SelectMany(d => d.DiscountCodes.Where(dc => dc.IsAssignedToCompany == false || dc.IsAssignedToCompany == null))
                .Count();
        }

        public static Tuple<IQueryable<DiscountCode>, string?> ChooseCodes(this Discount discount, int quantity, DbContext context)
        {
            IQueryable<DiscountCode> discountCodes;
            var checkDiscountType = context.Set<Discount>()
                .Where(d => d.Id == discount.Id)
                .Include(d => d.DiscountType)
                .Select(d => d.DiscountType)
                .FirstOrDefault();
            if (checkDiscountType.Name == DiscountTypes.PromotionalCode.ToString())
            {
                discountCodes = context.Set<Discount>().Where(d => d.Id == discount.Id)
                    .Include(d => d.DiscountCodes)
                    .SelectMany(d => d.DiscountCodes.Where(dc => dc.IsAssignedToCompany == false || dc.IsAssignedToCompany == null));
                var discountType = DiscountTypes.PromotionalCode.ToString();
                return Tuple.Create(discountCodes, discountType)!;
            }

            

            discountCodes = context.Set<Discount>().Where(d => d.Id == discount.Id)
                .Include(d => d.DiscountCodes)
                .SelectMany(d => d.DiscountCodes.Where(dc => dc.IsAssignedToCompany == false || dc.IsAssignedToCompany == null))
                .Take(quantity);
            return Tuple.Create(discountCodes, checkDiscountType.Name);
        }

        public static async Task OneUserUsageTypeCodesAssignToCompany(this IQueryable<DiscountCode> discountCodes, Company? company,
            double price, DbContext context)
        {
            
            company.CheckForNull(nameof(company));
            
            context.UpdateRange(discountCodes);
            await discountCodes.ForEachAsync(dc=>
            {
                
                context.Set<Offer>()

                    .Add(new Offer
                    {
                        Price = price,
                        Availability = dc.UsageLimit,
                        CompanyId = company.Id,
                        DiscountCodeId = dc.Id,
                    });
                
                dc.IsAssignedToCompany = true;
                dc.UsageLimit = 0;

            });
            await context.SaveChangesAsync();
        }

        public static async Task MultiUserUsageTypeCodeAssignToCompany(this IQueryable<DiscountCode> discountCodes, Company? company,
            int quantity, double price, DbContext context)
        {
            
            var codeGlobalAvailability = discountCodes.Max(dc => dc.UsageLimit);
            var code = await discountCodes.Where(dc => dc.UsageLimit == codeGlobalAvailability).FirstAsync();
            context.Update(code);
            var restUsage = code.UsageLimit - quantity;
            if (restUsage < 0)
            {
                throw new InvalidOperationException("Cannot assign usage limit more than available");
            }
            code.UsageLimit = restUsage;
            code.IsAssignedToCompany = false;
            await context.SaveChangesAsync();

            var offer = new Offer
            {
                
                Price = price,
                Availability = quantity,
                CompanyId = company.Id,
                DiscountCodeId = code.Id,
            };
            await context.Set<Offer>().AddAsync(offer);
            await context.SaveChangesAsync();

        }

        public static async Task AssignToCompany(this Tuple<IQueryable<DiscountCode>, string?> discountCodeInfo, Company? company,
            int? quantity, double price, DbContext context)
        {
            if (discountCodeInfo.Item2 == DiscountTypes.PromotionalCode.ToString())
            {
                if (quantity == null)
                {
                    throw new InvalidOperationException("Please provide a quantity also in assignment method");
                }

                await discountCodeInfo.Item1.MultiUserUsageTypeCodeAssignToCompany(company, (int)quantity, price, context);
            }
            else
            {
                await discountCodeInfo.Item1.OneUserUsageTypeCodesAssignToCompany(company, price, context);
            }
        }

        public static async Task ReassignToOtherCompany(this IQueryable<Offer> codes, int companyId, double price, DbContext context)
        {
            context.Set<Offer>().UpdateRange(codes);
            await codes.ForEachAsync(c =>
            {
                c.CompanyId = companyId;
                c.Price = price;
            });
            await context.SaveChangesAsync();
        }

        public static async Task<IQueryable<Offer>> ChooseAssignedDiscountCodes(this Discount? discount, int companyId, int quantity, DbContext context)
        {
            var discountType = await context.Set<DiscountType>().FindAsync(discount.DiscountTypeId);
            discountType.CheckForNull(nameof(discountType));
            var company = await context.Set<Company>().FindAsync(companyId);
            company.CheckForNull(nameof(company));
            var player = await context.Set<Player>().FindAsync(discount.PlayerId);
            if (!await company.HasPlayer(player, context))
            {
                throw new InvalidOperationException($"Player {player.ShortName} is not assigned to the current company");
            }


            var assignedCodes = context.Set<DiscountCode>()
                .Where(dc => dc.DiscountId == discount.Id)
                .Include(dc => dc.Offers)
                .SelectMany(dc => dc.Offers)
                .Where(o => o.CompanyId == companyId);
            if (!assignedCodes.Any())
            {
                throw new InvalidOperationException("Company does not have any codes for related discount");
            }

            var assignedCodesAvailable = assignedCodes.Where(o => o.Availability > 0);
            if (!assignedCodesAvailable.Any())
            {
                throw new InvalidOperationException("Company does not have available discount codes for the current discount");
            }

            if (discountType.Name == DiscountTypes.PromotionalCode.ToString())
            {
                var maxUsageAvailablePerCode = await assignedCodesAvailable.MaxAsync(o => o.Availability);
                var promotionCode = assignedCodesAvailable.Where(o => o.Availability == maxUsageAvailablePerCode);
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

    }
}
