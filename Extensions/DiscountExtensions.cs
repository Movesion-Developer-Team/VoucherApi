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
            string discountType;
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
                    .ThenInclude(dc => dc.Companies)
                    .SelectMany(d => d.DiscountCodes.Where(dc => dc.IsAssignedToCompany == false || dc.IsAssignedToCompany == null));
                discountType = DiscountTypes.PromotionalCode.ToString();
                return Tuple.Create(discountCodes, discountType)!;
            }

            discountType = context.Set<DiscountType>().Find(discount.DiscountTypeId).Name;

            discountCodes = context.Set<Discount>().Where(d => d.Id == discount.Id)
                .Include(d => d.DiscountCodes)
                .ThenInclude(dc => dc.Companies)
                .SelectMany(d => d.DiscountCodes.Where(dc => dc.IsAssignedToCompany == false || dc.IsAssignedToCompany == null))
                .Take(quantity);
            return Tuple.Create(discountCodes, discountType);
        }

        public static async Task OneUserUsageTypeCodesAssignToCompany(this IQueryable<DiscountCode> discountCodes, Company? company, DbContext context)
        {
            company.CheckForNull(nameof(company));
            await discountCodes.ForEachAsync(dc=> context.Set<CompanyDiscountCode>()
                .Add(new CompanyDiscountCode
                {
                    CompanyId = company.Id,
                    DiscountCodeId = dc.Id,
                }));
            
            await discountCodes.ForEachAsync(dc => dc.IsAssignedToCompany = true);
            await context.SaveChangesAsync();
        }

        public static async Task MultiUserUsageTypeCodeAssignToCompany(this IQueryable<DiscountCode> discountCodes, Company? company,
            int quantity, DbContext context)
        {
            await discountCodes.Where(dc => dc.Companies == null).ForEachAsync(dc => dc.Companies.Initialize());
            var maxCode = discountCodes.Max(dc => dc.UsageLimit);
            var code = await discountCodes.Where(dc => dc.UsageLimit == maxCode).FirstAsync();
            context.Update(code);
            var restUsage = code.UsageLimit - quantity;
            if (restUsage < 0)
            {
                throw new InvalidOperationException("Cannot assign usage limit more than available");
            }
            code.UsageLimit = quantity;
            code.IsAssignedToCompany = true;
            code.Companies.Add(company);
            await context.SaveChangesAsync();
            if (restUsage > 0)
            {
                var newCode = new DiscountCode
                {

                    Code = code.Code,
                    DiscountId = code.DiscountId,
                    UsageLimit = restUsage,
                    IsAssignedToUser = false,
                    IsAssignedToCompany = false,
                };
                await context.Set<DiscountCode>().AddAsync(newCode);
                await context.SaveChangesAsync();
            }
        }

        public static async Task AssignToCompany(this Tuple<IQueryable<DiscountCode>, string?> discountCodeInfo, Company? company,
            int? quantity, DbContext context)
        {
            if (discountCodeInfo.Item2 == DiscountTypes.PromotionalCode.ToString())
            {
                if (quantity == null)
                {
                    throw new InvalidOperationException("Please provide a quantity also in assignment method");
                }

                await discountCodeInfo.Item1.MultiUserUsageTypeCodeAssignToCompany(company, (int)quantity, context);
            }
            else
            {
                await discountCodeInfo.Item1.OneUserUsageTypeCodesAssignToCompany(company, context);
            }
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
