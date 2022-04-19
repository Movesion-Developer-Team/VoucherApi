using AutoMapper;
using Core.Domain;
using DTOs;
using Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MobilityManagerApi.Dtos.BodyDtos;
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
        private readonly GeneralResponseDto _response = new();
         

        public PlayerController(IMapper mapper, VoucherContext vContext)
        {

            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _response.Unit = await _unitOfWork.Player.GetAll();
                return Ok(_response);
            }
            catch (NullReferenceException ex)
            {
                _response.Message = ex.Message;
                return BadRequest(_response);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> FindById(int id)
        {
            try
            {
                var player = await _unitOfWork.Player.FindAsync(c => c.Id == id);
                _response.Unit = player.First();
                return Ok(_response);
            }
            catch (NullReferenceException ex)
            {
                _response.Message = ex.Message;
                return BadRequest(_response);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> FindIdByName(string name)
        {

            try
            {
                _response.Unit = await _unitOfWork.Player.FindAsync(c => c.ShortName.Contains(name));
                return Ok(_response);
            }
            catch (NullReferenceException ex)
            {
                _response.Message = ex.Message;
                return BadRequest(_response);
            }
        }




        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerDto playerDto)
        {
            var newPlayer = _mapper.Map<PlayerDto, Player>(playerDto);
            if (newPlayer == null)
            {
                throw new NullReferenceException("Object is not mapped, check mapping profile");
            }

            var id = await _unitOfWork.Player.AddAsync(newPlayer);
            await _unitOfWork.Complete();
            _response.Message = "New entity created";
            _response.Unit = id;
            return Ok(_response);
        }


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] BaseBody request)
        {
            var deleted = await _unitOfWork.Player.RemoveAsync(request.Id);
            if (!deleted)
            {
                _response.Message = "Player not found";
                return BadRequest(_response);
            }
            await _unitOfWork.Complete();
            _response.Message = "Deleted";
            return Ok(_response);
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> Change([FromBody] BasePlayerBody requestBody)
        {
            Player player = new();
            
            try
            {
                player = _unitOfWork.Player.FindAsync(c => c.Id == requestBody.Id).Result.First();
                _unitOfWork.Player.Update(player);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
            }


            if (_response.Message != null)
            {
                return BadRequest(_response);
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
            _response.Message = "Changes applied";
            _response.Unit = player;
            return Ok(_response);
        }

    }
}

