using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using DTOs.BodyDtos;
using DTOs.ResponseDtos;
using Enum.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace MobilityManagerApi.Tests
{
    internal class PlayerControllerTest : BaseTest
    {
        private CreateNewPlayerBodyDto _playerDto;
        private CreateNewCategoryBodyDto _categoryDto;
        private int? _playerId;
        private int? _categoryId;

        [OneTimeSetUp]
        public async Task TestSetup()
        {
            
            _categoryDto = new CreateNewCategoryBodyDto
            {
                Name = "TestCategory",
                Description = null
            };

            _categoryId =
                ((await _categoryController.CreateNewCategory(_categoryDto) as ObjectResult).Value as
                    CreateNewEntityResponseDto).Id;

            _playerDto = new CreateNewPlayerBodyDto()
            {
                ShortName = "SpaceX",
                FullName = "SpaceX",
                AppStoreLink = "https://lifeonthemars.int",
                LinkDescription = "Coming soon...",
                Color = "Green",
                CategoryId = _categoryId

            };

            _playerId = ((await _playerController.CreateNewPlayer(_playerDto) as ObjectResult)
                    .Value as CreateNewEntityResponseDto)
                .Id;
        }


        [OneTimeTearDown]
        public async Task TearDown()
        {
            _playerDto = null;
            await _categoryController.Delete((int) _categoryId);
            await _context.DisposeAsync();
        }

        [Test]
        [Author(nameof(TestAuthors.Arif))]
        [Order(1)]
        public async Task CreatePlayerTest()
        {
            _playerDto.ShortName = null;

            var result = await _playerController.CreateNewPlayer(_playerDto) as ObjectResult;
            if (result == null)
            {
                Assert.Fail("Method did not return ObjectResult");
            }

            if (result.StatusCode != StatusCodes.Status400BadRequest)
            {
                Assert.Fail("Forbidden to allow nullable for the name property");
            }

            _playerDto.ShortName = "Tesla";
            result = await _playerController.CreateNewPlayer(_playerDto) as ObjectResult;

            var resultedObject = result.Value as CreateNewEntityResponseDto;
            if (resultedObject == null)
            {
                Assert.Fail("ObjectResult does not contain entity id");
            }

            
            Assert.Multiple(() =>
            {
                Assert.IsInstanceOf(typeof(Int32), resultedObject.Id);
                
            });
            
        }

        [Test]
        [Author(nameof(TestAuthors.Arif))]
        [Order(2)]
        public async Task DeleteTest()
        {

            var id = 2;
            
            var deleteFunctionTrueValue = ((((await _playerController.Delete(id) as ObjectResult)!)
                    .Value as DeleteResponseDto)!)
                .IsDeleted;
            var deleteFunctionFalseValue = ((((await _playerController.Delete(id) as ObjectResult)!)
                    .Value as DeleteResponseDto)!)
                .IsDeleted;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(deleteFunctionTrueValue);
                Assert.IsTrue(!deleteFunctionFalseValue);
            });


        }

        [Test]
        [Author(nameof(TestAuthors.Arif))]
        [Order(3)]
        public async Task GetAllTest()
        {
            await _playerController.Delete(1);
            var players = await _playerController.GetAll() as ObjectResult;
            if (players.StatusCode != StatusCodes.Status400BadRequest)
            {
                Assert.Fail("Method should return BadRequest, when there is no entities in DB");
            }


            await _playerController.CreateNewPlayer(_playerDto);

            players = await _playerController.GetAll() as ObjectResult;
            if (players.StatusCode != StatusCodes.Status200OK)
            {
                Assert.Fail("Method did not return 200 response");
            }

            Assert.IsInstanceOf(typeof(GetAllPlayersResponseDto), players.Value);

        }

        [Test]
        [Author(nameof(TestAuthors.Arif))]
        [Order(4)]
        public async Task FindByIdTest()
        {
            _playerId = 3;
            List<PlayerMainResponseDto> resultsByDifferentConditions = new();
            List<int?> playerIds = new()
            {
                _playerId,
                2
            };
            ObjectResult objectResult;
            PlayerMainResponseDto expectedValue;
            foreach (var id in playerIds)
            {
                objectResult = await _playerController.FindById((int)id) as ObjectResult;
                expectedValue = objectResult.Value as PlayerMainResponseDto;
                resultsByDifferentConditions.Add(expectedValue);

            }

            Assert.Multiple(() =>
            {
                Assert.IsTrue(resultsByDifferentConditions[0].Player.ShortName == _playerDto.ShortName);
                Assert.IsTrue(resultsByDifferentConditions[1].Player == null);
                Assert.IsTrue(resultsByDifferentConditions[1].Message != null);
            });


        }

        [Test]
        [Author(nameof(TestAuthors.Arif))]
        [Order(5)]
        public async Task ChangeTest()
        {
            _playerDto.ShortName = "sun";

            var methodBody = _mapper.Map<CreateNewPlayerBodyDto, PlayerWithCategoriesAndDiscountTypesBodyDto>(_playerDto);
            methodBody.Id = (int)_playerId;


            var result = ((((await _playerController.Change(methodBody) as ObjectResult)!)
                    .Value as PlayerMainResponseDto)!)
                .Player;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.ShortName == _playerDto.ShortName);
                Assert.IsTrue((result.LinkDescription == _playerDto.LinkDescription)
                              && (result.AppStoreLink == _playerDto.AppStoreLink));
            });
        }

        [Test]
        [Author(nameof(TestAuthors.Arif))]
        public async Task FindByNameTest()
        {
            var objectOkResult = await _playerController.FindByName(_playerDto.ShortName) as ObjectResult;
            var objectBadResult = await _playerController.FindByName("NotExistingCategory666") as ObjectResult;
            var expectedValue = objectOkResult.Value as PlayerFindByNameResponseDto;
            var expectedBadValue = objectBadResult.Value as PlayerFindByNameResponseDto;

            Assert.Multiple(() =>
            {
                Assert.IsTrue(expectedValue.Players.First().ShortName.Contains(_playerDto.ShortName));
                Assert.IsTrue(objectBadResult.GetType() == typeof(BadRequestObjectResult));
                Assert.IsTrue(expectedBadValue.Message != null);
            });

        }

        [Test]
        [Author(nameof(TestAuthors.Arif))]
        public async Task AssignCategoryToPlayerTest()
        {

            var categoryId = GetIdFromResponse(await _categoryController.CreateNewCategory(_categoryDto));
            var playerId = GetIdFromResponse(await _playerController.CreateNewPlayer(_playerDto));
            var assignmentBody = new AssignCategoryToPlayerBodyDto
            {
                CategoryId = categoryId,
                PlayerId = playerId
            };

            await _playerController.AssignCategoryToPlayer(assignmentBody);
            var changedPlayer =  await _context.Set<Player>().FindAsync(playerId);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(changedPlayer?.Categories != null && changedPlayer.Categories.Any(c=>c.Name == _categoryDto.Name));
            });
        }

    }
}
