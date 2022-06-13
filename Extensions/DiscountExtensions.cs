using System.Diagnostics;
using System.Globalization;
using System.Text;
using AutoMapper;
using Core.Domain;
using CsvHelper;
using CsvHelper.Configuration;
using DTOs.BodyDtos;
using DTOs.MethodDto;
using Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Extensions
{
    public static class DiscountExtensions
    {
        public static bool HasAvailableDiscountCodes(this Discount discountId, DbContext context)
        {

            try
            {
                var check = context
                    .Set<DiscountCode>()
                    .Where(dc => dc.DiscountId == discountId.Id && dc.IsAssignedToUser == false 
                                                                && dc.TemporaryReserved == false)
                    .Any(dc => dc.UsageLimit > 0);
                return check;

            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public static async Task<int> GetDiscountLimit(this Discount discount, DbContext context)
        {
            
            if (!discount.HasAvailableDiscountCodes(context))
            {
                return 0;
            }
            var discountType = await context.Set<DiscountType>().FindAsync(discount.DiscountTypeId);

            if (discountType == null)
            {
                throw new InvalidOperationException("DiscountType is not assigned or not included in query");
            }
            if (discountType.Name == DiscountTypes.PromotionalCode.ToString())
            {
                return context.Set<DiscountCode>()
                    .Where(dc => dc.DiscountId == discount.Id && dc.UsageLimit>0)
                    .Select(dc => dc.UsageLimit)
                    .Max() ?? 0;
            }
            return await context
                .Set<DiscountCode>()
                .Where(dc => dc.DiscountId == discount.Id && dc.IsAssignedToUser!=true && dc.UsageLimit>0 && dc.TemporaryReserved != true )
                .CountAsync();
        }

        public static async Task<int> GetBatchFreeCodesLimit(this Batch? batch, DbContext context)
        {
            var discountType = await context.Set<DiscountType>().FindAsync(batch.DiscountTypeId);

            if (batch.DiscountTypeId == null)
            {
                throw new InvalidOperationException("Forbidden: batch must contain discount type");
            }

            if (discountType.Name == DiscountTypes.PromotionalCode.ToString())
            {
                return await context.Set<DiscountCode>()
                    .Where(dc=>dc.BatchId == batch.Id && dc.DiscountId == null && dc.IsAssignedToUser != true && dc.UsageLimit > 0 && dc.TemporaryReserved != true)
                    .Select(dc => dc.UsageLimit)
                    .MaxAsync() ?? 0;
            }

            return await context.Set<DiscountCode>()
                .Where(dc => dc.BatchId == batch.Id && dc.DiscountId == null 
                                                    && dc.UsageLimit>0 && dc.IsAssignedToUser != true
                                                    && dc.TemporaryReserved != true).CountAsync();
        }


        public static async Task<Tuple<IQueryable<DiscountCode>, string?>> ChooseMonoUserCodesFromBatch(this Batch batch, DbContext context)
        {
            IQueryable<DiscountCode> discountCodes;


            var checkDiscountType = await context.Set<DiscountType>().FindAsync(batch.DiscountTypeId);
            checkDiscountType.CheckForNull(nameof(checkDiscountType));
            if (checkDiscountType.Name == DiscountTypes.PromotionalCode.ToString())
            {
                throw new InvalidOperationException("This type of discounts is not valid for the current operation");
            }

            

            discountCodes = context.Set<DiscountCode>()
                .Where(dc => dc.BatchId == batch.Id && dc.UsageLimit > 0
                                                    && dc.IsAssignedToUser != true
                                                    && dc.TemporaryReserved != true)
                .Select(dc=>dc);
            return Tuple.Create(discountCodes, checkDiscountType.Name);
        }

        public static async Task MonoUserCodesAssignToDiscount(this IQueryable<DiscountCode> codes, Discount? discount, int quantity, DbContext context)
        {
            
            if (codes == null)
            {
                throw new InvalidOperationException("Batch does not contain any codes");
            }

            context.UpdateRange(codes);
            await codes.Take(quantity).ForEachAsync(dc=>
            {
                dc.DiscountId = discount.Id;
            });
            await context.SaveChangesAsync();
        }

        public static async Task MultiUserCodeAssignToDiscount(this DiscountCode code, Discount? discount,
            int quantity, DbContext context)
        {
            
            
            
            context.Update(code);

            var restUsage = code.UsageLimit - quantity;

            if (restUsage < 0)
            {
                throw new InvalidOperationException("Cannot assign usage limit more than available");
            }
            code.UsageLimit = restUsage;
           
            await context.SaveChangesAsync();

            var assignedCode = new DiscountCode
            {
                Code = code.Code,
                BatchId = code.BatchId,
                UsageLimit = quantity,
                DiscountId = discount.Id,
            };

            await context.Set<DiscountCode>().AddAsync(assignedCode);
            await context.SaveChangesAsync();

        }

        public static async Task AssignToDiscount(this Tuple<IQueryable<DiscountCode>, string?> discountCodeInfo, Discount? discount,
            int quantity, DbContext context)
        {   
            if (discountCodeInfo.Item2 == DiscountTypes.PromotionalCode.ToString())
            {
                if (quantity == null)
                {
                    throw new InvalidOperationException("Please provide a quantity also in assignment method");
                }

                await discountCodeInfo.Item1.First().MultiUserCodeAssignToDiscount(discount, quantity, context);
            }
            else
            {
                await discountCodeInfo.Item1.MonoUserCodesAssignToDiscount(discount, quantity, context);
            }
        }

        public static async Task ReassignToOtherDiscount(this IQueryable<DiscountCode> codes, int discountId, DbContext context)
        {
            
            context.Set<DiscountCode>().UpdateRange(codes);

            if (codes.Select(dc => dc.DiscountId).Distinct().Any(id => id == discountId))
            {
                throw new InvalidOperationException("You are trying to reassign codes to the same discount");
            }
            var discountForReassignment = await context.Set<Discount>()
                .Where(d => d.Id == discountId)
                .FirstOrDefaultAsync();
            if (discountForReassignment == null)
            {
                throw new InvalidOperationException("Discount is not found");
            }
            await codes.ForEachAsync(c =>
            {
                c.DiscountId = discountId;
            });

            await context.SaveChangesAsync();
        }

        public static async Task<bool> CompanyHasDiscount(this Company? company, Discount? discount, DbContext context)
        {
            return await context.Set<CompanyPortfolio>()
                .Where(o => o.DiscountId == discount.Id && o.CompanyId == company.Id)
                .AnyAsync();
        }

        public static async Task<IQueryable<DiscountCode>>? ChooseAssignedMonoUserCodes(this Discount? discount, int quantity, DbContext context) 
        {
            
            
            var discountType = await context.Set<DiscountType>().FindAsync(discount.DiscountTypeId);

            if (discountType.Name == DiscountTypes.PromotionalCode.ToString())
            {
                throw new InvalidOperationException("Current operation is not valid for this discount type");
            }

            discountType.CheckForNull(nameof(discountType));

            if (!discount.HasAvailableDiscountCodes(context))
            {
                throw new InvalidOperationException("Discount does not have any available codes");
            }

            var assignedCodes = context.Set<DiscountCode>()
                .Where(dc => dc.DiscountId == discount.Id 
                             && dc.TemporaryReserved == false
                             && dc.IsAssignedToUser == false
                             && dc.UsageLimit > 0)
                .Select(dc=>dc);

            return assignedCodes;

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

        public static async Task<List<DiscountCode>> UploadCsvFromPathToCodes(this string path, IMapper mapper)
        {
            using var streamReader = new StreamReader(path, Encoding.UTF8);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };

            var csvReader = new CsvReader(streamReader, config);
            return await csvReader.WriteToDiscountCodes(mapper);
        }


    }
}
