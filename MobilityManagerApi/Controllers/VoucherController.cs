using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Domain;
using DTOs.BodyDtos;
using Enum;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobilityManagerApi.Dtos.ResponseDtos;
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


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BaseVoucherBody>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BaseVoucherBody>>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> GetAll()
        {
            var response = new GeneralResponseDto<IQueryable<BaseVoucherBody>>();
            try
            {
                var vouchers = await _unitOfWork.Voucher.GetAll();
                response.Unit = vouchers.ProjectTo<BaseVoucherBody>(_mapper.ConfigurationProvider);
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
        [ProducesResponseType(typeof(GeneralResponseDto<BaseVoucherBody>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<BaseVoucherBody>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> FindById([FromBody] BaseBody request)
        {
            var response = new GeneralResponseDto<BaseVoucherBody>();
            try
            {
                var vouchers = _unitOfWork.Voucher.Find(c => c.Id == request.Id);
                response.Unit = await vouchers.ProjectTo<BaseVoucherBody>(_mapper.ConfigurationProvider).FirstAsync();
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
        [ProducesResponseType(typeof(GeneralResponseDto<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<bool>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Delete(BaseBody request)
        {
            var response = new GeneralResponseDto<bool>
            {
                Unit = await _unitOfWork.Voucher.RemoveAsync(request.Id)
            };
            if (!response.Unit)
            {
                response.Message = "Voucher not found";
                return BadRequest(response);
            }
            await _unitOfWork.Complete();
            response.Message = "Deleted";
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BaseVoucherBody>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BaseVoucherBody>>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> FindIdByName(string name)
        {
            var response = new GeneralResponseDto<IQueryable<BaseVoucherBody>>();
            try
            {
                var vouchers = _unitOfWork.Voucher.Find(c => c.Name.Contains(name));
                response.Unit = await Task.Run(()=>vouchers.ProjectTo<BaseVoucherBody>(_mapper.ConfigurationProvider));
                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponseDto<BaseVoucherBody>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<BaseVoucherBody>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Change([FromBody] BaseVoucherBody body)
        {
            Voucher voucher = new();
            var response = new GeneralResponseDto<BaseVoucherBody>();

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



            var listDtoProp = body.VoucherDto.GetType().GetProperties();
            foreach (var property in listDtoProp)
            {
                if (property.GetValue(body.VoucherDto) != null)
                {
                    _mapper.Map(body.VoucherDto, voucher);
                }

            }

            await _unitOfWork.Complete();
            response.Message = "Changes applied";
            response.Unit = body;
            return Ok(response);
        }
    }

}
