using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DTOs;
using Microsoft.EntityFrameworkCore;
using MobilityManagerApi.Controllers;
using NUnit.Framework;
using Persistence;

namespace MobilityManagerApi.Tests
{
    [TestFixture]
    public class BaseTest
    {
        protected enum Authors
        {
            Arif = 1
        }

        protected DbContextOptionsBuilder<VoucherContext> DbOptions;

        protected DbContext _context;

        protected Mapper? _mapper;

        protected CompanyController _companyController;

        protected CategoryController _categoryController;

        protected PlayerController _playerController;



        [OneTimeSetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.AddMaps(nameof(DTOs))));


            DbOptions = new DbContextOptionsBuilder<VoucherContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            _context = new VoucherContext(DbOptions.Options);



            _companyController = new(
                _mapper,
                _context as VoucherContext
            );

            _categoryController = new(
                _mapper,
                _context as VoucherContext);

            _playerController = new(
                _mapper,
                _context as VoucherContext);
        }


    }
}
