using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Domain;
using DTOs.BodyDtos;
using DTOs.ResponseDtos;
using Enum;
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
    public class VoucherController : ControllerBase, IControllerBaseActions
    {

        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;


        public VoucherController(IMapper mapper, VoucherContext vContext)
        {
            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);
        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(CreateNewEntityResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateNewEntityResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNewVoucher([FromBody] CreateNewVoucherBodyDto modelDto)
        {

            var response = new CreateNewEntityResponseDto();

            if (modelDto.Name.IsNullOrEmpty())
            {
                response.Message = "Please, provide voucher name";
                return BadRequest(response);
            }

            var newVoucher = _mapper.Map<Voucher>(modelDto);
            if (newVoucher == null)
            {
                response.Message = "Server side error: Object is not mapped, check mapping profile";
                return BadRequest(response);
            }
            int? id;
            try
            {
                id = await _unitOfWork.Voucher.AddAsync(newVoucher);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }

            await _unitOfWork.Complete();
            response.Message = "New entity created";
            response.Id = id;
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(VoucherMainResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(VoucherMainResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Change([FromBody] VoucherBodyDto body)
        {
            Voucher voucher = new();
            var response = new VoucherMainResponseDto();

            try
            {
                voucher = await _unitOfWork.Voucher.Find(c => c.Id == body.Id).FirstAsync();
                _unitOfWork.Voucher.Update(voucher);
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
                    _mapper.Map(body, voucher);
                }

            }

            await _unitOfWork.Complete();
            var idValue = listDtoProp.First(p => p.Name == "Id").GetValue(body);
            response.Message = idValue != null ? "Warning: changes applied, but new Id is not assigned, because it is forbidden on server side"
                : "Changes applied";
            response.Voucher = body;
            return Ok(response);
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllVouchersResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllVouchersResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var response = new GetAllVouchersResponseDto();
            try
            {
                var vouchers = await _unitOfWork.Voucher.GetAll();
                response.Vouchers = vouchers.ProjectTo<VoucherBodyDto>(_mapper.ConfigurationProvider);
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
        [ProducesResponseType(typeof(VoucherMainResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(VoucherMainResponseDto), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> FindById(int id)
        {
            var response = new VoucherMainResponseDto();
            try
            {
                var vouchers = _unitOfWork.Voucher.Find(c => c.Id == id);
                response.Voucher = await vouchers.ProjectTo<VoucherBodyDto>(_mapper.ConfigurationProvider).FirstAsync();
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
                IsDeleted = await _unitOfWork.Voucher.RemoveAsync(id)
            };
            if (!response.IsDeleted)
            {
                response.Message = "Voucher not found";
                return BadRequest(response);
            }
            await _unitOfWork.Complete();
            response.Message = "Deleted";
            return Ok(response);
        }
    }

}
