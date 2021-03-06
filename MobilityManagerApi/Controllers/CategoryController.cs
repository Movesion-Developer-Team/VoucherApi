using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BenefitsApi.Controllers;
using Core.Domain;
using DTOs.BodyDtos;
using DTOs.ResponseDtos;
using Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using UserStoreLogic;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]/")]
    [EnableCors]
    public class CategoryController : PadreController, IControllerBaseActions
    {
        
        public CategoryController(IMapper mapper, VoucherContext vContext) : base(mapper, vContext)
        {
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(CreateNewEntityResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateNewEntityResponseDto), StatusCodes.Status400BadRequest)]
        

        public async Task<IActionResult> CreateNewCategory([FromBody] CreateNewCategoryBodyDto category)
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
            response.Category = body;
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
                response.Categories = categories.ProjectTo<CategoryBodyDto>(_mapper.ConfigurationProvider);
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
                response.Category = await categories.ProjectTo<CategoryBodyDto>(_mapper.ConfigurationProvider).FirstAsync();
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
                response.Categories = await Task.Run(() =>
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
        [HttpDelete]
        [ProducesResponseType(typeof(DeleteResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DeleteResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = new DeleteResponseDto()
            {
                IsDeleted = await _unitOfWork.Category.RemoveAsync(id)
            };
            if (!response.IsDeleted)
            {
                response.Message = "Category not found";
                return BadRequest(response);
            }
            await _unitOfWork.Complete();
            response.Message = "Deleted";
            return Ok(response);
        }


        [AuthorizeRoles(Role.SuperAdmin)]

        [HttpGet]
        [ProducesResponseType(typeof(GetAllCategoriesForPlayerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllCategoriesForPlayerResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllCategoriesForPlayer([FromQuery] int playerId)
        {
            var response = new GetAllCategoriesForPlayerResponseDto();
            try
            {

                var categories = await _unitOfWork.Category.GetAllCategoriesForPlayer(playerId);
                response.Categories = _mapper.ProjectTo<CategoryBodyDto>(categories);
                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message =$"Internal server error: {ex.Message}";
                return BadRequest(response);
            }

        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin, Role.User)]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllCategoriesForCompanyResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllCategoriesForCompanyResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllCategoriesForCurrentCompany()
        {
            var response = new GetAllCategoriesForCompanyResponseDto();
            var currentUserInfo = await GetCurrentUserInfo();
            if (currentUserInfo.StatusCode != StatusCodes.Status200OK)
            {
                response.Message = currentUserInfo.Message;
                return BadRequest(response);
            }
            int? companyId = currentUserInfo.CompanyId;

            try
            {
                var categories = await _unitOfWork.Category.GetAllCategoriesForCompany((int)companyId);
                response.Categories = _mapper.ProjectTo<CategoryBodyDto>(categories);
                response.Message = "Done";
                return Ok(response);
                
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }


        [AuthorizeRoles(Role.SuperAdmin, Role.Admin, Role.User)]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllPlayersResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllPlayersResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPlayersForCurrentCategory([FromQuery] int categoryId)
        {
            var response = new GetAllPlayersResponseDto();
            var currentUserInfo = await GetCurrentUserInfo();
            if (currentUserInfo.StatusCode != StatusCodes.Status200OK)
            {
                response.Message = currentUserInfo.Message;
                return BadRequest(response);
            }
            var companyId = currentUserInfo.CompanyId;
            try
            {
                var players = await _unitOfWork.Category.GetAllPlayersForCategoryAndCompany((int) companyId, categoryId);
                response.Players = _mapper.ProjectTo<PlayerWithCategoriesAndDiscountTypesBodyDto>(players);
                response.Message = "Done";
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }

        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        [Route("{idCategory}")]
        public async Task<IActionResult> AddImageToCategory([FromRoute] int idCategory)
        {

            var response = new BaseResponse();
            var permittedExtensions = new string[]
            {
                ".jpeg",
                ".png",
                ".jpg"
            };
            try
            {
                await using var memoryStream = new MemoryStream();
                Request.EnableBuffering();
                var formCollection = await Request.ReadFormAsync();
                var fileName = formCollection.Files.First().FileName;
                var ext = Path.GetExtension(fileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                {
                    response.Message = "Current file extension is not valid. Please upload only jpg, jpeg or png files.";
                    return BadRequest(response);
                }
                await formCollection.Files.First().CopyToAsync(memoryStream);
                
                if (memoryStream.Length > 3000000)
                {
                    response.Message = "File Size should not exceed 3 mb";
                    return BadRequest(response);
                }

                if (memoryStream.Length == 0)
                {
                    response.Message = "Image is empty";
                    return BadRequest(response);
                }

                var image = new BaseImage
                {
                    Content = memoryStream.ToArray()
                };
                await _unitOfWork.Category.AddImageToCategory(image, idCategory);
                await _unitOfWork.Complete();
                response.Message = "Done";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
        }



    }
}
