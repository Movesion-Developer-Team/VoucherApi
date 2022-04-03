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
    [Route("[]")]
    public class CompanyController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        

        public CompanyController(IMapper mapper, VoucherContext vContext)
        {

            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);

        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost("/[action]")]
        
        public async Task<IActionResult> CreateNewCompany([FromBody]CompanyDto modelDto)
        {
            var newAgency = _mapper.Map<Company>(modelDto);
            await _unitOfWork.Company.AddAsync(newAgency);
            await _unitOfWork.Complete();
            return Ok();
        }

        


    }
}
