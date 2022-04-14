//using AutoMapper;
//using Core.Domain;
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
//    public class VoucherController : ControllerBase, IControllerBaseActions
//    {

//        private readonly IMapper _mapper;
//        private readonly UnitOfWork _unitOfWork;
//        private readonly GeneralResponseDto _response = new();


//        [FromBody]
//        public BaseVoucherBody? RequestBody { get; set; }


//        public VoucherController(IMapper mapper, VoucherContext vContext)
//        {
//            _mapper = mapper;
//            _unitOfWork = new UnitOfWork(vContext);
//        }


//        [AuthorizeRoles(Role.SuperAdmin)]
//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            try
//            {
//                _response.Unit = await _unitOfWork.Voucher.GetAll();
//                return Ok(_response);
//            }
//            catch (NullReferenceException ex)
//            {
//                _response.Message = ex.Message;
//                return BadRequest(_response);
//            }
//        }

//        [AuthorizeRoles(Role.SuperAdmin)]
//        [HttpGet]
//        public async Task<IActionResult> FindById([FromBody] BaseBody request)
//        {
//            try
//            {
//                var voucher = await _unitOfWork.Voucher.FindAsync(c => c.Id == request.Id);
//                _response.Unit = voucher.First();
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
//            var deleted = await _unitOfWork.Voucher.RemoveAsync(request.Id);
//            if (!deleted)
//            {
//                _response.Message = "Voucher not found";
//                return BadRequest(_response);
//            }
//            await _unitOfWork.Complete();
//            _response.Message = "Deleted";
//            return Ok(_response);
//        }

//        [HttpGet]
//        public async Task<IActionResult> FindIdByName()
//        {
//            if (RequestBody == null)
//            {
//                _response.Message = "You did not provide a body";
//                return BadRequest(_response);
//            }
//            var name = RequestBody.VoucherDto.Name;

//            if (name == null)
//            {
//                _response.Message = "Please, provide the name";
//                return BadRequest(_response);
//            }

//            try
//            {
//                _response.Unit = await _unitOfWork.Voucher.FindAsync(c => c.Name.Contains(name));
//                return Ok(_response);
//            }
//            catch (NullReferenceException ex)
//            {
//                _response.Message = ex.Message;
//                return BadRequest(_response);
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> Change()
//        {
//            Voucher voucher = new();
//            if (RequestBody == null)
//            {
//                _response.Message = "You did not provide a body";
//                return BadRequest(_response);
//            }
//            try
//            {
//                voucher = _unitOfWork.Voucher.FindAsync(c => c.Id == RequestBody.Id).Result.First();
//                _unitOfWork.Voucher.Update(voucher);
//            }
//            catch (NullReferenceException ex)
//            {
//                _response.Message = ex.Message;
//            }


//            if (_response.Message != null)
//            {
//                return BadRequest(_response);
//            }



//            var listDtoProp = RequestBody.VoucherDto.GetType().GetProperties();
//            foreach (var property in listDtoProp)
//            {
//                if (property.GetValue(RequestBody.VoucherDto) != null)
//                {
//                    _mapper.Map(RequestBody.VoucherDto, voucher);
//                }

//            }

//            await _unitOfWork.Complete();
//            _response.Message = "Changes applied";
//            _response.Unit = voucher;
//            return Ok(_response);
//        }
//    }

//}
