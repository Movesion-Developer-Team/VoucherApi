//using AutoMapper;
//using Core.Domain;
//using DTOs;
//using Enum;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;
//using MobilityManagerApi.Dtos.BodyDtos;
//using MobilityManagerApi.Dtos.ResponseDtos;
//using Persistence;
//using UserStoreLogic;

//namespace MobilityManagerApi.Controllers
//{
//    [ApiController]
//    [Route("[controller]/[action]")]
//    [EnableCors]
//    public class CategoryController : ControllerBase, IControllerBaseActions
//    {
//        private readonly IMapper _mapper;
//        private readonly UnitOfWork _unitOfWork;
//        private readonly GeneralResponseDto _response = new();

//        [FromBody]
//        public BaseCategoryBody? RequestBody { get; set; } 

//        public CategoryController(IMapper mapper, VoucherContext vContext)
//        {
//            _mapper = mapper;
//            _unitOfWork = new UnitOfWork(vContext);
//        }



//        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            try
//            {
//                _response.Unit = await _unitOfWork.Category.GetAll();
//                return Ok(_response);
//            }
//            catch (NullReferenceException ex)
//            {
//                _response.Message = ex.Message;
//                return BadRequest(_response);
//            }
//        }

//        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
//        [HttpGet]
//        public async Task<IActionResult> FindById([FromBody] BaseBody request)
//        {
//            try
//            {
//                var category = await _unitOfWork.Category.FindAsync(c => c.Id == request.Id);
//                _response.Unit = category.First();
//                return Ok(_response);
//            }
//            catch (NullReferenceException ex)
//            {
//                _response.Message = ex.Message;
//                return BadRequest(_response);
//            }
//        }

//        [AuthorizeRoles(Role.SuperAdmin)]
//        [HttpPost]
//        public async Task<IActionResult> Delete(BaseBody request)
//        {
//            var deleted = await _unitOfWork.Category.RemoveAsync(request.Id);
//            if (!deleted)
//            {
//                _response.Message = "Category not found";
//                return BadRequest(_response);
//            }
//            await _unitOfWork.Complete();
//            _response.Message = "Deleted";
//            return Ok(_response);
//        }

//        [Authorize]
//        [HttpPost]
//        public async Task<IActionResult> FindIdByName()
//        {
//            if (RequestBody == null)
//            {
//                _response.Message = "You did not provide a body";
//                return BadRequest(_response);
//            }
//            var name = RequestBody.CategoryDto.Name;

//            if (name == null)
//            {
//                _response.Message = "Please, provide the name";
//                return BadRequest(_response);
//            }

//            try
//            {
//                _response.Unit = await _unitOfWork.Category.FindAsync(c => c.Name.Contains(name));
//                return Ok(_response);
//            }
//            catch (NullReferenceException ex)
//            {
//                _response.Message = ex.Message;
//                return BadRequest(_response);
//            }
//        }

//        [AuthorizeRoles(Role.SuperAdmin)]
//        [HttpGet]
//        public async Task<IActionResult> FindByName([FromBody] BaseCategoryBody request)
//        {
//            if (string.IsNullOrEmpty(request.CategoryDto.Name))
//            {
//                _response.Message = "Please, provide the category name";
//                return BadRequest(_response);
//            }

//            try
//            {
//                var category = await _unitOfWork.Category.FindAsync(c => c.Name == request.CategoryDto.Name);
//                _response.Unit = category.First();
//                return Ok(_response);
//            }
//            catch (NullReferenceException ex)
//            {
//                _response.Message = ex.Message;
//                return BadRequest(_response);
//            }



//        }


//        [AuthorizeRoles(Role.SuperAdmin)]
//        [HttpPost]
//        public async Task<IActionResult> CreateNewCategory([FromQuery]CategoryDto category)
//        {
//            var newCategory = _mapper.Map<CategoryDto, Category>(category);
//            await _unitOfWork.Category.AddAsync(newCategory);
//            await _unitOfWork.Complete();
//            _response.Message = "New entity created";
//            return Ok(_response);
//        }


//        [AuthorizeRoles(Role.SuperAdmin)]
//        [HttpPost]
//        public async Task<IActionResult> Change()
//        {
//            Category category = new();
//            if (RequestBody == null)
//            {
//                _response.Message = "You did not provide a body";
//                return BadRequest(_response);
//            }
//            try
//            {
//                category = _unitOfWork.Category.FindAsync(c => c.Id == RequestBody.Id).Result.First();
//                _unitOfWork.Category.Update(category);
//            }
//            catch (NullReferenceException ex)
//            {
//                _response.Message = ex.Message;
//            }


//            if (_response.Message != null)
//            {
//                return BadRequest(_response);
//            }



//            var listDtoProp = RequestBody.CategoryDto.GetType().GetProperties();
//            foreach (var property in listDtoProp)
//            {
//                if (property.GetValue(RequestBody.CategoryDto) != null)
//                {
//                    _mapper.Map(RequestBody.CategoryDto, category);
//                }

//            }

//            await _unitOfWork.Complete();
//            _response.Message = "Changes applied";
//            _response.Unit = category;
//            return Ok(_response);
//        }






//    }
//}
