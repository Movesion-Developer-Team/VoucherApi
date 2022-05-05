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
using System.Text.Json;

namespace MobilityManagerApi.Tests
{
    [TestFixture(TestOf = typeof(CompanyController))]
    public class CompanyControllerTest : BaseTest
    {


        private CreateNewCompanyBodyDto _companyDto;
        private CreateNewPlayerBodyDto _playerDto;
        private CreateNewCategoryBodyDto _categoryDto;
        private int? _companyId;
        private int? _playerId;
        public int? _categoryId;

        [OneTimeSetUp]
        public async Task TestSetup()
        {
            _companyDto = new CreateNewCompanyBodyDto()
            {
                Name = "Hello",
                Address = "Moon",
                NumberOfEmployees = 7
            };
            _categoryDto = new CreateNewCategoryBodyDto
            {
                Name = "TestCategory",
                Description = "TestCategory description"
            };
            _categoryId = ((await _categoryController.CreateNewCategory(_categoryDto) as ObjectResult)
                    .Value as CreateNewEntityResponseDto)
                .Id;
            _playerDto = new CreateNewPlayerBodyDto
            {
                ShortName = "TestPlayer",
                FullName = "TestPlayer long name",
                CategoryId = _categoryId,
                PlayStoreLink = null,
                AppStoreLink = null,
                LinkDescription = null,
                Color = null
            };
            _companyId = ((await _companyController.CreateNewCompany(_companyDto) as ObjectResult)
                .Value as CreateNewEntityResponseDto)
                .Id;
            _playerId = ((await _playerController.CreateNewPlayer(_playerDto) as ObjectResult)
                    .Value as CreateNewEntityResponseDto)
                .Id;
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            _companyDto = null;
            await _playerController.Delete((int)_playerId);
            await _categoryController.Delete((int)_categoryId);
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

            Assert.Multiple(() =>
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

            var deleteFunctionTrueValue = ((((await _companyController.Delete(id) as ObjectResult)!)
                    .Value as DeleteResponseDto)!)
                .IsDeleted;
            var deleteFunctionFalseValue = ((((await _companyController.Delete(id) as ObjectResult)!)
                    .Value as DeleteResponseDto)!)
                .IsDeleted;

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
            await _companyController.Delete(1);
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
                Assert.IsTrue(resultsByDifferentConditions[0].Company.Name == _companyDto.Name);
                Assert.IsTrue(resultsByDifferentConditions[1].Company == null);
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
                .Company;

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
                Assert.IsTrue(expectedValue.Companies.First().Name.Contains(_companyDto.Name));
                Assert.IsTrue(objectBadResult.GetType() == typeof(BadRequestObjectResult));
                Assert.IsTrue(expectedBadValue.Message != null);
            });


        }

        [Test]
        [Author((nameof(Authors.Arif)))]
        public async Task AddPlayerToCompanyTest()
        {
            var body = new AddPlayerToCompanyBodyDto
            {
                PlayerId = _playerId,
                CompanyId = _companyId
            };
            var okResult = await _companyController.AddPlayerToCompany(body) as ObjectResult;
            body.PlayerId = _playerId + 1;
            var badResult = await _companyController.AddPlayerToCompany(body) as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.AreEqual(okResult.StatusCode, StatusCodes.Status200OK);
                Assert.AreEqual(badResult.StatusCode, StatusCodes.Status400BadRequest);
            });

        }

        [Test]
        [Author((nameof(Authors.Arif)))]
        public async Task GetAllPlayersTest()
        {

            var okResult = await _companyController.GetAllPlayersForOneCompany((int)_companyId) as ObjectResult;
            var players = (okResult.Value as GetAllPlayersForCurrentCompanyResponseDto).Players;
            Assert.Multiple(() =>
            {
                Assert.AreEqual(okResult.StatusCode, StatusCodes.Status200OK);
                Assert.IsTrue(players != null);
            });
        }

        [Test]
        [Author((nameof(Authors.Arif)))]
        public async Task GetAllCompaniesWithPlayersTest()
        {
            var getIdFromResponse = (IActionResult result) =>
                ((result as ObjectResult).Value as CreateNewEntityResponseDto).Id;
            var categoryBody = new CreateNewCategoryBodyDto
            {
                Name = "TestCategory",
                Description = "TestPlayer"
            };
            var categoryId = getIdFromResponse(await _categoryController.CreateNewCategory(categoryBody));
            var playerBody = new CreateNewPlayerBodyDto
            {
                ShortName = "TestPlayer",
                FullName = "TestPlayer",
                CategoryId = categoryId,
                PlayStoreLink = null,
                AppStoreLink = null,
                LinkDescription = null,
                Color = null
            };
            var playerId = getIdFromResponse(await _playerController.CreateNewPlayer(playerBody));
            var companyId = getIdFromResponse(await _companyController.CreateNewCompany(new CreateNewCompanyBodyDto
            {
                Name = "TestCompany",
                Address = "TestAddress",
                NumberOfEmployees = 123
            }));
            _companyController.AddPlayerToCompany(new AddPlayerToCompanyBodyDto()
                {CompanyId = companyId, PlayerId = playerId});

            var okResult = _companyController.GetAllCompaniesWithPlayers() as ObjectResult;
            var companies = (okResult.Value as GetAllCompaniesWithPlayersResponseDto);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
                Assert.IsTrue(companies.Companies.Any(c=>c.Players.Any()));
                string jsonCompanies = JsonSerializer.Serialize(companies);
                TestContext.Out.WriteLine(jsonCompanies);
            });
        }

    }
}