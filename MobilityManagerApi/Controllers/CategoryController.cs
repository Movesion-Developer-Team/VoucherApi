using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Core.Domain;
using DTOs;
using Enum;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MobilityManagerApi.Dtos.BodyDtos;
using MobilityManagerApi.Dtos.ResponseDtos;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using Persistence;
using UserStoreLogic;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly GeneralResponseDto _response = new();

        public CategoryController(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }



        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _response.Unit = await _unitOfWork.Category.GetAll();
                return Ok(_response);
            }
            catch (NullReferenceException ex)
            {
                _response.Message = ex.Message;
                return BadRequest(_response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> FindById([FromBody] BaseBody request)
        {
            try
            {
                var category = await _unitOfWork.Category.FindAsync(c => c.Id == request.Id);
                _response.Unit = category.First();
                return Ok(_response);
            }
            catch (NullReferenceException ex)
            {
                _response.Message = ex.Message;
                return BadRequest(_response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> Delete(BaseBody request)
        {
            var deleted = await _unitOfWork.Category.RemoveAsync(request.Id);
            if (!deleted)
            {
                _response.Message = "Category not found";
                return BadRequest(_response);
            }
            await _unitOfWork.Complete();
            _response.Message = "Deleted";
            return Ok(_response);
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> FindByName([FromBody] BaseCategoryBody request)
        {
            if (string.IsNullOrEmpty(request.CategoryDto.Name))
            {
                _response.Message = "Please, provide the category name";
                return BadRequest(_response);
            }

            try
            {
                var category = await _unitOfWork.Category.FindAsync(c => c.Name == request.CategoryDto.Name);
                _response.Unit = category.First();
                return Ok(_response);
            }
            catch (NullReferenceException ex)
            {
                _response.Message = ex.Message;
                return BadRequest(_response);
            }



        }


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> CreateNewCategory([FromQuery]CategoryDto category)
        {
            var newCategory = _mapper.Map<CategoryDto, Category>(category);
            await _unitOfWork.Category.AddAsync(newCategory);
            return Ok();
        }


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> Change([FromBody] BaseCategoryBody inputBody)
        {
            Category category = new();
            GeneralResponseDto response = new();
            try
            {
                category = _unitOfWork.Category.FindAsync(c => c.Id == inputBody.Id).Result.First();
                _unitOfWork.Category.Update(category);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
            }


            if (response.Message != null)
            {
                return BadRequest(response);
            }



            var listDtoProp = inputBody.CategoryDto.GetType().GetProperties();
            foreach (var property in listDtoProp)
            {
                if (property.GetValue(inputBody.CategoryDto) != null)
                {
                    _mapper.Map(inputBody.CategoryDto, category);
                }

            }

            await _unitOfWork.Complete();
            response.Message = "Changes applied";
            response.Unit = category;
            return Ok(response);
        }






    }
}
