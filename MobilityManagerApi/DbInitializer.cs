using Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using UserStoreLogic;

namespace MobilityManagerApi
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(UserDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task Initialize()
        {
            await _context.Database.EnsureCreatedAsync();
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

        }
    }
}
