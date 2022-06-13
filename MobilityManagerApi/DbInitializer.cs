using AutoMapper;
using Core.Domain;
using Enum;
using Extensions;
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
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DbInitializer(UserDbContext contextUserDb,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            VoucherContext context, IMapper mapper)
        {
            _contextUserDb = contextUserDb;
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = new UnitOfWork(context);
            _mapper = mapper;

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

            var superAdmins = new string[]
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
            int? elefanteId;
            int? mercurioId;
            foreach (var user in superAdmins)
            {
                var superAdminDoesNotExist = _userManager.FindByNameAsync(user).Result == null;
                if (superAdminDoesNotExist)
                {
                    var userIndex = Array.IndexOf(superAdmins, user);
                    var identityUser = new IdentityUser(user);
                    await _userManager.CreateAsync(identityUser, passwords[userIndex]);
                    Task.WaitAll();
                    await _userManager.AddToRoleAsync(identityUser, Role.SuperAdmin.ToString());
                }
            }

            if (!await _unitOfWork.Company.AnyAsync())
            {
                elefanteId = await _unitOfWork.Company.AddAsync(new Company
                {
                    Name = "Elefante",
                });
                mercurioId = await _unitOfWork.Company.AddAsync(new Company
                {

                    Name = "Mercurio",
                });
            }
            else
            {
                elefanteId = null;
                mercurioId = null;
            }

            

            var elefanteUsers = new string[]
            {
                "Umberto",
                "Vittorio",
                "Amadeo"
            };

            var userPassword = "King&12345";

            foreach (var user in elefanteUsers)
            {
                var userDoesNotExist = _userManager.FindByNameAsync(user).Result == null;
                if (userDoesNotExist)
                {
                    var identityUser = new IdentityUser(user);
                    await _userManager.CreateAsync(identityUser, userPassword);
                    Task.WaitAll();
                    
                    await _userManager.AddToRoleAsync(identityUser, Role.User.ToString());

                    var currentUser = await _userManager.FindByNameAsync(user);
                    var dbUser = new User
                    {
                        IdentityUserId = currentUser.Id,
                        CompanyId = elefanteId
                    };
                    await _unitOfWork.User.AddAsync(dbUser);
                }

                
            }


            

            var mercurioUsers = new string[]
            {
                "Eva",
                "Adam",
                "Leyla"
            };

            var mercurioPassword = "Planet&12345";

            foreach (var user in mercurioUsers)
            {
                var userDoesNotExist = _userManager.FindByNameAsync(user).Result == null;
                if (userDoesNotExist)
                {
                    var identityUser = new IdentityUser(user);
                    await _userManager.CreateAsync(identityUser, mercurioPassword);
                    Task.WaitAll();

                    await _userManager.AddToRoleAsync(identityUser, Role.User.ToString());
                    var currentUser = await _userManager.FindByNameAsync(user);
                    var dbUser = new User
                    {
                        IdentityUserId = currentUser.Id,
                        CompanyId = mercurioId
                    };
                    await _unitOfWork.User.AddAsync(dbUser);
                }

                
            }

            Category[] categories = new Category[]
            {
                new Category
                {
                Name = "SHARING",
                Description = "This is Car Sharing category"
                },
                new Category
                {
                Name = "BUS",
                Description = "This is Public Transport category"
                },
                new Category
                {
                Name = "TAXI",
                Description = "This is Taxi category"
                },
                new Category
                {
                    Name = "eMOBILITY",
                    Description = "This is eMOBILITY category"
                },
                new Category
                {
                    Name = "AEREI",
                    Description = "This is AEREI category"
                },
                new Category()
                {
                    Name = "TRENI",
                    Description = "This is TRENI category"
                }
            };
            if (!await _unitOfWork.Category.AnyAsync())
            {
                await _unitOfWork.Category.AddRangeAsync(categories);
            }

            DiscountType[] discountTypes = new DiscountType[]
            {
                new DiscountType
                {
                    Name = DiscountTypes.SingleUse.ToString()
                },
                new DiscountType
                {
                    Name = DiscountTypes.MultiUse.ToString()

                },
                new DiscountType
                {
                    Name = DiscountTypes.PromotionalCode.ToString()

                }
            };

            if (!await _unitOfWork.DiscountType.AnyAsync())
            {
                await _unitOfWork.DiscountType.AddRangeAsync(discountTypes);
            }


            await _unitOfWork.Complete();

            Player[] carSharingPlayers = new Player[]
            {
                new Player
                {
                    
                    ShortName = "SHARENOW",
                    FullName = "The world's leading free-floating car-sharing service",
                    PlayStoreLink = "https://play.google.com/store/apps/details?id=com.car2go&hl=en",
                    AppStoreLink = "https://apps.apple.com/it/app/car2go/id514921710",
                    LinkDescription = null,
                    Color = null,
                },

                new Player()
                {
                    ShortName = "ENJOY",
                    FullName = "Car-sharing service",
                    PlayStoreLink = "https://play.google.com/store/apps/details?id=com.eni.enjoy",
                    AppStoreLink = "https://itunes.apple.com/it/app/enjoy/id780315684?l=it&ls=1&mt=8",
                    LinkDescription = null,
                    Color = null,
                }
            };
            Player[] scooterSharingPlayers = new Player[]
            {
                new Player()
                {
                    ShortName = "HELBIZ",
                    FullName = "We are a dedicated team of innovators," +
                               " engineers, technologists, and creatives who are driven " +
                               "by the desire to improve people’s lives through technology.",

                    PlayStoreLink = "https://play.google.com/store/apps/details?id=com.helbiz.android&hl=en_AU",
                    AppStoreLink = "https://itunes.apple.com/us/app/helbiz/id1438844293?mt=8",
                    LinkDescription = null,
                    Color = null,
                },
                new Player()
                {
                    ShortName = "LIME",
                    FullName = "Lime is the world’s largest shared electric vehicle company." +
                               " We are on a mission to build a future where transportation is shared," +
                               " affordable and carbon-free.",

                    PlayStoreLink = "https://play.google.com/store/apps/details?id=com.limebike",
                    AppStoreLink = "https://apps.apple.com/app/lime-your-ride-anytime/id1199780189",
                    LinkDescription = null,
                    Color = null,
                },
                new Player()
                {
                    ShortName = "BIRD",
                    FullName = "Bird is passionate about vibrant communities that have less traffic, " +
                               "cleaner air, and safer streets.",

                    PlayStoreLink = "https://play.google.com/store/apps/details?id=co.bird.android",
                    AppStoreLink = "https://apps.apple.com/us/app/bird-be-free-enjoy-the-ride/id1260842311",
                    LinkDescription = null,
                    Color = null,
                }
            };
            Player[] aereiPlayers = new Player[]
            {
                new Player()
                {
                    ShortName = "ALITALIA",
                    FullName = "Italian airlines"
                },
                new Player()
                {
                    ShortName = "RYANAIR",
                    FullName = "Ryanair Holdings plc is Europe’s largest airline group and parent " +
                               "company of Ryanair, Ryanair UK, Buzz, Lauda and Malta Air. "
                },

            };
            Player[] trainPlayers = new Player[]
            {
                new Player()
                {
                    ShortName = "TRENITALIA",
                    FullName = "Italian railways"
                },
                new Player()
                {
                    ShortName = "ITALO",
                    FullName = "Italian railways"
                }
            };

            if (!await _unitOfWork.Player.AnyAsync())
            {
                foreach (var player in carSharingPlayers)
                {
                    var id = await _unitOfWork.Player.AddAsync(player);
                    await _unitOfWork.Player.AssignCategoryToPlayer((int)id, 1);

                    if(player.ShortName == "SHARENOW")
                    {
                        var discId = await _unitOfWork.Discount.AddAsync(new Discount
                        {
                            Name = "25 eur/giorno",
                            LinkTermsAndConditions = null,
                            UnityOfMeasurement = UnitiesOfMeasurement.Euro,
                            DiscountValue = 25,
                            InitialPrice = 50,
                            FinalPrice = 25,
                            DiscountTypeId = 1,
                            PlayerId = id
                        });

                        var currentDirectory = System.IO.Directory.GetParent(
                            System.IO.Directory.GetCurrentDirectory()).FullName;

                        var path = currentDirectory + @"\batches\sharenow.csv";
                        var codes = await path.UploadCsvFromPathToCodes(_mapper);
                        var batch = new Batch
                        {
                            UploadTime = DateTimeOffset.UtcNow,
                            PurchasePrice = 23,
                            UnityOfMeasurement = UnitiesOfMeasurement.Euro,
                            Value = 25,
                            DiscountTypeId = 1,
                            PlayerId = id
                        };
                        var batchId = await _unitOfWork.Discount.AddBatch(batch);
                        codes.ForEach(c =>
                        {
                            c.DiscountId = discId;
                            c.BatchId = batchId;
                            c.UsageLimit = 1;
                        });
                        await _unitOfWork.DiscountCode.AddRangeAsync(codes);
                        await _unitOfWork.Player.AssignPlayerToCompany(elefanteId, id);
                        await _unitOfWork.Player.AssignPlayerToCompany(mercurioId, id);
                        await _unitOfWork.Discount.AssignDiscountToCompany(discId, elefanteId);
                        await _unitOfWork.Complete();

                    }
                    else
                    {
                        await _unitOfWork.Player.AssignPlayerToCompany(elefanteId, id);
                        await _unitOfWork.Player.AssignPlayerToCompany(mercurioId, id);
                    }


                }
                foreach (var player in scooterSharingPlayers)
                {
                    var id = await _unitOfWork.Player.AddAsync(player);
                    await _unitOfWork.Player.AssignCategoryToPlayer((int)id, 1);
                    await _unitOfWork.Player.AssignCategoryToPlayer((int)id, 4);
                    if (player.ShortName == "HELBIZ")
                    {
                        var discId = await _unitOfWork.Discount.AddAsync(new Discount
                        {
                            Name = "5 corse per 3 euro",
                            LinkTermsAndConditions = null,
                            UnityOfMeasurement = UnitiesOfMeasurement.Euro,
                            DiscountValue = 2,
                            InitialPrice = 5,
                            FinalPrice = 3,
                            DiscountTypeId = 1,
                            PlayerId = id,
                            PriceInPoints = 300

                        });
                        var currentDirectory = System.IO.Directory.GetParent(
                            System.IO.Directory.GetCurrentDirectory()).FullName;

                        var path = currentDirectory + @"\batches\helbiz.csv";
                        var codes = await path.UploadCsvFromPathToCodes(_mapper);
                        var batch = new Batch
                        {
                            UploadTime = DateTimeOffset.UtcNow,
                            PurchasePrice = 2.50,
                            UnityOfMeasurement = UnitiesOfMeasurement.Euro,
                            Value = 5,
                            DiscountTypeId = 1,
                            PlayerId = id
                        };
                        var batchId = await _unitOfWork.Discount.AddBatch(batch);
                        codes.ForEach(c =>
                        {
                            c.DiscountId = discId;
                            c.BatchId = batchId;
                            c.UsageLimit = 1;
                        });
                        
                        await _unitOfWork.DiscountCode.AddRangeAsync(codes);

                        await _unitOfWork.Player.AssignPlayerToCompany(elefanteId, id);
                        await _unitOfWork.Player.AssignPlayerToCompany(mercurioId, id);
                        await _unitOfWork.Discount.AssignDiscountToCompany(discId, elefanteId);
                        await _unitOfWork.Discount.AssignDiscountToCompany(discId, mercurioId);

                        await _unitOfWork.Complete();
                    }
                    else
                    {
                        await _unitOfWork.Player.AssignPlayerToCompany(elefanteId, id);
                        await _unitOfWork.Player.AssignPlayerToCompany(mercurioId, id);

                    }
                }
                foreach (var player in aereiPlayers)
                {
                    var id = await _unitOfWork.Player.AddAsync(player);
                    await _unitOfWork.Player.AssignCategoryToPlayer((int)id, 5);
                    await _unitOfWork.Player.AssignPlayerToCompany(elefanteId, id);
                    await _unitOfWork.Player.AssignPlayerToCompany(mercurioId, id);

                }
                foreach (var player in trainPlayers)
                {
                    var id = await _unitOfWork.Player.AddAsync(player);
                    await _unitOfWork.Player.AssignCategoryToPlayer((int)id, 6);
                    await _unitOfWork.Player.AssignPlayerToCompany(elefanteId, id);
                }
            }

            await _unitOfWork.Complete();

        }
    }
}
