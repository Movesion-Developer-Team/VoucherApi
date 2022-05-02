using System.Globalization;
using AutoMapper;
using Core.Domain;
using CsvHelper;
using CsvHelper.Configuration;
using DTOs.BodyDtos;
using DTOs.MethodDto;
using DTOs.ResponseDtos;
using Enum;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using UserStoreLogic;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors]
    public class DiscountController : ControllerBase, IControllerBaseActions
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public DiscountController(IMapper mapper, VoucherContext vContext)
        {
            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);
        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UploadCsv([FromBody] UploadCsvBodyDto body)
        {
            var response = new BaseResponse();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            try
            {
                using var reader = new StreamReader(body.FilePath);
                using var csv = new CsvReader(reader, config);
                var recordsDto = csv.GetRecords<UploadCsvToDiscountDto>();
                recordsDto.AsParallel().ForAll(d=>d.PlayerId = body.PlayerId);
                recordsDto.AsParallel().ForAll(d => d.DiscountType = body.DiscountType);

                var records =
                    _mapper.ProjectTo<Discount>(recordsDto.ToList().AsQueryable());
                await _unitOfWork.Discount.AddRangeAsync(records);
                await _unitOfWork.Complete();
                response.Message = "Discounts are saved in Db";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            
        }

        
    }
}
