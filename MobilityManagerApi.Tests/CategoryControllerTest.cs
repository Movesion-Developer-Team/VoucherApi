using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTOs.BodyDtos;
using DTOs.ResponseDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace MobilityManagerApi.Tests
{
    internal class CategoryControllerTest : BaseTest
    {
        private CreateNewCategoryBodyDto? _categoryDto;
        private CreateNewPlayerBodyDto? _playerDto;
        private int? _categoryId;
        private int? _playerId;

        [OneTimeSetUp]
        public async Task TestSetUp()
        {
           

            _categoryDto = new ()
            {
                Name = "Planet Colonization",
                Description =
                    "This Planet will be colonized, because humans are not able to take care about their planet. "
            };
            

            _categoryId = ((((await _categoryController.CreateNewCategory(_categoryDto) as ObjectResult)!)
                    .Value as CreateNewEntityResponseDto)!)
                .Id;

            _playerDto = new()
            {
                ShortName = "TestPlayer",
                FullName = "TestPlayer",
                CategoryId = _categoryId,
                PlayStoreLink = null,
                AppStoreLink = null,
                LinkDescription = null,
                Color = "black"
            };
            _playerId = ((((await _playerController.CreateNewPlayer(_playerDto) as ObjectResult)!)
                    .Value as CreateNewEntityResponseDto)!)
                .Id;
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            _categoryDto = null;
            _playerDto = null;
            await _context.DisposeAsync();
        }

        [Test]
        [Author(nameof(Authors.Arif))]
        [Order(1)]
        public async Task CreateNewCategoryTest()
        {
            _categoryDto.Name = null;
            ObjectResult result;
            try
            {
                result = await _categoryController.CreateNewCategory(_categoryDto) as ObjectResult;
                if (result == null)
                {
                    Assert.Fail("Method did not return ObjectResult");
                }

                if (result.StatusCode != StatusCodes.Status400BadRequest)
                {
                    Assert.Fail("Forbidden to allow nullable for the name property");
                }

                _categoryDto.Name = "Hello";
                result = await _categoryController.CreateNewCategory(_categoryDto) as ObjectResult;

                var resultedObject = result.Value as CreateNewEntityResponseDto;
                if (resultedObject == null)
                {
                    Assert.Fail("ObjectResult does not contain entity id");
                }


                Assert.IsInstanceOf(typeof(Int32), resultedObject.Id);

            }
            catch(DbUpdateException ex)
            {
                Assert.Fail("Method should return BadRequest when name is not given");
            }
            

        }

        [Test]
        [Author(nameof(Authors.Arif))]
        [Order(2)]
        public async Task DeleteTest()
        {
            
            var id = 2;
            
            var deleteFunctionTrueValue = ((((await _categoryController.Delete(id) as ObjectResult)!)
                    .Value as DeleteResponseDto)!)
                .IsDeleted;
            var deleteFunctionFalseValue = ((((await _categoryController.Delete(id) as ObjectResult)!)
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
            await _categoryController.Delete(1);
            var categories = await _categoryController.GetAll() as ObjectResult;
            if (categories.StatusCode != StatusCodes.Status400BadRequest)
            {
                Assert.Fail("Method should return BadRequest, when there is no entities in DB");
            }


            await _categoryController.CreateNewCategory(_categoryDto);

            categories = await _categoryController.GetAll() as ObjectResult;
            if (categories.StatusCode != StatusCodes.Status200OK)
            {
                Assert.Fail("Method did not return 200 response");
            }

            Assert.IsInstanceOf(typeof(GetAllCategoriesResponseDto), categories.Value);

        }

        [Test]
        [Author(nameof(Authors.Arif))]
        [Order(4)]
        public async Task FindByIdTest()
        {
            _categoryId = 3;
            List<CategoryMainResponseDto> resultsByDifferentConditions = new();
            List<int?> categoryIds = new()
            {
                _categoryId,
                2
            };
            ObjectResult objectResult;
            CategoryMainResponseDto expectedValue;
            foreach (var id in categoryIds)
            {
                objectResult = await _categoryController.FindById((int)id) as ObjectResult;
                expectedValue = objectResult.Value as CategoryMainResponseDto;
                resultsByDifferentConditions.Add(expectedValue);

            }

            Assert.Multiple(() =>
            {
                Assert.IsTrue(resultsByDifferentConditions[0].Category.Name == _categoryDto.Name);
                Assert.IsTrue(resultsByDifferentConditions[1].Category == null);
                Assert.IsTrue(resultsByDifferentConditions[1].Message != null);
            });


        }

        [Test]
        [Author(nameof(Authors.Arif))]
        [Order(5)]
        public async Task ChangeTest()
        {
            _categoryDto.Name = "Parallel Universe";

            var methodBody = _mapper.Map<CreateNewCategoryBodyDto, CategoryBodyDto>(_categoryDto);
            methodBody.Id = (int)_categoryId;


            var result = ((((await _categoryController.Change(methodBody) as ObjectResult)!)
                    .Value as CategoryMainResponseDto)!)
                .Category;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.Name == _categoryDto.Name);
                Assert.IsTrue(result.Description == _categoryDto.Description);
            });
        }

        [Test]
        [Author(nameof(Authors.Arif))]
        public async Task FindByNameTest()
        {
            var objectOkResult = await _categoryController.FindByName(_categoryDto.Name) as ObjectResult;
            var objectBadResult = await _categoryController.FindByName("NotExistingCategory666") as ObjectResult;
            var expectedValue = objectOkResult.Value as CategoryFindByNameResponseDto;
            var expectedBadValue = objectBadResult.Value as CategoryFindByNameResponseDto;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(expectedValue.Categories.First().Name.Contains(_categoryDto.Name));
                Assert.IsTrue(objectBadResult.GetType() == typeof(BadRequestObjectResult));
                Assert.IsTrue(expectedBadValue.Message != null);
            });

        }

        [Test]
        [Author(nameof(Authors.Arif))]
        public async Task GetAllCategoriesForPlayerTest()
        {
            var categoryDto = new CreateNewCategoryBodyDto()
            {
                Name = "Planet Colonization",
                Description =
                    "This Planet will be colonized, because humans are not able to take care about their planet. "
            };


            var categoryId = ((((await _categoryController.CreateNewCategory(_categoryDto) as ObjectResult)!)
                    .Value as CreateNewEntityResponseDto)!)
                .Id;

            var playerDto = new CreateNewPlayerBodyDto()
            {
                ShortName = "TestPlayer",
                FullName = "TestPlayer",
                CategoryId = _categoryId,
                PlayStoreLink = null,
                AppStoreLink = null,
                LinkDescription = null,
                Color = "black"
            };
            var playerId = ((((await _playerController.CreateNewPlayer(playerDto) as ObjectResult)!)
                    .Value as CreateNewEntityResponseDto)!)
                .Id;

            var body = new GetAllCategoriesForPlayerBodyDto
            {
                PlayerId = (int)playerId
            };




            var result = await _categoryController.GetAllCategoriesForPlayer(body) as ObjectResult;

            var categories = (result.Value as GetAllCategoriesForPlayerResponseDto).Categories;
            
            Assert.Multiple(() =>
            {
                Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
                Assert.AreEqual((int?)categories.First().Id, _categoryId);
            });
        }
    }
}
