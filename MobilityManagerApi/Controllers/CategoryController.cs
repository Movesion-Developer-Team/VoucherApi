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
    public class CategoryController : ControllerBase, IControllerBaseActions
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;


        public CategoryController(IMapper mapper, VoucherContext vContext)
        {
            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);
        }



        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpGet]
        [ProducesResponseType(typeof(GeneralResponseDto<BaseCompanyBody>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<BaseCompanyBody>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var response = new GeneralResponseDto<IQueryable<BaseCategoryBody>>();
            try
            {
                var categories = await _unitOfWork.Category.GetAll();
                response.Unit = categories.ProjectTo<BaseCategoryBody>(_mapper.ConfigurationProvider);
                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpGet]
        [ProducesResponseType(typeof(GeneralResponseDto<BaseCompanyBody>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<BaseCompanyBody>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindById([FromBody] BaseBody request)
        {
            var response = new GeneralResponseDto<BaseCategoryBody>();
            try
            {
                var categories = _unitOfWork.Category.Find(c => c.Id == request.Id);
                response.Unit = await categories.ProjectTo<BaseCategoryBody>(_mapper.ConfigurationProvider).FirstAsync();
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
                Unit = await _unitOfWork.Category.RemoveAsync(request.Id)
            };
            if (!response.Unit)
            {
                response.Message = "Category not found";
                return BadRequest(response);
            }
            await _unitOfWork.Complete();
            response.Message = "Deleted";
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BaseCategoryBody>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BaseCategoryBody>>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> FindIdByName(string name)
        {
            var response = new GeneralResponseDto<IQueryable<BaseCategoryBody>>();

            try
            {
                var categories = _unitOfWork.Category.Find(c => c.Name.Contains(name));
                response.Unit = await Task.Run(() =>
                    categories.ProjectTo<BaseCategoryBody>(_mapper.ConfigurationProvider));
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
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BaseCategoryBody>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<IQueryable<BaseCategoryBody>>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> FindByName([FromBody] BaseCategoryBody request)
        {
            var response = new GeneralResponseDto<IQueryable<BaseCategoryBody>>();
            if (string.IsNullOrEmpty(request.CategoryDto.Name))
            {
                response.Message = "Please, provide the category name";
                return BadRequest(response);
            }

            try
            {
                var categories = _unitOfWork.Category.Find(c => c.Name == request.CategoryDto.Name);
                response.Unit = await Task.Run(() =>
                    categories.ProjectTo<BaseCategoryBody>(_mapper.ConfigurationProvider));
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
        [ProducesResponseType(typeof(GeneralResponseDto<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<int>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateNewCategory([FromQuery] CategoryDto category)
        {
            var response = new GeneralResponseDto<int>();
            if (category.Name.IsNullOrEmpty())
            {
                response.Message = "Please, provide category name";
            }
            var newCategory = _mapper.Map<CategoryDto, Category>(category);
            if (newCategory == null)
            {
                response.Message = "Server side error: Object is not mapped, check mapping profile";
                return BadRequest(response);
            }
            response.Unit = await _unitOfWork.Category.AddAsync(newCategory);
            await _unitOfWork.Complete();
            response.Message = "New entity created";
            return Ok(response);
        }


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(GeneralResponseDto<BaseCategoryBody>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GeneralResponseDto<BaseCategoryBody>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Change([FromBody] BaseCategoryBody body)
        {
            Category category = new();
            var response = new GeneralResponseDto<BaseCategoryBody>();

            try
            {
                category = await _unitOfWork.Category.Find(c => c.Id == body.Id).FirstAsync();
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



            var listDtoProp = body.CategoryDto.GetType().GetProperties();
            foreach (var property in listDtoProp)
            {
                if (property.GetValue(body.CategoryDto) != null)
                {
                    _mapper.Map(body.CategoryDto, category);
                }

            }

            await _unitOfWork.Complete();
            response.Message = "Changes applied";
            response.Unit = body;
            return Ok(response);
        }

    }
}
