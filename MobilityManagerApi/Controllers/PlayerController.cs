//using AutoMapper;
//using Core.Domain;
//using DTOs;
//using Enum;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;
//using MobilityManagerApi.Dtos.BodyDtos;
//using MobilityManagerApi.Dtos.ResponseDtos;
//using Persistence;
//using UserStoreLogic;

//namespace MobilityManagerApi.Controllers
//{
//    [ApiController]
//    [Route("[controller]/[action]")]
//    [EnableCors]
//    public class PlayerController : ControllerBase, IControllerBaseActions
//    {
//        private readonly IMapper _mapper;
//        private readonly UnitOfWork _unitOfWork;
//        private readonly GeneralResponseDto _response = new();

//        [FromBody]
//        public BasePlayerBody? RequestBody { get; set; }

//        public PlayerController(IMapper mapper, VoucherContext vContext)
//        {

//            _mapper = mapper;
//            _unitOfWork = new UnitOfWork(vContext);

//        }

//        [AuthorizeRoles(Role.SuperAdmin)]
//        [HttpPost]
//        public async Task<IActionResult> CreatePlayer([FromBody] PlayerDto playerDto)
//        {
//            var newPlayer = _mapper.Map<PlayerDto, Player>(playerDto);
//            if (newPlayer == null)
//            {
//                throw new NullReferenceException("Object is not mapped, check mapping profile");
//            }

//            await _unitOfWork.Player.AddAsync(newPlayer);
//            await _unitOfWork.Complete();
//            return Ok();
//        }

//        [AuthorizeRoles(Role.SuperAdmin)]
//        [HttpPost]
//        public async Task<IActionResult> DeletePlayer([FromBody] BaseBody request)
//        {

//            var deleted = await _unitOfWork.Player.RemoveAsync(request.Id);
//            if (!deleted)
//            {
//                throw new NullReferenceException("Player not found");
//            }

//            await _unitOfWork.Complete();
//            return Ok();
//        }


//        [AuthorizeRoles(Role.SuperAdmin)]
//        [HttpPost]
//        public async Task<IActionResult> GetAll()
//        {
//            try
//            {
//                _response.Unit = await _unitOfWork.Player.GetAll();
//                return Ok(_response);
//            }
//            catch (NullReferenceException ex)
//            {
//                _response.Message = ex.Message;
//                return BadRequest(_response);
//            }
//        }

//        [AuthorizeRoles(Role.SuperAdmin)]
//        [HttpPost]
//        public async Task<IActionResult> FindById([FromBody] BaseBody request)
//        {
//            try
//            {
//                var player = await _unitOfWork.Player.FindAsync(c => c.Id == request.Id);
//                _response.Unit = player.First();
//                return Ok(_response);
//            }
//            catch (NullReferenceException ex)
//            {
//                _response.Message = ex.Message;
//                return BadRequest(_response);
//            }
//        }

//        [AuthorizeRoles(Role.SuperAdmin)]
//        [HttpPost]
//        public async Task<IActionResult> Delete(BaseBody request)
//        {
//            var deleted = await _unitOfWork.Player.RemoveAsync(request.Id);
//            if (!deleted)
//            {
//                _response.Message = "Player not found";
//                return BadRequest(_response);
//            }
//            await _unitOfWork.Complete();
//            _response.Message = "Deleted";
//            return Ok(_response);
//        }

//        [AuthorizeRoles(Role.SuperAdmin)]
//        [HttpPost]
//        public async Task<IActionResult> FindIdByName()
//        {
//            if (RequestBody == null)
//            {
//                _response.Message = "You did not provide a body";
//                return BadRequest(_response);
//            }
//            var name = RequestBody.PlayerDto.ShortName;

//            if (name == null)
//            {
//                _response.Message = "Please, provide the name";
//                return BadRequest(_response);
//            }

//            try
//            {
//                _response.Unit = await _unitOfWork.Player.FindAsync(c => c.ShortName.Contains(name));
//                return Ok(_response);
//            }
//            catch (NullReferenceException ex)
//            {
//                _response.Message = ex.Message;
//                return BadRequest(_response);
//            }
//        }

//        [AuthorizeRoles(Role.SuperAdmin)]
//        [HttpPost]
//        public async Task<IActionResult> Change()
//        {
//            Player player = new();
//            if (RequestBody == null)
//            {
//                _response.Message = "You did not provide a body";
//                return BadRequest(_response);
//            }
//            try
//            {
//                player = _unitOfWork.Player.FindAsync(c => c.Id == RequestBody.Id).Result.First();
//                _unitOfWork.Player.Update(player);
//            }
//            catch (NullReferenceException ex)
//            {
//                _response.Message = ex.Message;
//            }


//            if (_response.Message != null)
//            {
//                return BadRequest(_response);
//            }



//            var listDtoProp = RequestBody.PlayerDto.GetType().GetProperties();
//            foreach (var property in listDtoProp)
//            {
//                if (property.GetValue(RequestBody.PlayerDto) != null)
//                {
//                    _mapper.Map(RequestBody.PlayerDto, player);
//                }

//            }

//            await _unitOfWork.Complete();
//            _response.Message = "Changes applied";
//            _response.Unit = player;
//            return Ok(_response);
//        }

//    }
//}

