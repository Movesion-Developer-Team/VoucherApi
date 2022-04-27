using DTOs.BodyDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobilityManagerApi.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTOs.ResponseDtos;

namespace MobilityManagerApi.Tests
{
    [TestFixture(TestOf = typeof(CompanyController))]
    public class CompanyControllerTest : BaseTest
    {
        

        private CreateNewCompanyBodyDto _companyDto;
        private int? _companyId;

        [OneTimeSetUp]
        public async Task TestSetup()
        {
            _companyDto = new CreateNewCompanyBodyDto() 
            {
                Name = "Hello",
                Address = "Moon",
                NumberOfEmployees = 7
            };
            
            _companyId = ((await _companyController.CreateNewCompany(_companyDto) as ObjectResult)
                .Value as CreateNewEntityResponseDto)
                .Id;
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            _companyDto = null;
            await _context.DisposeAsync();
        }

        [Test]
        [Author(nameof(Authors.Arif))]
        [Order(1)]
        public async Task CreateNewCompanyTest()
        {
            _companyDto.Name = null;

            var result = await _companyController.CreateNewCompany(_companyDto) as ObjectResult;
            if (result == null)
            {
                Assert.Fail("Method did not return ObjectResult");
            }

            if (result.StatusCode != StatusCodes.Status400BadRequest)
            {
                Assert.Fail("Forbidden to allow nullable for the name property");
            }

            _companyDto.Name = "Hello";
            result = await _companyController.CreateNewCompany(_companyDto) as ObjectResult;

            var resultedObject = result.Value as CreateNewEntityResponseDto;
            if (resultedObject == null)
            {
                Assert.Fail("ObjectResult does not contain entity id");
            }

            Assert.Multiple(()=>
            {
                Assert.IsInstanceOf(typeof(Int32), resultedObject.Id);
                Assert.AreEqual(2, resultedObject.Id);

            });
        }

        [Test]
        [Author(nameof(Authors.Arif))]
        [Order(2)]
        public async Task DeleteTest()
        {
           
            var id = 2;
            var baseBody = new BaseBody()
            {
                Id = id
            };
            var deleteFunctionTrueValue = ((((await _companyController.Delete(baseBody) as ObjectResult)!)
                    .Value as DeleteResponseDto)!)
                .Unit;
            var deleteFunctionFalseValue = ((((await _companyController.Delete(baseBody) as ObjectResult)!)
                    .Value as DeleteResponseDto)!)
                .Unit;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(deleteFunctionTrueValue);
                Assert.IsTrue(!deleteFunctionFalseValue);
            });


        }

        [Test]
        [Author(nameof(Authors.Arif))]
        [Order(3)]
        public async Task GetAllTest()
        {
            await _companyController.Delete(new BaseBody() {Id = 1});
            var companies = await _companyController.GetAll() as ObjectResult;
            if (companies.StatusCode != StatusCodes.Status400BadRequest)
            {
                Assert.Fail("Method should return BadRequest, when there is no entities in DB");
            }


            await _companyController.CreateNewCompany(_companyDto);

            companies = await _companyController.GetAll() as ObjectResult;
            if (companies.StatusCode != StatusCodes.Status200OK)
            {
                Assert.Fail("Method did not return 200 response");
            }

            Assert.IsInstanceOf(typeof(GetAllCompaniesResponseDto), companies.Value);

        }

        [Test]
        [Author(nameof(Authors.Arif))]
        [Order(4)]
        public async Task FindByIdTest()
        {
            _companyId = 3;
            List<CompanyMainResponseDto> resultsByDifferentConditions = new();
            List<int?> companyIds = new()
            {
                _companyId,
                2
            };
            ObjectResult objectResult;
            CompanyMainResponseDto expectedValue;
            foreach (var id in companyIds)
            {
                objectResult = await _companyController.FindById((int)id) as ObjectResult;
                expectedValue = objectResult.Value as CompanyMainResponseDto;
                resultsByDifferentConditions.Add(expectedValue);
                
            }
            
            Assert.Multiple(() =>
            {
                Assert.IsTrue(resultsByDifferentConditions[0].Unit.Name == _companyDto.Name);
                Assert.IsTrue(resultsByDifferentConditions[1].Unit == null);
                Assert.IsTrue(resultsByDifferentConditions[1].Message != null);
            });
            

        }

        [Test]
        [Author(nameof(Authors.Arif))]
        [Order(5)]
        public async Task ChangeTest()
        {
            _companyDto.Name = "sun";

            var methodBody = _mapper.Map<CreateNewCompanyBodyDto, CompanyBodyDto>(_companyDto);
            methodBody.Id = (int)_companyId;

            var result = ((((await _companyController.Change(methodBody) as ObjectResult)!)
                    .Value as CompanyMainResponseDto)!)
                .Unit;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.Name == _companyDto.Name);
                Assert.IsTrue((result.Address == _companyDto.Address)
                              && (result.NumberOfEmployees == _companyDto.NumberOfEmployees));
            });
        }

        [Test]
        [Author(nameof(Authors.Arif))]
        public async Task FindByNameTest()
        {
            var objectOkResult = await _companyController.FindByName(_companyDto.Name) as ObjectResult;
            var objectBadResult = await _companyController.FindByName("NotExistingCompany666") as ObjectResult;
            var expectedValue = objectOkResult.Value as CompanyFindByNameResponseDto;
            var expectedBadValue = objectBadResult.Value as CompanyFindByNameResponseDto;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(expectedValue.Unit.First().Name.Contains(_companyDto.Name));
                Assert.IsTrue(objectBadResult.GetType() == typeof(BadRequestObjectResult));
                Assert.IsTrue(expectedBadValue.Message != null);
            });


        }


    }
}