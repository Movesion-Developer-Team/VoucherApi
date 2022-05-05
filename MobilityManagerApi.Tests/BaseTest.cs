using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DTOs;
using DTOs.ResponseDtos;
using Microsoft.AspNetCore.Mvc;
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

        protected Func<IActionResult, int?> GetIdFromResponse = (result) => ((result as ObjectResult)?.Value as CreateNewEntityResponseDto)?.Id;

        protected DbContextOptionsBuilder<VoucherContext> DbOptions;

        protected DbContext _context;

        protected Mapper? _mapper;

        protected CompanyController _companyController;

        protected CategoryController _categoryController;

        protected PlayerController _playerController;
        protected DiscountController _discountController;



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
            _discountController = new(
                _mapper,
                _context as VoucherContext);
        }


    }
}
