using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Domain;
using DTOs;
using DTOs.BodyDtos;
using Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobilityManagerApi.Dtos.ResponseDtos;
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

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BasePlayerBody>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BasePlayerBody>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var response = new GeneralResponseDto<IQueryable<BasePlayerBody>>();

            try
            {
                var players = await _unitOfWork.Player.GetAll();
                response.Unit = players.ProjectTo<BasePlayerBody>(_mapper.ConfigurationProvider);
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
        [ProducesResponseType(typeof(GeneralResponseDto<BasePlayerBody>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<BasePlayerBody>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindById(int id)
        {
            var response = new GeneralResponseDto<BasePlayerBody>();

            try
            {
                var player = _unitOfWork.Player.Find(c => c.Id == id);
                response.Unit = await player.ProjectTo<BasePlayerBody>(_mapper.ConfigurationProvider).FirstAsync();
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
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BasePlayerBody>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BasePlayerBody>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindIdByName(string name)
        {

            var response = new GeneralResponseDto<IQueryable<BasePlayerBody>>();

            try
            {
                var player = _unitOfWork.Player.Find(c => c.ShortName.Contains(name));
                response.Unit = await Task.Run(() => player.ProjectTo<BasePlayerBody>(_mapper.ConfigurationProvider));
                return Ok(response);
                
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }



        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponseDto<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<int>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerDto playerDto)
        {
            var response = new GeneralResponseDto<int>();
            if (playerDto.ShortName.IsNullOrEmpty())
            {
                response.Message = "Please, provide player name";
                return BadRequest(response);
            }
            var newPlayer = _mapper.Map<PlayerDto, Player>(playerDto);
            if (newPlayer == null)
            {
                response.Message = "Server side error: Object is not mapped, check mapping profile";
                return BadRequest(response);
            }

            response.Unit = await _unitOfWork.Player.AddAsync(newPlayer);
            await _unitOfWork.Complete();
            response.Message = "New entity created";
            return Ok(response);
        }


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpDelete]
        [ProducesResponseType(typeof(GeneralResponseDto<PlayerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<PlayerDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromBody] BaseBody request)
        {
            var response = new GeneralResponseDto<PlayerDto>();
            var deleted = await _unitOfWork.Player.RemoveAsync(request.Id);
            if (!deleted)
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
        [ProducesResponseType(typeof(GeneralResponseDto<BasePlayerBody>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<BasePlayerBody>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Change([FromBody] BasePlayerBody requestBody)
        {
            Player player = new();
            var response = new GeneralResponseDto<BasePlayerBody>();

            try
            {
                player = await _unitOfWork.Player.Find(c => c.Id == requestBody.Id).FirstAsync();
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



            var listDtoProp = requestBody.PlayerDto.GetType().GetProperties();
            foreach (var property in listDtoProp)
            {
                if (property.GetValue(requestBody.PlayerDto) != null)
                {
                    _mapper.Map(requestBody.PlayerDto, player);
                }

            }

            await _unitOfWork.Complete();
            response.Message = "Changes applied";
            response.Unit = requestBody;
            return Ok(response);
        }

    }
}

