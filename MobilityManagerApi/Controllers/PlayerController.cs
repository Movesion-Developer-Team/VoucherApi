using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
using System.Drawing;
using System.Net;
using System.Security.Policy;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors]
    public class PlayerController : ControllerBase, IControllerBaseActions
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
         

        public PlayerController(IMapper mapper, VoucherContext vContext)
        {

            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);

        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(CreateNewEntityResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateNewEntityResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNewPlayer([FromBody] CreateNewPlayerBodyDto body)
        {
            var response = new CreateNewEntityResponseDto();
            if (body.ShortName.IsNullOrEmpty())
            {
                response.Message = "Please, provide player shortName";
                return BadRequest(response);
            }
            var newPlayer = _mapper.Map<Player>(body);
            if (newPlayer == null)
            {
                response.Message = "Server side error: Object is not mapped, check mapping profile";
                return BadRequest(response);
            }

            Category? category;
            try
            {
                category = await _unitOfWork.Category.Find(c => c.Id == body.CategoryId).FirstAsync();
            }
            catch(NullReferenceException ex)
            {
                response.Message = $"Category not found: {ex.Message}";
                return BadRequest(response);
            }
            
            newPlayer.Categories = new List<Category>();
            newPlayer.Categories.Add(category);
            DiscountType discountType;
            try
            {
                discountType = await _unitOfWork.Discount.FindDiscountType(body.DiscountTypeId);
            }
            catch(ArgumentNullException ex)
            {
                response.Message = $"Internal Server error: {ex.Message}";
                return BadRequest(response);
            }

            newPlayer.DiscountsTypes = new List<DiscountType>()
            {
                discountType
            };
            int? id;
            try
            {
                id = await _unitOfWork.Player.AddAsync(newPlayer);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            await _unitOfWork.Complete();
            response.Id = id;
            response.Message = "New entity created";
            return Ok(response);
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(PlayerMainResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PlayerMainResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Change([FromBody] PlayerWithCategoriesAndDiscountTypesBodyDto withCategoriesAndDiscountTypesBody)
        {
            Player player = new();
            var response = new PlayerMainResponseDto();

            try
            {
                player = await _unitOfWork.Player.Find(c => c.Id == withCategoriesAndDiscountTypesBody.Id).FirstAsync();
                _unitOfWork.Player.Update(player);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
            }


            if (response.Message != null)
            {
                return BadRequest(response);
            }



            var listDtoProp = withCategoriesAndDiscountTypesBody.GetType().GetProperties();
            foreach (var property in listDtoProp)
            {
                if (property.GetValue(withCategoriesAndDiscountTypesBody) != null)
                {
                    _mapper.Map(withCategoriesAndDiscountTypesBody, player);
                }

            }

            await _unitOfWork.Complete();

            var idValue = listDtoProp.First(p => p.Name == "Id").GetValue(withCategoriesAndDiscountTypesBody);
            response.Message = idValue != null ? "Warning: changes applied, but new Id is not assigned, because it is forbidden on server side"
                : "Changes applied";
            response.Player = withCategoriesAndDiscountTypesBody;
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllPlayersResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllPlayersResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] bool withImage = true)
        {
            var response = new GetAllPlayersResponseDto();

            try
            {
                var players = await _unitOfWork.Player.GetAll(withImage);
                response.Players = players.ProjectTo<PlayerWithCategoriesAndDiscountTypesBodyDto>(_mapper.ConfigurationProvider);
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
        [ProducesResponseType(typeof(PlayerMainResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PlayerMainResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindById(int id)
        {
            var response = new PlayerMainResponseDto();

            try
            {
                var player = await _unitOfWork.Player.Find(c => c.Id == id).Include(p=>p.Image).FirstAsync();
                response.Player = _mapper.Map<PlayerWithCategoriesAndDiscountTypesBodyDto>(player);
                
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
        [ProducesResponseType(typeof(PlayerFindByNameResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PlayerFindByNameResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindByName(string shortName)
        {
            var response = new PlayerFindByNameResponseDto();
            try
            {
                var players = _unitOfWork.Player.Find(c => c.ShortName == shortName).Include(p=>p.Image);
                response.Players = await Task.Run(() => players.ProjectTo<PlayerOnlyBodyDto>(_mapper.ConfigurationProvider));
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
                IsDeleted = await _unitOfWork.Player.RemoveAsync(id)
            };
            if (!response.IsDeleted)
            {
                response.Message = "Player not found";
                return BadRequest(response);
            }
            await _unitOfWork.Complete();
            response.Message = "Deleted";
            return Ok(response);
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignCategoryToPlayer([FromBody] AssignCategoryToPlayerBodyDto body)
        {
            var response = new BaseResponse();
            if (body.PlayerId == null)
            {
                response.Message = "Please provide Player Id";
            }

            if (body.CategoryId == null)
            {
                response.Message = "Please provide Category Id";
            }

            try
            {
                await _unitOfWork.Player.AssignCategoryToPlayer((int) body.PlayerId, (int) body.CategoryId);
                response.Message = "Done";
                return Ok(response);
            }
            catch(InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Message = $"Server internal error: {ex.Message}";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Server internal error: {ex.Message}";
                return BadRequest(response);
            }

        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCategoryFromPlayer([FromBody] DeleteCategoryFromPlayerBodyDto body)
        {
            var response = new BaseResponse();
            if (body.PlayerId == null)
            {
                response.Message = "Please provide Player Id";
                return BadRequest(response);
            }

            if (body.CategoryId == null)
            {
                response.Message = "Please provide Category Id";
                return BadRequest(response);

            }

            try
            {
                var deleted =
                    await _unitOfWork.Player.DeleteCategoryFromPlayer((int) body.PlayerId, (int) body.CategoryId);
                if (!deleted)
                {
                    response.Message = "Category is not assigned to the current Player";
                    return BadRequest(response);
                }

                response.Message = "Deleted";
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Message = ex.Message;
                return Ok(response);
            }



        }


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignDiscountTypeToPlayer([FromBody] AssignDiscountTypeToPlayerBodyDto body)
        {
            var response = new BaseResponse();
            try
            {
                await _unitOfWork.Player.AssignDiscountTypeToPlayer(body.PlayerId, body.DiscountTypeId);
                response.Message = "Done";
                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Unexpected server error: {ex.Message}";
                return BadRequest(response);

            }
        }


        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllDiscountTypesForPlayerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllDiscountTypesForPlayerResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllDiscountTypesForPlayer([FromQuery] int? playerId)
        {
            var response = new GetAllDiscountTypesForPlayerResponseDto();
            
            try
            {
                var discountTypes = await _unitOfWork.Player.GetAllDiscountTypesForPlayer(playerId);
                response.DiscountTypes = _mapper.ProjectTo<DiscountTypeBodyDto>(discountTypes);
                response.Message = "Done";
                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Message =$"Internal server error: {ex.Message}";
                return BadRequest(response);
            }

        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignPlayerToCompany([FromBody] AddPlayerToCompanyBodyDto body)
        {
            var response = new BaseResponse();
            try
            {
                await _unitOfWork.Player.AssignPlayerToCompany(body.CompanyId, body.PlayerId);
                response.Message = "Done";
                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
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
                response.Message = $"Unexpected server error: {ex.Message}";
                return BadRequest(response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddImageToPlayer([FromQuery] int idPlayer)
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
                await _unitOfWork.Player.AddImageToPlayer(image, idPlayer);
                await _unitOfWork.Complete();
                response.Message = "Done";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetImageOfPlayer([FromQuery] int playerId)
        {
            var response = new BaseResponse();

            try
            {
                var imageInBytes = await _unitOfWork.Player.GetImageOfPlayer(playerId);

                return File(imageInBytes.Content, "image/jpeg");
            }
            catch (ArgumentNullException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
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
                response.Message = ex.Message;
                return BadRequest(response);
            }
            
        }

    }
}

