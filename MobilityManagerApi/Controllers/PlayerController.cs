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
        public async Task<IActionResult> CreateNewPlayer([FromBody] CreateNewPlayerBodyDto playerDto)
        {
            var response = new CreateNewEntityResponseDto();
            if (playerDto.ShortName.IsNullOrEmpty())
            {
                response.Message = "Please, provide player shortName";
                return BadRequest(response);
            }
            var newPlayer = _mapper.Map<Player>(playerDto);
            if (newPlayer == null)
            {
                response.Message = "Server side error: Object is not mapped, check mapping profile";
                return BadRequest(response);
            }
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

    }
}

