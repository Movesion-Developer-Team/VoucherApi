using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using NUnit.Framework;
using UserStoreLogic.Controllers;

namespace UserStoreLogic.Tests
{
    [TestFixture]
    internal class BaseSetup
    {
        protected DbContextOptionsBuilder<UserDbContext> DbOptions;
        protected UserDbContext Context;
        protected IMapper Mapper;
        protected AuthController AuthController;
        protected readonly UserManager<IdentityUser> UserManager;

        [OneTimeSetUp]
        public void Setup()
        {
            var builder = WebApplication.CreateBuilder();

            DbOptions = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            Context = new UserDbContext(DbOptions.Options);
            //Mapper = new Mapper(new MapperConfiguration(cfg =>
            //    cfg.AddMaps(nameof(DTOs))));
            //UserManager = new UserManager<IdentityUser>();
            //AuthController = new AuthController();

        }

    }
}
