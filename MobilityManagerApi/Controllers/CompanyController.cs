using AutoMapper;
using Core.Domain;
using DTOs;
using Enum;
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
    public class CompanyController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly GeneralResponseDto _response = new();




        public CompanyController(IMapper mapper, VoucherContext vContext)
        {

            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);

        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _response.Unit = await _unitOfWork.Company.GetAll();
                return Ok(_response);
            }
            catch (NullReferenceException ex)
            {
                _response.Message = ex.Message;
                return BadRequest(_response);
            }
            
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> FindById([FromBody]BaseBody request)
        {
            try
            {
                var company = await _unitOfWork.Company.FindAsync(c => c.Id == request.Id);
                _response.Unit = company.First();
                return Ok(_response);
            }
            catch(NullReferenceException ex)
            {
                _response.Message = ex.Message;
                return BadRequest(_response);
            }
            
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> FindByName([FromBody] BaseCompanyBody request)
        {
            if (string.IsNullOrEmpty(request.CompanyDto.Name))
            {
                _response.Message = "Please, provide the company name";
                return BadRequest(_response);
            }

            try
            {
                var company = await _unitOfWork.Company.FindAsync(c => c.Name == request.CompanyDto.Name);
                _response.Unit = company.First();
                return Ok(_response);
            }
            catch (NullReferenceException ex)
            {
                _response.Message = ex.Message;
                return BadRequest(_response);
            }


            
        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateNewCompany([FromBody] CompanyDto modelDto)
        {
            var newAgency = _mapper.Map<Company>(modelDto);
            await _unitOfWork.Company.AddAsync(newAgency);
            await _unitOfWork.Complete();
            return Ok();
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] BaseBody request)
        {
            var deleted = await _unitOfWork.Company.RemoveAsync(request.Id);
            if (!deleted)
            {
                _response.Message = "Company not found";
                return BadRequest(_response);
            }
            await _unitOfWork.Complete();
            _response.Message = "Deleted";
            return Ok(_response);
        }


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> AddCategoryByCompanyIdAndCategoryId([FromBody] AddCategoryBody request)
        {
            if (request.CategoryId == null || request.CompanyId == null)
            {
                _response.Message = "Please, provide company id and category id";
                return BadRequest(_response);
            }

            try
            {
                await _unitOfWork.Company.AssignToCategory(request.CompanyId, request.CategoryId);
                await _unitOfWork.Complete();
                return Ok();
            }
            catch (NullReferenceException ex)
            {
                _response.Message = ex.Message;
                return BadRequest(_response);
            }
            
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> AddCategoryByCompanyIdAndCategoryName([FromBody] AddCategoryBody request)
        {
            if (request.CategoryName == null || request.CompanyId == null)
            {
                _response.Message = "Please, provide company id and category name";
                return BadRequest(_response);
            }

            try
            {
                await _unitOfWork.Company.AssignToCategory(request.CompanyId, request.CategoryName);
                await _unitOfWork.Complete();
                return Ok();
            }
            catch(NullReferenceException ex)
            {
                _response.Message = ex.Message;
                return BadRequest(_response);
            }
            
            
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> AddCategoryByCompanyNameAndCategoryName([FromBody] AddCategoryBody request)
        {
            if (request.CategoryName == null || request.CompanyName == null)
            {
                _response.Message = "Please, provide company name and category name";
                return BadRequest(_response);
            }

            try
            {
                await _unitOfWork.Company.AssignToCategory(request.CompanyName, request.CategoryName);
                await _unitOfWork.Complete();
                return Ok();
            }
            catch (NullReferenceException exception)
            {
                _response.Message = exception.Message;
                return BadRequest(_response);
            }
            
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> Change([FromBody] BaseCompanyBody inputBody)
        {
            Company company = new();
            GeneralResponseDto response = new();
            try
            {
                company = _unitOfWork.Company.FindAsync(c => c.Id == inputBody.Id).Result.First();
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



            var listDtoProp = inputBody.CompanyDto.GetType().GetProperties();
            foreach (var property in listDtoProp)
            {
                if (property.GetValue(inputBody.CompanyDto) != null)
                {
                    _mapper.Map(inputBody.CompanyDto, company);
                }

            }

            await _unitOfWork.Complete();
            response.Message = "Changes applied";
            response.Unit = company;
            return Ok(response);
        }

    }
}
