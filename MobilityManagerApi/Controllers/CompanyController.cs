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
    public class CompanyController : ControllerBase, IControllerBaseActions
    {

        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;




        public CompanyController(IMapper mapper, VoucherContext vContext)
        {

            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);

        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BaseCompanyBody>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BaseCompanyBody>>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> GetAll()

        {
            var response = new GeneralResponseDto<IQueryable<BaseCompanyBody>>();

            try
            {
                var companies = await _unitOfWork.Company.GetAll();
                response.Unit = companies.ProjectTo<BaseCompanyBody>(_mapper.ConfigurationProvider);
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
        [ProducesResponseType(typeof(GeneralResponseDto<BaseCompanyBody>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<BaseCompanyBody>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindById(int id)
        {
            var response = new GeneralResponseDto<BaseCompanyBody>();
            try
            {
                var companies = _unitOfWork.Company.Find(c => c.Id == id);
                response.Unit = await companies.ProjectTo<BaseCompanyBody>(_mapper.ConfigurationProvider).FirstAsync();
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
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BaseCompanyBody>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BaseCompanyBody>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindByName(string name)
        {
            var response = new GeneralResponseDto<IQueryable<BaseCompanyBody>>();
            try
            {
                var companies = _unitOfWork.Company.Find(c => c.Name == name);
                response.Unit = await Task.Run(() => companies.ProjectTo<BaseCompanyBody>(_mapper.ConfigurationProvider));
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
        [ProducesResponseType(typeof(GeneralResponseDto<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<int>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> FindIdByName(string name)
        {
            var response = new GeneralResponseDto<int>();
            try
            {
                var companies = _unitOfWork.Company.Find(c => c.Name.Contains(name));
                var company = await companies.ProjectTo<BaseCompanyBody>(_mapper.ConfigurationProvider).FirstAsync();
                response.Unit = company.Id;
                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }




        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponseDto<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<int>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNewCompany([FromBody] CompanyDto modelDto)
        {
            
            var response = new GeneralResponseDto<int>();

            if (modelDto.Name.IsNullOrEmpty())
            {
                response.Message = "Please, provide company name";
                return BadRequest(response);
            }

            var newAgency = _mapper.Map<Company>(modelDto);
            int id;
            try
            {
                id = await _unitOfWork.Company.AddAsync(newAgency);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }

            await _unitOfWork.Complete();
            response.Message = "New entity created";
            response.Unit = id;
            return Ok(response);
        }


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponseDto<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<bool>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromBody] BaseBody request)
        {
            var response = new GeneralResponseDto<bool>
            {
                Unit = await _unitOfWork.Company.RemoveAsync(request.Id)
            };
                if (!response.Unit)
            {
                response.Message = "Company not found";
                return BadRequest(response);
            }
            await _unitOfWork.Complete();
            response.Message = "Deleted";
            return Ok(response);
        }


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponseDto<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<bool>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategoryByCompanyIdAndCategoryId([FromBody] AddCategoryBody request)
        {
            var response = new GeneralResponseDto<bool>();
            if (request.CategoryId == null || request.CompanyId == null)
            {
                response.Message = "Please, provide company id and category id";
                response.Unit = false;
                return BadRequest(response);
            }

            try
            {
                await _unitOfWork.Company.AssignToCategory(request.CompanyId, request.CategoryId);
                await _unitOfWork.Complete();
                response.Unit = true;
                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                response.Unit = false;
                return BadRequest(response);
            }

        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponseDto<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<bool>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> AddCategoryByCompanyIdAndCategoryName([FromBody] AddCategoryBody request)
        {
            var response = new GeneralResponseDto<bool>();
            if (request.CategoryName == null || request.CompanyId == null)
            {
                response.Message = "Please, provide company id and category name";
                response.Unit = false;
                return BadRequest(response);
            }   
                
            try
            {
                await _unitOfWork.Company.AssignToCategory(request.CompanyId, request.CategoryName);
                await _unitOfWork.Complete();
                response.Unit = true;
                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                response.Unit = true;
                return BadRequest(response);
            }


        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponseDto<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<bool>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategoryByCompanyNameAndCategoryName([FromBody] AddCategoryBody request)
        {
            var response = new GeneralResponseDto<bool>();
            if (request.CategoryName == null || request.CompanyName == null)
            {
                response.Message = "Please, provide company name and category name";
                response.Unit = true;
                return BadRequest(response);
            }

            try
            {
                await _unitOfWork.Company.AssignToCategory(request.CompanyName, request.CategoryName);
                await _unitOfWork.Complete();
                response.Unit = true;
                return Ok(response);

            }
            catch (NullReferenceException exception)
            {
                response.Message = exception.Message;
                response.Unit = true;
                return BadRequest(response);
            }

        }


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponseDto<BaseCompanyBody>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<BaseCompanyBody>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Change([FromBody] BaseCompanyBody body)
        {
            Company company = new();
            var response = new GeneralResponseDto<BaseCompanyBody>();

            try
            {
                company = await _unitOfWork.Company.Find(c => c.Id == body.Id).FirstAsync();
                _unitOfWork.Company.Update(company);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
            }


            if (response.Message != null)
            {
                return BadRequest(response);
            }



            var listDtoProp = body.CompanyDto.GetType().GetProperties();
            foreach (var property in listDtoProp)
            {
                if (property.GetValue(body.CompanyDto) != null)
                {
                    _mapper.Map(body.CompanyDto, company);
                }

            }

            await _unitOfWork.Complete();
            response.Message = "Changes applied";
            response.Unit = _mapper.Map<Company, BaseCompanyBody>(company);
            return Ok(response);
        }

    }
}
