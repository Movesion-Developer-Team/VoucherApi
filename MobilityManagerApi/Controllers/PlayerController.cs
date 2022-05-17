using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Domain;
using DTOs.BodyDtos;
using DTOs.ResponseDtos;
using Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using UserStoreLogic;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors]
    public class PlayerController : ControllerBase, IControllerBaseActions
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
         

        public PlayerController(IMapper mapper, VoucherContext vContext)
        {

            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);

        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(CreateNewEntityResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateNewEntityResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNewPlayer([FromBody] CreateNewPlayerBodyDto body)
        {
            var response = new CreateNewEntityResponseDto();
            if (body.ShortName.IsNullOrEmpty())
            {
                response.Message = "Please, provide player shortName";
                return BadRequest(response);
            }
            var newPlayer = _mapper.Map<Player>(body);
            if (newPlayer == null)
            {
                response.Message = "Server side error: Object is not mapped, check mapping profile";
                return BadRequest(response);
            }

            Category? category;
            try
            {
                category = await _unitOfWork.Category.Find(c => c.Id == body.CategoryId).FirstAsync();
            }
            catch(NullReferenceException ex)
            {
                response.Message = $"Category not found: {ex.Message}";
                return BadRequest(response);
            }
            
            newPlayer.Categories = new List<Category>();
            newPlayer.Categories.Add(category);
            DiscountType discountType;
            try
            {
                discountType = await _unitOfWork.Discount.FindDiscountType(body.DiscountTypeId);
            }
            catch(ArgumentNullException ex)
            {
                response.Message = $"Internal Server error: {ex.Message}";
                return BadRequest(response);
            }

            newPlayer.DiscountsTypes = new List<DiscountType>()
            {
                discountType
            };
            int? id;
            try
            {
                id = await _unitOfWork.Player.AddAsync(newPlayer);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            await _unitOfWork.Complete();
            response.Id = id;
            response.Message = "New entity created";
            return Ok(response);
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(PlayerMainResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PlayerMainResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Change([FromBody] PlayerBodyDto body)
        {
            Player player = new();
            var response = new PlayerMainResponseDto();

            try
            {
                player = await _unitOfWork.Player.Find(c => c.Id == body.Id).FirstAsync();
                _unitOfWork.Player.Update(player);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
            }


            if (response.Message != null)
            {
                return BadRequest(response);
            }



            var listDtoProp = body.GetType().GetProperties();
            foreach (var property in listDtoProp)
            {
                if (property.GetValue(body) != null)
                {
                    _mapper.Map(body, player);
                }

            }

            await _unitOfWork.Complete();

            var idValue = listDtoProp.First(p => p.Name == "Id").GetValue(body);
            response.Message = idValue != null ? "Warning: changes applied, but new Id is not assigned, because it is forbidden on server side"
                : "Changes applied";
            response.Player = body;
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllPlayersResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllPlayersResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var response = new GetAllPlayersResponseDto();

            try
            {
                var players = await _unitOfWork.Player.GetAll();
                response.Players = players.ProjectTo<PlayerBodyDto>(_mapper.ConfigurationProvider);
                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PlayerMainResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PlayerMainResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindById(int id)
        {
            var response = new PlayerMainResponseDto();

            try
            {
                var player = _unitOfWork.Player.Find(c => c.Id == id);
                response.Player = await player.ProjectTo<PlayerBodyDto>(_mapper.ConfigurationProvider).FirstAsync();
                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        [ProducesResponseType(typeof(PlayerFindByNameResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PlayerFindByNameResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindByName(string shortName)
        {
            var response = new PlayerFindByNameResponseDto();
            try
            {
                var players = _unitOfWork.Player.Find(c => c.ShortName == shortName);
                response.Players = await Task.Run(() => players.ProjectTo<PlayerBodyDto>(_mapper.ConfigurationProvider));
                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpDelete]
        [ProducesResponseType(typeof(DeleteResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DeleteResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = new DeleteResponseDto()
            {
                IsDeleted = await _unitOfWork.Player.RemoveAsync(id)
            };
            if (!response.IsDeleted)
            {
                response.Message = "Player not found";
                return BadRequest(response);
            }
            await _unitOfWork.Complete();
            response.Message = "Deleted";
            return Ok(response);
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignCategoryToPlayer([FromBody] AssignCategoryToPlayerBodyDto body)
        {
            var response = new BaseResponse();
            if (body.PlayerId == null)
            {
                response.Message = "Please provide Player Id";
            }

            if (body.CategoryId == null)
            {
                response.Message = "Please provide Category Id";
            }

            try
            {
                await _unitOfWork.Player.AssignCategoryToPlayer((int) body.PlayerId, (int) body.CategoryId);
                response.Message = "Done";
                return Ok(response);
            }
            catch(InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Message = $"Server internal error: {ex.Message}";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Server internal error: {ex.Message}";
                return BadRequest(response);
            }

        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCategoryFromPlayer([FromBody] DeleteCategoryFromPlayerBodyDto body)
        {
            var response = new BaseResponse();
            if (body.PlayerId == null)
            {
                response.Message = "Please provide Player Id";
                return BadRequest(response);
            }

            if (body.CategoryId == null)
            {
                response.Message = "Please provide Category Id";
                return BadRequest(response);

            }

            try
            {
                var deleted =
                    await _unitOfWork.Player.DeleteCategoryFromPlayer((int) body.PlayerId, (int) body.CategoryId);
                if (!deleted)
                {
                    response.Message = "Category is not assigned to the current Player";
                    return BadRequest(response);
                }

                response.Message = "Deleted";
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Message = ex.Message;
                return Ok(response);
            }



        }


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignDiscountTypeToPlayer([FromBody] AssignDiscountTypeToPlayerBodyDto body)
        {
            var response = new BaseResponse();
            try
            {
                await _unitOfWork.Player.AssignDiscountTypeToPlayer(body.PlayerId, body.DiscountTypeId);
                response.Message = "Done";
                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Unexpected server error: {ex.Message}";
                return BadRequest(response);

            }
        }


        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(GetAllDiscountTypesForPlayerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllDiscountTypesForPlayerResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllDiscountTypesForPlayer([FromQuery] int? playerId)
        {
            var response = new GetAllDiscountTypesForPlayerResponseDto();
            
            try
            {
                var discountTypes = await _unitOfWork.Player.GetAllDiscountTypesForPlayer(playerId);
                response.DiscountTypes = _mapper.ProjectTo<DiscountTypeBodyDto>(discountTypes);
                response.Message = "Done";
                return Ok(Response);
            }
            catch (ArgumentNullException ex)
            {
                response.Message =$"Internal server error: {ex.Message}";
                return BadRequest(response);
            }

        }

    }
}

