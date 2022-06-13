using System.Security.Claims;
using AutoMapper;
using DTOs.ResponseDtos;
using Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace BenefitsApi.Controllers
{
    public class PadreController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly UnitOfWork _unitOfWork;

        public PadreController(IMapper mapper, VoucherContext vContext)
        {
            _mapper = mapper;
            _unitOfWork = new UnitOfWork(vContext);
        }

        protected async Task<PrivateGetCurrentUserInfoResponseDto> GetCurrentUserInfo()
        {
            var result = new PrivateGetCurrentUserInfoResponseDto();
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var id = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var role = identity.FindFirst(ClaimTypes.Role).Value;
                if (role.Contains(Role.SuperAdmin.ToString()))
                {
                    result.Message = "SuperAdmin can be assigned only to one company - Movesion";
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;
                }
                var currentUser = await _unitOfWork.User
                    .Find(u => u.IdentityUserId == id)
                    .FirstOrDefaultAsync();

                if (currentUser == null)
                {
                    result.Message = "User found";
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;
                }

                if (currentUser.CompanyId == null)
                {
                    result.Message = "User not assigned to the company";
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;
                }

                result.CompanyId = currentUser.CompanyId;
                result.StatusCode = StatusCodes.Status200OK;
                return result;
            }

            result.Message = "Claim not found";
            result.StatusCode = StatusCodes.Status400BadRequest;
            return result;
        }
    }
}
