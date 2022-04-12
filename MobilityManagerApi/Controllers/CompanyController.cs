using System.Reflection;
using System.Reflection.Emit;
using AutoMapper;
using Core.Domain;
using DTOs;
using Enum;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update;
using Persistence;
using UserStoreLogic;
using UserStoreLogic.DTOs;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    [Route("[]")]
    [EnableCors]
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
        public async Task<IActionResult> GetAllCompanies()
        {
           var allCompanies = await _unitOfWork.Company.GetAll();
            return Ok(allCompanies);
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost("/[action]")]
        public async Task<IActionResult> FindCompanyById(int id)
        {
            var company = await _unitOfWork.Company.FindAsync(c => c.Id == id);
            return Ok(company.First());
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost("/[action]")]
        public async Task<IActionResult> FindCompanyByName(string companyName)
        {
            var company = await _unitOfWork.Company.FindAsync(c => c.Name == companyName);
            return Ok(company.First());
        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpPost("/[action]")]
        public async Task<IActionResult> CreateNewCompany([FromBody]CompanyDto modelDto)
        {
            var newAgency = _mapper.Map<Company>(modelDto);
            await _unitOfWork.Company.AddAsync(newAgency);
            await _unitOfWork.Complete();
            return Ok();
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost("/[action]")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var deleted = await _unitOfWork.Company.RemoveAsync(id);
            if (!deleted)
            {
                throw new NullReferenceException("CompanyDto not found");
            }
            await _unitOfWork.Complete();
            return Ok(deleted);
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost("/[action]")]
        public async Task<IActionResult> ChangeCompanyName(int id, string name)
        {
            await _unitOfWork.Company.ChangeCompanyName(id, name);
            await _unitOfWork.Complete();
            return Ok();
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost("/[action]")]
        public async Task<IActionResult> ChangeCompanyContactDate(int companyId, DateTime newDate)
        {
            await _unitOfWork.Company.ChangeCompanyContactDate(companyId, newDate);
            await _unitOfWork.Complete();
            return Ok();
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost("/[action]")]
        public async Task<IActionResult> AddToCategoryByCompanyIdAndCategoryId(int companyId, int categoryId)
        {
            await _unitOfWork.Company.AssignToCategory(companyId, categoryId);
            await _unitOfWork.Complete();
            return Ok();
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost("/[action]")]
        public async Task<IActionResult> AddToCategoryByCompanyIdAndCategoryName(int companyId, string categoryName)
        {
            await _unitOfWork.Company.AssignToCategory(companyId, categoryName);
            await _unitOfWork.Complete();
            return Ok();
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost("/[action]")]
        public async Task<IActionResult> AddToCategoryByCompanyNameAndCategoryName(string companyName,
            string categoryName)
        {
            await _unitOfWork.Company.AssignToCategory(companyName, categoryName);
            await _unitOfWork.Complete();
            return Ok();
        }

        //[AuthorizeRoles(Role.SuperAdmin)]
        //[HttpPost("/[action]")]
        //public async Task<IActionResult> Change([FromBody] ChangeCompanyBody inputBody)
        //{
        //    Company company = new();
        //    GeneralResponseDto response = new GeneralResponseDto();
        //    try
        //    {
        //        company = _unitOfWork.Company.FindAsync(c => c.Id == inputBody.Id).Result.First();
        //        _unitOfWork.Company.Update(company);
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //        response.Message = ex.Message;
        //    }
            

        //    if (response.Message != null)
        //    {
        //        return BadRequest(response);
        //    }
            


        //    var listDtoProp = inputBody.CompanyDto.GetType().GetProperties();
        //    foreach (var property in listDtoProp)
        //    {
        //        if (property.GetValue(inputBody.CompanyDto) != null)
        //        {
        //            //PropertyInfo companyProperty;
        //            //try
        //            //{
        //            //    companyProperty = listCoreProp.First(prop => prop.Name == property.Name);

        //            //}
        //            //catch (ArgumentNullException)
        //            //{
        //            //    return BadRequest("Check mapper configuration for the current entity on backend");
        //            //}
        //            //catch (InvalidCastException exception)
        //            //{
        //            //    return BadRequest(exception.Message);
        //            //}

                    
                        
        //            _mapper.Map(inputBody.CompanyDto, company);

        //        }

        //    }

        //    await _unitOfWork.Complete();
        //    response.Message = "Changes applied";
        //    response.Unit = company;
        //    return Ok(response);
        //}

    }
}
