using Core.Domain;
using Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;
using UserStoreLogic;

namespace MobilityManagerApi
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserDbContext _contextUserDb;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CategoryRepository _categoryRepository;
        private readonly VoucherContext _voucherContext;

        public DbInitializer(UserDbContext contextUserDb,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            VoucherContext context)
        {
            _contextUserDb = contextUserDb;
            _roleManager = roleManager;
            _userManager = userManager;
            _voucherContext = context;
            _categoryRepository = new CategoryRepository(_voucherContext);
        }

        public async Task Initialize()
        {
            await _contextUserDb.Database.EnsureCreatedAsync();
            var roles = System.Enum.GetNames(typeof(Role));
            foreach (var role in roles)
            {
                var adminRoleExists = await _roleManager.RoleExistsAsync(role);
                if (!adminRoleExists)
                {
                    var currentRole = new IdentityRole(role);
                    await _roleManager.CreateAsync(currentRole);

                }
            }

            var users = new string[]
            {
                "Arif",
                "Salar",
                "Alberto"
            };
            var passwords = new string[]
            {
                "Arif&12345",
                "Salar&12345",
                "Alberto&12345"
            };

            foreach (var user in users)
            {
                var userDoesNotExist = _userManager.FindByNameAsync(user).Result == null;
                if (userDoesNotExist)
                {
                    var userIndex = Array.IndexOf(users, user);
                    var identityUser = new IdentityUser(user);
                    await _userManager.CreateAsync(identityUser, passwords[userIndex]);
                    Task.WaitAll();
                    await _userManager.AddToRoleAsync(identityUser, Role.SuperAdmin.ToString());
                }
            }

            Category[] categories = new Category[]
            {
                new Category
                {
                Name = "Car Sharing",
                Description = "This is Car Sharing category"
                },
                new Category
                {
                Name = "Public Transport",
                Description = "This is Public Transport category"
                },
                new Category
                {
                Name = "Taxi",
                Description = "This is Taxi category"
                }
            };
            if (!await _voucherContext.Categories.AnyAsync())
            {
                foreach (var category in categories)
                {

                    await _categoryRepository.AddAsync(category);
                }
            }

            DiscountType[] discountTypes = new DiscountType[]
            {
                new DiscountType
                {
                    Name = "SingleUse"
                },
                new DiscountType
                {
                    Name = "MultiUse"
                },
                new DiscountType
                {
                    Name = "PromotionalCode"
                }
            };

            if (!await _voucherContext.DiscountTypes.AnyAsync(
                    ))
            {
                foreach (var discountType in discountTypes)
                {

                    await _voucherContext.DiscountTypes.AddAsync(discountType);
                }
            }


            await _voucherContext.SaveChangesAsync();

        }
    }
}
