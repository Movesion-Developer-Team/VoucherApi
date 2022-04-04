using AutoMapper;
using Core.Domain;
using DTOs;
using Enum;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using UserStoreLogic;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;


        public PlayerController(IMapper mapper, VoucherContext vContext)
        {

            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);

        }

        [AuthorizeRolesAttribute(Role.SuperAdmin)]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreatePlayer(PlayerDto playerDto)
        {
            var newPlayer = _mapper.Map<PlayerDto, Player>(playerDto);
            if (newPlayer == null)
            {
                throw new NullReferenceException("Object is not mapped, check mapping profile");
            }

            await _unitOfWork.Player.AddAsync(newPlayer);
            await _unitOfWork.Complete();
            return Ok();
        }

        [AuthorizeRolesAttribute(Role.SuperAdmin)]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> DeletePlayer(int playerId)
        { 
            
            var deleted = await _unitOfWork.Player.RemoveAsync(playerId);
            if(!deleted)
            {
                throw new NullReferenceException("Player not found");
            }

            await _unitOfWork.Complete();
            return Ok();
        }


    }
}
