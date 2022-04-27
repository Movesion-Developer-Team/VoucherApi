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
    public class CategoryController : ControllerBase, IControllerBaseActions
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;


        public CategoryController(IMapper mapper, VoucherContext vContext)
        {
            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(CreateNewEntityResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateNewEntityResponseDto), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateNewCategory([FromQuery] CreateNewCategoryBodyDto category)
        {
            var response = new CreateNewEntityResponseDto();
            if (category.Name.IsNullOrEmpty())
            {
                response.Message = "Please, provide category name";
                return BadRequest(response);

            }
            var newCategory = _mapper.Map<CreateNewCategoryBodyDto, Category>(category);
            if (newCategory == null)
            {
                response.Message = "Server side error: Object is not mapped, check mapping profile";
                return BadRequest(response);
            }
            response.Id = await _unitOfWork.Category.AddAsync(newCategory);
            await _unitOfWork.Complete();
            response.Message = "New entity created";
            return Ok(response);
        }


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(CategoryMainResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CategoryMainResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Change([FromBody] CategoryBodyDto body)
        {
            Category category = new();
            var response = new CategoryMainResponseDto();

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



            var listDtoProp = body.GetType().GetProperties();
            foreach (var property in listDtoProp)
            {
                if (property.GetValue(body) != null)
                {
                    _mapper.Map(body, category);
                }

            }

            await _unitOfWork.Complete();
            var idValue = listDtoProp.First(p => p.Name == "Id").GetValue(body);
            response.Message = idValue != null ? "Warning: changes applied, but new Id is not assigned, because it is forbidden on server side"
                : "Changes applied";
            response.Unit = body;
            return Ok(response);
        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllCategoriesResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllCategoriesResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var response = new GetAllCategoriesResponseDto();
            try
            {
                var categories = await _unitOfWork.Category.GetAll();
                response.Unit = categories.ProjectTo<CategoryBodyDto>(_mapper.ConfigurationProvider);
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
        [ProducesResponseType(typeof(CategoryMainResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CategoryMainResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindById(int id)
        {
            var response = new CategoryMainResponseDto();
            try
            {
                var categories = _unitOfWork.Category.Find(c => c.Id == id);
                response.Unit = await categories.ProjectTo<CategoryBodyDto>(_mapper.ConfigurationProvider).FirstAsync();
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
        [ProducesResponseType(typeof(CategoryFindByNameResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CategoryFindByNameResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindByName(string name)
        {
            var response = new CategoryFindByNameResponseDto();
            try
            {
                var categories = _unitOfWork.Category.Find(c => c.Name == name);
                response.Unit = await Task.Run(() =>
                    categories.ProjectTo<CategoryBodyDto>(_mapper.ConfigurationProvider));
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
        [ProducesResponseType(typeof(DeleteResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DeleteResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(BaseBody request)
        {
            var response = new DeleteResponseDto()
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


        


        

    }
}
