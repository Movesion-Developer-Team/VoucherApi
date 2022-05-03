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
using Microsoft.Win32.SafeHandles;
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
        [ProducesResponseType(typeof(CsvToDiscountCodesResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CsvToDiscountCodesResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CsvToDiscountCodes()
        {
            var response = new CsvToDiscountCodesResponseDto();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                if (file.Length > 0)
                {
                    var stream = new FileStream(new SafeFileHandle(), FileAccess.Write);
                    await file.CopyToAsync(stream);
                    var reader = new StreamReader(stream);
                    var csv = new CsvReader(reader, config);

                    response.UnassignedCollectionId = await
                        _unitOfWork.UnassignedDiscountCodeCollections.AddAsync(new UnassignedDiscountCodeCollection());

                    var codes = _mapper.ProjectTo<DiscountCode>(csv.GetRecords<CsvCodeDto>().AsQueryable());

                    await codes.ForEachAsync((code) => code.UnassignedCollectionId = response.UnassignedCollectionId);

                    await _unitOfWork.DiscountCode.AddRangeAsync(codes);
                    
                    response.Message = "Discounts are saved in Db without assignment to the player!";

                    return Ok(response);
                }

                response.Message = "Please provide a file";
                return BadRequest();
            }
            catch (Exception ex)
            {
                response.Message = $"Internal Server error: {ex.Message}";
                return BadRequest(response);
            }
        }

        //[AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        //[HttpPost]
        //[ProducesResponseType(typeof(CsvToDiscountCodesResponseDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(CsvToDiscountCodesResponseDto), StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> AssignCodesCollection 

    }
}
