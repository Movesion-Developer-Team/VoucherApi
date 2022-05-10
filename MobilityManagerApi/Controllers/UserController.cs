using AutoMapper;
using Core.Domain;
using DTOs.BodyDtos;
using DTOs.ResponseDtos;
using Enum;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using UserStoreLogic;

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
    }
}
