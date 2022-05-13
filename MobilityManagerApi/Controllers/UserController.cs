using System.Security.Claims;
using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;
using DTOs.ResponseDtos;
using Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using UserStoreLogic;
using UserStoreLogic.DTOs.ResponseDtos;

namespace MobilityManagerApi.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors]
    public class UserController : ControllerBase, IControllerBaseActions
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public UserController(IMapper mapper, VoucherContext vContext)
        {
            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);
        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin, Role.User)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendRequestToJoinCompany([FromBody] SendRequestToJoinCompanyBodyDto body)
        {
            var response = new BaseResponse();
            try
            {
                var user = await _unitOfWork.User.Find(u => u.IdentityUserId == body.UserId).FirstOrDefaultAsync();
                if (user == null)
                {
                    response.Message = "User not found";
                    return BadRequest(response);
                }
                await _unitOfWork.User.SendJoinRequestToCompany(body.InviteCode, (int)user.Id, body.Message);
                response.Message = "Request sent successfully";
                return Ok(response);
            }
            catch (ArgumentNullException ex)
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

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(GetCurrentUserInfoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetCurrentUserInfoResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var response = new GetCurrentUserInfoResponseDto();
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = identity.FindFirst(ClaimTypes.Role).Value;
                if (role.Contains(Role.SuperAdmin.ToString()))
                {
                    response.Message = "SuperAdmin can be assigned only to one company - Movesion";
                    return BadRequest(response);
                }
                var currentUser = await _unitOfWork.User
                    .Find(u => u.IdentityUserId == id)
                    .FirstOrDefaultAsync();

                if (currentUser == null)
                {
                    response.Message = "User not found";
                    return BadRequest(response);
                }

                if (currentUser.CompanyId == null)
                {
                    response.Message = "User not assigned to the company";
                    return BadRequest(response);
                }

                response.CompanyId = currentUser.CompanyId;
                return Ok(response);
            }

            response.Message = "Claim not found";
            return BadRequest(response);
        }
    }
}
